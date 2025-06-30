namespace FactoryEntitlementProgram.Models
{
    public class EmployeeWorkSummaryViewModel
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public int WorkDaysCount { get; set; }
        public double OvertimeHours { get; set; }
        public decimal OvertimePayment { get; set; } 
    }
}
