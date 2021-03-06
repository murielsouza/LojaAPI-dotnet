namespace LojaAPI.Infra.Database;

public class QueryAllUsersWithClaims
{
    private readonly IConfiguration configuration;

    public QueryAllUsersWithClaims(IConfiguration configuration)
    {
        this.configuration = configuration;
    }
    public async Task<IEnumerable<FuncionarioResponse>> Execute(int page, int rows) {
        var db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        var query = @"SELECT Email, ClaimValue as Nome FROM AspNetUsers u INNER JOIN AspNetUserClaims 
                    c on u.id = c.UserId and ClaimType = 'Nome' ORDER BY Nome 
                    OFFSET (@page -1) * @rows ROWS FETCH NEXT @rows ROWS ONLY";
        return await db.QueryAsync<FuncionarioResponse>(query, new { page, rows });
    }
}
