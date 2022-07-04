namespace LojaAPI.Endpoints.Produtos;

public class ProdutoDelete
{
    public static string Template => "/produto/{id:guid}";
    public static string [] Methods => new string[] { HttpMethod.Delete.ToString() };
    public static Delegate Handle => Action;

    public static async Task<IResult> Action([FromRoute] Guid Id, ApplicationDbContext context)
    {
        var produto = await context.Produtos.Where(p => p.Id == Id).FirstOrDefaultAsync();
        var tags = await context.Tags.ToListAsync();
        if (produto == null)
        {
            return Results.NotFound("Produto não existe no Banco de Dados");
        }
        foreach (var t in tags)
        {
            if (t.ProdutoId == produto.Id)
            {
                context.Tags.Remove(t);
            }
        }
        context.Produtos.Remove(produto);
        await context.SaveChangesAsync();
        return Results.Ok();
    }
}
