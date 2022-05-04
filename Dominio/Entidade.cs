﻿namespace LojaAPI.Dominio;

public abstract class Entidade
{
    public Entidade()
    {
        Id = Guid.NewGuid(); //criando Id quando estância uma categoria e o seu produto
    }
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string CriadoPor { get; set; }
    public DateTime CriadoEm { get; set; }
    public string EditadoPor { get; set; }
    public DateTime EditadoEm { get; set; }
}
