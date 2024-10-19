using System;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.CircuitBreaker;
using UserQueueManager.Contracts.FaultHandlers;
using UserQueueManager.Contracts.Storage;
using UserQueueManager.Contracts.Web;
using UserQueueManager.Core.FaultHadlers;
using UserQueueManager.Core.Storage;

namespace UserQueueManager.Core.Infrastructure;

/// <summary>
/// Методы регистрации обработчиков.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Регистрация реализации хранилища товарных очередей.
    /// </summary>
    /// <param name="services">контейнер.</param>
    /// <returns>контейнер с зарегистрированной реализацией.</returns>
    public static IServiceCollection AddProductQueueStorage(this IServiceCollection services)
     => services.AddSingleton<IProductsQueuesStorage, ProductsQueueStorage>();

    public static IServiceCollection AddFaultPolicyHandler(this IServiceCollection services)
    {
        var optionsComplex = new CircuitBreakerStrategyOptions
        {
            FailureRatio = 0.5,
            SamplingDuration = TimeSpan.FromSeconds(60),
            MinimumThroughput = 3,
            BreakDuration = TimeSpan.FromSeconds(120),
            ShouldHandle = new PredicateBuilder().Handle<Exception>(),
        };
        
        services.AddResiliencePipeline(FaultPolicyKeys.CircuitBreakerPolicyKey, builder => {
            builder
                .AddCircuitBreaker(optionsComplex)
                .Build();
        });

        return services
                .AddTransient<IOrchestratorFaultPolicyHandler, OrchestratorFaultPolicyHandler>();
                
    } 

}
