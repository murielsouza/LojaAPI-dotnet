using LojaAPI.Dominio.Produtos;
using LojaAPI.Infra.Database;
using Microsoft.AspNetCore.Mvc;

namespace LojaAPI.Endpoints.Produtos;

public class ProdutoPut
{
    public static string Template => "/produtos/{id:guid}";
    public static string [] Methods => new string [] { HttpMethod.Put.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action([FromRoute] Guid Id, ProdutoRequest produtoRequest, ApplicationDbContext context)
    {
        var categoria = context.Categorias.Where(c => c.Id == produtoRequest.CategoriaId).FirstOrDefault();
        if (categoria == null)
        {
            return Results.NotFound("Categoria não existe no Banco de Dados");
        }
        var tags = context.Tags.ToList();
        var produto = context.Produtos.Where(p => p.Id == Id).FirstOrDefault();
        if (produto == null)
        {
            return Results.NotFound("Produto não existe no Banco de Dados");
        }
        produto.EditarProduto(produtoRequest.Nome, produtoRequest.Descricao, produtoRequest.TemEstoque, produtoRequest.Ativo);
        produto.Categoria = categoria;
        if (produtoRequest.Tags != null)
        {
            produto.Tags = new List<Tag>();
            foreach (var item in produtoRequest.Tags)
            {
                var ver = true;
                foreach (var t in tags)
                {
                    if (t.Nome == item)
                    {
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
        if (!produto.IsValid)
        {
            return Results.ValidationProblem(produto.Notifications.ConvertToProblemDetails());
        }
        context.SaveChanges();
        return Results.Ok();
    }
}
