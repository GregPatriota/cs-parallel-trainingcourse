using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

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

interface Ex3
{
    IDictionary<int, int> CountFrequency(int[] array);
    void PrintLn();
}

/// <summary>
/// Versão 01 da implementação, usando um recurso compartilhado onde toda as threads competem por este mesmo recurso para escrever o número de ocorrências de cada número
/// Nesta versão, para evitar a condição de corrida e que o resultado no final seja consistente terá uma trava de sincronismo
/// </summary>
class Version01 : Ex3
{
    private static Dictionary<int, int> _sharedDict;
    private static object _lock => new object();

    public Version01()
    {
        _sharedDict = new();
    }
    public IDictionary<int, int> CountFrequency(int[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            AddOnDict(array[i]);
        }

        return _sharedDict;
    }

    private void AddOnDict(int key)
    {
        lock (_lock)
        {
            if (_sharedDict.ContainsKey(key)) _sharedDict[key]++;
            else _sharedDict.Add(key, 1);
        }
    }

    public void PrintLn()
    {
        Console.WriteLine("Output: [");
        foreach (KeyValuePair<int, int> kvp in _sharedDict)
        {
            Console.Write("(" + kvp.Key + ", " + kvp.Value + ") ");
        }
        Console.WriteLine("]");
    }
}

/// <summary>
/// Versão 02 da implementação, cada thread passa a ter seus contadores privados e não há mais competicação para escrever em um mesmo recurso compartilhado, mitigando assim a condição de corrida
/// </summary>
class Version02 : Ex3
{
    public Version02() { }

    public IDictionary<int, int> CountFrequency(int[] array)
    {
        Dictionary<int, int> dict = new();

        for (int i = 0; i < array.Length; i++)
        {
            if (dict.ContainsKey(array[i]))
            {
                dict[array[i]]++;
            }
            else
            {
                dict.Add(array[i], 1);
            }
        }
        return dict;
    }

    public void PrintLn()
    {
        Console.WriteLine("Versão 02");
    }
}

/// <summary>
/// Versão 03 da implementação, tentando fazer um hibrido entre as duas implementações anteriores mas para escrever no recurso compartilhado e fazendo uso de metódos para paralelizar a execução
/// </summary>
class Version03 : Ex3
{
    private static ConcurrentDictionary<int, int> _sharedDict;
    public Version03()
    {
        _sharedDict = new();
    }

    public IDictionary<int, int> CountFrequency(int[] array)
    {
        Parallel.For(0, array.Length, i => {
            if (_sharedDict.ContainsKey(array[i]))
                _sharedDict[array[i]]++;
            else
                _sharedDict.TryAdd(array[i], 1);
        });

        return _sharedDict;
    }

    public void PrintLn()
    {
        Console.WriteLine("Output: [");
        foreach (KeyValuePair<int, int> kvp in _sharedDict)
        {
            Console.Write("(" + kvp.Key + ", " + kvp.Value + ") ");
        }
        Console.WriteLine("]");
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Ler N da entrada
        //int N = int.Parse(Console.ReadLine()); //Trecho comentado para responder aos tópicos 3 e 4 do exercício 3
        int N = 1000000000;

        // Criar o vetor de tamanho N
        int[] vetor = new int[N];

        // Inicializar o vetor com inteiros aleatórios
        Random rand = new Random();
        for (int i = 0; i < N; i++)
        {
            vetor[i] = rand.Next(1, 100);
        }

        // Continue a Implementação
        // ...
        //vetor = vetor.Order().ToArray();
        //int M = 1; // Número de Threads
        int M = int.Parse(Console.ReadLine()); // Número de Threads
        Dictionary<int, int> globalDict = new(); //estrutura de dados compartilhada
        int sumTotal = 0; //Variável compartilhada para acumular o somatório (sem trava para concorrência)
        int startFrom = 0; //Variáveis para controle para dividir o vetor entre as threads
        int rangeByThread = (int)Math.Floor((double)N / M);
        int rangeByThreadModulus = N % M; //Ao menos uma das thread vai ser executada com vetor maior para operar

        //PrintInputLn(vetor);

        Ex3 ex3 = new Version03();
        //Ex3 ex3 = new Version01(); //Cada uma das versões das implementações seguindo a mesma interface
        //Ex3 ex3 = new Version02();
        //Ex3 ex3 = new Version03();

        // Iniciar a medição de tempo
        Stopwatch timer = new Stopwatch();

        timer.Start();

        for (int i = 0; i < M; i++)
        {
            int startFromLoop = startFrom; //Cópia das variaveis para auxiliar na criação das threads dentro do laço
            int rangeByThreadLoop = rangeByThread;
            int rangeByThreadModulusLoop = rangeByThreadModulus;

            var t = new Thread(() => { ex3.CountFrequency(vetor[startFromLoop..(startFromLoop + rangeByThreadLoop + rangeByThreadModulusLoop - 1)]); });
            t.Start();
            t.Join();

            startFrom = ((i + 1) * rangeByThread) + rangeByThreadModulus;
            rangeByThreadModulus = 0; //Ajuste de cálculo para que quando o tamanho da entrada não seja exatamente divisivel pelo numero de threads, apenas a primeira thread ficará um um vetor um pouco maior
        }

        timer.Stop();
        Console.WriteLine("Tempo de execução com " + M + " threads: " + timer.ElapsedMilliseconds + " ms");
        //ex3.PrintLn();
    }

    public static void PrintInputLn(int[] input)
    {
        Console.WriteLine("Input: [");
        for (int i = 0; i < input.Length; i++)
        {
            Console.Write(input[i] + " ");
        }
        Console.WriteLine("]");
    }
}
