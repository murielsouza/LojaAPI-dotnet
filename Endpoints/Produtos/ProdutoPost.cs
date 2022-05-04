using LojaAPI.Dominio.Produtos;
using LojaAPI.Infra.Database;

namespace LojaAPI.Endpoints.Produtos;

public class ProdutoPost
{
    public static string Template => "/produtos";
    public static string [] Methods => new string [] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(ProdutoRequest produtoRequest, ApplicationDbContext context) {
        var categoria = context.Categorias.Where(c => c.Id == produtoRequest.CategoriaId).First();
        var tags = context.Tags.ToList();
        var produto = new Produto
        {
            Categoria = categoria,
            Nome = produtoRequest.Nome,
            Descricao= produtoRequest.Descricao,
            TemEstoque= produtoRequest.TemEstoque,
            CriadoPor = "TEST",
            CriadoEm = DateTime.Now,
            EditadoPor = "TEST",
            EditadoEm = DateTime.Now,
        };
        if (produtoRequest.Tags != null)
        {
            produto.Tags = new List<Tag>();
            foreach (var item in produtoRequest.Tags) {
                var ver = true;
                foreach (var t in tags) {
                    if (t.Nome == item) {
                        produto.Tags.Add(t);
                        ver = false;
                        break;
                    }
                }
                if (ver)
                {
                    produto.Tags.Add(new Tag { Nome = item });
                }    
            }
        }
        context.Produtos.Add(produto);
        context.SaveChanges();
        return Results.Created($"/produtos/{produto.Id}", produto.Id);
    }
}
