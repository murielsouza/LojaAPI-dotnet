namespace LojaAPI.Dominio.Produtos;

public class Categoria : Entidade
{
    public string Nome { get; private set; }
    public bool Ativo { get; private set; } 
    public Categoria(string nome, string criadoPor, string editadoPor)
    {
        Nome = nome;
        Ativo = true;
        CriadoPor = criadoPor;
        EditadoPor = editadoPor;
        CriadoEm = DateTime.Now;
        EditadoEm =  DateTime.Now;

        Validate();
    }
    public void EditarCategoria(string nome, bool ativo, string editadoPor) {
        Ativo = ativo;
        Nome = nome;
        EditadoPor = editadoPor;
        EditadoEm = DateTime.Now;
        Validate();
    }
    private void Validate()
    {
        var contract = new Contract<Categoria>()
                    .IsNotEmpty(Nome, "Nome", "Campo Nome é obrigatório")
                    .IsNotNullOrWhiteSpace(Nome, "Nome", "Campo Nome é obrigatório")
                    .IsGreaterOrEqualsThan(Nome, 3, "Name", "O nome tem que ter mais de dois caracteres")
                    .IsNotNullOrEmpty(CriadoPor, "CriadoPor", "Campo CriadoPor é obrigatório")
                    .IsNotNullOrEmpty(EditadoPor, "EditadoPor", "Campo EditadoPor é obrigatório");
        AddNotifications(contract);
    }
}