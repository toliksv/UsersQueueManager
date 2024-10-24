using System;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UserQueueManager.Contracts.Data;
using UserQueueManager.Contracts.Storage;
using System.Text;
using UserQueueManager.Contracts.Web.Data;
using Microsoft.Extensions.Logging;

namespace UserQueueManager.FileRepository.Storage;

/// <inheritdoc cref="IProductsQueueRepository"/> 
/// <remarks>
/// репозиторий для файлового хранилища.
/// </remarks>
internal class ProductsQueueRepository : IProductsQueueRepository
{
    private readonly ProductsQueueFileRepositoryOptions _options;
    private readonly ILogger<ProductsQueueRepository> _logger;

    private readonly object _lockObj = new object();

    public ProductsQueueRepository(IOptions<ProductsQueueFileRepositoryOptions> options, ILogger<ProductsQueueRepository> logger)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<bool> Initialize(List<SetProductQueueRequest> initializationData, CancellationToken cancellationToken)
    {
        // если файла нет или файл пустой, инициализируем даными.
        var fileExists = File.Exists(_options.StorageFileName);
        var needInitialize = !fileExists;
        if (fileExists)
        {
            var fileContent = await File.ReadAllTextAsync(_options.StorageFileName, cancellationToken);
            needInitialize = needInitialize || (!fileContent?.Any() ?? true);
        }

        if (needInitialize)
        {
            var dictionary = new Dictionary<Guid, Queue<User>>();
            foreach (var productQueue in initializationData)
            {
                if (!dictionary.TryAdd(productQueue.IdProduct, new Queue<User>(productQueue.UsersQueue)))
                {
                    _logger.LogWarning("Не удалось добавить очередь продукта {IdProduct}, {ProductName}", productQueue.IdProduct, productQueue.ProductName);
                }
            }
            await SaveStorage(dictionary, cancellationToken);
            return true;
        }

        return false;
    }

    public async Task<Dictionary<Guid, Queue<User>>> LoadStorage(CancellationToken cancellationToken)
    {
        if (!File.Exists(_options.StorageFileName))
        {
            return null;
        }

        using (var streamResder = File.OpenText(_options.StorageFileName))
        {
            return JsonConvert.DeserializeObject<Dictionary<Guid, Queue<User>>>(await streamResder.ReadToEndAsync(cancellationToken));
        }
    }

    public async Task SaveStorage(Dictionary<Guid, Queue<User>> storage, CancellationToken cancellationToken)
    {
        var jsonData = JsonConvert.SerializeObject(storage, Formatting.Indented);
        lock (_lockObj)
        {              
                
            using (var stream = File.OpenWrite(_options.StorageFileName))
            using (var streamWriter = new StreamWriter(stream, Encoding.UTF8))
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    streamWriter.Write(jsonData);
                    streamWriter.Flush();
                }
            }
        }
    }
}
