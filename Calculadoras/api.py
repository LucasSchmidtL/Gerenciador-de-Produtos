import os
from fastapi import FastAPI, Depends, HTTPException, Request
from fastapi.middleware.cors import CORSMiddleware
from fastapi.staticfiles import StaticFiles
from fastapi.templating import Jinja2Templates
from fastapi.responses import HTMLResponse, JSONResponse, RedirectResponse
from pydantic import BaseModel, Field
from typing import Dict, List, Optional, Tuple
from sqlalchemy.orm import Session
from sqlalchemy import text

from .core.database import SessionLocal, get_db_connection
from .services.calculator_service import CalculatorService
from .models.equacao_model import EquacaoModel
from .models.longarinas_model import LongarinaModel
from .models.funcoes_longarinas_model import FuncaoLongarinaModel

app = FastAPI()

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

BASE_DIR = os.path.dirname(os.path.abspath(__file__))
STATIC_DIR = os.path.join(BASE_DIR, "static")
app.mount("/static", StaticFiles(directory=STATIC_DIR), name="static")

TEMPLATES_DIR = os.path.join(BASE_DIR, "templates")
templates = Jinja2Templates(directory=TEMPLATES_DIR)

def get_db() -> Session:
    db = SessionLocal()
    try:
        yield db
    finally:
        db.close()

# Modelos Pydantic ajustados para entrada de dados fixos + parâmetros dinâmicos
class ResultadoItem(BaseModel):
    perfil: str
    peso_linear: float
    flt: float
    flecha: float
    situacao: str
    m_drlflt: float
    m_drdelta: float

class CalcResponse(BaseModel):
    codigo_longarina: str
    resultados: List[ResultadoItem]

class CalcRequest(BaseModel):
    perfil: str
    comprimento: float
    pesoSuportado: float
    acessorios: int
    parametros: Dict[str, float] = Field(default_factory=dict)

class EquacaoResult(BaseModel):
    valor: float | str
    unidade: str = Field(..., description="Unidade de medida (texto)")

class Equacao(BaseModel):
    nome_equacao: str
    equacao: str
    unidade: Optional[str] = ""
    abreviacao: Optional[str] = ""
    norma: Optional[str] = ""
    componente: Optional[str] = ""
    secao: Optional[str] = ""
    consideracoes: Optional[str] = ""

class EquacaoResponse(BaseModel):
    equacoes: List[Equacao]


@app.post("/calculate", response_model=CalcResponse)
def calculate(req: CalcRequest, db: Session = Depends(get_db)):
    # Use seu serviço para calcular os resultados
    service = CalculatorService(db)

    resultados_raw = service.calculate_for_perfil(req.perfil, req.parametros)

    # Monta os resultados no formato esperado (exemplo genérico)
    resultados = []
    for r in resultados_raw:
        resultados.append(ResultadoItem(
            perfil=req.perfil,
            peso_linear=r.get('peso_linear', 0),
            flt=r.get('flt', 0),
            flecha=r.get('flecha', 0),
            situacao=r.get('situacao', 'OK'),
            m_drlflt=r.get('m_drlflt', 0),
            m_drdelta=r.get('m_drdelta', 0)
        ))

    return CalcResponse(
        codigo_longarina="Código123",  # você gera/obtém isso
        resultados=resultados
    )

@app.get("/equacoes", response_model=EquacaoResponse)
def obter_equacoes(db: Session = Depends(get_db)):
    models = db.query(EquacaoModel).all()
    equacoes = []
    for e in models:
        equacoes.append(Equacao(
            nome_equacao=e.nome_equacao,
            equacao=e.equacao,
            unidade=e.unidade,
            abreviacao=e.abreviacao,
            norma=e.norma,
            componente=e.componente,
            secao=e.secao,
            consideracoes=e.consideracoes
        ))
    return {"equacoes": equacoes}

@app.get("/longarinas", response_class=HTMLResponse)
def exibir_longarinas(request: Request, db: Session = Depends(get_db)):
    longarinas = db.query(LongarinaModel).all()
    funcoes = db.query(FuncaoLongarinaModel).all()

    service = CalculatorService(db)
    resultados_por_perfil = {}

    for longarina in longarinas:
        perfil = longarina.Perfil
        try:
            resultados_raw = service.calculate_for_perfil(perfil, parametros={})
            resultados = {
                nome: {"valor": valor, "unidade": unidade}
                for nome, (valor, unidade) in resultados_raw.items()
                if isinstance(valor, (int, float))
            }
            resultados_por_perfil[perfil] = resultados
        except Exception:
            resultados_por_perfil[perfil] = {}

    return templates.TemplateResponse(
        "index.html",  # Corrigido aqui
        {
            "request": request,
            "longarinas": longarinas,
            "funcoes": funcoes,
            "resultados_por_perfil": resultados_por_perfil
        }
    )

@app.get("/executar/{perfil}")
def executar_equacoes_por_perfil(perfil: str, db: Session = Depends(get_db)):
    eqs = db.execute(
        text("SELECT NomeEquacao, Equacao FROM funcoes_longarinas WHERE Perfil = :perfil"),
        {"perfil": perfil}
    ).fetchall()
    dados = db.execute(
        text("SELECT * FROM longarinas WHERE Perfil = :perfil"),
        {"perfil": perfil}
    ).fetchone()
    if not dados:
        return JSONResponse(status_code=404, content={"erro": "Perfil não encontrado"})

    variaveis = dict(dados._mapping)
    from asteval import Interpreter
    ae = Interpreter()
    ae.symtable.update(variaveis)  # <- aqui está o correto
    resultados = {}
    for nome, expr in eqs:
        try:
            resultados[nome] = ae.eval(expr)
        except Exception as e:
            resultados[nome] = f"Erro: {e}"

    return {"perfil": perfil, "resultados": resultados}


@app.get("/resultados/{perfil}", response_class=HTMLResponse)
def exibir_resultados_perfil(perfil: str, request: Request, db: Session = Depends(get_db)):
    service = CalculatorService(db)
    try:
        resultados = service.calculate_for_perfil(perfil, parametros={})
    except ValueError as e:
        raise HTTPException(status_code=404, detail=str(e))

    return templates.TemplateResponse(
        "resultados.html",
        {
            "request": request,
            "perfil": perfil,
            "resultados": resultados
        }
    )

@app.get("/", include_in_schema=False)
def root():
    return RedirectResponse(url="/longarinas")