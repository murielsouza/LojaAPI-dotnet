using Dapper;
using Microsoft.Data.SqlClient;

namespace LojaAPI.Endpoints.Funcionarios;

public class FuncionarioGetAll
{
    public static string Template => "/funcionarios"; 
    public static string [] Methods => new string [] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;
    public static IResult Action(int? page, int? rows, IConfiguration configuration) {
        var db = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
        var query = @"SELECT Email, ClaimValue as Nome FROM AspNetUsers u INNER JOIN AspNetUserClaims 
                    c on u.id = c.UserId and ClaimType = 'Nome' ORDER BY Nome 
                    OFFSET (@page -1) * @rows ROWS FETCH NEXT @rows ROWS ONLY";
        //OFFSET é para fazer paginação utilizando o Dapper
        //vantagem Dapper já transforma para um objeto nesse caso a colsulta virará um objeto do tipo FuncionarioResponse
        var funcionarios = db.Query<FuncionarioResponse>(query, new {page, rows}); //objeto anonimo para passar os parametros para a consulta SQL Query, tem q ter o mesmo nome do @ dentro da consulta
        return Results.Ok(funcionarios);
    }
}
