using MediatR;
using Microsoft.AspNetCore.Mvc;
using LogisticIntegration.Application.Settlement.Commands;
using LogisticIntegration.Domain.Exceptions;

namespace LogisticIntegration.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettlementController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SettlementController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("{id}/calculate")]
        public async Task<IActionResult> CalculateTripSettlement(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var command = new CalculateTripSettlementCommand(id);
                var result = await _mediator.Send(command, cancellationToken);
                return Ok(new { success = result, message = "Trip settlement calculated successfully." });
            }
            catch (SettlementNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
