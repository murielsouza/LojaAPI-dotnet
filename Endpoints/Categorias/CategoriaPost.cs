using LojaAPI.Dominio.Produtos;
using LojaAPI.Infra.Database;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace LojaAPI.Endpoints.Categorias;

public class CategoriaPost //convenção: recurso  + metodo de acesso para cadastrar categoria
{
    public static string Template => "/categorias"; //=> indica que está setando ao criar a propriedade template 
    public static string [] Methods => new string [] { HttpMethod.Post.ToString() }; //forma de acesso somente método post
    public static Delegate Handle => Action;

    [Authorize(Policy = "SomenteFuncionario")]
    public static async Task<IResult> Action(CategoriaRequest categoriaRequest, HttpContext http, ApplicationDbContext context) {
        var userId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var categoria = new Categoria(categoriaRequest.Nome, userId, userId);
        if (!categoria.IsValid) {
            return Results.BadRequest(categoria.Notifications);
        }
        await context.Categorias.AddAsync(categoria);
        await context.SaveChangesAsync();
        return Results.Created($"/categorias/{categoria.Id}", categoria.Id);
    }
}
