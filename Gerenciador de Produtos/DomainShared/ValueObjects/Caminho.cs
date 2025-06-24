// Projeto: Gerenciador de Produtos
// Descrição: Implementa o Value Object DataCriacao
// Data de Criação: 2025-06-19
// Autor: Thayne Pacheco Valério
// Repositório: ISA

//✅ Representa um caminho de arquivo
//✅ Campo opcional → Pode receber null ou string vazia
//✅ Validação: Se for preenchido, tem que ser um caminho válido(sem caracteres inválidos)
//✅ Não precisa verificar existência física do arquivo (a menos que você queira)
//✅Implementa Equals, GetHashCode e ToString para comparação e exibição.

using System;
using System.IO;

namespace GerenciadorDeProdutos.DomainShared.ValueObjects
{
    public class Caminho
    {
        public string? Valor { get; }

        public Caminho(string? valor)
        {
            if (!string.IsNullOrWhiteSpace(valor))
            {
                if (valor.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
                    throw new ArgumentException("O caminho contém caracteres inválidos.");
            }

            Valor = valor;
        }

        public bool EstaPreenchido => !string.IsNullOrWhiteSpace(Valor);

        public override bool Equals(object? obj)
        {
            return obj is Caminho outro && Valor == outro.Valor;
        }

        public override int GetHashCode() => Valor?.GetHashCode() ?? 0;

        public override string ToString() => Valor ?? string.Empty;
    }
}