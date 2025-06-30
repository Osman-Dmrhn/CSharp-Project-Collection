using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace FactoryEntitlementProgram.Models
{
        public class WorkdayDetailViewModel
        {
        public int? Id { get; set; }
        public DateTime WorkDate { get; set; }
        public bool IsAbsent { get; set; }
        public bool IsExcused { get; set; }

        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }

        public TimeSpan? OvertimeHours { get; set; }
        
        [NotMapped] // Bu EF tarafından yok sayılır
        public double? OvertimeHoursRaw { get; set; }

        public string Note { get; set; }
    }

        public class EmployeeWorkdayDetailsViewModel
        {
        public int EmployeeId { get; set; }

        public string FullName { get; set; }
        public List<WorkdayDetailViewModel> WorkdayDetails { get; set; }

        [ValidateNever]
        public List<DateTime> Holidays { get; set; }  
    }
}
