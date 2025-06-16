#calculadoras/api/endpoints/equacoes.py

from fastapi import APIRouter, Depends
from sqlalchemy.orm import Session
from ...models.equacao_model import EquacaoModel
from ...dependencies import get_db
from ...schemas import Equacao, EquacaoResponse,CalcRequest, ExecucaoEquacoesResponse, CalcPerfilRequest
from ...services.calculator_service import CalculatorService

router = APIRouter()

@router.get("/equacoes", response_model=EquacaoResponse)
def obter_equacoes(db: Session = Depends(get_db)):
    equacoes = db.query(EquacaoModel).all()
    for e in equacoes:
        print(f"Nome: {e.nome_equacao}, Equação: {e.equacao}")
    return {"equacoes": [Equacao(
        nome_equacao=e.nome_equacao,
        equacao=e.equacao,
        unidade=e.unidade,
        abreviacao=e.abreviacao,
        norma=e.norma,
        componente=e.componente,
        secao=e.secao,
        consideracoes=e.consideracoes
    ) for e in equacoes]}



@router.post("/executar", response_model=ExecucaoEquacoesResponse)
def executar_equacoes(
    entrada: CalcRequest,
    db: Session = Depends(get_db)
):
    service = CalculatorService(db)
    resultados = service.calcular_dinamico(entrada.variaveis | entrada.parametros)
    return {
        "perfil": "executado_dinamicamente",
        "resultados": resultados
    }

@router.post("/executar_por_perfil", response_model=ExecucaoEquacoesResponse)
def executar_equacoes_por_perfil(
    entrada: CalcPerfilRequest,
    db: Session = Depends(get_db)
):
    print(f"Recebido no endpoint: perfil={entrada.perfil}, variaveis={entrada.variaveis}, parametros={entrada.parametros}")

    service = CalculatorService(db)
    resultados = service.calculate_for_perfil(
        perfil=entrada.perfil,
        params=entrada.variaveis | entrada.parametros
    )
    return {
        "perfil": entrada.perfil,
        "resultados": resultados
    }

