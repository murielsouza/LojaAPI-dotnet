namespace LojaAPI.Endpoints.Produtos;

public class ProdutoPut
{
    public static string Template => "/produto/{id:guid}";
    public static string[] Methods => new string[] { HttpMethod.Put.ToString() };
    public static Delegate Handle => Action;

    public static async Task<IResult> Action([FromRoute] Guid Id, ProdutoRequest produtoRequest, HttpContext http, ApplicationDbContext context)
    {
        var userId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var categoria = await context.Categorias.Where(c => c.Id == produtoRequest.CategoriaId).FirstOrDefaultAsync();
        var tags = await context.Tags.ToListAsync();
        var atualizaListaTags = new List<Tag>();
        var produto = await context.Produtos.Where(p => p.Id == Id).FirstOrDefaultAsync();
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
        foreach (var t in produtoRequest.Tags)
        {
            atualizaListaTags.Add(new Tag(t));
        }
        produto.EditarProduto(produtoRequest.Nome, categoria, atualizaListaTags, produtoRequest.Descricao, produtoRequest.Preco, produtoRequest.TemEstoque, produtoRequest.Ativo, userId);
        if (!produto.IsValid)
        {
            return Results.ValidationProblem(produto.Notifications.ConvertToProblemDetails());
        }
        await context.SaveChangesAsync();
        return Results.Ok();
    }
}
