namespace LojaAPI.Endpoints.Produtos;

public class ProdutoGetVitrine
{
    public static string Template => "/produtos/vitrine";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action(ApplicationDbContext context, int page = 1, int rows = 10, string orderBy = "nome") //valores padrão caso não coloquem nada
    {
        if(rows > 10)
        {
            return Results.Problem(title: "Row com no máximo 10 registros", statusCode: 400);
        }
        var queryBase = context.Produtos.AsNoTracking()
            .Include(p => p.Categoria)
            .Include(p => p.Tags)
            .Where(p => p.TemEstoque && p.Ativo && p.Categoria.Ativo);
        if(orderBy == "nome")
        {
            queryBase = queryBase.OrderBy(p => p.Nome);
        }
        else if(orderBy == "preco")
        {
            queryBase = queryBase.OrderBy(p => p.Preco);
        }
        else
        {
            return Results.Problem(title: "Ordem somente por nome ou preço", statusCode: 400);
        }

        var queryFilter = queryBase.Skip((page - 1) * rows).Take(rows); //page.Value or rows.Value se não tiver valor padrão e puder vir null
        var produtos = await queryFilter.ToListAsync();
        var response = produtos.Select(p => new ProdutoResponse(p.Id, p.Nome, p.Categoria.Nome, p.Descricao, p.Preco, p.Tags, p.TemEstoque, p.Ativo));
        return Results.Ok(response);
    }
}
