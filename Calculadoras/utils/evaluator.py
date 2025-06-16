from asteval import Interpreter
import math
import re

class Evaluador:
    def __init__(self):
        # Cria o interpretador seguro do asteval
        self.aeval = Interpreter()
        # Carrega as funções matemáticas e constantes no symtable
        self._carregar_funcoes()

    def _carregar_funcoes(self):
        # Dicionário com as funções permitidas
        funcoes = {
            'sin': math.sin,
            'cos': math.cos,
            'tan': math.tan,
            'log': math.log,
            'sqrt': math.sqrt,
            'exp': math.exp,
            'abs': abs,
            'pow': pow,
            'round': round,
        }
        # Atualiza o symtable do interpretador com essas funções
        self.aeval.symtable.update(funcoes)
        # Adiciona constantes importantes
        self.aeval.symtable['pi'] = math.pi
        self.aeval.symtable['e'] = math.e
        # Guarda o conjunto de nomes válidos (funções + constantes)
        self._funcoes = set(funcoes) | {'pi', 'e'}

    def _limpar_variaveis(self):
        """
        Remove do symtable todas as variáveis que não sejam funções
        ou constantes carregadas inicialmente. Garante que chamadas
        anteriores não vazem para a próxima avaliação.
        """
        for chave in list(self.aeval.symtable):
            if chave not in self._funcoes:
                del self.aeval.symtable[chave]

    def avaliar(self, formula: str, parametros: dict) -> float:
        """
        Avalia a expressão (string) passada em `formula`, usando os
        valores do dicionário `parametros` como variáveis.
        Retorna o resultado arredondado a 2 casas.
        """

        # 1) Limpa qualquer variável antiga
        self._limpar_variaveis()

        # 2) Insere os parâmetros (variáveis) no symtable
        self.aeval.symtable.update(parametros)

        # 3) Validação leve de variáveis:
        #    - Extrai todos os tokens que parecem nomes de variáveis/funções
        tokens = set(re.findall(r"\b[a-zA-Z_][a-zA-Z0-9_]*\b", formula))
        #    - Calcula quais desses tokens NÃO estão em funcoes/constantes
        vars_nao_func = tokens - self._funcoes
        #    - Identifica quais desse não-funções NÃO foram fornecidos
        faltando = [v for v in vars_nao_func if v not in parametros]
        if faltando:
            # Se faltar algum parâmetro, aborta com erro claro
            raise ValueError(f"Parâmetros ausentes: {', '.join(faltando)}")

        # 4) Avalia a expressão
        resultado = self.aeval(formula)

        # 5) Se o interpretador registrou algum erro interno, relança
        if self.aeval.error:
            raise ValueError(self.aeval.error[0].get_error())

        # 6) Se for número, arredonda; senão, retorna tal qual
        if isinstance(resultado, (int, float)):
            return round(resultado, 2)
        else:
            raise ValueError(f"Resultado inválido: {resultado}")

