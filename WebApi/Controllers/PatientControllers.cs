using Domain.DTOs.PatientDTO;
using Domain.Filter;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientControllers(PatientService patientService) : ControllerBase
{
    [HttpGet]
public async Task<IActionResult> GetAllAsync([FromQuery] PatientFilter filter)
    => Ok(await patientService.GetAllAsync(filter));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync(int id)
        => Ok(await patientService.GetByIdAsync(id));

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreatePatientDTO patient)
        => Ok(await patientService.CreateAsync(patient));

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdatePatientDTO patient)
        => Ok(await patientService.UpdateAsync(id, patient));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
        => Ok(await patientService.DeleteAsync(id));
}
