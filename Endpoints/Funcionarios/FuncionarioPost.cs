using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace LojaAPI.Endpoints.Funcionarios;

public class FuncionarioPost //convenção: recurso  + metodo de acesso para cadastrar categoria
{
    public static string Template => "/funcionarios"; //=> indica que está setando ao criar a propriedade template 
    public static string [] Methods => new string [] { HttpMethod.Post.ToString() }; //forma de acesso somente método post
    public static Delegate Handle => Action;

    public static IResult Action(FuncionarioRequest funcionarioRequest, UserManager<IdentityUser> userManager) {
       var user = new IdentityUser { UserName = funcionarioRequest.Email, Email = funcionarioRequest.Email};
       var result = userManager.CreateAsync(user, funcionarioRequest.Senha).Result; //senha passa por aqui
        if (!result.Succeeded) {
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());
        }
        var userClaims = new List<Claim> {
            new Claim("CodigoFuncionario", funcionarioRequest.funcionarioCodigo),
            new Claim("Nome", funcionarioRequest.Nome)
        };
        var claimsResult = userManager.AddClaimsAsync(user, userClaims).Result; //Add varias claims ao mesmo tempo
        if (!claimsResult.Succeeded) {
            return Results.ValidationProblem(claimsResult.Errors.ConvertToProblemDetails());
        }
       // claimResult = userManager.AddClaimAsync(user, new Claim("Nome", funcionarioRequest.Nome)).Result; //Add atributo especifico de funcionário

        return Results.Created($"/funcionarios/{user.Id}", user.Id);
    }
}
