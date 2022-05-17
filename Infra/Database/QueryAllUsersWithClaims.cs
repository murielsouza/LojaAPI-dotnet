namespace LojaAPI.Infra.Database;

public class QueryAllUsersWithClaims
{
    private readonly IConfiguration configuration;

    public QueryAllUsersWithClaims(IConfiguration configuration)
    {
        this.configuration = configuration;
    }
    public void Execute(int page, int rows) { 
        
    }
}
