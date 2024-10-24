using System;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using UserQueueManager.Contracts.FaultHandlers;
using UserQueueManager.Contracts.Web;
using UserQueueManager.Contracts.Web.Data;

namespace UserQueueManager.Core.FaultHadlers;

/// <inheritdoc cref="IOrchestratorFaultPolicyHandler" />
internal class OrchestratorFaultPolicyHandler : IOrchestratorFaultPolicyHandler
{
    private readonly IOrchestratorController _orchestratorController;
    private readonly ResiliencePipeline _resiliencePipeline;

    public OrchestratorFaultPolicyHandler(IOrchestratorController orchestratorController, [FromKeyedServices(FaultPolicyKeys.CircuitBreakerPolicyKey)]ResiliencePipeline resiliencePipeline)
    {
        _orchestratorController = orchestratorController ?? throw new ArgumentNullException(nameof(orchestratorController));
        _resiliencePipeline = resiliencePipeline ?? throw new ArgumentNullException(nameof(resiliencePipeline));
    }

    public async Task ProductBooked(ProductBookedRequest request, CancellationToken cancellationToken)
        => await _resiliencePipeline.ExecuteAsync(async tkn => await _orchestratorController.ProductBooked(request, tkn), cancellationToken);
}
