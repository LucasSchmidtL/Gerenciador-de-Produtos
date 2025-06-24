// Projeto: Gerenciador de Produtos
// Descrição: Implementa o Value Object DataCriacao
// Data de Criação: 2025-06-19
// Autor: Thayne Pacheco Valério
// Repositório: ISA

// ✅ Representa o tipo da variável (int, float, boolean, string, etc.)
// ✅ Recebe um texto que representa o valor, tem uma lista de status válidos
// ✅ Preenchimento obrigatório'
// ✅Implementa Equals, GetHashCode e ToString para comparação e exibição.

using System;
using System.Collections.Generic;
using System.Linq;

namespace GerenciadorDeProdutos.DomainShared.ValueObjects
{
    public class VariavelTipo
    {
        public static readonly VariavelTipo Inteiro = new("Int");
        public static readonly VariavelTipo Decimal = new("Decimal");
        public static readonly VariavelTipo Booleano = new("Boolean");
        public static readonly VariavelTipo Texto = new("Text");
        public static readonly VariavelTipo Data = new("Date");
        public static readonly VariavelTipo DateTime = new("DateTime");
        public static readonly VariavelTipo Double = new("Double");

        private static readonly List<VariavelTipo> Todos = new()
        {
            Inteiro,
            Decimal,
            Booleano,
            Texto,
            Data,
            DateTime,
            Double
        };

        public string Valor { get; }

        private VariavelTipo(string valor)
        {
            Valor = valor;
        }

        public static VariavelTipo Criar(string valor)
        {
            var tipoExistente = Todos.FirstOrDefault(t => t.Valor.Equals(valor, StringComparison.OrdinalIgnoreCase));

            if (tipoExistente == null)
                throw new ArgumentException($"Tipo inválido. Os tipos válidos são: {string.Join(", ", Todos.Select(t => t.Valor))}");

            return tipoExistente;
        }

        public static IEnumerable<VariavelTipo> ObterTodos() => Todos.AsReadOnly();

        public bool EhInteiro() => this == Inteiro;
        public bool EhDecimal() => this == Decimal;
        public bool EhBooleano() => this == Booleano;
        public bool EhTexto() => this == Texto;
        public bool EhData() => this == Data;
        public bool EhDateTime() => this == DateTime;
        public bool EhDouble() => this == Double;

        public override bool Equals(object? obj) => obj is VariavelTipo outro && Valor.Equals(outro.Valor, StringComparison.OrdinalIgnoreCase);

        public override int GetHashCode() => Valor.ToLowerInvariant().GetHashCode();

        public override string ToString() => Valor;
    }
}
