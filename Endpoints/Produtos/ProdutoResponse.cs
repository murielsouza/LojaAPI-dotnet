namespace LojaAPI.Endpoints.Produtos;
public record ProdutoResponse(Guid Id, string Nome, string CategoriaNome, string Descricao, decimal preco,  List<Tag> Tags, bool TemEstoque, bool Ativo);