namespace LojaAPI.Endpoints.Pedidos;

public class PedidoGet
{
    public static string Template => "/pedidos/{id:guid}";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    public static async Task<IResult> Action([FromRoute] Guid id, ApplicationDbContext context, HttpContext http, UserManager<IdentityUser> userManager) //valores padrão caso não coloquem nada
    {
        var clienteClaim = http.User.Claims.
            First(c => c.Type == ClaimTypes.NameIdentifier);
        var funcionarioClaim = http.User.Claims.
            FirstOrDefault(c => c.Type == "CodigoFuncionario");

        var pedido = context.Pedidos
            .Include(p => p.Produtos)
            .FirstOrDefault(p => p.Id == id);

        if(pedido.ClienteId != clienteClaim.Value && funcionarioClaim == null)
        {
            //return Results.Forbid();
            return Results.Problem(title: "PedidoGet", detail: "O Usuário Logado não está autorizado a acessar os Dados", statusCode: 403);
        }
        var client = await userManager.FindByIdAsync(pedido.ClienteId);
        var produtosResponse = pedido.Produtos.Select(p => new PedidoProduto(p.Id, p.Nome, p.Preco));
        var response = new PedidoResponse(pedido.Id, client.Email, produtosResponse, pedido.EnderecoEntrega, pedido.Total);
        return Results.Ok(response);
    }
}
