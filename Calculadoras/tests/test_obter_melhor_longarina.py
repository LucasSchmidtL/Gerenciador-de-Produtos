import types
import pytest

from calculadoras.services.calculator_service import CalculatorService

# --------------------------------------------------
#  Dummy models & repos
# --------------------------------------------------
class DummyLongarina:
    def __init__(self, perfil: str, peso_linear: float):
        self.Perfil = perfil
        self.perfil = perfil  # compatibilidade
        self.PesoLinear = peso_linear
        # quaisquer outros atributos reais…

class DummyRepo:
    def __init__(self, objs):
        self._objs = objs

    def get_all(self):
        return self._objs

    def get_all_perfis(self):
        return [o.Perfil for o in self._objs]

    def get_by_perfil(self, perfil: str):
        for o in self._objs:
            if o.Perfil == perfil:
                return o
        return None

# --------------------------------------------------
#  Fixture de serviço isolado (sem BD real)
# --------------------------------------------------
@pytest.fixture
def service(monkeypatch):
    svc = CalculatorService(session=types.SimpleNamespace())

    # Mocks vazios para equação/função (nenhum cálculo simbólico necessário)
    monkeypatch.setattr(svc, "equacao_repo", DummyRepo([]))
    monkeypatch.setattr(svc, "funcao_repo", DummyRepo([]))

    # Três longarinas – duas válidas, uma invalidada
    longarinas = [
        DummyLongarina("P1", 1.5),  # válida e menor
        DummyLongarina("P2", 2.0),  # válida mas maior
        DummyLongarina("P3", 1.2),  # falta algo → será filtrada na lógica, dependendo do contexto
    ]
    monkeypatch.setattr(svc, "longarina_repo", DummyRepo(longarinas))

    return svc

# --------------------------------------------------
#  Teste principal
# --------------------------------------------------

def test_obter_melhor_longarina(service):
    codigo = service.obter_melhor_longarina(parametros={})
    assert codigo == "P3"
