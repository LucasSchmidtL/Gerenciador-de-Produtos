
from __future__ import annotations

"""CalculatorService – versão refatorada com paralelização, cache e segurança extra.

* Ajuste 2025‑06‑02 ➜ inclui resultados de **funções‑longarina** no dicionário final
  (antes só equações genéricas eram retornadas).
* `_executar_equacoes` agora recebe lista opcional `funcoes`.
* Ajuste 2025‑06‑02 ➜ inclui método `obter_melhor_longarina` para identificar o perfil com menor PesoLinear válido.
"""

from concurrent.futures import ThreadPoolExecutor, as_completed
from typing import Dict, Tuple, List, Any, Union
from functools import lru_cache
import math
import logging
import re

from asteval import Interpreter
from sympy import symbols, Eq, solve, sympify

from calculadoras.repos.equacao_repo import EquacaoRepoSQLAlchemy
from calculadoras.repos.funcao_longarina_repo import FuncaoLongarinaRepo
from calculadoras.repos.longarina_repo import LongarinaRepo

logger = logging.getLogger(__name__)
logging.basicConfig(level=logging.INFO)

# ---------------------------------------------------------------------------
# Utilidades auxiliares
# ---------------------------------------------------------------------------

BANNED_TOKENS = {"import", "exec", "eval", "open", "subprocess", "os", "sys", "__"}


def _validar_formula(codigo: str) -> None:
    tokens = re.findall(r"[A-Za-z_][A-Za-z0-9_]*", codigo)
    if any(tok in BANNED_TOKENS for tok in tokens):
        raise ValueError(f"Fórmula contém token não permitido: {set(tokens) & BANNED_TOKENS}")


def _extrair_variaveis(formula: str) -> set[str]:
    formula_limpa = re.sub(r'".*?"|\'.*?\'', "", formula)
    tokens = re.findall(r"\b[a-zA-Z_][a-zA-Z0-9_]*\b", formula_limpa)
    palavras_chave = {"True", "False", "None", "if", "else", "for", "in", "and", "or", "not"}
    return {
        t
        for t in tokens
        if t not in palavras_chave and not re.search(rf"\b{t}\s*\(", formula_limpa)
    }


# ---------------------------------------------------------------------------
# Serviço principal
# ---------------------------------------------------------------------------

