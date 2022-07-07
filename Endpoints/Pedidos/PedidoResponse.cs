namespace LojaAPI.Endpoints.Pedidos;
public record PedidoResponse(Guid Id, string Email, IEnumerable<PedidoProduto> Produtos, string EnderecoEntrega, decimal Total);
public record PedidoProduto(Guid Id, string Nome, decimal Preco);