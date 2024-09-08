using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
   private ReaderWriterLockSlim lockSlim => new ReaderWriterLockSlim();
   private double temperatura = 0;

   private void AtualizaSensor(int qtdAtualizacoes)
   {
       for (int i = 0; i < qtdAtualizacoes; i++)
       {
           Random r = new();
           Task.Delay(r.Next(1, 91)).Wait();
           try
           {
               lockSlim.EnterWriteLock();
               temperatura = r.Next(0, 100) + r.NextDouble();
               Console.WriteLine($"[Sensor] Temperatura atualizada: {temperatura:0.00}ºC");
           }
           finally
           {
               if(lockSlim.IsWriteLockHeld)
                   lockSlim.ExitWriteLock();
           }
       }
   }

   private void LerSensor(int usuarioID, int usuarioLeituras)
   {
       for (int i = 0; i < usuarioLeituras; i++)
       {
           Task.Delay((new Random()).Next(1, 87)).Wait();
           try
           {
               lockSlim.EnterReadLock();
               Console.WriteLine($"Usuário {usuarioID}: Temperatura lida: {temperatura:0.00}ºC");
           }
           finally
           {
               if(lockSlim.IsReadLockHeld)
                   lockSlim.ExitReadLock();
           }
       }
   }

   static void Main(string[] args)
   {

       // Obter a quantidade de usuários e atualizações do sensor
       string[] entrada = Console.ReadLine().Split();
       int usuarios = int.Parse(entrada[0]);
       int atualizacoes = int.Parse(entrada[1]);

       // Obter a quantidade de leituras realizadas por cada usuário
       int[] leituras = new int[usuarios];
       for (int i = 0; i < usuarios; i++)
       {
           leituras[i] = int.Parse(Console.ReadLine());
       }

       // Continue a implementação
       Program p = new();
       List<Task> conjuntoUsuarios = new();
       foreach (var (valor, index) in leituras.Select((valor, index) => (valor, index+1)))
       {
           Task u = Task.Run(() =>
           {
               p.LerSensor(index, valor);
           });
           conjuntoUsuarios.Add(u);
       }

       Task s = Task.Run(() => 
       { 
           p.AtualizaSensor(atualizacoes);
       });

       Task.WaitAll(conjuntoUsuarios.ToArray()); //talvez precise adicionar a task s aqui também no wait
   }
}
