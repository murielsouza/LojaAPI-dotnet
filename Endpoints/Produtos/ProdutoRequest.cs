namespace LojaAPI.Endpoints.Produtos;

public class ProdutoRequest
{
    public Guid CategoriaId { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public List<string> Tags { get; set; }
    public bool TemEstoque { get; set; }
    public bool Ativo { get; set; }
}