# calculadoras/models/longarina_model.py

from sqlalchemy import Column, String, Float, Integer
from calculadoras.core.database import Base

class LongarinaModel(Base):
    __tablename__ = "longarinas"

    # Chave primária: perfil (string única)
    Perfil                         = Column(String(50), primary_key=True, index=True)

    # Demais colunas, todas do tipo numérico
    PesoLinear                           = Column(Float, nullable=False)
    AreaSecaoTransversal                 = Column(Float, nullable=False)
    Espessura                            = Column(Float, nullable=False)
    MomentoInerciaX                      = Column(Float, nullable=False)
    RaioGiracaoX                         = Column(Float, nullable=False)
    DistanciaTorcaoCentroideX            = Column(Float, nullable=False)
    MomentoInerciaY                      = Column(Float, nullable=False)
    ModuloResistenciaElasticoSecaoX      = Column(Float, nullable=False)
    RaioGiracaoY                         = Column(Float, nullable=False)
    DistanciaTorcaoCentroideY            = Column(Float, nullable=False)
    LarguraNominalAlma                   = Column(Float, nullable=False)
    LarguraNominalMesa                   = Column(Float, nullable=False)
    ConstanteTorcao                      = Column(Float, nullable=False)
    ConstanteEmpenamentoSecaoTransversal = Column(Float, nullable=False)
    Altura                               = Column(Float, nullable=False)
    RigidezConectorColunaLongarina       = Column(Float, nullable=False)
    Simetria                             = Column(String, nullable=False)
    Secao                                = Column(String, nullable=False)
    
