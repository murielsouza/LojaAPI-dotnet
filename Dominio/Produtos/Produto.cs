namespace LojaAPI.Dominio.Produtos;

public class Produto : Entidade
{
    public Guid CategoriaId { get; set; } // one to one
    public Categoria Categoria { get; set; }
    public string Descricao { get; set; }
    public List<Tag> Tags { get; set; } //one to many
    public bool TemEstoque { get; set; }
    public bool Ativo { get; set; } = true;
}
