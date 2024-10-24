using System;
using RestEase;
using UserQueueManager.Contracts.Web.Data;

namespace UserQueueManager.Contracts.Web;

/// <summary>
/// Общий интерфейс для API сервисов мониторинга.
/// </summary>
[BasePath("api/monitoring")]
public interface IMonitorServiceController
{
    /// <summary>
    /// Установить очередь товару.
    /// </summary>
    /// <param name="request"><see cref="SetProductQueueRequest"/></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Post("set-queue")]
    Task SetProductQueue([Body] SetProductQueueRequest request, CancellationToken cancellationToken);
}
