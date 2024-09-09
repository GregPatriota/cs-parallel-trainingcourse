using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    private int _vagasDisponiveis;
    private string _acao;
    private EventWaitHandle _eventWaitHandle;
    private SemaphoreSlim _semaphore;

    public Program(int c)
    {
        _vagasDisponiveis = c;
        _eventWaitHandle = new AutoResetEvent(false);
        _semaphore = new SemaphoreSlim(c);
        _acao = string.Empty;
    }

    private void FiscalDeEstacionamento()
    {
        while (true)
        {
            if(_acao == null) return;

            _eventWaitHandle.WaitOne();
            Console.WriteLine($"Evento: Veículo {_acao}. Vagas disponíveis: {_vagasDisponiveis}");
        }
    }

    private async Task Veiculo(int v)
    {
        try
        {
            Console.WriteLine($"Veículo {v} esperando para entrar...");
            await _semaphore.WaitAsync();
            Console.WriteLine($"Veículo {v} estacionou.");
            Interlocked.Decrement(ref _vagasDisponiveis);
            Interlocked.Exchange(ref _acao, "entrou");
            _eventWaitHandle.Set();
            await Task.Delay(new Random().Next(200, 1000));
        }
        finally
        {
            _semaphore.Release();
            Console.WriteLine($"Veículo {v} saiu.");
            Interlocked.Increment(ref _vagasDisponiveis);
            Interlocked.Exchange(ref _acao, "saiu");
            _eventWaitHandle.Set();
        }
    }

    static void Main()
    {
        // Obter a capacidade do estacionamento (C) e o número de veículos (V)
        string[] entrada = Console.ReadLine().Split();
        int C = int.Parse(entrada[0]);
        int V = int.Parse(entrada[1]);

        // Continue a implementação
        Program p = new(C);
        List<Task> veiculos = new();

        _ = Task.Run(() => { p.FiscalDeEstacionamento(); });

        for (int i = 1; i <= V; i++)
        {
            int vec = i;
            Task t = Task.Run(async () => { await p.Veiculo(vec); });
            veiculos.Add(t);
        }
        Task.WaitAll(veiculos.ToArray());
    }
}
