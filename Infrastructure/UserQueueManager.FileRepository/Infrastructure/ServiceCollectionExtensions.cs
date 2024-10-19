using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using UserQueueManager.FileRepository.Storage;
using UserQueueManager.Contracts.Storage;

namespace UserQueueManager.FileRepository.Infrastructure;

/// <summary>
/// Методы расширения для регистрации обработчиков.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Регистрация реализации файлового репозитория.
    /// </summary>
    /// <param name="services">контейнер.</param>
    /// <param name="configuration">конфигурация.</param>
    /// <returns>контейнер с зарегенной реализацией.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IServiceCollection UseFileRepository(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        services.AddOptions();
        services.Configure<ProductsQueueFileRepositoryOptions>(configuration.GetSection(nameof(ProductsQueueFileRepositoryOptions)));
        services.AddSingleton<IProductsQueueRepository, ProductsQueueRepository>();
        return services;
    }
}
