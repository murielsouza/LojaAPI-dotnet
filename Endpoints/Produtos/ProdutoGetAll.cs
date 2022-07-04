namespace LojaAPI.Endpoints.Produtos;

public class ProdutoGetAll
{
    public static string Template => "/produtos";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "SomenteFuncionario")]
    public static async Task<IResult> Action(ApplicationDbContext context)
    {
        var produtos = await context.Produtos.AsNoTracking().Include(p => p.Categoria).Include(p => p.Tags).OrderBy(p => p.Nome).ToListAsync();
        var response = produtos.Select(p => new ProdutoResponse(p.Id, p.Nome, p.Categoria.Nome, p.Descricao, p.Preco, p.Tags, p.TemEstoque, p.Ativo));
        return Results.Ok(response);
    }
}
