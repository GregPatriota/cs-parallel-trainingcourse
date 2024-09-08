using System;
using System.Diagnostics;
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

/// <summary>
/// A partir de números muito grandes já começo a ver uma grande melhoria, precisa ser >= 20.000.000.
/// </summary>
class Program
{
   private readonly static object _lock = new object();

   /// <summary>
   /// Executa a soma de todos os quadrados até o valor máximo totalNumeros de forma sequencial
   /// </summary>
   private double Sequencial(int totalNumeros)
   {
       double acumulador = 0;

       for (int i = 1; i <= totalNumeros; i++)
       {
           acumulador += i ^ 2;
       }

       return acumulador;
   }

   /// <summary>
   /// Executa a soma de todos os quadrados até o valor máximo totalNumeros de forma paralela, explorando o máximo de threads
   /// </summary>
   private double Paralelo(int totalNumeros)
   {
       double acumulador = 0;
       int threads = Environment.ProcessorCount;
       double r = totalNumeros / threads;
       int range = (int)Math.Floor(r); //Para obter o melhor desempenho, usaremos todas as threads do processador
       int resto = totalNumeros % threads; //Em caso de não ser uma divisão perfeita, o resto dos números serão atribuidos ao primeiro processo

       Parallel.For(0, threads, (index) =>
       {
           int init = (index * range) + 1 + (index == 0 ? 0 : resto); //O primeiro processo recebe o seu "chunk + a sobra", os demais apenas o chunk
           int end = init + range + (index == 0 ? resto : 0);
           double somaLocal = 0;

           for (int i = init; i < end; i++)
           {
               somaLocal += i ^ 2;
           }
           Incrementa(ref acumulador, somaLocal);
       });

       return acumulador;
   }

   /// <summary>
   /// Metodo de incremeto na variavel compartilhada com uma trava que evita a condição de corrida
   /// </summary>
   /// <param name="dest">Valor que receberá o incremento</param>
   /// <param name="src">Valor a ser adicionado</param>
   private void Incrementa(ref double dest, double src)
   {
       lock (_lock)
       {
           dest += src;
       }
   }

   static void Main(string[] args)
   {
       int N = int.Parse(Console.ReadLine());

       // Continue a Implementação
       // ...
       Program p = new Program();

       Stopwatch timer = new Stopwatch();
       timer.Start();
       double resultSequencial = p.Sequencial(N);
       timer.Stop();
       long t_sequencial = timer.ElapsedMilliseconds;

       timer.Restart();
       double resultParalelo = p.Paralelo(N);
       timer.Stop();
       long t_paralelo = timer.ElapsedMilliseconds;
       //Apenas uma forma de verificar quando os dois métodos chegam ao mesmo resultado, em caso de divergencia é printado o 0(zero)
       double result = (resultSequencial == resultParalelo) ? resultSequencial : 0;

       Console.WriteLine($"Resultado = {result}.\nTempo de execução Sequencial/Paralelo: {t_sequencial}/{t_paralelo}ms, SpeedUp = {((double)t_sequencial/t_paralelo):0.000}");

   }
}