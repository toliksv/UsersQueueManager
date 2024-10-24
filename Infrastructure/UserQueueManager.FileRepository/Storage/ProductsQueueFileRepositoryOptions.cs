using System;

namespace UserQueueManager.FileRepository.Storage;

/// <summary>
/// Опции хранения очередей в файле.
/// </summary>
public class ProductsQueueFileRepositoryOptions
{
    /// <summary>
    /// Имя файла с данными.
    /// </summary>
    public string StorageFileName { get; set; } = "ProductsQueue.jsn";

    internal void Returns(Func<ProductsQueueFileRepositoryOptions> value)
    {
        throw new NotImplementedException();
    }
}
