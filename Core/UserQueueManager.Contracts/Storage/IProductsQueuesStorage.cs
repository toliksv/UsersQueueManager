using System;
using UserQueueManager.Contracts.Data;

namespace UserQueueManager.Contracts.Storage;

/// <summary>
/// Хранилище очереди на товары.
/// </summary>
public interface IProductsQueuesStorage
{
    /// <summary>
    /// Список продуктов.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<List<Guid>> GetProducts(CancellationToken cancellationToken);

    /// <summary>
    /// Получение очереди пользователей за товаром.
    /// </summary>
    /// <param name="idProduct">идентификатор товара.</param>
    /// <param name="cancellationToken">токен отмены.</param>
    /// <returns>очередь пользователей.</returns>
    Task<Queue<User>> GetProductQueue(Guid idProduct, CancellationToken cancellationToken);

    /// <summary>
    /// Установить товару очередь за товаром.
    /// </summary>
    /// <param name="idProduct"></param>
    /// <param name="users"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SetProductQueue(Guid idProduct, Queue<User> users, CancellationToken cancellationToken);
}
