using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Domain.Interfaces;

namespace Workers;

public class OrderWorker: IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public OrderWorker(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    
    private async Task Process()
    {
        using var scope = _scopeFactory.CreateScope();
        var orderService = scope.ServiceProvider.GetRequiredService<IOrderGateway>();

        while (true)
        {
            var orderEntities = (await orderService.GetPending(10)).ToList();
            
            foreach (var order in orderEntities)
            {
                await orderService.SetStatus(order.Id, "processing");
            }

            await Task.Delay(10000);
            
            foreach (var order in orderEntities)
            {
                await orderService.SetStatus(order.Id, "done");
            }
        }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(Process, cancellationToken);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}