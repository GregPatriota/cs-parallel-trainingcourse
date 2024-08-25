using System;
using System.Diagnostics;
using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

class Program
{
    public static int SumArray(int[] array)
    {
        int totalByRange = 0;
        for (int i = 0; i < array.Length; i++)
        {
            totalByRange += array[i];
        }
        return totalByRange;
    }

    static void Main(string[] args)
    {
        // Ler N da entrada
        int N = int.Parse(Console.ReadLine());

        int[] sequence = new int[N];

        // Inicializar o array com inteiros aleatórios
        Random rand = new Random();
        for (int i = 0; i < N; i++)
        {
            sequence[i] = rand.Next(1, 100);
        }

        int M = 4; // Número de Threads

        // Iniciar a contagem do tempo de execução
        Stopwatch timer = new Stopwatch();
        timer.Start();

        // Continue a Implementação
        //...
        int sumTotal = 0; //Variável compartilhada para acumular o somatório (sem trava para concorrência, o que poderá gerar uma condição de corrida)
        int startFrom = 0; //Variáveis para controle para dividir o vetor entre as threads
        int rangeByThread = (int)Math.Floor((double)N / M);
        int rangeByThreadModulus = N % M; //Ao menos uma das thread vai ser executada com vetor maior para operar

        for (int i = 0; i < M; i++)
        {
            int startFromLoop = startFrom; //Cópia das variaveis para auxiliar na criação das threads dentro do laço
            int rangeByThreadLoop = rangeByThread;
            int rangeByThreadModulusLoop = rangeByThreadModulus;

            new Thread(() => { sumTotal += SumArray(sequence[startFromLoop..(startFromLoop + rangeByThreadLoop + rangeByThreadModulusLoop - 1)]); }).Start(); //Uso de range para dividir o vetor
            startFrom = ((i + 1) * rangeByThread) + rangeByThreadModulus;
            rangeByThreadModulus = 0;
        }

        // Finalizar a contagem do tempo de execução
        timer.Stop();
        Console.WriteLine("Tempo de execução com " + M + " threads: " + timer.ElapsedMilliseconds + " ms");
    }

}

