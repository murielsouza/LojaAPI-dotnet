namespace LojaAPI.Endpoints.Pedidos;

public class PedidoAllGet
{
    public static string Template => "/pedidos/";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    public static async Task<IResult> Action(ApplicationDbContext context, HttpContext http, int page = 1, int rows = 10, string orderBy = "data") //valores padrão caso não coloquem nada
    {
        //IQueryable<Pedido> queryBase = null;
        var queryBase = context.Pedidos.AsNoTracking();
        var user = http.User;
        if (rows > 10)
        {
            return Results.Problem(title: "Row com no máximo 10 registros", statusCode: 400);
        }
        if(user.HasClaim(c => c.Type == "Cpf"))
        {
            var clienteId = user.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            queryBase = queryBase
                .Include(p => p.Produtos)
                .Where(p => p.ClienteId == clienteId);
        }
        else if (user.HasClaim(c => c.Type == "CodigoFuncionario"))
        {
            queryBase = queryBase
                .Include(p => p.Produtos);
        }
        else
        {
            return Results.Problem(title: "O Usuário Logado não está autorizado a acessar os Dados", statusCode: 403);
        }

        if (orderBy == "data")
        {
            queryBase = queryBase.OrderBy(p => p.EditadoEm);
        }
        else if(orderBy == "valor")
        {
            queryBase = queryBase.OrderBy(p => p.Total);
        }
        else
        {
            return Results.Problem(title: "Ordem somente por data ou total do pedido", statusCode: 400);
        }

        var queryFilter = queryBase.Skip((page - 1) * rows).Take(rows);
        var pedidos = await queryFilter.ToListAsync();
        IEnumerable<PedidoProduto> products = null;
        var response = pedidos.Select(p => new PedidoResponse(p.Id, p.ClienteId, products, p.EnderecoEntrega, p.Total));
        return Results.Ok(response);
    }
}
