using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RestEase;
using UserQueueManager.Contracts.Web;
using UserQueueManager.Contracts.Web.Data;
using UserQueueManager.Core.Infrastructure;
using UserQueueManager.Orchestrator.Core.ProductQueue;
using UserQueueManager.Orchestrator.Core.ProductRoutes;
using UserQueueManager.Orchestrator.Core.RequestsProcessing;
using UserQueueManager.Orchestrator.Options;

namespace UserQueueManager.Orchestrator.Infrastructure;

/// <summary>
/// Методы расширения для регистрацйии контроллеров и обработчиков.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Регистрация обработчиков в Orchestrator.Core.
    /// </summary>
    /// <param name="services">контейнер.</param>
    /// <returns>контейнер с зарегистрироваными обработчиками.</returns>
    public static IServiceCollection AddCoreHandlers(this IServiceCollection services)
     => services.AddTransient<IProductQueueHandler, ProductQueueHandler>()
        .AddSingleton<IMonitoringServiceControllerProvider, MonitoringServiceControllerProvider>()
        .AddSingleton<IRequestQueue, RequestQueue>()
        .AddHostedService<RequestQueueHandler>();

    /// <summary>
    /// Регистрация контроллеров для сервисов мониторинга.
    /// </summary>
    /// <param name="services">контейнер.</param>
    /// <param name="configuration">конфигруция.</param>
    /// <returns>контейнер с зарегистрированными контроллерами.</returns>
    public static IServiceCollection AddMontorServicesControllers(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        services.AddOptions();
        var optionsSection = configuration.GetSection(nameof(MonitorServiceOptions));
        services.Configure<List<MonitorServiceOptions>>(optionsSection);
        var list = optionsSection.Get<List<MonitorServiceOptions>>();
        foreach (var serviceOptions in list)
        {
            services.AddKeyedTransient(typeof(IMonitorServiceController), serviceOptions.ServiceKey, (sp, key) => RestClient.For<IMonitorServiceController>(serviceOptions.ServiceBaseAddress));
        }

        services.AddFaultPolicyHandler();
        return services;
    }

    /// <summary>
    /// С чего-то стартовать нужно. Стартовые данные прописал в конфигурации.
    /// </summary>
    /// <param name="services">контейнер.</param>
    /// <param name="configuration">конфигурациюя.</param>
    /// <returns>контейнер с зарегеной конфигурацией</returns>
    public static IServiceCollection RegisterStartConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        var confSection = configuration.GetSection("StartConfiguration");
        services.Configure<List<SetProductQueueRequest>>(confSection);
        return services;
    }
}
