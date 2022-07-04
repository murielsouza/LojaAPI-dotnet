namespace LojaAPI.Dominio.Produtos;

public class Tag
{
    public int Id { get; private set; }
    public string Nome { get; private set; }
    public Guid ProdutoId { get; private set; }
    
    public Tag(string nome)
    {
        Nome = nome;
    }
}
