using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("🔑 Digite sua API Key da Cohere:");
        var apiKey = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            Console.WriteLine("❌ API Key inválida. Encerrando o programa.");
            return;
        }

        var cohere = new CohereService(apiKey);

        // 📅 Entrada da data da viagem
        DateTime dataViagem;
        while (true)
        {
            Console.Write("\n📅 Informe a data da viagem (dd/mm/aaaa): ");
            var inputData = Console.ReadLine();

            if (DateTime.TryParseExact(inputData, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dataViagem))
                break;
            else
                Console.WriteLine("❌ Data inválida! Por favor, digite no formato dd/mm/aaaa.\n");
        }

        // 🌎 Lista pré-definida de preferências
        string[] opcoes = {
            "Clima quente",
            "Clima frio",
            "Cultura local",
            "Natureza",
            "Gastronomia",
            "Aventura",
            "Relaxamento",
            "História"
        };

        Console.WriteLine("\n🌍 Escolha suas preferências de viagem (digite os números separados por vírgula):\n");

        for (int i = 0; i < opcoes.Length; i++)
        {
            Console.WriteLine($"  {i + 1}. {opcoes[i]}");
        }

        List<string> preferencias = new();

        // 🎯 Entrada guiada das preferências
        while (true)
        {
            Console.Write("\n👉 Selecione suas preferências: ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("❌ Por favor, informe pelo menos uma opção.");
                continue;
            }

            var indices = input.Split(',', StringSplitOptions.RemoveEmptyEntries);
            bool valido = true;

            foreach (var indiceStr in indices)
            {
                if (int.TryParse(indiceStr.Trim(), out int indice) && indice >= 1 && indice <= opcoes.Length)
                {
                    preferencias.Add(opcoes[indice - 1]);
                }
                else
                {
                    valido = false;
                    break;
                }
            }

            if (valido && preferencias.Count > 0)
                break;

            preferencias.Clear();
            Console.WriteLine("❌ Entrada inválida. Digite os números separados por vírgula (exemplo: 1, 3, 5).");
        }

        // 💡 Construindo o prompt final
        var prompt = $@"
        Sugira 3 destinos de viagem para a data {dataViagem:dd/MM/yyyy}.
        Considere que o viajante valoriza: {string.Join(", ", preferencias)}.
        Inclua uma breve justificativa para cada destino, destacando o motivo pelo qual ele se encaixa nessas preferências.
        Responda em português, de forma clara e envolvente.
        ";

        Console.WriteLine("\n🚀 Gerando sugestões de viagem com a IA...\n");

        try
        {
            var resposta = await cohere.GenerateTextAsync(prompt);

            Console.WriteLine("🤖 Sugestões da IA:\n");
            Console.WriteLine(resposta);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ Erro ao chamar a API: {ex.Message}");
        }

        Console.WriteLine("\n✅ Pressione qualquer tecla para sair...");
        Console.ReadKey();
    }
}
