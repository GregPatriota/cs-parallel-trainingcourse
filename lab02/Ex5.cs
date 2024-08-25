using System;
using System.Dynamic;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
    private void EscreveNovoArquivo(PokemonDTO pokemon)
    {
        string fileName = "pokedex.txt";

        using (StreamWriter stream = new(fileName))
        {
            stream.WriteLine(pokemon.ToString());
        }
    }

    private void EscreveNovoArquivo(IList<PokemonDTO> pokemons)
    {
        string fileName = "pokedex.txt";

        using (StreamWriter stream = new(fileName))
        {
            foreach (PokemonDTO pokemon in pokemons)
            {
                stream.WriteLine(pokemon.ToString());
            }
        }
    }

    private void EscreveNoArquivo(PokemonDTO pokemon)
    {
        string fileName = "pokedex.txt";

        using (StreamWriter stream = File.AppendText(fileName))
        {
            stream.WriteLine(pokemon.ToString());
        }
    }

    private void EscreveNoArquivo(IList<PokemonDTO> pokemons)
    {
        string fileName = "pokedex.txt";

        using (StreamWriter stream = File.AppendText(fileName))
        {
            foreach (PokemonDTO pokemon in pokemons)
            {
                stream.WriteLine(pokemon.ToString());
            }
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

        //var c = p.GetPokemon(pokemonIds[0]);
        var c = p.GetPokemonAsync(pokemonIds[0]).Result;

        Console.WriteLine(c);
    }
}