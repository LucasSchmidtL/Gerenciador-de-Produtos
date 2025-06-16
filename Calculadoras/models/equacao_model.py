# calculadoras/models/equacao_model.py
from sqlalchemy import Column, String, Text
from ..core.database import Base

class EquacaoModel(Base):
    __tablename__ = 'equacoes'

    nome_equacao   = Column("NomeEquacao", String(255), primary_key=True, index=True)
    equacao        = Column("Equacao", Text, nullable=False)
    unidade        = Column("Unidade", String(50), nullable=True)
    abreviacao     = Column("Abreviacao", String(50), nullable=True)
    norma          = Column("Norma", String(100), nullable=True)
    componente     = Column("Componente", String(100), nullable=True)
    secao          = Column("Secao", String(100), nullable=True)
    consideracoes  = Column("Consideracoes", Text, nullable=True)