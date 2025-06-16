from fastapi import APIRouter, Request, Depends, HTTPException
from fastapi.responses import HTMLResponse
from sqlalchemy.orm import Session

from ...dependencies import get_db
from ...models.longarinas_model import LongarinaModel
from ...models.funcoes_longarinas_model import FuncaoLongarinaModel
from ...services.calculator_service import CalculatorService
from ...core.templates import templates   # Jinja2Templates já configurado em core
from ...schemas import CalculoInput, CalcRequest, CalcResponse, ResultadoItem

router = APIRouter(prefix="/ui", tags=["interface"])


@router.get("/", response_class=HTMLResponse)
def exibir_interface(request: Request, db: Session = Depends(get_db)):
    """
    Renderiza a interface principal (`base.html`) contendo
    as abas Dimensionamento e Funções.
    """
    # --- dados vindos do banco ---------------------------------------------
    longarinas = db.query(LongarinaModel).all()
    funcoes    = db.query(FuncaoLongarinaModel).all()

    # --- calcula ­resultados para cada perfil ------------------------------
    service = CalculatorService(db)
    resultados_por_perfil: dict[str, dict] = {}

    for item in longarinas:
        try:
            raw = service.calculate_for_perfil(item.Perfil, parametros={})
            # transforma tupla (valor, unidade) ➜ dict mais simples
            resultados_por_perfil[item.Perfil] = {
                nome: {"valor": v, "unidade": u} for nome, (v, u) in raw.items()
            }
        except Exception:
            resultados_por_perfil[item.Perfil] = {}

    # --- valores de cálculo de longarina --------------
    variaveis = [
        {"simbolo": "L", "nome": "LongarinaComprimento", "valor": 2300, "descricao": "Comprimento da Longarina"}, #ok
        {"simbolo": "P", "nome": "CargaPlano",           "valor": 10000, "descricao": "CargaPlano"}, #ok
    ]

    # --- parâmetros fixos da tabela (se quiser preencher dinamicamente) ----
    parametros = [
        {"simbolo": "E ", "nome": "ModuloElasticidade",                "valor": 210000,    "descricao": "Módulo de Elasticidade"}, #ok
        {"simbolo": "Cb", "nome": "FatorMomentoFletorFlexaoSimples",   "valor": 1,         "descricao": "Fator de modificação para diagrama de momento fletor"},#ok
        {"simbolo": "G ", "nome": "ModuloCisalhamento",                "valor": 77000,     "descricao": "Módulo de Cisalhamento"}, #ok
        {"simbolo": "fy", "nome": "LimiteEscoamento",                  "valor": 300,       "descricao": "Limite de Escoamento"}, #ok
        {"simbolo": "Cp", "nome": "CoeficientePeso",                   "valor": 1.3,       "descricao": "Coeficiente de Peso"}, #ok
        {"simbolo": "Cs", "nome": "CoeficienteSeguranca",              "valor": 1.4,       "descricao": "Coeficiente de Segurança"},  #ok
        {"simbolo": "v ", "nome": "CoeficientePoisson",                "valor": 0.3,       "descricao": "Coeficiente de Poisson"}, #ok
        {"simbolo": "Kx", "nome": "RigidezSentidoX",                   "valor": 1,         "descricao": "Rigidez Longarina Sentido X"}, #ok
        {"simbolo": "Ky", "nome": "RigidezSentidoY",                   "valor": 1,         "descricao": "Rigidez Longarina Sentido Y"}, #ok
        {"simbolo": "Kz", "nome": "RigidezSentidoZ",                   "valor": 1,         "descricao": "Rigidez Longarina Sentido Z"}, #ok
        {"simbolo": "Ic", "nome": "MomentoInerciaColuna",              "valor": 281877.41, "descricao": "Momento de Inércia da Coluna"}, #ok
        {"simbolo": "h ", "nome": "AlturaEntreNiveis",                 "valor": 2800,      "descricao": "Altura Entre Níveis"}, #ok
        {"simbolo": "Bm", "nome": "CoeficienteLongarinaPrincipal",     "valor": 1,         "descricao": "Coeficiente da Longarina Principal"}, #ok
        {"simbolo": "B0", "nome": "CoeficenteLongarinaRotacao",        "valor": 1,         "descricao": "Coeficiente da Longarina Rotação"}, #ok
        {"simbolo": "Bt", "nome": "CoeficienteLongarinaDeslocamento",  "valor": 1,         "descricao": "Coeficiente da Longarina Deslocamento"}, #ok
        {"simbolo": "Lx", "nome": "ComprimentoLivreLongarinaX",        "valor": 2300,      "descricao": "Comprimento Livre da Longarina no Sentido X"},#ok
        {"simbolo": "Ly", "nome": "ComprimentoLivreLongarinaY",        "valor": 2300,      "descricao": "Comprimento Livre da Longarina no Sentido Y"},#ok
        {"simbolo": "Lz", "nome": "ComprimentoLivreLongarinaZ",        "valor": 2300,      "descricao": "Comprimento Livre da Longarina no Sentido Z"}, #ok
        # ... adicione os demais se quiser gerar pelo backend
    ]

    # --- renderiza o template base.html ------------------------------------
    return templates.TemplateResponse(
        "base.html",
        {
            "request": request,
            "longarinas": longarinas,
            "funcoes": funcoes,
            "resultados_por_perfil": resultados_por_perfil,
            "variaveis": variaveis,
            "parametros": parametros,       # usado em tab1_dimensionamento.html
        },
    )



@router.post("/calculate", response_model=CalcResponse)
def calcular_longarinas(req: CalcRequest, db: Session = Depends(get_db)):
    service = CalculatorService(db)
    saida: list[ResultadoItem] = []

    dados_entrada = {
        "LongarinaComprimento": req.variaveis.get("LongarinaComprimento", 0),
        "CargaPlano": req.variaveis.get("CargaPlano", 0),
        **req.parametros
    }

    for lg in db.query(LongarinaModel).all():
        try:
            raw = service.calculate_for_perfil(lg.Perfil, dados_entrada)

            flt = raw.get("FLT", ("–", ""))[0]
            flecha = raw.get("Flecha", ("–", ""))[0]
            m_drlflt = raw.get("MDRLFLT", ("–", ""))[0]
            m_drdelta = raw.get("MDRDelta", ("–", ""))[0]

            situacao = "Verdadeiro" if isinstance(flecha, (int, float)) and flecha <= 10 else "Falso"

            saida.append(ResultadoItem(
                perfil=lg.Perfil,
                peso_linear=lg.PesoLinear,
                flt=flt,
                flecha=flecha,
                situacao=situacao,
                m_drlflt=m_drlflt,
                m_drdelta=m_drdelta,
            ))

        except Exception as err:
            saida.append(ResultadoItem(
                perfil=lg.Perfil,
                peso_linear=lg.PesoLinear,
                flt=f"erro: {err}",
                flecha="–",
                situacao="ERRO",
                m_drlflt="–",
                m_drdelta="–",
            ))

    # Seleciona o melhor perfil com situação verdadeira e menor peso
    candidatos_validos = [r for r in saida if r.situacao == "Verdadeiro"]
    melhor = min(candidatos_validos, key=lambda x: x.peso_linear) if candidatos_validos else None

    return CalcResponse(
        codigo_longarina=melhor.perfil if melhor else "",
        resultados=saida
    )