using System;

namespace UserQueueManager.Contracts.Web.Data;

/// <summary>
/// Запрос на уведомление оркестратора об обновлении обчереди.
/// </summary>
public class ProductBookedRequest
{
    /// <summary>
    /// Идентификатор продукта.
    /// </summary>
    public Guid IdProduct { get; set; }
}
