using Microsoft.EntityFrameworkCore;
using Domain.DTOs.ReportDTO;
using Domain.Enums;
using Domain.Response;
using Infrastructure.Data;
using Infrastructure.Interfaces;

namespace Infrastructure.Services
{
    public class ReportService(DataContext context) : IReportService
    {
        private static Territories? ParseTerritorySafe(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return null;
            return Enum.TryParse<Territories>(input, true, out var result) ? result : null;
        }

        private async Task<HospitalReportExtendedDto> GetStatisticsByTerritoryAsync(Territories territory, DateTime from, DateTime to)
        {
            var patientsQuery = context.Patients
                .Where(p => !p.IsDeleted &&
                            p.TerritoryName == territory &&
                            p.RecordDate.Date >= from.Date &&
                            p.RecordDate.Date <= to.Date);

            return new HospitalReportExtendedDto
            {
                PatientsTotal = await patientsQuery.CountAsync(),
                RecoveredTotal = await patientsQuery.CountAsync(p => p.RecoveryDate != null &&
                    p.RecoveryDate.Value.Date >= from.Date && p.RecoveryDate.Value.Date <= to.Date),

                FluAndColdTotal = await patientsQuery.CountAsync(p => p.Disease == DiseaseType.Flu || p.Disease == DiseaseType.Cold),
                FluAndColdRecovered = await patientsQuery.CountAsync(p => (p.Disease == DiseaseType.Flu || p.Disease == DiseaseType.Cold) && p.RecoveryDate != null),

                TyphoidTotal = await patientsQuery.CountAsync(p => p.Disease == DiseaseType.Fever),
                TyphoidRecovered = await patientsQuery.CountAsync(p => p.Disease == DiseaseType.Fever && p.RecoveryDate != null),

                HepatitisTotal = await patientsQuery.CountAsync(p => p.Disease == DiseaseType.Hepatitis),
                HepatitisRecovered = await patientsQuery.CountAsync(p => p.Disease == DiseaseType.Hepatitis && p.RecoveryDate != null),

                OtherDiseasesTotal = await patientsQuery.CountAsync(p =>
                    p.Disease != DiseaseType.Flu &&
                    p.Disease != DiseaseType.Cold &&
                    p.Disease != DiseaseType.Fever &&
                    p.Disease != DiseaseType.Hepatitis),

                OtherDiseasesRecovered = await patientsQuery.CountAsync(p =>
                    p.Disease != DiseaseType.Flu &&
                    p.Disease != DiseaseType.Cold &&
                    p.Disease != DiseaseType.Fever &&
                    p.Disease != DiseaseType.Hepatitis &&
                    p.RecoveryDate != null)
            };
        }

        public async Task<Response<HospitalReportExtendedDto>> GetStatisticsByTerritoryNameAsync(string territoryName, DateTime from, DateTime to)
        {
            var territory = ParseTerritorySafe(territoryName);
            if (territory is null)
                return new Response<HospitalReportExtendedDto>(System.Net.HttpStatusCode.NotFound, "Территория не найдена");

            var report = await GetStatisticsByTerritoryAsync(territory.Value, from, to);
            return new Response<HospitalReportExtendedDto>(report);
        }

        public async Task<HospitalReportExtendedDto> GetStatisticsFirdavsi(DateTime from, DateTime to)
            => await GetStatisticsByTerritoryAsync(Territories.Firdavsi, from, to);

        public async Task<HospitalReportExtendedDto> GetStatisticsShohmansur(DateTime from, DateTime to)
            => await GetStatisticsByTerritoryAsync(Territories.Shohmansur, from, to);

        public async Task<HospitalReportExtendedDto> GetStatisticsSino(DateTime from, DateTime to)
            => await GetStatisticsByTerritoryAsync(Territories.Sino, from, to);

