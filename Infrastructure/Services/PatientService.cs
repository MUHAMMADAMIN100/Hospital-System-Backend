// PatientService без пагинации
using System.Net;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.DTOs.PatientDTO;
using Domain.Entities;
using Domain.Filter;
using Domain.Response;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class PatientService(DataContext context, IMapper mapper)
{
    // Получить всех пациентов с фильтрацией, без пагинации
    public async Task<List<GetPatientDTO>> GetAllAsync(PatientFilter filter)
    {
        var query = context.Patients
            .AsNoTracking()
            .Where(p => !p.IsDeleted)
            .AsQueryable();

        if (!string.IsNullOrEmpty(filter.HospitalRegistrationNumber))
            query = query.Where(p => p.HospitalRegistrationNumber == filter.HospitalRegistrationNumber);

        if (!string.IsNullOrEmpty(filter.Name))
            query = query.Where(p => p.Name.Contains(filter.Name));

        var data = await query
            .ProjectTo<GetPatientDTO>(mapper.ConfigurationProvider)
            .ToListAsync();

        return data;
    }

    // Получить пациента по ID
    public async Task<Response<GetPatientDTO>> GetByIdAsync(int id)
    {
        var patient = await context.Patients
            .AsNoTracking()
            .ProjectTo<GetPatientDTO>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (patient == null)
            return new Response<GetPatientDTO>(HttpStatusCode.NotFound, "Patient not found");

        return new Response<GetPatientDTO>(patient);
    }

    // Создать нового пациента
    public async Task<Response<GetPatientDTO>> CreateAsync(CreatePatientDTO patientDTO)
    {
        var hospitalExists = await context.Hospitals
            .AnyAsync(h => h.RegistrationNumber == patientDTO.HospitalRegistrationNumber);

        if (!hospitalExists)
            return new Response<GetPatientDTO>(HttpStatusCode.NotFound, "Hospital not found");

        var patient = mapper.Map<Patient>(patientDTO);
        await context.Patients.AddAsync(patient);
        var result = await context.SaveChangesAsync();

        return result > 0
            ? new Response<GetPatientDTO>(mapper.Map<GetPatientDTO>(patient))
            : new Response<GetPatientDTO>(HttpStatusCode.InternalServerError, "Failed to create patient");
    }

    // Обновить данные пациента
    public async Task<Response<GetPatientDTO>> UpdateAsync(int id, UpdatePatientDTO patientDto)
    {
        var existingPatient = await context.Patients.FindAsync(id);
        if (existingPatient == null)
            return new Response<GetPatientDTO>(HttpStatusCode.NotFound, "Patient not found");

        if (patientDto.RecoveryDate.HasValue && patientDto.RecoveryDate.Value < existingPatient.RecordDate)
            return new Response<GetPatientDTO>(HttpStatusCode.BadRequest, "Recovery date cannot be earlier than record date");

        mapper.Map(patientDto, existingPatient);

        var result = await context.SaveChangesAsync();

        return result > 0
            ? new Response<GetPatientDTO>(mapper.Map<GetPatientDTO>(existingPatient))
            : new Response<GetPatientDTO>(HttpStatusCode.InternalServerError, "Failed to update patient");
    }

    // Мягкое удаление пациента
    public async Task<Response<string>> DeleteAsync(int id)
    {
        var patient = await context.Patients.FindAsync(id);
        if (patient == null)
            return new Response<string>(HttpStatusCode.NotFound, "Patient not found");

        patient.IsDeleted = true;
        var result = await context.SaveChangesAsync();

        return result > 0
            ? new Response<string>("Patient deleted successfully")
            : new Response<string>(HttpStatusCode.InternalServerError, "Failed to delete patient");
    }
}
