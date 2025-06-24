// Projeto: Gerenciador de Produtos
// Descrição: Implementa o Value Object DataCriacao
// Data de Criação: 2025-06-19
// Autor: Thayne Pacheco Valério
// Repositório: ISA

// ✅ Representa o nome da variável (comp, alt,prof, etc.)
// ✅ Recebe um texto que representa o valor, só pode ser composto por: loetras, números e underlines
// ✅ Preenchimento obrigatório'
// ✅Implementa Equals, GetHashCode e ToString para comparação e exibição.


using System.Text.RegularExpressions;

namespace GerenciadorDeProdutos.DomainShared.ValueObjects
{
    public class VariavelNome
    {
        private static readonly Regex NomeValidoRegex = new(@"^[a-zA-Z_][a-zA-Z0-9_]*$");

        public string Valor { get; }

        public VariavelNome(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new ArgumentException("Nome da variável não pode ser nulo ou vazio.", nameof(valor));

            if (!NomeValidoRegex.IsMatch(valor))
                throw new ArgumentException("Nome da variável inválido. Deve começar com letra ou underscore e conter apenas letras, números e underscores.");

            Valor = valor;
        }

        public override bool Equals(object? obj)
        {
            return obj is VariavelNome outro && Valor == outro.Valor;
        }

        public override int GetHashCode() => Valor.GetHashCode();

        public override string ToString() => Valor;
    }
}
