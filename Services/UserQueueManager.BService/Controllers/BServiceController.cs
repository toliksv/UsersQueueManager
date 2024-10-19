using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserQueueManager.Contracts.Data;
using UserQueueManager.Contracts.Storage;
using UserQueueManager.Contracts.Web.Data;

namespace UserQueueManager.BService.Controllers
{
    [Route("api/monitoring")]
    [ApiController]
    public class BServiceController : ControllerBase
    {
        private readonly IProductsQueuesStorage _productsQueuesStorage;

        public BServiceController(IProductsQueuesStorage productsQueuesStorage)
        {
            _productsQueuesStorage = productsQueuesStorage ?? throw new ArgumentNullException(nameof(productsQueuesStorage));
        }

        /// <summary>
        /// Установить очередь товару.
        /// </summary>
        /// <param name="request"><see cref="SetProductQueueRequest"/></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("set-queue")]
        public async Task<IActionResult> SetProductQueue([FromBody] SetProductQueueRequest request, CancellationToken cancellationToken)
        {
            if (request?.UsersQueue is null)
            {
                return BadRequest();
            }

            await _productsQueuesStorage.SetProductQueue(request.IdProduct, new Queue<User>(request.UsersQueue), cancellationToken);
            return Ok();
        }
    }
}
