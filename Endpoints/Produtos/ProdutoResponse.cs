namespace LojaAPI.Endpoints.Produtos;
public class ProdutoResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public Guid CategoriaId { get; set; }
    public string Categoria { get; set; }
    public string Descricao { get; set; }
    public List<Tag> Tags { get; set; }
    public bool TemEstoque { get; set; }
    public bool Ativo { get; set; }
}