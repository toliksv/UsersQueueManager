using System;
using UserQueueManager.Contracts.Data;
using UserQueueManager.Contracts.Web.Data;

namespace UserQueueManager.Contracts.Storage;

/// <summary>
/// Репозиторий для хранения очередей за товарами.
/// </summary>
public interface IProductsQueueRepository
{
    /// <summary>
    /// Сохранить хранилище.
    /// </summary>
    /// <param name="storage">хранилище.</param>
    /// <param name="cancellationToken">токен отмены.</param>
    /// <returns>ожидание сохранения.</returns>
    Task SaveStorage(Dictionary<Guid, Queue<User>> storage, CancellationToken cancellationToken);

    /// <summary>
    /// Загрузка хранилища.
    /// </summary>
    /// <param name="cancellationToken">токен отмены.</param>
    /// <returns>Загруженное хранилище.</returns>
    Task<Dictionary<Guid, Queue<User>>> LoadStorage(CancellationToken cancellationToken);

    /// <summary>
    /// Инийиализация данными.
    /// </summary>
    /// <param name="initializationData">данные инициализации.</param>
    /// <param name="cancellationToken">токен отмены.</param>
    /// <returns>ожидание инициализации. <see langword="true"/> - инициализация произошла.</returns>
    Task<bool> Initialize(List<SetProductQueueRequest> initializationData, CancellationToken cancellationToken);
}
