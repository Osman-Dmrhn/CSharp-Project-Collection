using FactoryEntitlementProgram.Models;

namespace FactoryEntitlementProgram.Interfaces
{
    public interface IWorkDayService
    {
        Task<WorkDay> GetByIdAsync(int id);
        Task<IEnumerable<WorkDay>> GetAllAsync();
        Task<IEnumerable<WorkDay>> GetByEmployeeAsync(int employeeId);
        Task<IEnumerable<WorkDay>> GetByDateRangeAsync(int employeeId, DateTime start, DateTime end);
        Task<IEnumerable<WorkDay>> GetUnexcusedAbsencesAsync(int employeeId, DateTime start, DateTime end);
        Task<List<WorkDay>> GetWorkdaysByEmployeeIdAsync(int employeeId);
        Task AddRangeAsync(IEnumerable<WorkDay> workDays);
        Task<List<WorkDay>> GetByEmployeeAndMonthAsync(int employeeId, int year, int month);

        Task AddAsync(WorkDay workDay);
        Task UpdateAsync(WorkDay workDay);
        Task DeleteAsync(int id);
    }
}
