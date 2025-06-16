# calculadoras/repos/equacao_repo.py
from typing import Dict
from sqlalchemy.orm import Session

# Importa o modelo SQLAlchemy (não o dataclass antigo)
from ..models.equacao_model import EquacaoModel
from ..core.database import get_db_connection

class EquacaoRepoSQLAlchemy:
    """
    Repositório de Equações usando SQLAlchemy para interação com banco de dados.
    """
    def __init__(self, session: Session):
        self.session = session

    def listar_todas(self):
        return self.session.query(EquacaoModel).all()

    def buscar_por_nome(self, nome: str):
        return self.session.query(EquacaoModel).filter_by(nome=nome).first()
        
    # devolve lista – é o que o CalculatorService espera
    def get_all(self) -> list[EquacaoModel]:
        return self.session.query(EquacaoModel).all()

    # (opcional) se quiser também ter a versão em dicionário:
    def get_all_dict(self) -> dict[str, EquacaoModel]:
        result = self.session.query(EquacaoModel).all()
        return {e.nome_equacao: e for e in result}
