using System;
using UserQueueManager.Contracts.Storage;

namespace UserQueueManager.RemotePlatform.RemotePlatform;

/// <inheritdoc cref="IRemotePlatformRequester" />
internal class RemotePlatformRequester : IRemotePlatformRequester
{
    private readonly IProductsQueuesStorage _productsQueuesStorage;

    private readonly Random _randomGenerator;

    public RemotePlatformRequester(IProductsQueuesStorage productsQueuesStorage)
    {
        _productsQueuesStorage = productsQueuesStorage ?? throw new ArgumentNullException(nameof(productsQueuesStorage));
        _randomGenerator = new Random();
    }

    public async Task<Guid> BookProduct(CancellationToken cancellationToken)
    {
        await GetElapsed(cancellationToken);
        return await GetRandomProduct(cancellationToken);
    }

    private Task GetElapsed(CancellationToken cancellationToken)
    {
        // от секунды до 45 секунд     
        var elapsedPeriod = _randomGenerator.Next(1000, 45000);
        return Task.Delay(elapsedPeriod, cancellationToken);
    }

    private async Task<Guid> GetRandomProduct(CancellationToken cancellationToken)
    {
        var products = await _productsQueuesStorage.GetProducts(cancellationToken);
        while (products.Count == 0 && !cancellationToken.IsCancellationRequested)
        {
            _ = await _productsQueuesStorage.GetProductQueue(Guid.NewGuid(), cancellationToken);
            products = await _productsQueuesStorage.GetProducts(cancellationToken);
        }

        var randomIndex = _randomGenerator.Next(0, products.Count - 1);
        return products[randomIndex];
    }
}
