using Microsoft.AspNetCore.Mvc;
using TennisBookings.Merchandise.Api.Models.Output;

namespace TennisBookings.Merchandise.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StockController : ControllerBase
{
    [HttpGet("total")]
    public IActionResult GetStockTotal()
    {
        // return Ok("{\"stockItemTotal\":100}");
        // return new JsonResult("{\"stockItemTotal\":100}");
        return Ok(new StockTotalOutputModal(){ StockItemTotal = 100 });
    }
}