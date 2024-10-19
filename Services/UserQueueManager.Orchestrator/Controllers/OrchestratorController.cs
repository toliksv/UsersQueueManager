using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserQueueManager.Contracts.Web.Data;
using UserQueueManager.Orchestrator.Core.RequestsProcessing;

namespace UserQueueManager.Orchestrator.Controllers
{
    /// <summary>
    /// Контроллер оркестратора.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrchestratorController : ControllerBase
    {
        private readonly IRequestQueue _requestQueue;

        public OrchestratorController(IRequestQueue requestQueue)
        {
            _requestQueue = requestQueue ?? throw new ArgumentNullException(nameof(requestQueue));
        }

        /// <summary>
        /// Уведомить оркестратора о бронировании товара.
        /// </summary>
        /// <param name="request"><see cref="ProductBookedRequest"/></param>
        /// <param name="cancellationToken">токен отмены.</param>
        /// <returns>ожидание обработки.</returns>
        [HttpPost("product-booked")]
        public Task<IActionResult> ProductBooked([FromBody] ProductBookedRequest request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                return Task.FromResult<IActionResult>(BadRequest());
            }

            _requestQueue.Enqueue(request.IdProduct);
            return Task.FromResult<IActionResult>(Ok());
        }
    }
}
