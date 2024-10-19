using System;
using System.Collections.Concurrent;

namespace UserQueueManager.Orchestrator.Core.RequestsProcessing;

/// <inheritdoc cref="IRequestQueue" />
internal class RequestQueue : IRequestQueue
{
    private readonly ConcurrentQueue<Guid> _queue;
    private TaskCompletionSource<bool> _taskComplectionSource;

    private ILogger<RequestQueue> _logger;

    public RequestQueue(ILogger<RequestQueue> logger)
    {
        _taskComplectionSource = new TaskCompletionSource<bool>();
        _queue = new ConcurrentQueue<Guid>();
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Guid Dequeue()
    {
        if (_queue.TryDequeue(out var result))
        {
            return result;
        }

        return Guid.Empty;
    }

    public void Enqueue(Guid idProduct)
    {
        _queue.Enqueue(idProduct);
        _logger.LogInformation("Добавлено событие для обработки. Id товара {IdProduct}", idProduct);
        _taskComplectionSource?.TrySetResult(true);        
    }


    public Task<bool> WaitForRequest(CancellationToken cancellationToken)
    {
        if (_queue.Count > 0)
        {
            return Task.FromResult(true);
        }

        if (_taskComplectionSource.Task.IsCompleted)
        {
            _taskComplectionSource = new TaskCompletionSource<bool>();
            cancellationToken.Register(() => _taskComplectionSource?.TrySetResult(false));
        }

        _logger.LogInformation("Ождаем появления события ...");
        return _taskComplectionSource.Task;
    }
}
