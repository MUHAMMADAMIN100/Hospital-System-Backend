namespace Domain.DTOs.ReportDTO
{
    public class HospitalReportExtendedDto
    {
        public int PatientsTotal { get; set; }
        public int RecoveredTotal { get; set; }

        public int FluAndColdTotal { get; set; }
        public int FluAndColdRecovered { get; set; }

        public int TyphoidTotal { get; set; }
        public int TyphoidRecovered { get; set; }

        public int HepatitisTotal { get; set; }
        public int HepatitisRecovered { get; set; }

        public int OtherDiseasesTotal { get; set; }
        public int OtherDiseasesRecovered { get; set; }

        // Проценты для наглядности
        public decimal RecoveryPercent =>
            PatientsTotal > 0 ? Math.Round((decimal)RecoveredTotal * 100 / PatientsTotal, 2) : 0;

        public decimal FluAndColdPercent =>
            FluAndColdTotal > 0 ? Math.Round((decimal)FluAndColdRecovered * 100 / FluAndColdTotal, 2) : 0;

        public decimal TyphoidPercent =>
            TyphoidTotal > 0 ? Math.Round((decimal)TyphoidRecovered * 100 / TyphoidTotal, 2) : 0;

        public decimal HepatitisPercent =>
            HepatitisTotal > 0 ? Math.Round((decimal)HepatitisRecovered * 100 / HepatitisTotal, 2) : 0;

        public decimal OtherDiseasesPercent =>
            OtherDiseasesTotal > 0 ? Math.Round((decimal)OtherDiseasesRecovered * 100 / OtherDiseasesTotal, 2) : 0;
    }
}
