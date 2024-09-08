using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
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
class Program
{
   /// <summary>
   /// Sequencial:
   /// Testa todos os números entre 1 e k que são primos
   /// </summary>
   private IList<int> CalculaNumeroPrimos(int k)
   {
       return Enumerable.Range(1, k).Where(el => VerificaPrimo(el)).ToList();
   }

   /// <summary>
   /// Método otimizado para verificar se um dado número é primo ou não
   /// </summary>
   private bool VerificaPrimo(int current_k)
   {
       if (current_k == 2) //Caso base 01: 2 é primo!
           return true;
       if (current_k <= 1 || current_k % 2 == 0) //Caso base 02: 1 não é primo e números pares (diferente de 2) também não
           return false;

       for (int i = 3; i <= Math.Sqrt(current_k); i += 2) //Dentre os números impares, se há algum que divida o número testado
       {
           if (current_k % i == 0)
               return false;
       }
       return true;
   }

   private static void ImprimeResultado(IList<int> vetorResultado)
   {
       Console.WriteLine($"[{vetorResultado.Select((el) => el.ToString()).Aggregate((acc, cur) => $"{acc}, {cur}")}]");
   }

   static void Main(string[] args)
   {
       int N = int.Parse(Console.ReadLine());
       string[] input = Console.ReadLine().Split(' ');
       int[] sequence = new int[N];

       for (int i = 0; i < N; i++)
       {
           sequence[i] = int.Parse(input[i]);
       }

       // Continue a Implementação
       // ...
       Program p = new();
       Stopwatch timer = new();
       timer.Start();
       foreach (int i in sequence)
       {
           var l = p.CalculaNumeroPrimos(i);
           ImprimeResultado(l);
       }
       timer.Stop();
       long timer_base = timer.ElapsedMilliseconds;

       //Com Task:
       timer.Restart();
       IList<Task> tasks = new List<Task>();
       foreach (int i in sequence)
       {
           var t = Task.Run(() =>
           {
               var l = p.CalculaNumeroPrimos(i);
               ImprimeResultado(l);
           });
           tasks.Add(t);
       }
       Task.WaitAll(tasks.ToArray());
       timer.Stop();
       long timer_task = timer.ElapsedMilliseconds;

       //Com Parallel.For:
       timer.Restart();
       Parallel.For(0, sequence.Length, (index) =>
       {
           var l = p.CalculaNumeroPrimos(sequence[index]);
           ImprimeResultado(l);
       });
       timer.Stop();
       long timer_parallelFor = timer.ElapsedMilliseconds;

       //Com Parallel.ForEach
       timer.Restart();
       Parallel.ForEach(sequence, (el) =>
       {
           var l = p.CalculaNumeroPrimos(el);
           ImprimeResultado(l);
       });
       timer.Stop();
       long timer_parallelForEach = timer.ElapsedMilliseconds;

       Console.WriteLine($"Base: {timer_base}ms, Task: {timer_task}ms, For: {timer_parallelFor}ms, ForEach: {timer_parallelForEach}ms");
   }
}