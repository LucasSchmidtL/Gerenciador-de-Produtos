// Projeto: Gerenciador de Produtos
// Descrição: Implementa o Value Object DataCriacao
// Data de Criação: 2025-06-19
// Autor: Thayne Pacheco Valério
// Repositório: ISA

//✅ Tipo numérico (double, por exemplo)
//✅ Pode ser opcional(aceita null)
//✅ Pode receber valores quebrados (ex: 1.5, 2.75, etc.)
//✅Implementa Equals, GetHashCode e ToString para comparação e exibição.

using System;

namespace GerenciadorDeProdutos.DomainShared.ValueObjects
{
    public class Altura
    {
        public double? Valor { get; }

        public Altura(double? valor)
        {
            if (valor.HasValue && valor <= 0)
                throw new ArgumentException("Altura deve ser maior que zero.");

            Valor = valor;
        }

        public bool EstaPreenchida => Valor.HasValue;

        public override bool Equals(object? obj)
        {
            return obj is Altura outra && Valor == outra.Valor;
        }

        public override int GetHashCode() => Valor.HasValue ? Valor.GetHashCode() : 0;

        public override string ToString() => Valor.HasValue ? $"{Valor} m" : string.Empty;
    }
}
