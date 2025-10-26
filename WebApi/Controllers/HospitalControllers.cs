using Domain.DTOs.HospitalDTO;
using Domain.Filter;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HospitalControllers(HospitalService hospital) : ControllerBase
{
    [HttpGet]
 public async Task<IActionResult> GetAllAsync([FromQuery] HospitalFilter filter)
    => Ok(await hospital.GetAllAsync(filter));

    [HttpGet("by-id")]
    public async Task<IActionResult> GetByIdAsync(string registrationNumber)
        => Ok(await hospital.GetById(registrationNumber));
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateHospitalDTO DTO)
        => Ok(await hospital.CreateAsync(DTO));

    [HttpPut]
    public async Task<IActionResult> UpdateAsync(string registrationNumber, [FromBody] UpdateHospitalDTO DTO)
        => Ok(await hospital.UpdateAsync(registrationNumber, DTO));

   [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
        => Ok(await hospital.DeleteAsync(id));
}
