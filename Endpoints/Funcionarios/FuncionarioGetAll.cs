using LojaAPI.Infra.Database;

namespace LojaAPI.Endpoints.Funcionarios;

public class FuncionarioGetAll
{
    public static string Template => "/funcionarios"; 
    public static string [] Methods => new string [] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;
    public static IResult Action(int? page, int? rows, QueryAllUsersWithClaims query) {
        if (page == null || page < 0 || rows == null || rows < 0 || rows > 10) {
            return Results.BadRequest("Preencha os campos de page e rows (> 0), Rows não pode ser maior que 10");
        }
        return Results.Ok(query.Execute(page.Value, rows.Value));
    }
}
