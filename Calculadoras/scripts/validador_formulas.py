import re
import pyodbc
from asteval import Interpreter

# CONFIGURAÇÃO DE CONEXÃO
SERVIDOR = "02-3168"
BANCO = "isa_calculadora"
CONEXAO_STR = f"DRIVER={{SQL Server}};SERVER={SERVIDOR};DATABASE={BANCO};Trusted_Connection=yes"

# Nomes de tabela e colunas
TABELA_FUNCOES = "funcoes_longarinas"
COLUNA_ID = "id"
COLUNA_FORMULA = "formula"
TABELA_VARIAVEIS = "dbo.longarinas"

# Regex para capturar nomes de variáveis
def extrair_variaveis(formula: str) -> set:
    palavras = set(re.findall(r'\b[a-zA-Z_]\w*\b', formula))
    funcoes_comuns = {"sin", "cos", "tan", "sqrt", "log", "exp", "abs", "pi", "pow", "min", "max"}
    palavras_reservadas = {"if", "else", "and", "or", "not", "in"}
    return palavras - funcoes_comuns - palavras_reservadas

def verificar_variaveis_e_sintaxe():
    conn = pyodbc.connect(CONEXAO_STR)
    cursor = conn.cursor()

    # Obter variáveis válidas da tabela dbo.longarinas
    cursor.execute(f"SELECT TOP 1 * FROM {TABELA_VARIAVEIS}")
    colunas_validas = {column[0] for column in cursor.description}

    # Obter fórmulas
    cursor.execute(f"SELECT {COLUNA_ID}, {COLUNA_FORMULA} FROM {TABELA_FUNCOES}")
    linhas = cursor.fetchall()

    variaveis_usadas_total = set()
    aeval = Interpreter()

    print("⏳ Verificando fórmulas...\n")

    for id_funcao, formula in linhas:
        usadas = extrair_variaveis(formula)
        variaveis_usadas_total.update(usadas)

        faltando = usadas - colunas_validas
        if faltando:
            print(f"[ID {id_funcao}] ❌ Variáveis não encontradas em '{TABELA_VARIAVEIS}':")
            for var in sorted(faltando):
                print(f"  - {var}")
            print(f"→ Fórmula: {formula}\n")

        # Validação de sintaxe
        try:
            # Fornece valores fictícios para avaliação
            contexto_fake = {var: 1 for var in colunas_validas}
            aeval.symtable.update(contexto_fake)
            aeval(formula)
            if aeval.error:
                print(f"[ID {id_funcao}] ⚠️ Erro de sintaxe na fórmula:")
                for err in aeval.error:
                    print(f"  - {err.get_error()}")
                print(f"→ Fórmula: {formula}\n")
            aeval.error = []  # limpa os erros acumulados
        except Exception as e:
            print(f"[ID {id_funcao}] ❌ Erro ao avaliar fórmula: {e}")
            print(f"→ Fórmula: {formula}\n")

    # Verificar variáveis nunca usadas em nenhuma fórmula
    nunca_usadas = colunas_validas - variaveis_usadas_total
    if nunca_usadas:
        print("\n🔎 Variáveis da tabela que **nunca foram usadas** em nenhuma fórmula:")
        for var in sorted(nunca_usadas):
            print(f"  - {var}")

    conn.close()

# Executar
verificar_variaveis_e_sintaxe()
