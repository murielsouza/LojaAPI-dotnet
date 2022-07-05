namespace LojaAPI.Dominio.Usuarios;

public class UsuarioCreator
{
    private readonly UserManager<IdentityUser> _userManager;

    public UsuarioCreator(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<(IdentityResult, string)> Create(string email, string senha, List<Claim> claims)
    {
        var newUser = new IdentityUser { UserName = email, Email = email };
        var result = await _userManager.CreateAsync(newUser, senha);
        if (!result.Succeeded)
        {
            return (result, String.Empty);
        }
        return (await _userManager.AddClaimsAsync(newUser, claims), newUser.Id);
    }
}
