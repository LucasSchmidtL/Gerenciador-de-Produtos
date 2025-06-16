import os
import pandas as pd
from sqlalchemy import create_engine, text

BASE_DIR = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
CSV_FOLDER = os.path.join(BASE_DIR,  "database")

SERVER = "02-3168"
DATABASE = "isa_calculadora"

def main():
    table_name = input("Digite o nome da tabela (sem .csv): ").strip()
    csv_path = os.path.join(CSV_FOLDER, f"{table_name}.csv")

    if not os.path.exists(csv_path):
        print(f"❌ Arquivo '{csv_path}' não encontrado.")
        return

    df = pd.read_csv(csv_path)

    connection_string = (
        f"mssql+pyodbc://@{SERVER}/{DATABASE}?driver=ODBC+Driver+17+for+SQL+Server&trusted_connection=yes"
    )
    engine = create_engine(connection_string)

    drop_sql = f"IF OBJECT_ID('dbo.{table_name}', 'U') IS NOT NULL DROP TABLE dbo.{table_name};"

    with engine.begin() as conn:  # Usar begin para garantir commit
        conn.execute(text(drop_sql))
        print(f"✅ Tabela '{table_name}' antiga (se existia) foi excluída.")

        df.to_sql(table_name, con=conn, schema='dbo', if_exists='replace', index=False)
        print(f"✅ Nova tabela '{table_name}' criada com sucesso a partir de '{csv_path}'.")

if __name__ == "__main__":
    main()
