using System;
using UserQueueManager.Contracts.Web;

namespace UserQueueManager.Orchestrator.Core.ProductRoutes;

/// <summary>
/// Машрутизация сервисов мониторинга в зависимости от товара.
/// </summary>
public interface IMonitoringServiceControllerProvider
{
    /// <summary>
    /// Возвращает контроллер сервиса мониторинга в зависимости от товара.
    /// </summary>
    /// <param name="idProduct">идентификатор продута.</param>
    /// <returns></returns>
    IMonitorServiceController GetMonitorServiceController(Guid idProduct);
}
