using LojaAPI.Dominio.Produtos;
using LojaAPI.Infra.Database;
using Microsoft.AspNetCore.Authorization;

namespace LojaAPI.Endpoints.Categorias;

public class CategoriaPost //convenção: recurso  + metodo de acesso para cadastrar categoria
{
    public static string Template => "/categorias"; //=> indica que está setando ao criar a propriedade template 
    public static string [] Methods => new string [] { HttpMethod.Post.ToString() }; //forma de acesso somente método post
    public static Delegate Handle => Action;

    //[Authorize]
    public static IResult Action(CategoriaRequest categoriaRequest, ApplicationDbContext context) {
        var categoria = new Categoria(categoriaRequest.Nome, "TESTE", "TESTE");
        if (!categoria.IsValid) {
            return Results.BadRequest(categoria.Notifications);
        }
        context.Categorias.Add(categoria);
        context.SaveChanges();
        return Results.Created($"/categorias/{categoria.Id}", categoria.Id);
    }
}
