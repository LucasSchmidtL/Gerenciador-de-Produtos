from sqlalchemy.orm import Session
from calculadoras.models.longarinas_model import LongarinaModel

class LongarinaRepo:
    def __init__(self, session: Session):
        self.session = session

    def get_all(self) -> list[LongarinaModel]:
        return self.session.query(LongarinaModel).all()

    def get_by_perfil(self, perfil: str) -> LongarinaModel | None:
        return (
            self.session
            .query(LongarinaModel)
            .filter(LongarinaModel.Perfil == perfil)
            .first()
        )

    def get_all_perfis(self) -> list[str]:
        return [l.Perfil for l in self.get_all()]
