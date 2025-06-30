namespace FactoryEntitlementProgram.Models
{
    public class WorkDay
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public DateTime WorkDate { get; set; }

        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }

        public bool IsHoliday { get; set; }     // Resmi tatil mi?
        public bool IsAbsent { get; set; }      // O gün işe gelmedi mi?
        public bool IsExcused { get; set; }     // Devamsızlık mazeretli mi?

        public string Note { get; set; }        // Örn: "Gece vardiyası", "Raporlu"

        public TimeSpan? OvertimeHours { get; set; }
    }
}
