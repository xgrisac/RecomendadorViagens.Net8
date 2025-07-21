using RecomendadorViagens.Models;

namespace RecomendadorViagens.Services;

public class RecomendadorService
{
    private readonly List<Destino> _destinos;

    public RecomendadorService()
    {
        _destinos = new List<Destino>
        {
            new("Florianópolis",
                new() { "clima", "natureza", "gastronomia" },
                new() { "Praias paradisíacas", "Trilhas ecológicas", "Frutos do mar frescos" }),

            new("Gramado",
                new() { "cultura", "gastronomia", "natureza" },
                new() { "Arquitetura europeia", "Culinária alem�", "Montanhas e lagos" }),

            new("Salvador",
                new() { "cultura", "clima", "gastronomia" },
                new() { "Centro histórico", "Comida baiana", "Praias quentes" }),

            new("Foz do Iguaçu",
                new() { "natureza", "cultura" },
                new() { "Cataratas do Igua�u", "Parque das Aves", "Tr�plice fronteira" }),

            new("São Paulo",
                new() { "gastronomia", "cultura" },
                new() { "Museus", "Restaurantes premiados", "Vida noturna agitada" })
        };
    }

    public List<Destino> Recomendar(PreferenciasCliente cliente)
    {
        var ranqueados = _destinos
            .Select(destino => new
            {
                Destino = destino,
                Pontuacao = destino.Categorias
                    .Count(c => cliente.Preferencias.Contains(c, StringComparer.OrdinalIgnoreCase))
            })
            .Where(d => d.Pontuacao > 0)
            .OrderByDescending(d => d.Pontuacao)
            .Take(3)
            .Select(d => d.Destino)
            .ToList();

        return ranqueados;
    }

    public Destino? RecomendacaoFinal(List<Destino> recomendados, PreferenciasCliente cliente)
    {
        if (recomendados == null || recomendados.Count == 0)
            return null; // ou lance uma exce��o personalizada

        return recomendados
            .OrderByDescending(d =>
                d.Categorias.Count(c => cliente.Preferencias.Contains(c, StringComparer.OrdinalIgnoreCase)))
            .First();
    }
}