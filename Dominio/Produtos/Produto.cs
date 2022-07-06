namespace LojaAPI.Dominio.Produtos;

public class Produto : Entidade
{
    public string Nome { get; private set; }
    public Guid CategoriaId { get; private set; }
    public Categoria Categoria { get; private set; }
    public string Descricao { get; private set; }
    public List<Tag>? Tags { get; private set; }
    public bool TemEstoque { get; private set; }
    public bool Ativo { get; private set; } = true;
    public decimal Preco { get; private set; }
    public ICollection<Pedido> Pedidos { get; private set; } //sujei a classe Produto mas por uma boa causa, agr o ApplicationDbContext vai poder criar uma Tabela de Relacionamento PedidoxProdutos no Banco de Dados sem precisar criar uma classe(eg. PedidoProdutos.cs) para representar esse relacionamento

    private Produto() { }
    public Produto(string nome, Categoria categoria, List<Tag> tags, string descricao, decimal preco, bool temEstoque, string criadoPor, string editadoPor)
    {
        Descricao = descricao;
        TemEstoque = temEstoque;
        Nome = nome;
        Categoria = categoria;
        Preco = preco;
        Tags = tags;
        Ativo = true;
        CriadoPor = criadoPor;
        EditadoPor = editadoPor;
        CriadoEm = DateTime.Now;
        EditadoEm = DateTime.Now;

        Validate();
    }
    public void EditarProduto(string nome, Categoria categoria, List<Tag> tags, string descricao, decimal preco, bool temEstoque, bool ativo, string editadoPor)
    {
        Nome = nome;
        Categoria = categoria;
        Tags = tags;
        Descricao = descricao;
        Preco = preco;
        TemEstoque = temEstoque;
        Ativo = ativo;
        EditadoPor = editadoPor;
        EditadoEm = DateTime.Now;
        Validate();
    }
    private void Validate()
    {
        var contract = new Contract<Produto>()
                    .IsNotEmpty(Nome, "Nome", "Campo Nome é obrigatório")
                    .IsNotNullOrWhiteSpace(Nome, "Nome", "Campo Nome é obrigatório")
                    .IsGreaterOrEqualsThan(Nome, 3, "Nome", "O nome tem que ter mais de dois caracteres")
                    .IsNotNull(Categoria, "Categoria", "A Categoria não foi encontrada")
                    .IsGreaterThan(Preco, 0, "Preço", "O preço do produto tem que ser maior que Zero ")
                    .IsNotNullOrEmpty(CriadoPor, "CriadoPor", "Campo CriadoPor é obrigatório")
                    .IsNotNullOrEmpty(EditadoPor, "EditadoPor", "Campo EditadoPor é obrigatório");
        AddNotifications(contract);
    }
}
