using System;
using Microsoft.Extensions.Options;
using UserQueueManager.Contracts.Web;
using UserQueueManager.Orchestrator.Options;

namespace UserQueueManager.Orchestrator.Core.ProductRoutes;

internal class MonitoringServiceControllerProvider : IMonitoringServiceControllerProvider
{
    private readonly IServiceProvider _serviceProvider;

    private readonly ILogger<MonitoringServiceControllerProvider> _logger;

    private readonly Dictionary<Guid, string> _productMappings;

    public MonitoringServiceControllerProvider(IServiceProvider serviceProvider, IOptions<List<MonitorServiceOptions>> monitorServiceOptions, ILogger<MonitoringServiceControllerProvider> logger)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        var optionsList = monitorServiceOptions?.Value ?? throw new ArgumentNullException(nameof(monitorServiceOptions));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _productMappings = new Dictionary<Guid, string>();

        foreach (var option in optionsList)
        {
            foreach (var productId in option.Products)
            {
                if (!_productMappings.TryAdd(productId, option.ServiceKey))
                {
                    _logger.LogWarning("Не удалось добавить сервис {ServiceKey}, для продукта {IdProduct} уже машрутизация на другой сервис", option.ServiceKey, productId);
                }
            }
        }
    }

    IMonitorServiceController IMonitoringServiceControllerProvider.GetMonitorServiceController(Guid idProduct)
    {
        if (_productMappings.TryGetValue(idProduct, out var serviceKey))
        {
            return _serviceProvider.GetRequiredKeyedService<IMonitorServiceController>(serviceKey);
        }

        throw new InvalidOperationException($"Не найден сервис мониторинга, для товара {idProduct}");
    }
}
