namespace LojaAPI.Endpoints.Categorias;

public class CategoriaResponse //entidade que será retornada para o usuário, ocultar coisas desnecessárias para ele
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public bool Ativo { get; set; }
}