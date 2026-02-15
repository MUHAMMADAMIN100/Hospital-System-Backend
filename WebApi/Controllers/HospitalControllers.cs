using Domain.DTOs.HospitalDTO;
using Domain.Filter;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HospitalController : ControllerBase
{
    private readonly HospitalService _hospital;

    public HospitalController(HospitalService hospital)
    {
        _hospital = hospital;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] HospitalFilter filter)
        => Ok(await _hospital.GetAllAsync(filter));

    [HttpGet("by-id")]
    public async Task<IActionResult> GetByIdAsync([FromQuery] string registrationNumber)
        => Ok(await _hospital.GetById(registrationNumber));

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateHospitalDTO dto)
        => Ok(await _hospital.CreateAsync(dto));

    [HttpPut]
    public async Task<IActionResult> UpdateAsync(
        [FromQuery] string registrationNumber,
        [FromBody] UpdateHospitalDTO dto)
        => Ok(await _hospital.UpdateAsync(registrationNumber, dto));

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
        => Ok(await _hospital.DeleteAsync(id));
}