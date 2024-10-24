using System;

namespace UserQueueManager.RemotePlatform.RemotePlatform;

/// <summary>
/// Имитация запроса / ответа от удаленной платформы.
/// </summary>
public interface IRemotePlatformRequester
{
    /// <summary>
    /// Запрос на бронирование товара.
    /// </summary>
    /// <param name="cancellationToken">токен отмены.</param>
    /// <returns>забронированный товар (случайный).</returns>
    Task<Guid> BookProduct(CancellationToken cancellationToken);
}
