using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Dynamic;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

/**
* //////////////////////////////////
* // As avaliações e observações //
* // das implementações estão no //
* //   documento em anexado      //
* /////////////////////////////////
* .       (\__/)  ||
* .       (•ㅅ•)  ||
* .       ( 　 づ || 
**/
internal record PokemonDTO
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string Nome { get; set; }
    [JsonPropertyName("types")]
    public IList<PokemonTiposDTO> Tipos { get; set; }

    public override string ToString()
    {
        return $"{Nome}, {Tipos.Select(el => el.ToString()).Aggregate((acc, x) => $"{acc}, {x}")}";
    }
}

internal record PokemonTiposDTO
{
    [JsonPropertyName("type")]
    public PokemonTipoDTO Tipo { get; set; }

    public override string ToString()
    {
        return $"{Tipo}";
    }
}

internal record PokemonTipoDTO
{
    [JsonPropertyName("name")]
    public string Nome { get; set; }

    public override string ToString()
    {
        return Nome;
    }
}


class Program
{
    private readonly object _lock = new();

    private void EscreveNoArquivo(PokemonDTO pokemon)
    {
        string fileName = "pokedex.txt";

        lock(_lock)
        {
            using (StreamWriter stream = File.AppendText(fileName))
            {
                stream.WriteLine(pokemon.ToString());
            }
        }
    }

    private void EscreveNoArquivo(string pokemons)
    {
        string fileName = "pokedex.txt";

        using (StreamWriter stream = File.AppendText(fileName))
        {
            stream.WriteLine(pokemons);
        }
    }

    private PokemonDTO GetPokemon(int id)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://pokeapi.co/api/v2/pokemon/{id}/");
        request.Method = "GET";

        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        {
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string result = reader.ReadToEnd();
                return JsonSerializer.Deserialize<PokemonDTO>(result);
            }
        }
    }

    private async Task<PokemonDTO> GetPokemonAsync(int id)
    {
        using (HttpClient client = new HttpClient())
        {
            client.BaseAddress = new Uri("https://pokeapi.co/api/v2/");
            HttpResponseMessage response = await client.GetAsync($"pokemon/{id}/");

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<PokemonDTO>(result);
            }
            else
            {
                return null;
            }
        }
    }

    static void Main(string[] args)
    {
        string[] input = Console.ReadLine().Split(' ');
        int P = input.Length;
        int[] pokemonIds = new int[P];

        for (int i = 0; i < P; i++)
        {
            pokemonIds[i] = int.Parse(input[i]);
        }

        // Continue a Implementação
        // ...
        Program p = new();
        ConcurrentBag<PokemonDTO> pokemonBag = new();
        List<Task> tasks = new();
        Stopwatch timer = new();

        //Versão sem Paralelismo/Concorrência
        timer.Start();
        foreach(var id in pokemonIds)
        {
            p.EscreveNoArquivo(p.GetPokemon(id));
        }
        timer.Stop();
        long tempoSequencial = timer.ElapsedMilliseconds;

        p.EscreveNoArquivo("\n-----------------\n");

        //Versão com Parallel.For ou Parallel.ForEach
        timer = new();
        timer.Start();
        Parallel.ForEach(pokemonIds, id => 
        {
            pokemonBag.Add(p.GetPokemon(id));
        });
        p.EscreveNoArquivo(pokemonBag.Select(el => el.ToString()).Aggregate((acc, nxt) => acc + "\n" + nxt));
        timer.Stop();
        long tempoParallel = timer.ElapsedMilliseconds;

        p.EscreveNoArquivo("\n-----------------\n");

        //Versão com async/await
        timer = new();
        timer.Start();
        foreach (var id in pokemonIds)
        {
            var t = Task.Run(async () =>
            {
                var pokemonDTO = p.GetPokemonAsync(id);
                p.EscreveNoArquivo(await pokemonDTO);
            });
            tasks.Add(t);
        }
        Task.WaitAll(tasks.ToArray());

        timer.Stop();
        long tempoAsyncAwait = timer.ElapsedMilliseconds;

        Console.WriteLine($"Senquencial: {tempoSequencial}ms, Parallel: {tempoParallel}ms, Async/Await: {tempoAsyncAwait}ms");
    }
}