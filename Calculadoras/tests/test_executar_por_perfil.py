import requests
import json

BASE_URL = "http://localhost:8000"

def test_executar_por_perfil():
    url = f"{BASE_URL}/executar_por_perfil"

    payload = {
        "perfil": "CAD 48 #1.50mm",  # Substitua por um perfil real do seu banco
        "variaveis": {
            "PesoSuportado": 1500,
            "LongarinaComprimento": 3.0
        },
        "parametros": {
            "Temperatura": 25
        }
    }

    headers = {
        "Content-Type": "application/json"
    }

    try:
        response = requests.post(url, data=json.dumps(payload), headers=headers)
        print(f"Status code: {response.status_code}")
        print("Resposta JSON:")
        print(json.dumps(response.json(), indent=2))
    except Exception as e:
        print("Erro ao fazer a requisição:")
        print(e)

if __name__ == "__main__":
    test_executar_por_perfil()
