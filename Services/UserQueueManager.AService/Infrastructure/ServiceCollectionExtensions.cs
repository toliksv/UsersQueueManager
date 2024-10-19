using RestEase.HttpClientFactory;
using UserQueueManager.Contracts.Web;
using UserQueueManager.Core.Infrastructure;

namespace UserQueueManager.AService.Infrastructure;

/// <summary>
/// Методы расширения для регистрации обработчиков.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Регистрация клиента к оркестратору.
    /// </summary>
    /// <param name="services">контейнер.</param>
    /// <param name="configuration">конфигурация.</param>
    /// <returns></returns>
    public static IServiceCollection AddOrchestratorClient(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddFaultPolicyHandler()
            .AddRestEaseClient<IOrchestratorController>(configuration.GetValue<string>("OrchestratorAddress"));
        return services;    
    }
   
}
