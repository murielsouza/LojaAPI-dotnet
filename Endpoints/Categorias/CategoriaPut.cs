using LojaAPI.Infra.Database;
using Microsoft.AspNetCore.Mvc;

namespace LojaAPI.Endpoints.Categorias;

public class CategoriaPut
{
    public static string Template => "/categorias/{id}";
    public static string [] Methods => new string [] { HttpMethod.Put.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action([FromRoute] Guid Id, CategoriaRequest categoriaRequest, ApplicationDbContext context)
    {
        var categoria = context.Categorias.Where(c => c.Id == Id).FirstOrDefault();
        categoria.Nome = categoriaRequest.Nome;
        categoria.Ativo = categoriaRequest.Ativo;
        context.SaveChanges();
        return Results.Ok();
    }
}
