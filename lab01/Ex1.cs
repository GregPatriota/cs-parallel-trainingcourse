using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

class ProgramEx1
{
    static void Main(string[] args)
    {
        // Ler o valor de N da entrada
        int N = int.Parse(Console.ReadLine());

        // Criar um array de strings de tamanho N
        string[] frases = new string[N];

        // Ler as N frases da entrada e salvá-las no array
        for (int i = 0; i < N; i++)
        {
            frases[i] = Console.ReadLine();
        }

        // Continue a Implementação (Criar as threads e etc)
        // ...
        Barrier barrier = new Barrier(N); //Uso de uma barreira para garantir que todas as threads terminarão juntas e ninguém ficará para trás

        foreach (string frase in frases)
        {
            string local = frase;
            new Thread(() =>
            {
                Console.WriteLine($"Thread {Environment.CurrentManagedThreadId}: {local}");
                barrier.SignalAndWait();
            }).Start();
        }
    }
}