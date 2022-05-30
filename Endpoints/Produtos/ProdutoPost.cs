using LojaAPI.Dominio.Produtos;
using LojaAPI.Infra.Database;
using Microsoft.AspNetCore.Authorization;

namespace LojaAPI.Endpoints.Produtos;

public class ProdutoPost
{
    public static string Template => "/produtos";
    public static string [] Methods => new string [] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "SomenteFuncionario")]
    public static IResult Action(ProdutoRequest produtoRequest, ApplicationDbContext context) {
        var categoria = context.Categorias.Where(c => c.Id == produtoRequest.CategoriaId).FirstOrDefault();
        if (categoria == null) {
            return Results.NotFound("Categoria não existe no Banco de Dados");
        }
        var tags = context.Tags.ToList();
        var listaTags = new List<Tag>();
        if (produtoRequest.Tags != null)
        {
            foreach (var item in produtoRequest.Tags)
            {
                var ver = true;
                foreach (var t in tags)
                {
                    if (t.Nome == item)
                    {
                        listaTags.Add(t);
                        ver = false;
                        break;
                    }
                }
                if (ver)
                {
                    listaTags.Add(new Tag { Nome = item });
                }
            }
        }
        var produto = new Produto(produtoRequest.Nome, produtoRequest.Descricao, produtoRequest.TemEstoque, "TEST", "TEST") {
            Categoria = categoria,
            Tags = listaTags
        };
        if (!produto.IsValid)
        {
            return Results.ValidationProblem(produto.Notifications.ConvertToProblemDetails()); //método de extensão 
        }
        context.Produtos.Add(produto);
        context.SaveChanges();
        return Results.Created($"/produtos/{produto.Id}", produto.Id);
    }
}
