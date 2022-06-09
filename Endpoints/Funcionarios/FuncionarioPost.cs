namespace LojaAPI.Endpoints.Funcionarios;

public class FuncionarioPost //convenção: recurso  + metodo de acesso para cadastrar categoria
{
    public static string Template => "/funcionarios"; //=> indica que está setando ao criar a propriedade template 
    public static string [] Methods => new string [] { HttpMethod.Post.ToString() }; //forma de acesso somente método post
    public static Delegate Handle => Action;

    [Authorize(Policy = "SomenteFuncionario")]
    public static async Task<IResult> Action(FuncionarioRequest funcionarioRequest, HttpContext http,UserManager<IdentityUser> userManager) {
        var userId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var newUser = new IdentityUser { UserName = funcionarioRequest.Email, Email = funcionarioRequest.Email};
       var result = await userManager.CreateAsync(newUser, funcionarioRequest.Senha);
        if (!result.Succeeded) {
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());
        }
        var userClaims = new List<Claim> {
            new Claim("CodigoFuncionario", funcionarioRequest.funcionarioCodigo),
            new Claim("Nome", funcionarioRequest.Nome),
            new Claim("CriadoPor", userId)
        };
        var claimsResult = await userManager.AddClaimsAsync(newUser, userClaims);
        if (!claimsResult.Succeeded) {
            return Results.ValidationProblem(claimsResult.Errors.ConvertToProblemDetails());
        }
       // claimResult = userManager.AddClaimAsync(user, new Claim("Nome", funcionarioRequest.Nome)).Result; //Add atributo especifico de funcionário

        return Results.Created($"/funcionarios/{newUser.Id}", newUser.Id);
    }
}
