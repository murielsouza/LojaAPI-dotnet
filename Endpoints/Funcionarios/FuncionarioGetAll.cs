using LojaAPI.Infra.Database;
using Microsoft.AspNetCore.Authorization;

namespace LojaAPI.Endpoints.Funcionarios;

[Authorize(Policy = "SomenteFuncionario")]
public class FuncionarioGetAll
{
    public static string Template => "/funcionarios"; 
    public static string [] Methods => new string [] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;
    public static async Task<IResult> Action(int? page, int? rows, QueryAllUsersWithClaims query) {
        if (page == null || page < 0 || rows == null || rows < 0 || rows > 10) {
            return Results.BadRequest("Preencha os campos de page e rows (> 0), Rows não pode ser maior que 10");
        }
        var result = await query.Execute(page.Value, rows.Value);
        return Results.Ok(result);
    }
}
