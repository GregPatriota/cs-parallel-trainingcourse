using System;
using System.Diagnostics;
using System.Threading.Tasks;


/**
* 
* .      /)─―ヘ/)
*  　 ＿／　　　　＼
*   ／　　　　●　　　●
*  ｜　　　　　　　▼ |
*  ｜　　　　　　　 ノ 　
*__ U￣U￣￣￣U￣U
* */
class Program
{
   /// <summary>
   /// Calcula a média dos valores de um dado array de valores
   /// </summary>
   private double Media(double[] sequence)
   {
       double total = 0;
       for (int i = 0; i < sequence.Length; i++)
       {
           total += sequence[i];
       }

       return total / sequence.Length;
   }

   /// <summary>
   /// Calcula a mediana dos valores de um dado array de valores
   /// </summary>
   private double Mediana(double[] sequence)
   {
       Array.Sort(sequence);

       return (sequence.Length % 2 == 1) ?
           sequence[sequence.Length / 2] :
           (sequence[sequence.Length / 2 - 1] + sequence[sequence.Length / 2]) / 2;
   }

   /// <summary>
   /// Calcula a variancia dos valores de um dado array de valores e a média deste array
   /// </summary>
   private double Variancia(double[] sequence, double media)
   {
       double totalDosQuadrados = 0;
       for (int i = 0; i < sequence.Length; i++)
       {
           totalDosQuadrados += Math.Pow(sequence[i] - media, 2);
       }
       return totalDosQuadrados / sequence.Length;
   }

   /// <summary>
   /// Calcula o desvio padrão dado a variância de um array de valores
   /// </summary>
   private double DesvioPadrao(double variancia)
   {
       return Math.Sqrt(variancia);
   }


   static void Main(string[] args)
   {
       // Ler N da entrada
       int N = int.Parse(Console.ReadLine());

       double[] sequence = new double[N];

       // Inicializar o array com reais aleatórios entre 0 e 100
       Random rand = new Random();
       for (int i = 0; i < N; i++)
       {
           sequence[i] = rand.NextDouble() * 100;
       }

       // Continue a Implementação
       // ...
       Stopwatch timer = new();
       Program p = new();

       timer.Start();
       var s_media = p.Media(sequence);
       var s_mediana = p.Mediana(sequence);
       var s_variancia = p.Variancia(sequence, s_media);
       var s_desvio = p.DesvioPadrao(s_variancia);
       timer.Stop();
       var s_time = timer.ElapsedMilliseconds;

       timer = new();
       timer.Start();
       var media = Task.Run(() => p.Media(sequence)); //Executa o calculo de média em uma thread separada, não há dependencia de valores anteriores
       var mediana = Task.Run(() => p.Mediana(sequence));//Executa o calculo da mediana em uma thread separada, não há dependencia de valores anteriores
       var variancia = Task.Run(() => p.Variancia(sequence, media.Result)); //Executa o calculo de variância em uma thread separada, mas bloqueará a execução até que o resultado de média esteja pronto
       var desvio = Task.Run(() => p.DesvioPadrao(variancia.Result)); //Executa o calculo de desvio padrão em uma thread separada, mas bloqueará a execução até que o resultado de variância esteja pronto
       desvio.Wait(); //Aguarda o resultado da thread de Desvio Padrão esteja pronto
       timer.Stop();

       var p_time = timer.ElapsedMilliseconds;

       Console.WriteLine($"Media = {media.Result}\nMediana = {mediana.Result}\nVariancia = {variancia.Result}\nDesvio Padrao = {desvio.Result}");
       Console.WriteLine($"Tempo Total = {s_time}/{p_time}ms");

   }
}