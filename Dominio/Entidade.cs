namespace LojaAPI.Dominio;

public abstract class Entidade : Notifiable<Notification> //Flunt para validação
{
    public Entidade()
    {
        Id = Guid.NewGuid(); 
    }
    public Guid Id { get; set; }
    public string CriadoPor { get; set; }
    public DateTime CriadoEm { get; set; }
    public string EditadoPor { get; set; }
    public DateTime EditadoEm { get; set; }
}
