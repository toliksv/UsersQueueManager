using System;

namespace UserQueueManager.Orchestrator.Core.ProductQueue;

/// <summary>
/// Управление очередями товаров.
/// </summary>
public interface IProductQueueHandler
{
    /// <summary>
    /// ОБработать очередь.
    /// </summary>
    /// <param name="idProduct">идентификатор забронированого товара.</param>    
    /// <param name="cancellationTokena">токен отмены</param>
    /// <returns>ожидание обработки очереди.</returns>
    Task Handle(Guid idProduct, CancellationToken cancellationTokena);
}
