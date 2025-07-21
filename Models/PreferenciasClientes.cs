namespace RecomendadorViagens.Models;

public class PreferenciasCliente
{
    public DateTime DataViagem { get; set; }
    public List<string> Preferencias { get; set; } = new();
}