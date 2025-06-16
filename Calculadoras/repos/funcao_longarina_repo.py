#calculadoras/repos/funcao_longarina_repo.py
from sqlalchemy.orm import Session
from calculadoras.models.funcoes_longarinas_model import FuncaoLongarinaModel

class FuncaoLongarinaRepo:
    def __init__(self, session: Session):
        self.session = session

    def get_all(self) -> list[FuncaoLongarinaModel]:
        return self.session.query(FuncaoLongarinaModel).all()

    def get_by_nome(self, nome: str) -> FuncaoLongarinaModel | None:
        return (
            self.session
            .query(FuncaoLongarinaModel)
            .filter(FuncaoLongarinaModel.nome == nome)
            .first()
        )
