// Projeto: Gerenciador de Produtos
// Descrição: Implementa o Value Object DataCriacao
// Data de Criação: 2025-06-19
// Autor: Thayne Pacheco Valério
// Repositório: ISA
 
// ✅Representa uma Tag que pode ser associada a um ItemERP.
// ✅A identidade da Tag é baseada no Nome.


using GerenciadorDeProdutos.DomainShared.ValueObjects;
using System;


namespace GerenciadorDeProdutos.Domain.ItemERP.Entities
{
    /// <summary>
    /// Representa uma Tag utilizada para categorizar ItemERP.
    /// A identidade da Tag é definida pelo Nome.
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// Identificador único da Tag.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Nome da Tag. Não pode ser nulo ou vazio.
        /// </summary>
        public Nome Nome { get; }

        /// <summary>
        /// Cria uma nova instância de Tag.
        /// </summary>
        /// <param name="id">Identificador da Tag.</param>
        /// <param name="nome">Nome da Tag.</param>
        /// <exception cref="ArgumentNullException">Lançada se o nome for nulo.</exception>
        public Tag(int id, Nome nome)
        {
            Id = id;
            Nome = nome ?? throw new ArgumentNullException(nameof(nome));
        }

        /// <summary>
        /// Verifica se esta Tag é igual a outra, comparando pelo Nome.
        /// </summary>
        public override bool Equals(object? obj)
        {
            return obj is Tag outra && Nome.Equals(outra.Nome);
        }

        /// <summary>
        /// Gera o código hash baseado no Nome da Tag.
        /// </summary>
        public override int GetHashCode() => Nome.GetHashCode();

        /// <summary>
        /// Retorna uma representação textual da Tag.
        /// </summary>
        public override string ToString() => $"{Id} - {Nome}";
    }
}

