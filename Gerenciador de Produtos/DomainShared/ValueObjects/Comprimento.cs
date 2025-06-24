// Projeto: Gerenciador de Produtos
// Descrição: Implementa o Value Object DataCriacao
// Data de Criação: 2025-06-19
// Autor: Thayne Pacheco Valério
// Repositório: ISA


//✅ Recebe um número representando um comprimento em milimetros;
//✅ Tipo numérico (double, por exemplo)
//✅ Pode ser opcional(aceita null)
//✅ Pode receber valores quebrados (ex: 1.5, 2.75, etc.)
//✅Implementa Equals, GetHashCode e ToString para comparação e exibição.

using System;

namespace GerenciadorDeProdutos.DomainShared.ValueObjects
{
    public class Comprimento
    {
        public double? Valor { get; }

        public Comprimento(double? valor)
        {
            if (valor.HasValue && valor <= 0)
                throw new ArgumentException("Comprimento deve ser maior que zero.");

            Valor = valor;
        }

        public bool EstaPreenchido => Valor.HasValue;

        public override bool Equals(object? obj)
        {
            return obj is Comprimento outro && Valor == outro.Valor;
        }

        public override int GetHashCode() => Valor.HasValue ? Valor.GetHashCode() : 0;

        public override string ToString() => Valor.HasValue ? $"{Valor} mm" : string.Empty;
    }
}