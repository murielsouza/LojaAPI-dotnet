using LojaAPI.Infra.Database;
using Microsoft.AspNetCore.Mvc;

namespace LojaAPI.Endpoints.Categorias;

public class CategoriaDelete
{
    public static string Template => "/categorias/{id}";
    public static string [] Methods => new string [] { HttpMethod.Delete.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action([FromRoute] Guid Id, ApplicationDbContext context)
    {
        var categoria = context.Categorias.Where(c => c.Id == Id).First();
        context.Categorias.Remove(categoria);
        context.SaveChanges();
        return Results.Ok();
    }
}
