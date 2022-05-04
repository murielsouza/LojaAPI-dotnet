using LojaAPI.Dominio.Produtos;
using LojaAPI.Infra.Database;
using Microsoft.AspNetCore.Mvc;

namespace LojaAPI.Endpoints.Produtos;

public class ProdutoPut
{
    public static string Template => "/produtos/{id}";
    public static string [] Methods => new string [] { HttpMethod.Put.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action([FromRoute] Guid Id, ProdutoRequest produtoRequest, ApplicationDbContext context)
    {
        var categoria = context.Categorias.Where(c => c.Id == produtoRequest.CategoriaId).First();
        var tags = context.Tags.ToList();
        var produto = context.Produtos.Where(p => p.Id == Id).FirstOrDefault();
        produto.Categoria = categoria;
        produto.Nome = produtoRequest.Nome;
        produto.Descricao = produtoRequest.Descricao;
        produto.Ativo = produtoRequest.Ativo;
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
        context.SaveChanges();
        return Results.Ok();
    }
}
