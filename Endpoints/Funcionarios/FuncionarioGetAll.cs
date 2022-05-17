using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace LojaAPI.Endpoints.Funcionarios;

public class FuncionarioGetAll
{
    public static string Template => "/funcionarios"; 
    public static string [] Methods => new string [] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;
    public static IResult Action(int page, int rows, UserManager<IdentityUser> userManager) {
       // var users = userManager.Users.ToList();
       var users = userManager.Users.Skip((page - 1) * rows).Take(rows).ToList(); //paginação
        var funcionarios = new List<FuncionarioResponse>();
        foreach (var user in users) { //muitos acessos, corrigir com paginação 
            var claims = userManager.GetClaimsAsync(user).Result;
            var claimName = claims.FirstOrDefault(c => c.Type == "Nome");
            var username = claimName != null ? claimName.Value : string.Empty;
            funcionarios.Add(new FuncionarioResponse(user.Email, username));
        }
        return Results.Ok(funcionarios);
    }
}
