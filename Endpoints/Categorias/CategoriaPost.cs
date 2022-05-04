using LojaAPI.Dominio.Produtos;
using LojaAPI.Infra.Database;

namespace LojaAPI.Endpoints.Categorias;

public class CategoriaPost //convenção: recurso  + metodo de acesso para cadastrar categoria
{
    public static string Template => "/categorias"; //=> indica que está setando ao criar a propriedade template 
    public static string [] Methods => new string [] { HttpMethod.Post.ToString() }; //forma de acesso somente método post
    public static Delegate Handle => Action;

    public static IResult Action(CategoriaRequest categoriaRequest, ApplicationDbContext context) {
        var categoria = new Categoria
        {
            Nome = categoriaRequest.Nome,
            CriadoPor = "TEST",
            CriadoEm = DateTime.Now,
            EditadoPor = "TEST",
            EditadoEm = DateTime.Now,
};
        context.Categorias.Add(categoria);
        context.SaveChanges();
        return Results.Created($"/categorias/{categoria.Id}", categoria.Id);
    }
}
