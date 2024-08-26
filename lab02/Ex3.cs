using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;


class Program
{
   /// <summary>
   /// Usa o LINQ para fazer a contagem de palavras em um texto com CaseInsensitive
   /// </summary>
   private int ContaPalavras(string palavraChave, string texto)
   {
       return texto.Split(' ').Count(palavra => palavra.Equals(palavraChave, StringComparison.OrdinalIgnoreCase));
   }


   static void Main(string[] args)
   {
       string fileName = Console.ReadLine();
       int N = int.Parse(Console.ReadLine());

       string[] words = new string[N];

       for (int i = 0; i < N; i++)
       {
           words[i] = Console.ReadLine();
       }

       string text;
       try
       {
           text = File.ReadAllText(fileName);
       }
       catch (Exception e)
       {
           Console.WriteLine("Erro ao ler o arquivo: ", e.Message);
           return;
       }

       // Continue a Implementação
       // ...
       Program p = new();
       Stopwatch timer = new();
       IList<Task> tasks = new List<Task>();

       timer.Start();
       foreach (string word in words) //Cada contagem de palavra irá ser executada em uma Task independente
       {
           var t = Task.Run(() =>
           {
               var total = p.ContaPalavras(word, text);
               Console.WriteLine($"{word} ({total})");
           });
           tasks.Add(t);
       }
       Task.WaitAll(tasks.ToArray()); //Irá esperar todas as Tasks terminarem a execução para seguir

       timer.Stop();
       Console.WriteLine($"Em {timer.ElapsedMilliseconds}ms");
   }
}