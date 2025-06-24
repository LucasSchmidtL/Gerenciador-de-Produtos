// Projeto: Gerenciador de Produtos
// Descrição: Implementa o Value Object DataCriacao
// Data de Criação: 2025-06-19
// Autor: Thayne Pacheco Valério
// Repositório: ISA

// ✅ Representa o estado do produto (Ativo, Inativo, Obsoleto, Indefinido)
// ✅ Recebe um texto que representa o status, tem uma lista de status válidos
// ✅ Preenchimento obrigatório'
// ✅Implementa Equals, GetHashCode e ToString para comparação e exibição.

using System;

namespace GerenciadorDeProdutos.DomainShared.ValueObjects
{
    public class Status
    {
        public static readonly Status Ativo = new("Ativo");
        public static readonly Status Inativo = new("Inativo");
        public static readonly Status Inativo = new("obsoleto");
        public static readonly Status Inativo = new("Indefinido");

        private static readonly List<Status> Todos = new() { Ativo, Inativo, obsoleto, Indefinido };

        public string Valor { get; }

        private Status(string valor)
        {
            Valor = valor;
        }

        public static Status Criar(string valor)
        {
            var statusExistente = Todos.FirstOrDefault(s => s.Valor.Equals(valor, StringComparison.OrdinalIgnoreCase));

            if (statusExistente == null)
                throw new ArgumentException($"Status inválido. Os valores válidos são: {string.Join(", ", Todos.Select(s => s.Valor))}");

            return statusExistente;
        }

        public static IEnumerable<Status> ObterTodos() => Todos.AsReadOnly();

        public bool EhAtivo() => this == Ativo;
        public bool EhInativo() => this == Inativo;

        public override bool Equals(object? obj) => obj is Status outro && Valor.Equals(outro.Valor, StringComparison.OrdinalIgnoreCase);

        public override int GetHashCode() => Valor.ToLowerInvariant().GetHashCode();

        public override string ToString() => Valor;
    }
}
