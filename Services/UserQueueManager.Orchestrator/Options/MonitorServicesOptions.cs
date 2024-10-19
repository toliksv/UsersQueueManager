using System;

namespace UserQueueManager.Orchestrator.Options;

/// <summary>
/// Опции для регистрации контроллеров сервисов мониторинга.
/// </summary>
public class MonitorServiceOptions
{
    /// <summary>
    /// Базовый адрес сервиса.
    /// </summary>
    public string ServiceBaseAddress { get; set; }

    /// <summary>
    /// Ключ сервиса.
    /// </summary>
    public string ServiceKey { get; set; }

    /// <summary>
    /// Список товаров, которые мониторит сервис.
    /// </summary>
    public List<Guid> Products { get; set; }
}
