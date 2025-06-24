// Projeto: Gerenciador de Produtos
// Descrição: Implementa o Value Object DataCriacao
// Data de Criação: 2025-06-19
// Autor: Thayne Pacheco Valério
// Repositório: ISA

// ✅ Cria uma nova Tag, garantindo que o nome seja único.
// ✅ Criar/Editar: Verifica unicidade do nome.
// ✅ Apagar: Verifica se a tag está em uso. Se estiver, só permite apagar se o usuário confirmar
///   a remoção da tag dos itens relacionados.


using GerenciadorDeProdutos.Domain.ItemERP.Entities;
using GerenciadorDeProdutos.Domain.ItemERP.Repositories;

namespace GerenciadorDeProdutos.Domain.ItemERP.Services
{
    /// <summary>
    /// Serviço de domínio responsável pelas regras de negócio relacionadas à Tag.
    /// </summary>
    /// <remarks>
    /// Regras principais:
    /// - Criar/Editar: Verifica unicidade do nome.
    /// - Apagar: Verifica se a Tag está em uso. Se estiver, só permite apagar se o usuário confirmar
    ///   a remoção da Tag de todos os itens relacionados.
    /// </remarks>
    public class TagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly IItemERPRepository _itemERPRepository;

        /// <summary>
        /// Inicializa uma nova instância de <see cref="TagService"/>.
        /// </summary>
        /// <param name="tagRepository">Repositório de Tags.</param>
        /// <param name="itemERPRepository">Repositório de ItemERP.</param>
        public TagService(ITagRepository tagRepository, IItemERPRepository itemERPRepository)
        {
            _tagRepository = tagRepository;
            _itemERPRepository = itemERPRepository;
        }

        /// <summary>
        /// Cria uma nova Tag, garantindo que o nome seja único.
        /// </summary>
        /// <param name="tag">A Tag a ser criada.</param>
        /// <exception cref="InvalidOperationException">Se já existir uma Tag com o mesmo nome.</exception>
        public void Criar(Tag tag)
        {
            if (_tagRepository.ExisteComNome(tag.Nome))
                throw new InvalidOperationException("Já existe uma tag com esse nome.");

            _tagRepository.Adicionar(tag);
        }

        /// <summary>
        /// Edita uma Tag existente, garantindo que o novo nome seja único.
        /// </summary>
        /// <param name="tag">A Tag a ser atualizada.</param>
        /// <exception cref="InvalidOperationException">Se já existir outra Tag com o mesmo nome.</exception>
        public void Editar(Tag tag)
        {
            if (_tagRepository.ExisteComNome(tag.Nome, tag.Id))
                throw new InvalidOperationException("Já existe outra tag com esse nome.");

            _tagRepository.Atualizar(tag);
        }

        /// <summary>
        /// Verifica se a Tag pode ser apagada, ou seja, se ela não está em uso por nenhum ItemERP.
        /// </summary>
        /// <param name="tag">A Tag a ser verificada.</param>
        /// <returns>True se puder apagar, false se estiver em uso.</returns>
        public bool PodeApagar(Tag tag)
        {
            return !_itemERPRepository.ExisteTagUsada(tag.Id);
        }

        /// <summary>
        /// Apaga a Tag. Se ela estiver em uso, só permite apagar caso o usuário confirme a remoção da Tag de todos os itens relacionados.
        /// </summary>
        /// <param name="tag">A Tag a ser apagada.</param>
        /// <param name="removerDasItemERP">Se true, remove a Tag de todos os ItemERP antes de apagar.</param>
        /// <exception cref="InvalidOperationException">
        /// Se a Tag estiver em uso e o parâmetro <paramref name="removerDasItemERP"/> não for true.
        /// </exception>
        public void Apagar(Tag tag, bool removerDasItemERP)
        {
            if (_itemERPRepository.ExisteTagUsada(tag.Id))
            {
                if (removerDasItemERP)
                {
                    _itemERPRepository.RemoverTagDeTodosOsItens(tag.Id);
                }
                else
                {
                    throw new InvalidOperationException("Tag está em uso em algum ItemERP.");
                }
            }

            _tagRepository.Remover(tag);
        }
    }
}

