using LedgerFlow.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LedgerFlow.API.Common;

public static class ApiErrors
{
    public static IActionResult FromException(Exception ex)
    {
        return ex switch
        {
            InsufficientFundsException e =>
                new BadRequestObjectResult(new
                {
                    error = "INSUFFICIENT_FUNDS",
                    message = e.Message
                }),

            DbUpdateConcurrencyException =>
                new ConflictObjectResult(new
                {
                    error = "CONCURRENT_MODIFICATION",
                    message = "Account was modified concurrently. Please retry."
                }),

            _ =>
                new ObjectResult(new
                {
                    error = "INTERNAL_ERROR",
                    message = "Unexpected error occurred."
                })
                { StatusCode = 500 }
        };
    }
}
