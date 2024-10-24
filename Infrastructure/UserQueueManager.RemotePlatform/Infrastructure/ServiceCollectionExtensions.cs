using System;
using Microsoft.Extensions.DependencyInjection;
using UserQueueManager.RemotePlatform.RemotePlatform;

namespace UserQueueManager.RemotePlatform.Infrastructure;

/// <summary>
/// Методы регистрации обработчиков.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Добавление имитации ответа удаленной платформы и ее обработки.
    /// </summary>
    /// <param name="services">контейнер.</param>
    /// <returns>контейнер с зарегистрированными обработчиками.</returns>
    public static IServiceCollection AddRemotePlatformHandlers(this IServiceCollection services)
        => services.AddSingleton<IRemotePlatformRequester, RemotePlatformRequester>()
           .AddHostedService<RemotePlatformResponseHandler>();
}
