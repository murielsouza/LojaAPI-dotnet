namespace LojaAPI.Endpoints.Clientes;
public class ClientePost 
{
    public static string Template => "/clientes";
    public static string [] Methods => new string [] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action(ClienteRequest clienteRequest, UsuarioCreator usuarioCreator) {
        var userClaims = new List<Claim>
        {
            new Claim("Cpf", clienteRequest.Cpf),
            new Claim("Nome", clienteRequest.Nome),
        };
        (IdentityResult identity, string userId) result = 
            await usuarioCreator.Create(clienteRequest.Email, clienteRequest.Senha, userClaims);
        if (!result.identity.Succeeded)
        {
            return Results.ValidationProblem(result.identity.Errors.ConvertToProblemDetails());
        }
        return Results.Created($"/clientes/{result.userId}", result.userId);
    }
}
