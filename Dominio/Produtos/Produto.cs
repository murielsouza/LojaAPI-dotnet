namespace LojaAPI.Dominio.Produtos;

public class Produto : Entidade
{
    public Guid CategoriaId { get; private set; } // one to one
    public Categoria Categoria { get; private set; }
    public string Descricao { get; private set; }
    public List<Tag>? Tags { get; private set; } //one to many
    public bool TemEstoque { get; private set; }
    public bool Ativo { get; private set; } = true;
    public decimal Preco { get; private set; }
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
