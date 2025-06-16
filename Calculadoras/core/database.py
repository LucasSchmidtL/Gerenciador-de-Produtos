# calculadoras/core/database.py
from sqlalchemy import create_engine
from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy.orm import sessionmaker
from contextlib import contextmanager

# Substitua pelo nome do seu servidor, banco e driver
DATABASE_URL = (
    "mssql+pyodbc://localhost/isa_calculadora?driver=ODBC+Driver+17+for+SQL+Server&Trusted_Connection=yes"
)

engine = create_engine(DATABASE_URL)
SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)

Base = declarative_base()

# Função para criar uma conexão com o banco de dados
@contextmanager
def get_db_connection():
    db = SessionLocal()
    try:
        yield db
    finally:
        db.close()
