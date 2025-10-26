using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DTOs.ReportDTO;
using Domain.Response;

namespace Infrastructure.Interfaces
{
    public interface IReportService
    {
        Task<Response<HospitalReportExtendedDto>> GetStatisticsByTerritoryNameAsync(string territoryName, DateTime from, DateTime to);

        // ✅ Исправлено: теперь возвращает список DTO
        Task<List<HospitalReportExtendedByTerritoryDto>> GetStatisticsAllTerritoriesExtended(DateTime from, DateTime to);

        Task<HospitalReportExtendedDto> GetStatisticsFirdavsi(DateTime from, DateTime to);
        Task<HospitalReportExtendedDto> GetStatisticsShohmansur(DateTime from, DateTime to);
        Task<HospitalReportExtendedDto> GetStatisticsSino(DateTime from, DateTime to);
        Task<HospitalReportExtendedDto> GetStatisticsSomoni(DateTime from, DateTime to);

        Task<HospitalReportExtendedDto> GetStatisticsAllTerritories(DateTime from, DateTime to);
    }
}
