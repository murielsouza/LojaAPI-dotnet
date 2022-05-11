using Flunt.Validations;

namespace LojaAPI.Dominio.Produtos;

public class Categoria : Entidade
{
    public bool Ativo { get; private set; }  //editar somente de dentro da classe (private set)
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
    public void EditarCategoria(string nome, bool ativo) {
        Ativo = ativo;
        Nome = nome;
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