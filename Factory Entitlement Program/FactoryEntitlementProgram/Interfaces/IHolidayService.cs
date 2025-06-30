using FactoryEntitlementProgram.Models;

namespace FactoryEntitlementProgram.Interfaces
{
    public interface IHolidayService
    {
        Task<Holiday> GetHolidayByIdAsync(int id);
        Task<IEnumerable<Holiday>> GetAllHolidaysAsync();
        Task<IEnumerable<Holiday>> GetHolidaysInRangeAsync(DateTime startDate, DateTime endDate);
        Task AddHolidayAsync(Holiday holiday);
        Task UpdateHolidayAsync(Holiday holiday);
        Task DeleteHolidayAsync(int id);
    }
}
