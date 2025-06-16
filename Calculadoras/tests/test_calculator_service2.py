import types
import pytest

# Importa o serviço diretamente do seu módulo
from calculadoras.services.calculator_service import CalculatorService


# ---------------------------------------------------------------------------
#  Stubs / dummies de banco – não fazem acesso real ao BD
# ---------------------------------------------------------------------------

class _StubEquacao:
    """Representa um registro de equação/função que vem do banco."""
    def __init__(self, nome: str, formula: str):
        self.nome_equacao = nome
        self.equacao = formula
        # os demais campos não são necessários para o teste

class _StubLongarina:
    """Registro simplificado de longarina com alguns atributos numéricos."""
    def __init__(self, perfil: str, inercia_x: float):
        self.Perfil = perfil
        self.InerciaX = inercia_x
        # outros campos podem ser adicionados aqui conforme sua necessidade

class _FakeRepo:
    """Repo genérico configurável para retornar uma lista fixa ao chamar get_all."""
    def __init__(self, registros):
        self._registros = registros

    def get_all(self):
        return self._registros

    # para LongarinaRepo precisamos também de get_by_perfil()
    def get_by_perfil(self, perfil):
        # assume apenas um registro com o mesmo perfil
        for r in self._registros:
            if getattr(r, 'Perfil', None) == perfil:
                return r
        return None

# ---------------------------------------------------------------------------
#  Caso de teste principal – valida variável de usuário no contexto
# ---------------------------------------------------------------------------

def test_calculo_com_variavel_usuario(monkeypatch):
    """Valida que variáveis vindas do usuário (ex.: ModuloElasticidade) são reconhecidas."""

    # 1. Prepara stubs de dados
    longarina = _StubLongarina(perfil="U80x40x2", inercia_x=1.2e6)  # InerciaX fictícia

    funcoes_longarina = [
        _StubEquacao(
            "CargaCriticaFlambagemX",
            "pi**2 * ModuloElasticidade * InerciaX / (LongarinaComprimento**2)"
        )
    ]

    equacoes_genericas = []  # não precisamos neste teste

    # 2. Cria o serviço com uma sessão fake (não será usada)
    service = CalculatorService(session=types.SimpleNamespace())

    # 3. Monkey‑patch nos repositórios internos do serviço
    monkeypatch.setattr(service, "longarina_repo", _FakeRepo([longarina]))
    monkeypatch.setattr(service, "funcao_repo", _FakeRepo(funcoes_longarina))
    monkeypatch.setattr(service, "equacao_repo", _FakeRepo(equacoes_genericas))

    # 4. Executa
    variaveis = {
        "ModuloElasticidade": 2.05e11,   # 205 GPa
        "LongarinaComprimento": 500.0    # mm
    }

    resultados = service.calcular_dinamico(variaveis, perfil="U80x40x2")

    # 5. Verifica – a função deve existir e ter valor numérico
    assert "CargaCriticaFlambagemX" in resultados, "Variável não calculada"
    assert isinstance(resultados["CargaCriticaFlambagemX"], float), "Valor não é numérico"

# ---------------------------------------------------------------------------
# Rode com pytest no terminal:   pytest -q test_calculator_service.py
# ---------------------------------------------------------------------------
