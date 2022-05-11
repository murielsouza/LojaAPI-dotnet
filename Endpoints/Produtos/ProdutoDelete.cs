using LojaAPI.Infra.Database;
using Microsoft.AspNetCore.Mvc;

namespace LojaAPI.Endpoints.Produtos;

public class ProdutoDelete
{
    public static string Template => "/produtos/{id:guid}";
    public static string[] Methods => new string[] { HttpMethod.Delete.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action([FromRoute] Guid Id, ApplicationDbContext context)
    {
        var produto = context.Produtos.Where(p => p.Id == Id).FirstOrDefault();
        if (produto == null)
        {
            return Results.NotFound("Produto não existe no Banco de Dados");
        }
        context.Produtos.Remove(produto);
        context.SaveChanges();
        return Results.Ok();
    }
}
