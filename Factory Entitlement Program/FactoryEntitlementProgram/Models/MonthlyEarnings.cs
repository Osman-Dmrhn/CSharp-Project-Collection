namespace FactoryEntitlementProgram.Models
{
    public class MonthlyEarnings
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }

        public double TotalNormalHours { get; set; }
        public double TotalOvertimeHours { get; set; }

        public int UnexcusedAbsenceCount { get; set; }
        public double AbsencePenaltyHours { get; set; }
        public decimal AbsencePenaltyAmount { get; set; }

        public decimal TotalPayment { get; set; }
        public decimal NetPayment { get; set; }

        public DateTime CalculatedAt { get; set; }
    }
}
