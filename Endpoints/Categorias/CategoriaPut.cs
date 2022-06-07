﻿using LojaAPI.Infra.Database;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LojaAPI.Endpoints.Categorias;

public class CategoriaPut
{
    public static string Template => "/categoria/{id:guid}";
    public static string [] Methods => new string [] { HttpMethod.Put.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action([FromRoute] Guid Id, HttpContext http, CategoriaRequest categoriaRequest, ApplicationDbContext context)
    {
        var userId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var categoria = context.Categorias.Where(c => c.Id == Id).FirstOrDefault();
        if (categoria == null)
        {
            return Results.NotFound("Categoria não existe no Banco de Dados");
        }
        categoria.EditarCategoria(categoriaRequest.Nome, categoriaRequest.Ativo, userId);
        if (!categoria.IsValid) {
            return Results.ValidationProblem(categoria.Notifications.ConvertToProblemDetails());
        }
        context.SaveChanges();
        return Results.Ok();
    }
}
