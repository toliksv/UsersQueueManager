using System;
using RestEase;
using UserQueueManager.Contracts.Web.Data;

namespace UserQueueManager.Contracts.Web;

/// <summary>
/// Контроллер для взаимодействия с API Оркестратора.
/// </summary>
[BasePath("api/orchestrator")]
public interface IOrchestratorController
{
    /// <summary>
    /// Уведомить оркестратора о бронировании товара.
    /// </summary>
    /// <param name="request"><see cref="ProductBookedRequest"/></param>
    /// <param name="cancellationToken">токен отмены.</param>
    /// <returns>ожидание обработки.</returns>
    [Post("product-booked")]
    Task ProductBooked([Body] ProductBookedRequest request, CancellationToken cancellationToken);
}
