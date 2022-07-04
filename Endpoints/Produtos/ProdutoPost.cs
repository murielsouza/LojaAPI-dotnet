namespace LojaAPI.Endpoints.Produtos;

public class ProdutoPost
{
    public static string Template => "/produtos";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "SomenteFuncionario")]
    public static async Task<IResult> Action(ProdutoRequest produtoRequest, HttpContext http, ApplicationDbContext context)
    {
        var userId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var categoria = await context.Categorias.Where(c => c.Id == produtoRequest.CategoriaId).FirstOrDefaultAsync();
        var tags = await context.Tags.ToListAsync();
        var listaTags = new List<Tag>();
        if (produtoRequest.Tags != null)
        {
            foreach (var t in produtoRequest.Tags)
            {
                listaTags.Add(new Tag (t));          
            }
        }
        var produto = new Produto(produtoRequest.Nome, categoria, listaTags, produtoRequest.Descricao, produtoRequest.Preco, produtoRequest.TemEstoque, userId, userId);
        if (!produto.IsValid)
        {
            return Results.ValidationProblem(produto.Notifications.ConvertToProblemDetails()); //método de extensão 
        }
        await context.Produtos.AddAsync(produto);
        await context.SaveChangesAsync();
        return Results.Created($"/produtos/{produto.Id}", produto.Id);
    }
}
