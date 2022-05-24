using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LojaAPI.Endpoints.Segurança;

public class TokenPost 
{
    public static string Template => "/token"; 
    public static string [] Methods => new string [] { HttpMethod.Post.ToString() }; 
    public static Delegate Handle => Action;

    public static IResult Action(LoginRequest loginRequest, UserManager<IdentityUser> userManager) {
        var user = userManager.FindByEmailAsync(loginRequest.Email).Result;
        if (user == null){
            Results.BadRequest("Usuário não encontrado!");
        }
        if (!userManager.CheckPasswordAsync(user, loginRequest.Senha).Result){
            Results.BadRequest("Senha incorreta");
        }
        var key = Encoding.ASCII.GetBytes("A@fderwfQQSDXCCer34");
        var tokenDescription = new SecurityTokenDescriptor { //vai gerar o Token
            Subject = new ClaimsIdentity(new Claim[] {
                new Claim(ClaimTypes.Email, loginRequest.Email),
            }),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature), //assinar credenciais
            Audience = "LojaAPI",
            Issuer = "Issuer"
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescription);
        return Results.Ok(new
        {
            token = tokenHandler.WriteToken(token)
        });
    }
}
