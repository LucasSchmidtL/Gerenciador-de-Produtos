// Projeto: Gerenciador de Produtos
// Descrição: Implementa o Value Object DataCriacao
// Data de Criação: 2025-06-19
// Autor: Thayne Pacheco Valério
// Repositório: ISA

namespace GerenciadorDeProdutos.Domain.ItemERP
{
    public interface IItemERPRepository
    {
        bool ExisteTagUsada(int tagId);
        void RemoverTagDeTodosOsItens(int tagId);
        // Outros métodos relacionados a ItemERP...
    }
}