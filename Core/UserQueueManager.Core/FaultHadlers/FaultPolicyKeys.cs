using System;

namespace UserQueueManager.Core.FaultHadlers;

/// <summary>
/// Ключи для политик отказа.
/// </summary>
public static class FaultPolicyKeys
{
    /// <summary>
    /// Ключ для CircuitBreaker.
    /// </summary>
    public const string CircuitBreakerPolicyKey = "CircuitBreakerKey";
}
