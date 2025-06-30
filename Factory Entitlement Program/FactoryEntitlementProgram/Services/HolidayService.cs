using FactoryEntitlementProgram.Interfaces;
using FactoryEntitlementProgram.Models;
using Microsoft.EntityFrameworkCore;

namespace FactoryEntitlementProgram.Services
{
    public class HolidayService:IHolidayService
    {
        private readonly AppDbContext _context;

        public HolidayService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Holiday> GetHolidayByIdAsync(int id)
        {
            return await _context.Holidays.FindAsync(id);
        }

        public async Task<IEnumerable<Holiday>> GetAllHolidaysAsync()
        {
            return await _context.Holidays.ToListAsync();
        }

        public async Task<IEnumerable<Holiday>> GetHolidaysInRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Holidays
                .Where(h => h.Date >= startDate && h.Date <= endDate)
                .ToListAsync();
        }

        public async Task AddHolidayAsync(Holiday holiday)
        {
            _context.Holidays.Add(holiday);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateHolidayAsync(Holiday holiday)
        {
            _context.Holidays.Update(holiday);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteHolidayAsync(int id)
        {
            var holiday = await _context.Holidays.FindAsync(id);
            if (holiday != null)
            {
                _context.Holidays.Remove(holiday);
                await _context.SaveChangesAsync();
            }
        }
    }
}
