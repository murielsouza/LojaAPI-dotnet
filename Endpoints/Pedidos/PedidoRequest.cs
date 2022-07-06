namespace LojaAPI.Endpoints.Pedidos;

public record PedidoRequest(List<Guid> ProdutosId, string EnderecoEntrega);