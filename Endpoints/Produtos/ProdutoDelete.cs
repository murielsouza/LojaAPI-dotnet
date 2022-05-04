using LojaAPI.Infra.Database;
using Microsoft.AspNetCore.Mvc;

namespace LojaAPI.Endpoints.Produtos;

public class ProdutoDelete
{
    public static string Template => "/produtos/{id}";
    public static string[] Methods => new string[] { HttpMethod.Delete.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action([FromRoute] Guid Id, ApplicationDbContext context)
    {
        var produto = context.Produtos.Where(p => p.Id == Id).First();
        context.Produtos.Remove(produto);
        context.SaveChanges();
        return Results.Ok();
    }
}
