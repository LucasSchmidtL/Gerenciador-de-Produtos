from fastapi import APIRouter, Depends
from sqlalchemy.orm import Session

from ...dependencies import get_db
from ...services.calculator_service import CalculatorService
from ...schemas import CalcRequest, CalcResponse, ResultadoItem

router = APIRouter()

@router.post("/calculate", response_model=CalcResponse)
def calcular_longarinas(req: CalcRequest, db: Session = Depends(get_db)):
    """
    Calcula resultados para todos os perfis registrados, com base nos
    parâmetros fornecidos (incluindo comprimento e carga suportada).
    """

    # 1. Prepara o dicionário de parâmetros incluindo variáveis e parâmetros
    parametros = {
        **(req.variaveis or {}),
        **(req.parametros or {})
    }

    # 2. Executa o serviço de cálculo
    service = CalculatorService(db)
    colunas, tabela = service.calcular_para_todos_perfis(parametros)

    # 3. Constrói a resposta para o frontend
    resultados_list = []
    for row in tabela:
        if row.get("Erro"):
            resultados_list.append(ResultadoItem(
                perfil     = row.get("Perfil", ""),
                peso_linear= 0.0,
                flt        = f"Erro: {row['Erro']}",
                flecha     = f"Erro: {row['Erro']}",
                situacao   = "ERRO",
                m_drlflt   = f"Erro: {row['Erro']}",
                m_drdelta  = f"Erro: {row['Erro']}",
            ))
        else:
            resultados_list.append(ResultadoItem(
                perfil     = row.get("Perfil", ""),
                peso_linear= row.get("PesoLinear", 0.0),
                flt        = row.get("FLT", 0.0),
                flecha     = row.get("Flecha", 0.0),
                situacao   = row.get("Situacao", ""),
                m_drlflt   = row.get("MDRLFLT", 0.0),
                m_drdelta  = row.get("MDRDelta", 0.0),
            ))

    return CalcResponse(codigo_longarina="", resultados=resultados_list)
