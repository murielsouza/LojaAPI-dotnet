namespace LojaAPI.Endpoints.Produtos;
public record ProdutoResponse(Guid Id, string Nome, Guid CategoriaId, string Descricao, List<Tag> Tags, bool TemEstoque, bool Ativo);