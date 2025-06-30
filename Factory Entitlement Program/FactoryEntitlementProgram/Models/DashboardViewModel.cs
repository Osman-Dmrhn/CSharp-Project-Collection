namespace FactoryEntitlementProgram.Models
{
    public class DashboardViewModel
    {
        public string FullName { get; set; }
        public string EmployeeCode { get; set; }

        public List<DateTime> AttendedDays { get; set; } = new();
        public List<DateTime> ExcusedDays { get; set; } = new();
        public List<DateTime> UnexcusedDays { get; set; } = new();

        public List<DateTime> HolidayDays { get; set; } = new();

        public decimal NormalHoursPay { get; set; }
        public decimal OvertimeHoursPay { get; set; }

        public double TotalNormalHours { get; set; }
        public double TotalOvertimeHours { get; set; }
        public int UnexcusedAbsenceCount { get; set; }
        public double AbsencePenaltyHours { get; set; }
        public decimal AbsencePenaltyAmount { get; set; }
        public decimal NetPayment { get; set; }
    }
}
