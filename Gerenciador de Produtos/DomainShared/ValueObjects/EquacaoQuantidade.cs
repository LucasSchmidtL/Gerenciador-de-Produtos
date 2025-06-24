// Projeto: Gerenciador de Produtos
// Descrição: Implementa o Value Object DataCriacao
// Data de Criação: 2025-06-19
// Autor: Thayne Pacheco Valério
// Repositório: ISA

//✅ Recebe um texto representando uma fórmula de Altura (exemplo: "2 * Comp + 5");
//✅ Agora o valor é opcional (pode ser nulo ou vazio).
//✅Implementa Equals, GetHashCode e ToString para comparação e exibição.

using System;

namespace GerenciadorDeProdutos.DomainShared.ValueObjects
{
    public class EquacaoQuantidade
    {
        public string? Valor { get; }

        public EquacaoQuantidade(string? valor)
        {
            // Valor pode ser nulo ou vazio, então não há validação aqui.
            Valor = valor;
        }

        public override bool Equals(object? obj)
        {
            return obj is EquacaoQuantidade outro && Valor == outro.Valor;
        }

        public override int GetHashCode() => (Valor ?? string.Empty).GetHashCode();

        public override string ToString() => Valor ?? string.Empty;
    }
}