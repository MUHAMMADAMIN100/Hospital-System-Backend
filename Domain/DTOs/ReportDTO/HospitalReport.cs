using System;

namespace Domain.DTOs.ReportDTO
{
    // Общий отчёт по всем территориям
    public class HospitalReport
    {
        public int PatientsTotalAllTerritories { get; set; }
        public int RecoveredTotalAllTerritories { get; set; }

        public decimal RecoveryPercentInPeriod =>
            PatientsTotalAllTerritories > 0
                ? Math.Round((decimal)RecoveredTotalAllTerritories * 100 / PatientsTotalAllTerritories, 2)
                : 0;

        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
