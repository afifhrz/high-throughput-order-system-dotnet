using LedgerFlow.API.Common;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LedgerFlow.API.Payments;

[ApiController]
[Route("payments")]
public class PaymentController : ControllerBase
{
    private readonly PaymentService _service;

    public PaymentController(PaymentService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] PaymentRequest request,
        [FromHeader(Name = "Idempotency-Key")] string idempotencyKey)
    {
        if (string.IsNullOrWhiteSpace(idempotencyKey))
            return BadRequest(new { error = "IDEMPOTENCY_KEY_REQUIRED" });

        try
        {
            Log.Information("Create.Payments: Request incoming with idempotency-key: {IdempotencyKey}", idempotencyKey);
            var transactionId = await _service.ProcessAsync(request, idempotencyKey);
            return Ok(new PaymentResponse(transactionId));
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Create.Payments: Error processing payment with idempotency-key: {IdempotencyKey}", idempotencyKey);
            return ApiErrors.FromException(ex);
        }
    }
}
