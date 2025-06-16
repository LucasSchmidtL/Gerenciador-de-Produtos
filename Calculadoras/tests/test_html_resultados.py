import re
from fastapi.testclient import TestClient
from calculadoras.main import app            # ajuste para seu módulo real
from calculadoras.services.calculator_service import CalculatorService
import types

# ------------------------------------------------------------------
# Dummy repos iguais aos que usamos no teste anterior
# ------------------------------------------------------------------
class DummyLongarina:
    def __init__(self, perfil, peso):
        self.Perfil = perfil
        self.perfil = perfil
        self.PesoLinear = peso

class DummyRepo:
    def __init__(self, objs):
        self._objs = objs
    def get_all(self):
        return self._objs
    def get_all_perfis(self):
        return [o.Perfil for o in self._objs]
    def get_by_perfil(self, perfil):
        for o in self._objs:
            if o.Perfil == perfil:
                return o
        return None

# ------------------------------------------------------------------
# Teste
# ------------------------------------------------------------------
def test_html_resultados(monkeypatch):
    client = TestClient(app)

    # --- Monkey-patch dos repositórios ---
    svc = CalculatorService(session=types.SimpleNamespace())
    longarinas = [
        DummyLongarina("P1", 1.5),
        DummyLongarina("P2", 1.2),   # menor peso
    ]
    monkeypatch.setattr(svc, "longarina_repo", DummyRepo(longarinas))
    monkeypatch.setattr(svc, "equacao_repo", DummyRepo([]))
    monkeypatch.setattr(svc, "funcao_repo", DummyRepo([]))

    # Forçamos o endpoint a usar nosso service patchado
    monkeypatch.setattr(
        "calculadoras.api.endpoints.resultados.CalculatorService",  # caminho real
        lambda db: svc
    )

    resp = client.get("/resultados/P1")   # qualquer perfil de entrada
    assert resp.status_code == 200

    # HTML deve conter value=\"P2\" – menor peso linear
    assert re.search(r'id=\"resultado\"[^>]*value=\"P2\"', resp.text), resp.text
