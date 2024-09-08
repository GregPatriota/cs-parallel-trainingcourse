using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
   private static long totalVotosA = 0, totalVotosB = 0;

   private static string revelarResultado()
   {
       return totalVotosA > totalVotosB ? "A": "B";
   }

   static void Main(string[] args)
   {
       // Leitura do número de regiões
       int R = int.Parse(Console.ReadLine());
       List<Task> eleicao = new();

       var votos = new List<(int votosChapaA, int votosChapaB)>();

       // Leitura dos votos em cada região
       for (int i = 0; i < R; i++)
       {
           string[] entrada = Console.ReadLine().Split();
           int votosRegiaoA = int.Parse(entrada[0]);
           int votosRegiaoB = int.Parse(entrada[1]);
           votos.Add((votosRegiaoA, votosRegiaoB));
       }

       // Continue a implementação
       foreach (var voto in votos)
       {
           Task tA = Task.Run(() => {
               Task.Delay((new Random()).Next(1, 250)).Wait();
               Interlocked.Add(ref totalVotosA, voto.votosChapaA);
           });

           Task tB = Task.Run(() => {
               Task.Delay((new Random()).Next(1, 250)).Wait();
               Interlocked.Add(ref totalVotosB, voto.votosChapaB);
           });

           eleicao.Add(tA);
           eleicao.Add(tB);
       }
       Task.WaitAll(eleicao.ToArray());

       Console.WriteLine($"Chapa A: {totalVotosA} Votos\nChapa B: {totalVotosB} Votos");
       Console.WriteLine($"A chapa {revelarResultado()} venceu a eleição!");
   }
}
