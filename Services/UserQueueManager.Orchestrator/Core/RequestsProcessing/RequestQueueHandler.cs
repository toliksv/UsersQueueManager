using System;
using Microsoft.Extensions.Options;
using Polly;
using UserQueueManager.Contracts.Storage;
using UserQueueManager.Contracts.Web.Data;
using UserQueueManager.Core.FaultHadlers;
using UserQueueManager.Orchestrator.Core.ProductQueue;
using UserQueueManager.Orchestrator.Core.ProductRoutes;

namespace UserQueueManager.Orchestrator.Core.RequestsProcessing;

/// <inheritdoc cref="IRequestQueueHandler" />
internal class RequestQueueHandler : BackgroundService, IRequestQueueHandler
{
    private readonly IRequestQueue _requestQueue;

    private readonly IProductQueueHandler _productQueueHandler;

    private readonly ILogger<RequestQueueHandler> _logger;

    private readonly IProductsQueueRepository _productsQueueRepository;

    private readonly IMonitoringServiceControllerProvider _monitoringServiceControllerProvider;

    private readonly ResiliencePipeline _resiliencePipeline;

    private readonly List<SetProductQueueRequest> _initializeData;

    public RequestQueueHandler(IRequestQueue requestQueue, IProductQueueHandler productQueueHandler, ILogger<RequestQueueHandler> logger, IProductsQueueRepository productsQueueRepository, IOptions<List<SetProductQueueRequest>> initializeData, IMonitoringServiceControllerProvider monitoringServiceControllerProvider, [FromKeyedServices(FaultPolicyKeys.CircuitBreakerPolicyKey)]ResiliencePipeline resiliencePipeline)
    {
        _requestQueue = requestQueue ?? throw new ArgumentNullException(nameof(requestQueue));
        _productQueueHandler = productQueueHandler ?? throw new ArgumentNullException(nameof(productQueueHandler));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _productsQueueRepository = productsQueueRepository ?? throw new ArgumentNullException(nameof(productsQueueRepository));
        _initializeData = initializeData?.Value ?? throw new ArgumentNullException(nameof(initializeData));
        _monitoringServiceControllerProvider = monitoringServiceControllerProvider ?? throw new ArgumentNullException(nameof(monitoringServiceControllerProvider));
        _resiliencePipeline = resiliencePipeline ?? throw new ArgumentNullException(nameof(resiliencePipeline));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Запуск обработчика запросов.");
        await Initialize(stoppingToken);
        await Handle(stoppingToken);
    }

    public async Task Handle(CancellationToken cancellationToken)
    {
        while (await _requestQueue.WaitForRequest(cancellationToken))
        {
            try
            {
                var idProduct = _requestQueue.Dequeue();
                if (idProduct != Guid.Empty)
                {
                    await _productQueueHandler.Handle(idProduct, cancellationToken);
                }
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Ошибка при обработке события");

            }
        }
    }

    private async Task Initialize(CancellationToken cancellationToken)
    {
        try
        {
            var dataInitialized = await _productsQueueRepository.Initialize(_initializeData, cancellationToken);
            if (dataInitialized)
            {
                _logger.LogInformation("Данные инициализированы");
                foreach (var request in _initializeData)
                {
                    await SendInitializationData(request, cancellationToken);
                }
            }
        }
        catch (System.Exception e)
        {
            _logger.LogError(e, "Ошибка при инициализации данных");
        }
    }

    private async Task SendInitializationData(SetProductQueueRequest setProductQueueRequest, CancellationToken cancellationToken)
    {
        try
        {
            if (setProductQueueRequest is null)
            {
                throw new ArgumentNullException(nameof(setProductQueueRequest));
            }

            var controller = _monitoringServiceControllerProvider.GetMonitorServiceController(setProductQueueRequest.IdProduct);
            await _resiliencePipeline.ExecuteAsync(async tkn => await controller.SetProductQueue(setProductQueueRequest, tkn), cancellationToken);
        }
        catch (System.Exception e)
        {
            _logger.LogError(e, "Не удалось отправить данные инициализации для товара {IdProduct} {ProductName}", setProductQueueRequest.IdProduct, setProductQueueRequest.ProductName);
        }
    }
}
