# calculadoras/schemas.py
from pydantic import BaseModel, Field
from typing import Dict, List, Optional, Union


class CalcRequest(BaseModel):
    variaveis : Dict[str, float] = Field(default_factory=dict)
    parametros: Dict[str, float] = Field(default_factory=dict)

class CalcPerfilRequest(BaseModel):
    perfil: str
    variaveis: Dict[str, float] = Field(default_factory=dict)
    parametros: Dict[str, float] = Field(default_factory=dict)



# ────────────────────────────
#  EQUAÇÕES (vêm do banco)
# ────────────────────────────
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


# ────────────────────────────
#  EXECUÇÃO DINÂMICA (GET /executar)
# ────────────────────────────
class ExecucaoEquacoesResponse(BaseModel):
    perfil: str
    resultados: Dict[str, Union[float, str]]


# ────────────────────────────
#  RESULTADOS (usados no front)
# ────────────────────────────
class ResultadoItem(BaseModel):
    perfil: str            # vem do banco (LongarinaModel.Perfil)
    peso_linear: float     # idem
    flt: float | str       # calculado
    flecha: float | str
    situacao: str
    m_drlflt: float | str
    m_drdelta: float | str


class CalcResponse(BaseModel):
    codigo_longarina: str = ""
    resultados: List[ResultadoItem]


# ────────────────────────────
#  TESTE / DEBUG (opcional)
# ────────────────────────────
class CalculoInput(BaseModel):
    # se ainda precisar desse schema em algum teste interno:
    #perfil: str
    LongarinaComprimento: float
    PesoSuportado: float
    acessorios: str
    parametros: Dict[str, float]
