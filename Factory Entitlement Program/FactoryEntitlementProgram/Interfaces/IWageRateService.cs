using FactoryEntitlementProgram.Models;

namespace FactoryEntitlementProgram.Interfaces
{
    public interface IWageRateService
    {
        Task<WageRate> GetByIdAsync(int id);
        Task<IEnumerable<WageRate>> GetAllAsync();
        Task<WageRate> GetCurrentRateAsync(DateTime date);
        Task AddAsync(WageRate wageRate);
        Task UpdateAsync(WageRate wageRate);
        Task DeleteAsync(int id);
    }
}
