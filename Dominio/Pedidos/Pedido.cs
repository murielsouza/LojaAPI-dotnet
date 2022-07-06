namespace LojaAPI.Dominio.Pedidos;

public class Pedido : Entidade
{
    public string ClienteId { get; private set; } 
    public List<Produto>? Produtos { get; private set; }
    public decimal Total { get; private set; }
    public string EnderecoEntrega { get; private set; }

    private Pedido() { }

    public Pedido (string clienteId, string clienteNome, List<Produto> produtos, string enderecoEntrega)
    {
        ClienteId = clienteId;
        Produtos = produtos;
        EnderecoEntrega = enderecoEntrega;
        CriadoPor = clienteNome;
        EditadoPor = clienteNome;
        CriadoEm = DateTime.UtcNow;
        EditadoEm = DateTime.UtcNow;
        Validate();

        Total = 0;
        foreach(var p in produtos)
        {
            Total += p.Preco;
        }
    }

    private void Validate() 
    {
        var contract = new Contract<Pedido>()
            .IsNotNull(ClienteId, "Cliente", "O cliente do Pedido não foi encontrado")
            .IsNotNull(Produtos, "Produtos", "Nenhum Produto foi inserido no Pedido")
            .IsGreaterThan(Produtos, 0, "Produtos", "Nenhum Produto do Pedido foi encontrado no Banco de Dados")
            .IsNotNullOrWhiteSpace(EnderecoEntrega, "EnderecoEntrega", "O endereço de entrega deve está preenchido");
        AddNotifications(contract);
    }
}
