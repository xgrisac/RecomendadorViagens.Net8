using RecomendadorViagens.Models;
using RecomendadorViagens.Services;

/* "Neste protótipo, optamos por um sistema baseado em regras, sem IA, mas estruturado de forma que uma futura integração com modelos 
 de linguagem (como OpenAI) possa ser feita com facilidade."
*/

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("🧳 Bem-vindo ao Recomendador Inteligente de Destinos de Viagem!\n");

bool continuar = true;

while (continuar)
{
    DateTime dataViagem;
    while (true)
    {
        Console.Write("Informe a data da viagem (dd/mm/aaaa): ");
        var inputData = Console.ReadLine();

        if (DateTime.TryParseExact(inputData, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dataViagem))
            break;
        else
            Console.WriteLine("Data inválida, por favor digite no formato dd/mm/aaaa.");
    }

    List<string> preferencias;
    while (true)
    {
        Console.Write("Informe suas preferências (clima, cultura, natureza, gastronomia), separadas por vírgula: ");
        var preferenciasInput = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(preferenciasInput))
        {
            preferencias = preferenciasInput.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                           .Select(p => p.Trim().ToLower())
                                           .ToList();

            if (preferencias.Count > 0)
                break;
        }
        Console.WriteLine("Por favor, informe ao menos uma preferência válida.");
    }

    var cliente = new PreferenciasCliente
    {
        DataViagem = dataViagem,
        Preferencias = preferencias
    };

    var recomendador = new RecomendadorService();
    var recomendados = recomendador.Recomendar(cliente);

    if (recomendados.Count == 0)
    {
        Console.WriteLine("\n❌ Nenhum destino foi encontrado para suas preferências.");
    }
    else
    {
        Console.WriteLine("\n📍 Destinos recomendados:");
        foreach (var destino in recomendados)
        {
            Console.WriteLine($"\n🌆 {destino.Nome}");
            Console.WriteLine(" - Atrativos:");
            destino.Atrativos.ForEach(a => Console.WriteLine($"   • {a}"));
            Console.WriteLine(" - Justificativa: Combina com suas preferências: " +
                              string.Join(", ", destino.Categorias.Intersect(preferencias, StringComparer.OrdinalIgnoreCase)));
        }

        var melhorDestino = recomendador.RecomendacaoFinal(recomendados, cliente);
        Console.WriteLine($"\n🏆 Recomendação final: {melhorDestino.Nome}");
        Console.WriteLine($"Critério: maior compatibilidade com suas preferências.");
    }

    Console.WriteLine("\nDeseja consultar uma nova viagem? (s/n): ");
    var resposta = Console.ReadLine()?.Trim().ToLower();
    continuar = resposta == "s" || resposta == "sim";
}

Console.WriteLine("\nObrigado por usar nosso sistema de recomendações. Boa viagem! ✈️");