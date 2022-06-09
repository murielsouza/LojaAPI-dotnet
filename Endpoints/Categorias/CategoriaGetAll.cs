namespace LojaAPI.Endpoints.Categorias;

public class CategoriaGetAll
{
    public static string Template => "/categorias"; 
    public static string [] Methods => new string [] { HttpMethod.Get.ToString() }; //forma de acesso somente método get
    public static Delegate Handle => Action;

    public static IResult Action(ApplicationDbContext context) {
        var categorias = context.Categorias.ToList();
        var response = categorias.Select(c => new CategoriaResponse { Id = c.Id, Nome = c.Nome, Ativo = c.Ativo });
        return Results.Ok(response);
    }
}
