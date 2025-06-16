# calculadoras/models/funcao_longarina_model.py

from sqlalchemy import Column, Integer, String, Text
from calculadoras.core.database import Base

class FuncaoLongarinaModel(Base):
    __tablename__ = "funcoes_longarinas"

    # Se quiser um id numérico auto-increment:
    nome           = Column("nome", String(255), primary_key=True, index=True)
    formula        = Column("formula", Text, nullable=False)
    unidade        = Column("Unidade", String(50), nullable=True)
    abreviacao     = Column("Abreviacao", String(50), nullable=True)
    norma          = Column("Norma", String(100), nullable=True)
    secao          = Column("Secao", String(100), nullable=True)
    consideracoes  = Column("Consideracoes", Text, nullable=True)