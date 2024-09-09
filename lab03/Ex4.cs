using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

class Program
{
   private ConcurrentQueue<int> _totalDeSolicitacoes;
   private BlockingCollection<int> _pedidosDisponiveis;

   public Program(int n)
   {
       _totalDeSolicitacoes = new();
       _pedidosDisponiveis = new(n);
   }

   private void CriarSolicitacoes(int solicitacao)
   {
       _totalDeSolicitacoes.Enqueue(solicitacao);
   }

   private void Finalizar()
   {
       _pedidosDisponiveis.CompleteAdding();
   }

   private async Task CriarPedidoAsync(int vendedor)
   {
       Random rnd = new();
       await Task.Delay(rnd.Next(1, 250));
       while (_totalDeSolicitacoes.TryDequeue(out int pedido))
       {
           Console.WriteLine($"Vendedor {vendedor}: Pedido #{pedido:000} criado.");
           _pedidosDisponiveis.Add(pedido);
           await Task.Delay(rnd.Next(1, 500));
       }
   }

   private void EntregarPedido(int entregador)
   {
       foreach (int pedido in _pedidosDisponiveis.GetConsumingEnumerable())
       {
           Console.WriteLine($"Entregador {entregador}: Pedido #{pedido:000} entregue.");
       }
   }

   static void Main(string[] args)
   {
       // Leitura do número total de pedidos (N), vendedores (V) e entregadores (E)
       string[] entrada = Console.ReadLine().Split();
       int N = int.Parse(entrada[0]);
       int V = int.Parse(entrada[1]);
       int E = int.Parse(entrada[2]);

       // Continue a implementação
       Program p = new(N);
       List<Task> todosOsFuncionarios = new();
       List<Task> quadroDeVendedores = new();

       //Criando uma fila de solicitacoes para ser consumida pelos vendedores
       for(int i = 1; i <= N; i++)
       {
           p.CriarSolicitacoes(i);
       }

       //Criando os vendedores que irao consumir de uma fila e alimentar uma Coleção (produtor-consumidor)
       for(int i = 1; i <= V; i++)
       {
           int vendId = i;
           Task vend = Task.Run(async () => { await p.CriarPedidoAsync(vendId); });
           todosOsFuncionarios.Add(vend);
           quadroDeVendedores.Add(vend);
       }

       //Vendedores que irão consumir da coleção (produtor-consumidor)
       for (int i = 1;i <= E; i++)
       {
           int entrId = i;
           Task entr = Task.Run(() => { p.EntregarPedido(entrId); });
           todosOsFuncionarios.Add(entr);
       }
       Task.WhenAll(quadroDeVendedores).ContinueWith((act) => { p.Finalizar(); }); //A estrutura produtor-consumidor só irá parar de ser alimentada quando todas as threads de vendedores finalizar
       Task.WaitAll(todosOsFuncionarios.ToArray()); // Espera todas as threads terminarem
   }
}
