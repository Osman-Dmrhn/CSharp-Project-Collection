using FactoryEntitlementProgram.Models;

namespace FactoryEntitlementProgram.Interfaces
{
    public interface IMonthlyEarningsService
    {
        Task<MonthlyEarnings> GetByIdAsync(int id);
        Task<IEnumerable<MonthlyEarnings>> GetAllAsync();
        Task<IEnumerable<MonthlyEarnings>> GetByEmployeeIdAsync(int employeeId);
        Task<MonthlyEarnings> GetByEmployeeAndMonthAsync(int employeeId, int year, int month);

        Task<List<MonthlyEarnings>> CalculateMonthlyEarningsAsync(int year, int month);
        Task AddAsync(MonthlyEarnings earnings);
        Task UpdateAsync(MonthlyEarnings earnings);
        Task DeleteAsync(int id);
    }
}
