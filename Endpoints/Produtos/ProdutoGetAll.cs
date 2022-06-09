namespace LojaAPI.Endpoints.Produtos;

public class ProdutoGetAll
{
    public static string Template => "/produtos";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(ApplicationDbContext context)
    {
        var produtos = context.Produtos.ToList();
        var response = produtos.Select(p => new ProdutoResponse { Id = p.Id, CategoriaId = p.CategoriaId ,Nome = p.Nome, Descricao = p.Descricao, Tags = p.Tags, TemEstoque = p.TemEstoque,Ativo = p.Ativo });
        return Results.Ok(response);
    }
}
