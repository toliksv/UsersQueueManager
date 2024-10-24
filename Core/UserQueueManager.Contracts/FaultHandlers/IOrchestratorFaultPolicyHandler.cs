using System;
using UserQueueManager.Contracts.Web.Data;

namespace UserQueueManager.Contracts.FaultHandlers;

/// <summary>
/// Обертка для политики CircuitBreaker для <see cref="IOrchestratorCoontroller" />
/// </summary>
public interface IOrchestratorFaultPolicyHandler
{
    /// <summary>
    /// Уведомить оркестратора о бронировании товара.
    /// </summary>
    /// <param name="request"><see cref="ProductBookedRequest"/></param>
    /// <param name="cancellationToken">токен отмены.</param>
    /// <returns>ожидание обработки.</returns>    
    Task ProductBooked(ProductBookedRequest request, CancellationToken cancellationToken);
}
