namespace LojaAPI.Endpoints.Produtos;

public record ProdutoRequest(Guid CategoriaId, string Nome, string Descricao, List<string> Tags, bool TemEstoque, bool Ativo);