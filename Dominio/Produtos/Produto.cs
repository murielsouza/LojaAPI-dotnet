namespace LojaAPI.Dominio.Produtos;

public class Produto : Entidade
{
    public Guid CategoriaId { get; set; } // one to one
    public Categoria Categoria { get; set; }
    public string Descricao { get; private set; }
    public List<Tag> Tags { get; set; } //one to many
    public bool TemEstoque { get; private set; }
    public bool Ativo { get; private set; } = true;
    public Produto(string nome, string descricao, bool temEstoque, string criadoPor, string editadoPor)
    {
        Descricao = descricao;
        TemEstoque = temEstoque;
        Nome = nome;
        Ativo = true;
        CriadoPor = criadoPor;
        EditadoPor = editadoPor;
        CriadoEm = DateTime.Now;
        EditadoEm = DateTime.Now;

        Validate();
    }
    public void EditarProduto(string nome, string descricao, bool temEstoque, bool ativo)
    {
        Nome = nome;
        Descricao = descricao;
        TemEstoque = temEstoque;
        Ativo = ativo;
        Validate();
    }
    private void Validate()
    {
        var contract = new Contract<Produto>()
                    .IsNotEmpty(Nome, "Nome", "Campo Nome é obrigatório")
                    .IsNotNullOrWhiteSpace(Nome, "Nome", "Campo Nome é obrigatório")
                    .IsGreaterOrEqualsThan(Nome, 3, "Name", "O nome tem que ter mais de dois caracteres")
                    .IsNotNullOrEmpty(CriadoPor, "CriadoPor", "Campo CriadoPor é obrigatório")
                    .IsNotNullOrEmpty(EditadoPor, "EditadoPor", "Campo EditadoPor é obrigatório");
        AddNotifications(contract);
    }
}