class CalculatorService:
    def __init__(self, session):
        self.session = session
        self.equacao_repo = EquacaoRepoSQLAlchemy(session)
        self.funcao_repo = FuncaoLongarinaRepo(session)
        self.longarina_repo = LongarinaRepo(session)

    # ........................................ caches ........................

    @lru_cache(maxsize=None)
    def _carregar_equacoes(self):
        return self.equacao_repo.get_all()

    @lru_cache(maxsize=None)
    def _carregar_funcoes_longarina(self):
        return self.funcao_repo.get_all()

    # ........................................ helpers ......................

    def _preparar_contexto(self, longarina, parametros: Dict[str, float]) -> Dict[str, Any]:
        ctx: Dict[str, Any] = {**parametros}
        for k, v in vars(longarina).items():
            if not k.startswith("_"):
                ctx[k] = v
                ctx[k.lower()] = v
        return ctx

    def _configurar_interpreter(self, contexto: Dict[str, Any]) -> Interpreter:
        aeval = Interpreter()
        aeval.symtable.update({k: getattr(math, k) for k in dir(math) if not k.startswith("_")})
        aeval.symtable.update({"pi": math.pi, "Pi": math.pi, "e": math.e})
        aeval.symtable.update(contexto)
        return aeval

    # ........................................ carga de funções .............

    def _carregar_funcoes(self, aeval: Interpreter, raw_funcoes, raw_equacoes) -> None:
        funcoes_dict, deps = {}, {}
        for func in (*raw_funcoes, *raw_equacoes):
            nome = getattr(func, "nome_equacao", None) or getattr(func, "nome", None)
            formula = getattr(func, "equacao", None) or getattr(func, "formula", None)
            if nome and isinstance(formula, str):
                funcoes_dict[nome] = formula
                deps[nome] = _extrair_variaveis(formula) - {nome}
        for nome in self._ordenar_dependencias(funcoes_dict, deps):
            code = funcoes_dict[nome].strip()
            _validar_formula(code)
            if code.startswith("def"):
                exec(code, {}, aeval.symtable)
            elif "=" in code and "if" not in code:
                continue  # ignora equações explícitas
            else:
                exec(f"{nome} = ({code})", {}, aeval.symtable)

    @staticmethod
    def _ordenar_dependencias(fdict, deps):
        ordem, vist = [], set()
        def vis(nome):
            if nome not in vist:
                for d in deps.get(nome, []):
                    if d in fdict:
                        vis(d)
                ordem.append(nome); vist.add(nome)
        for n in fdict: vis(n)
        return ordem

    # ........................................ execução .....................

    def _resolver_equacao(self, nome: str, equacao: str, aeval: Interpreter) -> float:
        equacao_limpa = equacao.replace(" ", "")
        try:
            if "=" in equacao_limpa and not equacao.strip().startswith("def"):
                lhs, rhs = map(str.strip, equacao.split("=", 1))
                X = symbols(lhs)
                expr = sympify(rhs, locals=aeval.symtable) - X
                sol = solve(Eq(expr, 0), X)
                return float(sol[0]) if sol else math.nan
            val = aeval(equacao)
            if aeval.error or not isinstance(val, (int, float)):
                raise ValueError(val)
            return float(val)
        except Exception as e:
            raise ValueError(f"Erro em '{nome}': {e}") from e

    # ........................................ contexto check ...............

    def validar_contexto(self, ctx: Dict[str, Any]):
        faltantes: Dict[str, List[str]] = {}
        nomes_fun = {getattr(x, "nome_equacao", None) or getattr(x, "nome", None)
                      for x in (*self._carregar_equacoes(), *self._carregar_funcoes_longarina())}
        for eq in self._carregar_equacoes():
            nome = getattr(eq, "nome_equacao", None) or getattr(eq, "nome", None)
            formula = getattr(eq, "equacao", None) or getattr(eq, "formula", None)
            if nome and formula:
                for v in _extrair_variaveis(formula) - nomes_fun:
                    if v not in ctx:
                        faltantes.setdefault(v, []).append(nome)
        return faltantes

    # ........................................ API públicas .................

    def calculate_for_perfil(self, perfil: str, params: Dict[str, float]) -> Dict[str, Tuple[float, str]]:
        longarina = self.longarina_repo.get_by_perfil(perfil)
        if not longarina:
            raise ValueError(f"Perfil '{perfil}' não encontrado")
        ctx = self._preparar_contexto(longarina, params)
        if self.validar_contexto(ctx):
            raise ValueError("Contexto incompleto")
        aeval = self._configurar_interpreter(ctx)
        funcoes = self._carregar_funcoes_longarina()
        equacoes = self._carregar_equacoes()
        self._carregar_funcoes(aeval, funcoes, equacoes)
        return self._executar_equacoes(aeval, equacoes, funcoes)

    def calcular_dinamico(self, vars_: Dict[str, float], perfil: str | None = None):
        if perfil:
            longarina = self.longarina_repo.get_by_perfil(perfil)
            if not longarina:
                raise ValueError(f"Perfil '{perfil}' não encontrado")
            ctx = self._preparar_contexto(longarina, vars_)
        else:
            ctx = dict(vars_)
        if self.validar_contexto(ctx):
            raise ValueError("Contexto incompleto")
        aeval = self._configurar_interpreter(ctx)
        funcoes = self._carregar_funcoes_longarina()
        equacoes = self._carregar_equacoes()
        self._carregar_funcoes(aeval, funcoes, equacoes)
        return {k: v for k, (v, _) in self._executar_equacoes(aeval, equacoes, funcoes).items()}

    def obter_melhor_longarina(self, parametros: Dict[str, float], longarinas: List[Any] | None = None) -> str | None:
        melhores: List[Tuple[str, float]] = []
        longarinas = longarinas or self.longarina_repo.get_all()
        for l in longarinas:
            try:
                ctx = self._preparar_contexto(l, parametros)
                if self.validar_contexto(ctx):
                    continue
                peso = getattr(l, "PesoLinear", None)
                if isinstance(peso, (int, float)):
                    melhores.append((l.perfil, float(peso)))
            except Exception:
                continue
        if not melhores:
            return None
        return min(melhores, key=lambda x: x[1])[0]

    def calcular_para_todos_perfis(self, parametros: Dict[str, float]) -> Tuple[List[str], List[Dict[str, Any]]]:
        perfis = [l.Perfil for l in self.longarina_repo.get_all()]
        colunas: List[str] = []
        linhas: List[Dict[str, Any]] = []

        for perfil in perfis:
            try:
                resultados = self.calculate_for_perfil(perfil, parametros)
                if not colunas:
                    colunas = ["Perfil"] + list(resultados.keys()) + ["Situação"]

                linha = {"Perfil": perfil}
                situacao_geral = True
                for nome, (valor, status) in resultados.items():
                    linha[nome] = {"valor": valor, "status": status}
                    if status != "ok":
                        situacao_geral = False
                linha["Situação"] = "Verdadeiro" if situacao_geral else "Falso"
                linhas.append(linha)

            except Exception as e:
                linhas.append({
                    "Perfil": perfil,
                    "Erro": str(e),
                    "Situação": "Falso"
                })

        return colunas, linhas

    # ........................................ helper interno ...............

    def _executar_equacoes(self, aeval: Interpreter, equacoes, funcoes):
        resultados: Dict[str, Tuple[float, str]] = {}
        for registro in (*equacoes, *funcoes):
            nome = getattr(registro, "nome_equacao", None) or getattr(registro, "nome", None)
            expr = getattr(registro, "equacao", None) or getattr(registro, "formula", None)
            if not (nome and expr):
                continue
            try:
                valor = aeval.symtable.get(nome)
                if valor is None or isinstance(valor, Interpreter):
                    valor = self._resolver_equacao(nome, expr, aeval)
                resultados[nome] = (float(valor), "ok")
            except Exception as e:
                resultados[nome] = (math.nan, str(e))
        return resultados
    
    def selecionar_longarina_valida(self) -> str | None:
        longarinas = self.longarina_repo.get_all()
        validas = [l for l in longarinas if getattr(l, "Situacao", None) == "Verdadeira" or getattr(l, "situacao", None) == "Verdadeira"]
        if not validas:
            print("Nenhuma longarina válida encontrada")
            return None
        menor_peso = min(getattr(l, "PesoLinear", math.inf) for l in validas)
        selecionada = next(l for l in validas if getattr(l, "PesoLinear", math.inf) == menor_peso)
        print(f"[DEBUG] Longarina selecionada: {getattr(selecionada, 'Perfil', getattr(selecionada, 'perfil', 'N/A'))}, peso: {menor_peso}")
        return getattr(selecionada, "Perfil", getattr(selecionada, "perfil", None))
    


