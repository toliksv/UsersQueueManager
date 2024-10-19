using UserQueueManager.Contracts.Storage;
using UserQueueManager.Contracts.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using UserQueueManager.RemotePlatform.RemotePlatform;
using UserQueueManager.Contracts.Web.Data;
using UserQueueManager.Contracts.FaultHandlers;

namespace UserQueueManager.RemotePlatform;

/// <summary>
/// Обработка имитированного ответа от удаленной платформы.
/// </summary>
public class RemotePlatformResponseHandler : BackgroundService
{
    private readonly IOrchestratorFaultPolicyHandler _orchestratorFaultPolicyHandler;
    private readonly ILogger<RemotePlatformResponseHandler> _logger;

    private readonly IRemotePlatformRequester _remotePlatformRequester;

    public RemotePlatformResponseHandler(IOrchestratorFaultPolicyHandler orchestratorController, ILogger<RemotePlatformResponseHandler> logger, IRemotePlatformRequester remotePlatformRequester)
    {
        _orchestratorFaultPolicyHandler = orchestratorController ?? throw new ArgumentNullException(nameof(orchestratorController));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _remotePlatformRequester = remotePlatformRequester ?? throw new ArgumentNullException(nameof(remotePlatformRequester));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var idProduct = await _remotePlatformRequester.BookProduct(stoppingToken);
                if (idProduct != Guid.Empty)
                {
                    _logger.LogInformation("Забронирован товар {IdProduct}, отсылаем уведомление оркестратору", idProduct);
                    await _orchestratorFaultPolicyHandler.ProductBooked(new ProductBookedRequest { IdProduct = idProduct }, stoppingToken);
                }
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Ошибка при уведомлении о броонировании");
            }
        }
    }
}
