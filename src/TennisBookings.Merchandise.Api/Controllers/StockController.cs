using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TennisBookings.Merchandise.Api.Data;
using TennisBookings.Merchandise.Api.Models.Output;
using TennisBookings.Merchandise.Api.Stock;

namespace TennisBookings.Merchandise.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StockController : ControllerBase
{
    private readonly IProductDataRepository _productDataRepository;
    private readonly IStockCalculator _stockCalculator;

    public StockController(IProductDataRepository productDataRepository, IStockCalculator stockCalculator)
    {
        _productDataRepository = productDataRepository;
        _stockCalculator = stockCalculator;
    }
    [HttpGet("total")]
    public async Task<ActionResult<StockTotalOutputModal>> GetStockTotal()
    {
        // return Ok("{\"stockItemTotal\":100}");
        // return new JsonResult("{\"stockItemTotal\":100}");
        var products = await _productDataRepository.GetProductsAsync();
        var stockTotalCount = _stockCalculator.CalculateStockTotal(products);
        
        return Ok(new StockTotalOutputModal(){ StockItemTotal = stockTotalCount });
    }
}