namespace LojaAPI.Endpoints.Clientes;

public record ClienteRequest(string Email, string Senha, string Nome, string Cpf);
