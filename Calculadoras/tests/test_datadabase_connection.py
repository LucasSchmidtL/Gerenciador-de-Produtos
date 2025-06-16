from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker
from ..core.database import DATABASE_URL

# Teste da conexão com o banco
def test_database_connection():
    engine = create_engine(DATABASE_URL)
    SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)
    
    try:
        db = SessionLocal()
        # Tentando uma simples consulta para verificar a conexão
        result = db.execute("SELECT 1")
        assert result.fetchone() is not None
        print("Conexão bem-sucedida ao banco de dados!")
    except Exception as e:
        print(f"Erro ao conectar ao banco de dados: {e}")
    finally:
        db.close()
