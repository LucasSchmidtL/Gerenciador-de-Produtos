// Projeto: Gerenciador de Produtos
// Descrição: Implementa o Value Object DataCriacao
// Data de Criação: 2025-06-19
// Autor: Thayne Pacheco Valério
// Repositório: ISA


//✅A classe Descricao representa uma descrição textual.
//✅Ela garante que o valor não seja nulo ou vazio.
//✅Implementa Equals, GetHashCode e ToString para comparação e exibição.

using System;

namespace GerenciadorDeProdutos.DomainShared.ValueObjects
{

    public class Descricao
    {
        public string Valor { get; }

        public Descricao(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new ArgumentException("A descrição não pode ser vazia.");

            Valor = valor;
        }

        public override bool Equals(object obj)
        {
            return obj is Descricao outro && Valor == outro.Valor;
        }

        public override int GetHashCode() => Valor.GetHashCode();

        public override string ToString() => Valor;
    }
}