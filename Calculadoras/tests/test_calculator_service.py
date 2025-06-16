# calculadoras/tests/test_calculator_service.py

import pytest
from calculadoras.services.calculator_service import CalculatorService

@ pytest.fixture
def service():
     return CalculatorService()

def test_calculate_using_profile(service):
    # Testa dimensionar_colunas usando calculate_for_perfil para fornecer todo o contexto
    resultados = service.calculate_for_perfil(
        "Perfil1",
    parametros_dinamicos={
        "escoamento": 1.0,
        "comprimentoPlanoDestravado": 2300.0
    }
    )
    # Deve retornar o resultado de dimensionar_colunas com tipo correto
    valor, unidade = resultados.get("dimensionar_colunas", (None, None))
    assert isinstance(valor, float)
    assert isinstance(unidade, str)

def test_calculate_for_perfil_contains_equations(service):
     # inclui o parâmetro dinâmico 'escoamento'
     resultados = service.calculate_for_perfil(
         "Perfil1",
         parametros_dinamicos={
              "escoamento": 1.0,
              "comprimentoPlanoDestravado": 2300.0,
         }
     )
     assert isinstance(resultados, dict)
     # Deve conter ao menos a função dimensionar_colunas
     assert "dimensionar_colunas" in resultados
     val, uni = resultados["dimensionar_colunas"]
     assert isinstance(val, float)
     assert isinstance(uni, str)