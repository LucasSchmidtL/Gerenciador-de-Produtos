from abc import ABC, abstractmethod
from typing import Dict, Any

class EquacaoRepo(ABC):
    """
    Interface para repositórios de Equacao.
    Define o contrato: qualquer implementação deve fornecer todas as equações disponíveis.
    """

    @abstractmethod
    def get_all(self) -> Dict[str, Any]:
        """
        Retorna um dicionário mapeando o nome da equação (str) para a instância de Equacao ou modelo SQLAlchemy.
        """
        ...


class PerfilRepo(ABC):
    """
    Interface para repositórios de Perfil.
    Define o contrato: qualquer implementação deve fornecer dados de um perfil específico.
    """

    @abstractmethod
    def get(self, nome_perfil: str) -> Dict[str, float]:
        """
        Recebe o nome do perfil e retorna um dicionário com os parâmetros numéricos associados.
        """
        ...


class ParametrosFixosRepo(ABC):
    """
    Interface para repositórios de parâmetros fixos.
    Define o contrato: qualquer implementação deve disponibilizar todos os parâmetros fixos.
    """

    @abstractmethod
    def get_all(self) -> Dict[str, float]:
        """
        Retorna um dicionário com o nome de cada parâmetro (str) e seu valor (float).
        """
        ...

