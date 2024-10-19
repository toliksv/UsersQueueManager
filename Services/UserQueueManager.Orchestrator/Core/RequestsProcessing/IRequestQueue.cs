using System;

namespace UserQueueManager.Orchestrator.Core.RequestsProcessing;

/// <summary>
/// Очередь событий от сервисов мониторинга на обработку
/// </summary>
public interface IRequestQueue
{
    /// <summary>
    /// Добавить в очередь запрос.
    /// </summary>
    /// <param name="idProduct">идентификатор товара.</param>    
    /// <returns>ожидание добавления</returns>
    void Enqueue(Guid idProduct);

    /// <summary>
    /// Извлечь запрос из очереди.
    /// </summary>    
    /// <returns>идентификатор товара.</returns>
    Guid Dequeue();

    /// <summary>
    /// Ожидание наличия в очереди запросов.
    /// </summary>
    /// <param name="cancellationToken">токен отмены.</param>
    /// <returns></returns>
    Task<bool> WaitForRequest(CancellationToken cancellationToken);
}
