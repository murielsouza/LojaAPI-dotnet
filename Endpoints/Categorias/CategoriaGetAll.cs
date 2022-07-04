﻿namespace LojaAPI.Endpoints.Categorias;

public class CategoriaGetAll
{
    public static string Template => "/categorias"; 
    public static string [] Methods => new string [] { HttpMethod.Get.ToString() }; //forma de acesso somente método get
    public static Delegate Handle => Action;

    public static async Task<IResult> Action(ApplicationDbContext context) {
        var categorias = await context.Categorias.AsNoTracking().ToListAsync();
        var response = categorias.Select(c => new CategoriaResponse(c.Id, c.Nome, c.Ativo));
        return Results.Ok(response);
    }
}
