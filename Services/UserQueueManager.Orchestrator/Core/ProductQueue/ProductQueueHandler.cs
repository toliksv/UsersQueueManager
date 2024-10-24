using System;
using Polly;
using UserQueueManager.Contracts.Storage;
using UserQueueManager.Contracts.Web.Data;
using UserQueueManager.Core.FaultHadlers;
using UserQueueManager.Orchestrator.Core.ProductRoutes;

namespace UserQueueManager.Orchestrator.Core.ProductQueue;

/// <inheritdoc cref="IProductQueueHandler" />.
internal class ProductQueueHandler : IProductQueueHandler
{
    private readonly IProductsQueuesStorage _productsQueuesStorage;
    private readonly IMonitoringServiceControllerProvider _monitoringServiceControllerProvider;
    private readonly ResiliencePipeline _resiliencePipeline;

    public ProductQueueHandler(IProductsQueuesStorage productsQueuesStorage, IMonitoringServiceControllerProvider monitoringServiceControllerProvider, [FromKeyedServices(FaultPolicyKeys.CircuitBreakerPolicyKey)]ResiliencePipeline resiliencePipeline)
    {
        _productsQueuesStorage = productsQueuesStorage ?? throw new ArgumentNullException(nameof(productsQueuesStorage));
        _monitoringServiceControllerProvider = monitoringServiceControllerProvider ?? throw new ArgumentNullException(nameof(monitoringServiceControllerProvider));
        _resiliencePipeline = resiliencePipeline ?? throw new ArgumentNullException(nameof(resiliencePipeline));
    }

    public async Task Handle(Guid idProduct, CancellationToken cancellationToken)
    {
        var productQueue = await _productsQueuesStorage.GetProductQueue(idProduct, cancellationToken);
        if (productQueue is null)
        {
            throw new InvalidOperationException($"Не найдена очередь для товара Id:{idProduct}");
        }

        // Извлекаем пользователя.
        var happyUser = productQueue.Dequeue();
        // и в конец очереди его
        productQueue.Enqueue(happyUser);
        await _productsQueuesStorage.SetProductQueue(idProduct, productQueue, cancellationToken);

        // рассылка.
        var request = new SetProductQueueRequest { IdProduct = idProduct, UsersQueue = productQueue.ToList(), };
        var controller = _monitoringServiceControllerProvider.GetMonitorServiceController(idProduct);
        await _resiliencePipeline.ExecuteAsync(async tkn => await controller.SetProductQueue(request, tkn), cancellationToken);
    }
}
