import csv
from pathlib import Path

def carregar_parametros_fixos(caminho_csv: Path = Path(__file__).parent.parent / "database" / "parametros.csv") -> dict:
    """
    Lê o arquivo parametros.csv e retorna um dicionário com os parâmetros fixos.
    Espera um CSV com cabeçalhos: parametro,valor
    """
    parametros = {}
    try:
        with open(caminho_csv, newline='', encoding='utf-8-sig') as f:
            reader = csv.DictReader(f)
            for row in reader:
                chave = row['parametro']
                try:
                    valor = float(row['valor'])
                except ValueError:
                    raise ValueError(f"Valor inválido para o parâmetro '{chave}': {row['valor']}")
                parametros[chave] = valor
    except FileNotFoundError:
        raise FileNotFoundError(f"Arquivo {caminho_csv} não encontrado.")
    return parametros


def solicitar_parametros_dinamicos(campos: list[str]) -> dict:
    """
    Solicita ao usuário a entrada dos valores para os parâmetros listados em 'campos'.
    """
    parametros = {}
    for campo in campos:
        while True:
            try:
                valor = float(input(f"Digite o valor de {campo}: "))
                parametros[campo] = valor
                break
            except ValueError:
                print(f"Valor inválido para {campo}. Por favor, insira um número.")
    return parametros
