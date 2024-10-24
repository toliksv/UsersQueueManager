using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using UserQueueManager.Contracts.Data;
using UserQueueManager.Contracts.Storage;

namespace UserQueueManager.Core.Storage;

/// <inheritdoc cref="IProductsQueuesStorage" />
internal class ProductsQueueStorage : IProductsQueuesStorage
{
    private readonly ConcurrentDictionary<Guid, Queue<User>> _productQueries;
    private readonly IProductsQueueRepository _productQueueRepository;
    private readonly ILogger<ProductsQueueStorage> _logger;

    public ProductsQueueStorage(IProductsQueueRepository productQueueRepository, ILogger<ProductsQueueStorage> logger)
    {
        _productQueueRepository = productQueueRepository ?? throw new ArgumentNullException(nameof(productQueueRepository));
        _productQueries = new ConcurrentDictionary<Guid, Queue<User>>();
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }


    public async Task<Queue<User>> GetProductQueue(Guid idProduct, CancellationToken cancellationToken)
    {
        if (_productQueries.IsEmpty)
        {
            await LoadDictionary(cancellationToken);
        }

        if (_productQueries.TryGetValue(idProduct, out var productQuery))
        {
            return productQuery;
        }

        return null;
    }

    public ValueTask<List<Guid>> GetProducts(CancellationToken cancellationToken)
    {
        if (!cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromResult(_productQueries.Keys.ToList());
        }

        return ValueTask.FromResult(null as List<Guid>);
    }

    public async Task SetProductQueue(Guid idProduct, Queue<User> usersQueue, CancellationToken cancellationToken)
    {
        if (usersQueue is null)
        {
            throw new ArgumentNullException(nameof(usersQueue));
        }

        _productQueries.AddOrUpdate(idProduct, key => usersQueue, (keyUpd, qu) => usersQueue);
        await _productQueueRepository.SaveStorage(_productQueries.ToDictionary(), cancellationToken);
        _logger.LogInformation("Для товара {IdProduct} обновлена очередь {@Queue}", idProduct, usersQueue);
    }

    private async ValueTask LoadDictionary(CancellationToken cancellationToken)
    {
        var fileDictionary = await _productQueueRepository.LoadStorage(cancellationToken);
        if (fileDictionary is not null)
        {
            foreach (var keyPair in fileDictionary)
            {
                _productQueries.AddOrUpdate(keyPair.Key, key => keyPair.Value, (updKey, qu) => keyPair.Value);
            }
        }
    }


}
