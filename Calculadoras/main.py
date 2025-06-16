import logging
from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from fastapi.staticfiles import StaticFiles
from calculadoras.api.endpoints.resultados import router as resultados_router

from calculadoras.api.endpoints import calculo, equacoes, longarinas, resultados
from calculadoras.core.templates import templates

# Configuração básica do logging (apenas uma vez, no entrypoint)
logging.basicConfig(
    level=logging.DEBUG,
    format="%(asctime)s | %(levelname)s | %(name)s | %(message)s",
    handlers=[
        logging.StreamHandler()
    ]
)

logger = logging.getLogger(__name__)
logger.info("Iniciando aplicação FastAPI")

app = FastAPI()
app.include_router(resultados_router)

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

app.mount("/static", StaticFiles(directory="calculadoras/static"), name="static")

# Registro dos endpoints
app.include_router(calculo.router)
app.include_router(equacoes.router)
app.include_router(longarinas.router)
app.include_router(resultados.router)

logger.info("Endpoints registrados com sucesso")
