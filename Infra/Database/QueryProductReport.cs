namespace LojaAPI.Infra.Database;

public class QueryProductReport
{
    private readonly IConfiguration configuration;

    public QueryProductReport(IConfiguration configuration)
    {
        this.configuration = configuration;
    }
    public async Task<IEnumerable<ProductReportResponse>> Execute() { //Campos tem que ter o mesmo nome do Response (eg. O nome do campo p.Id é Id, ou seja, o mesmo do ProductReportResponse.cs)
        var db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
        var query = @"SELECT p.Id, p.Nome, COUNT(*) Qtd FROM
                        PedidoProdutos pp
                        INNER JOIN Produtos p ON p.Id = pp.ProdutosID
                        GROUP BY p.Id, p.Nome
                        ORDER BY Qtd desc";
        return await db.QueryAsync<ProductReportResponse>(query);
    }
}
