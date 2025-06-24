// Projeto: Gerenciador de Produtos
// Descrição: Implementa o Value Object DataCriacao
// Data de Criação: 2025-06-19
// Autor: Thayne Pacheco Valério
// Repositório: ISA

// ✅ Profundidade opcional
// ✅ Representa valor numérico em milímetros
// ✅ Pode receber valores quebrados (ex: 1.5, 2.75)
// ✅ Valida que, se preenchida, deve ser maior que zero
// ✅Implementa Equals, GetHashCode e ToString para comparação e exibição.

using System;

namespace GerenciadorDeProdutos.DomainShared.ValueObjects
{
public class Profundidade
{
    public double? Valor { get; }

    public Profundidade(double? valor)
    {
        if (valor.HasValue && valor <= 0)
            throw new ArgumentException("Profundidade deve ser maior que zero.");

        Valor = valor;
    }

    public bool EstaPreenchida => Valor.HasValue;

    public override bool Equals(object? obj)
    {
        return obj is Profundidade outra && Valor == outra.Valor;
    }

    public override int GetHashCode() => Valor.HasValue ? Valor.GetHashCode() : 0;

    public override string ToString() => Valor.HasValue ? $"{Valor} mm" : string.Empty;
}
}
