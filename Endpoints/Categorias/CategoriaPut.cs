using LojaAPI.Infra.Database;
using Microsoft.AspNetCore.Mvc;

namespace LojaAPI.Endpoints.Categorias;

public class CategoriaPut
{
    public static string Template => "/categorias/{id:guid}";
    public static string [] Methods => new string [] { HttpMethod.Put.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action([FromRoute] Guid Id, CategoriaRequest categoriaRequest, ApplicationDbContext context)
    {
        var categoria = context.Categorias.Where(c => c.Id == Id).FirstOrDefault();
        if (categoria == null)
        {
            return Results.NotFound("Categoria não existe no Banco de Dados");
        }
        categoria.EditarCategoria(categoriaRequest.Nome, categoriaRequest.Ativo);
        if (!categoria.IsValid) {
            return Results.ValidationProblem(categoria.Notifications.ConvertToProblemDetails());
        }
        context.SaveChanges();
        return Results.Ok();
    }
}
