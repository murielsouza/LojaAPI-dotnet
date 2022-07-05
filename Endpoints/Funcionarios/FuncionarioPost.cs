namespace LojaAPI.Endpoints.Funcionarios;

public class FuncionarioPost //convenção: recurso  + metodo de acesso para cadastrar categoria
{
    public static string Template => "/funcionarios"; //=> indica que está setando ao criar a propriedade template 
    public static string [] Methods => new string [] { HttpMethod.Post.ToString() }; //forma de acesso somente método post
    public static Delegate Handle => Action;

    [Authorize(Policy = "SomenteFuncionario")]
    public static async Task<IResult> Action(FuncionarioRequest funcionarioRequest, HttpContext http, UsuarioCreator usuarioCreator) {
        var userId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var userClaims = new List<Claim> {
            new Claim("CodigoFuncionario", funcionarioRequest.funcionarioCodigo),
            new Claim("Nome", funcionarioRequest.Nome),
            new Claim("CriadoPor", userId)
        };
        (IdentityResult identity, string userId) result =
            await usuarioCreator.Create(funcionarioRequest.Email, funcionarioRequest.Senha, userClaims);
        if (!result.identity.Succeeded)
        {
            return Results.ValidationProblem(result.identity.Errors.ConvertToProblemDetails());
        }
        return Results.Created($"/funcionarios/{result.userId}", result.userId);
    }
}
