using System;

namespace Domain.DTOs.ReportDTO
{
    // Краткий отчёт по одной больнице или территории
    public class HospitalReportDto
    {
        public string HospitalName { get; set; }
        public string TerritoryName { get; set; }

        public int PatientsTotal { get; set; }
        public int RecoveredTotal { get; set; }

        public decimal RecoveryPercent =>
            PatientsTotal > 0
                ? Math.Round((decimal)RecoveredTotal * 100 / PatientsTotal, 2)
                : 0;

        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
