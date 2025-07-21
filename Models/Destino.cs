namespace RecomendadorViagens.Models;

public class Destino
{
    public string Nome { get; set; }
    public List<string> Categorias { get; set; } = new();
    public List<string> Atrativos { get; set; } = new();

    public Destino(string nome, List<string> categorias, List<string> atrativos)
    {
        Nome = nome;
        Categorias = categorias;
        Atrativos = atrativos;
    }
}
