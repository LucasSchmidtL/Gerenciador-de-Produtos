# 📂 Estrutura de Pastas - Camada Domain (Projeto: Gerenciador de Produtos)

📁 /Documentacao
├── 📁 Arquitetura
│   ├── VisaoGeral.md
│   ├── LinguagemUbiqua.md
│   ├── AggregateRoots.md
│   └── PadroesTecnicos.md
├── 📁 Diagramas
│   ├── 📁 ERD
│   │   ├── GerenciadorDeProdutos.drawsql
│   │   └── GerenciadorDeProjetos.drawsql
│   └── 📁 UML
│       └── Produto_Aggregate.png
├── 📁 RegrasDeNegocio
│   ├── Produto.md
│   ├── Agrupador.md
│   ├── Componente.md
│   ├── ItemERP.md
│   └── DesenvolvimentoDeProduto.md
└── 📁 ReleaseNotes
    └── Versao_X_Y.md


📁 Domain
├── 📁 Produto
│    ├── 📁 Entities
│    ├── 📁 Services
│    ├── 📁 Repositories
│    ├── 📁 FuncoesProdutoAgrupador
│    └── 📁 VariaveisProduto
├── 📁 Agrupador
│    ├── 📁 Entities
│    ├── 📁 Services
│    ├── 📁 Repositories
│    ├── 📁 FuncoesAgrupadorAgrupador
│    ├── 📁 FuncoesAgrupadorComponente
│    ├── 📁 FuncoesAgrupadorItemERP
│    └── 📁 VariaveisAgrupador
├── 📁 Componente
│    ├── 📁 Entities
│    ├── 📁 Services
│    ├── 📁 Repositories
│    └── 📁 VariaveisComponente
├── 📁 ItemERP
│    ├── 📁 Entities
│    │    ├── ItemERP
│    │    ├── ItemERPComposto
│    │    ├── ItemERPVinculado
│    │    ├── ItemERPRevisao
│    │    ├── Desenho
│    │    ├── DesenhoRevisao
│    │    ├── Tag
│    │    ├── Perfil
│    │    └── PerfilRevisao
│    ├── 📁 Services
│    ├── 📁 Repositories
│    │    ├──IItemERPRepository
│    │    ├──ITagRepository
│    ├── 📁 VariaveisItemERPComposto


📁 /DomainShared
├── 📁 Enums 
│    ├── Status.cs
│    ├── TipoVariavel.cs
│    └── TipoItemERP.cs
├── 📁 ValueObjects
│    ├── Altura.cs
│    ├── AprovadoPor.cs
│    ├── Caminho.cs
│    ├── Comprimento.cs
│    ├── Data.cs
│    ├── DataCriacao.cs
│    ├── Descricao.cs
│    ├── EquacaoComprimento.cs
│    ├── EquacaoProfundidade.cs
│    ├── EquacaoAltura.cs
│    ├── EquacaoQuantidade.cs
│    ├── ImplementadoPor.cs
│    ├── Motivo.cs
│    ├── Nome.cs
│    ├── NomeComercial.cs
│    ├── Numero.cs
│    ├── Precificador.cs
│    ├── Profundidade.cs
│    ├── Status.cs
│    ├── VariavelTipo.cs
│    └── VariavelNome.cs

├── 📁 Infrastructure
├── 📁 Application
├── 📁 API (ou Web)
├── 📁 Tests

---

## 📌 Observações:

✅ Cada pasta representa um **Aggregate Root** ou uma **entidade de domínio relevante**.  
✅ As subpastas de **Variáveis** indicam que cada contexto (Produto, Agrupador, Componente, ItemERP) tem **suas próprias variáveis e regras de negócio associadas**.  
✅ Dentro de cada pasta deverão estar:  
- Entidades de Domínio (`.cs`)  
- Regras de Negócio  
- Métodos de Domínio  
- Objetos de Valor (se aplicável)  
- Eventuais Eventos de Domínio (se você começar a aplicar Event Sourcing ou DDD avançado)  

✅ O padrão da pasta segue o conceito de **organização por contexto de domínio**, recomendado em arquiteturas DDD.

---

## ✅ Exemplo de Localização de Arquivos:

- `/Domain/Produto/Produto.cs`
- `/Domain/Produto/VariaveisProduto/VariavelProduto.cs`
- `/Domain/Agrupador/Agrupador.cs`
- `/Domain/Agrupador/VariaveisAgrupador/VariavelAgrupador.cs`
- `/Domain/ItemERP/Desenho/Desenho.cs`
- `/Domain/ItemERP/Tag/Tag.cs`

---


/// <summary>
/// Serviço de domínio responsável por [⚠️ Descreva aqui a responsabilidade principal do Service].
/// </summary>
/// <remarks>
/// Regras de negócio principais:
/// - [⚠️ Regra 1: Exemplo → Validação de unicidade]
/// - [⚠️ Regra 2: Exemplo → Restrições antes de apagar]
/// - [⚠️ Regra 3: Outras regras relevantes]
/// </remarks>
public class NomeDoService
{
    ...
    
    /// <summary>
    /// [⚠️ Breve descrição do que o método faz]
    /// </summary>
    /// <param name="parametro1">[⚠️ Descrição do parâmetro 1]</param>
    /// <param name="parametro2">[⚠️ Descrição do parâmetro 2]</param>
    /// <returns>[⚠️ Se houver retorno, descreva o que significa]</returns>
    /// <exception cref="[⚠️ Tipo da Exceção]">[⚠️ Condição que gera a exceção]</exception>
    public void MetodoExemplo(...)
    {
        ...
    }

    ...
}

/// <summary>
/// Representa [⚠️ Nome da Entidade] dentro do domínio de [⚠️ Nome do Contexto].
/// </summary>
/// <remarks>
/// Regras de negócio associadas:
/// - [⚠️ Regra 1: Exemplo → Nome não pode ser vazio]
/// - [⚠️ Regra 2: Exemplo → Identidade baseada em ID]
/// - [⚠️ Regra 3: Exemplo → Precisa ter ao menos um Item associado]
/// </remarks>
public class NomeDaEntidade
{
    /// <summary>
    /// Identificador único da entidade.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// [⚠️ Nome da propriedade] [⚠️ Breve descrição da propriedade]
    /// </summary>
    public Tipo Propriedade1 { get; }

    /// <summary>
    /// [⚠️ Nome da propriedade] [⚠️ Breve descrição da propriedade]
    /// </summary>
    public Tipo Propriedade2 { get; }

    /// <summary>
    /// Inicializa uma nova instância de <see cref="NomeDaEntidade"/>.
    /// </summary>
    /// <param name="id">Identificador da entidade.</param>
    /// <param name="propriedade1">[⚠️ Descreva o parâmetro]</param>
    /// <param name="propriedade2">[⚠️ Descreva o parâmetro]</param>
    public NomeDaEntidade(int id, Tipo propriedade1, Tipo propriedade2)
    {
        ...
    }

    /// <summary>
    /// Verifica se a entidade é igual a outra, com base na identidade (ID ou outro critério).
    /// </summary>
    /// <param name="obj">Objeto a comparar.</param>
    /// <returns>True se forem considerados iguais.</returns>
    public override bool Equals(object? obj)
    {
        ...
    }

    /// <summary>
    /// Gera o código hash com base na identidade da entidade.
    /// </summary>
    public override int GetHashCode()
    {
        ...
    }

    /// <summary>
    /// Retorna uma representação textual da entidade.
    /// </summary>
    public override string ToString()
    {
        ...
    }
}
