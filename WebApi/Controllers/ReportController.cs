using Domain.DTOs.ReportDTO;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportController(IReportService reportService) : ControllerBase
{
    [HttpGet("all")]
    public async Task<IActionResult> GetStatisticsAllTerritories(
        [FromQuery] DateTime dateFrom,
        [FromQuery] DateTime dateTo)
    {
        var response = await reportService.GetStatisticsAllTerritories(dateFrom, dateTo);
        return Ok(response);
    }

    [HttpGet("all-extended")]
    public async Task<IActionResult> GetStatisticsAllTerritoriesExtended(
     [FromQuery] DateTime dateFrom,
     [FromQuery] DateTime dateTo)
    {
        var response = await reportService.GetStatisticsAllTerritoriesExtended(dateFrom, dateTo);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetStatisticsByTerritory(
        [FromQuery] string territoryName,
        [FromQuery] DateTime dateFrom,
        [FromQuery] DateTime dateTo)
    {
        var response = await reportService.GetStatisticsByTerritoryNameAsync(territoryName, dateFrom, dateTo);
        if (!response.IsSuccess)
            return NotFound(response.Message);

        return Ok(response.Data);
    }

    [HttpGet("firdavsi")]
    public async Task<IActionResult> GetStatisticsFirdavsi([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
        => Ok(await reportService.GetStatisticsFirdavsi(dateFrom, dateTo));

    [HttpGet("shohmansur")]
    public async Task<IActionResult> GetStatisticsShohmansur([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
        => Ok(await reportService.GetStatisticsShohmansur(dateFrom, dateTo));

    [HttpGet("sino")]
    public async Task<IActionResult> GetStatisticsSino([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
        => Ok(await reportService.GetStatisticsSino(dateFrom, dateTo));

    [HttpGet("somoni")]
    public async Task<IActionResult> GetStatisticsSomoni([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
        => Ok(await reportService.GetStatisticsSomoni(dateFrom, dateTo));
}
