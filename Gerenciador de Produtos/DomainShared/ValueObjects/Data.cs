// Projeto: Gerenciador de Produtos
// Descrição: Implementa o Value Object DataCriacao
// Data de Criação: 2025-06-19
// Autor: Thayne Pacheco Valério
// Repositório: ISA


//✅ Representa uma data simples (exemplo: data de criação, data de aprovação, etc.)
//✅ Campo opcional → pode aceitar null
//✅ Se informado, deve ser uma data válida
//✅Implementa Equals, GetHashCode e ToString para comparação e exibição.

using System;

namespace GerenciadorDeProdutos.DomainShared.ValueObjects
{
    public class Data
    {
        public DateTime? Valor { get; }

        public Data(DateTime? valor)
        {
            Valor = valor;
        }

        public bool EstaPreenchida => Valor.HasValue;

        public override bool Equals(object? obj)
        {
            return obj is Data outra && Valor == outra.Valor;
        }

        public override int GetHashCode() => Valor.HasValue ? Valor.GetHashCode() : 0;

        public override string ToString() => Valor?.ToString("yyyy-MM-dd") ?? string.Empty;
    }
}
