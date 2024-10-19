using System;
using UserQueueManager.Contracts.Data;

namespace UserQueueManager.Contracts.Web.Data;

/// <summary>
/// Запрос на установку очереди ожидания товара.Ы
/// </summary>
public class SetProductQueueRequest
{
    /// <summary>
    /// Идентификатор товара.
    /// </summary>
    public Guid IdProduct { get; set; }

    /// <summary>
    /// Наименование товара.
    /// </summary>
    public string ProductName { get; set; }

    /// <summary>
    /// Очередь пользователей.
    /// </summary>
    public List<User> UsersQueue { get; set; }
}
