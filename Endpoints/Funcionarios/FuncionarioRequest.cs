namespace LojaAPI.Endpoints.Funcionarios;

public record FuncionarioRequest(string Email, string Senha, string Nome, string funcionarioCodigo);