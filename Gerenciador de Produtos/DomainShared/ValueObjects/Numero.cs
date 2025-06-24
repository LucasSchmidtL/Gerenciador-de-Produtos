// Projeto: Gerenciador de Produtos
// Descrição: Implementa o Value Object DataCriacao
// Data de Criação: 2025-06-19
// Autor: Thayne Pacheco Valério
// Repositório: ISA

//✅ Recebe um texto;
//✅ Preenchimento obrigatório;
//✅ Recebe String, não pode ser nulo ou vazio;
//✅Implementa Equals, GetHashCode e ToString para comparação e exibição.

using System;

namespace GerenciadorDeProdutos.DomainShared.ValueObjects
{
    public class Numero
    {
        public int Valor { get; }

        public Numero(int valor)
        {
            // Se quiser validar algum valor específico, adicione aqui.
            Valor = valor;
        }

        public override bool Equals(object? obj)
        {
            return obj is Numero outro && Valor == outro.Valor;
        }

        public override int GetHashCode() => Valor.GetHashCode();

        public override string ToString() => Valor.ToString();
    }
}