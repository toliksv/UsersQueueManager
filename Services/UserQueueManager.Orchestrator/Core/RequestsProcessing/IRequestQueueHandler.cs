using System;
using UserQueueManager.Orchestrator.Core.ProductQueue;

namespace UserQueueManager.Orchestrator.Core.RequestsProcessing;

/// <summary>
/// Обработчик очереди событий от сервисов мониторинга.
/// </summary>
public interface IRequestQueueHandler
{
    /// <summary>
    /// Обработка событий в очереди.
    /// </summary>
    /// <param name="cancellationToken">токен отмены.</param>
    /// <returns>ожидание обработки</returns>
    Task Handle(CancellationToken cancellationToken);
}
