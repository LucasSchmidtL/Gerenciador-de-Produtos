// Projeto: Gerenciador de Produtos
// Descrição: Implementa o Value Object DataCriacao
// Data de Criação: 2025-06-19
// Autor: Thayne Pacheco Valério
// Repositório: ISA

//✅ Tipo texto 
//✅ Pode ser opcional(aceita null)
//✅Implementa Equals, GetHashCode e ToString para comparação e exibição.

using System;

namespace GerenciadorDeProdutos.DomainShared.ValueObjects
{
    public class AprovadoPor
    {
        public string? Valor { get; }

        public AprovadoPor(string? valor)
        {
            Valor = valor;
        }

        public override bool Equals(object? obj)
        {
            return obj is AprovadoPor outro && Valor == outro.Valor;
        }

        public override int GetHashCode() => (Valor ?? string.Empty).GetHashCode();

        public override string ToString() => Valor ?? string.Empty;
    }
}