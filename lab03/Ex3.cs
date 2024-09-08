using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
   private readonly SemaphoreSlim _slim;

   public Program(int cozinheiros) 
   {
       _slim = new(cozinheiros);
   }

   private async Task PreparaRangoAsync(string nome, int tempo)
   {
       try
       {
           _slim?.Wait();
           Console.WriteLine($"O prato \'{nome}\' começou a ser preparado.");
           await Task.Delay(tempo);
           Console.WriteLine($"O prato \'{nome}\' está pronto");
       }
       finally
       {
           _slim?.Release();
       }
   }

   static void Main(string[] args)
   {

       // Obter o número de pedidos (P) e de cozinheiros (C)
       string[] entrada = Console.ReadLine().Split();
       int P = int.Parse(entrada[0]);
       int C = int.Parse(entrada[1]);

       var pedidos = new List<(string prato, int tempo)>();

       for (int i = 0; i < P; i++)
       {
           string[] pedido = Console.ReadLine().Split(',');
           string prato = pedido[0].Trim();
           int tempo = int.Parse(pedido[1].Trim());
           pedidos.Add((prato, tempo));
       }

       // Continue a implementação
       Program p = new(C);
       List<Task> tasks = new();

       foreach (var (prato, tempo) in pedidos)
       {
           Task t =  p.PreparaRangoAsync(prato, tempo);
           tasks.Add(t);
       }

       Task temp = Task.WhenAll(tasks.ToArray());
       temp.Wait();
   }
}