        public async Task<HospitalReportExtendedDto> GetStatisticsSomoni(DateTime from, DateTime to)
            => await GetStatisticsByTerritoryAsync(Territories.Somoni, from, to);

        public async Task<HospitalReportExtendedDto> GetStatisticsAllTerritories(DateTime from, DateTime to)
        {
            from = DateTime.SpecifyKind(from, DateTimeKind.Utc);
            to = DateTime.SpecifyKind(to, DateTimeKind.Utc);

            var patientsQuery = context.Patients
                .Where(p => !p.IsDeleted &&
                            p.RecordDate.Date >= from.Date &&
                            p.RecordDate.Date <= to.Date);

            return new HospitalReportExtendedDto
            {
                PatientsTotal = await patientsQuery.CountAsync(),
                RecoveredTotal = await patientsQuery.CountAsync(p => p.RecoveryDate != null &&
                    p.RecoveryDate.Value.Date >= from.Date && p.RecoveryDate.Value.Date <= to.Date),

                FluAndColdTotal = await patientsQuery.CountAsync(p => p.Disease == DiseaseType.Flu || p.Disease == DiseaseType.Cold),
                FluAndColdRecovered = await patientsQuery.CountAsync(p => (p.Disease == DiseaseType.Flu || p.Disease == DiseaseType.Cold) && p.RecoveryDate != null),

                TyphoidTotal = await patientsQuery.CountAsync(p => p.Disease == DiseaseType.Fever),
                TyphoidRecovered = await patientsQuery.CountAsync(p => p.Disease == DiseaseType.Fever && p.RecoveryDate != null),

                HepatitisTotal = await patientsQuery.CountAsync(p => p.Disease == DiseaseType.Hepatitis),
                HepatitisRecovered = await patientsQuery.CountAsync(p => p.Disease == DiseaseType.Hepatitis && p.RecoveryDate != null),

                OtherDiseasesTotal = await patientsQuery.CountAsync(p =>
                    p.Disease != DiseaseType.Flu &&
                    p.Disease != DiseaseType.Cold &&
                    p.Disease != DiseaseType.Fever &&
                    p.Disease != DiseaseType.Hepatitis),

                OtherDiseasesRecovered = await patientsQuery.CountAsync(p =>
                    p.Disease != DiseaseType.Flu &&
                    p.Disease != DiseaseType.Cold &&
                    p.Disease != DiseaseType.Fever &&
                    p.Disease != DiseaseType.Hepatitis &&
                    p.RecoveryDate != null)
            };
        }

        // ✅ Новый метод, возвращающий массив данных по всем территориям
        public async Task<List<HospitalReportExtendedByTerritoryDto>> GetStatisticsAllTerritoriesExtended(DateTime from, DateTime to)
        {
            from = DateTime.SpecifyKind(from, DateTimeKind.Utc);
            to = DateTime.SpecifyKind(to, DateTimeKind.Utc);

            var territories = Enum.GetValues<Territories>();
            var reports = new List<HospitalReportExtendedByTerritoryDto>();

            foreach (var territory in territories)
            {
                var stats = await GetStatisticsByTerritoryAsync(territory, from, to);
                reports.Add(new HospitalReportExtendedByTerritoryDto
                {
                    Territory = territory.ToString(),
                    PatientsTotal = stats.PatientsTotal,
                    RecoveredTotal = stats.RecoveredTotal,
                    FluAndColdTotal = stats.FluAndColdTotal,
                    FluAndColdRecovered = stats.FluAndColdRecovered,
                    TyphoidTotal = stats.TyphoidTotal,
                    TyphoidRecovered = stats.TyphoidRecovered,
                    HepatitisTotal = stats.HepatitisTotal,
                    HepatitisRecovered = stats.HepatitisRecovered,
                    OtherDiseasesTotal = stats.OtherDiseasesTotal,
                    OtherDiseasesRecovered = stats.OtherDiseasesRecovered
                });
            }

            return reports;
        }
    }
}
