namespace LojaAPI.Endpoints.Produtos;

[Authorize(Policy = "SomenteFuncionario")]
public class RelatorioProdutos
{
    public static string Template => "/produtos/relatorio"; 
    public static string [] Methods => new string [] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;
    public static async Task<IResult> Action(QueryProductReport query) {
        var result = await query.Execute();
        return Results.Ok(result);
    }
}
