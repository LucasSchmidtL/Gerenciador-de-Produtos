// Projeto: Gerenciador de Produtos
// Descrição: Implementa o Value Object DataCriacao
// Data de Criação: 2025-06-19
// Autor: Thayne Pacheco Valério
// Repositório: ISA

/// Contrato para operações de persistência de Tags.
/// 
using GerenciadorDeProdutos.Domain.ItemERP.Tag;
using GerenciadorDeProdutos.DomainShared.ValueObjects;
using System.Collections.Generic;

namespace GerenciadorDeProdutos.Domain.ItemERP.Tag
{
    public interface ITagRepository
    {
        void Adicionar(Tag tag);
        void Atualizar(Tag tag);
        void Remover(Tag tag);
        Tag? ObterPorId(int id);
        IEnumerable<Tag> ObterTodos();
        bool ExisteComNome(Nome nome);
        bool ExisteComNome(Nome nome, int ignorarId);
    }
}


