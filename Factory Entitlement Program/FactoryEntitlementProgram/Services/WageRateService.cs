using FactoryEntitlementProgram.Interfaces;
using FactoryEntitlementProgram.Models;
using Microsoft.EntityFrameworkCore;

namespace FactoryEntitlementProgram.Services
{
    public class WageRateService:IWageRateService
    {
        private readonly AppDbContext _context;

        public WageRateService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<WageRate> GetByIdAsync(int id)
        {
            return await _context.WageRates.FindAsync(id);
        }

        public async Task<IEnumerable<WageRate>> GetAllAsync()
        {
            return await _context.WageRates
                .OrderByDescending(w => w.EffectiveFrom)
                .ToListAsync();
        }

        public async Task<WageRate> GetCurrentRateAsync(DateTime date)
        {
            return await _context.WageRates
                .Where(w => w.EffectiveFrom <= date)
                .OrderByDescending(w => w.EffectiveFrom)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(WageRate wageRate)
        {
            _context.WageRates.Add(wageRate);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(WageRate wageRate)
        {
            _context.WageRates.Update(wageRate);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var wageRate = await _context.WageRates.FindAsync(id);
            if (wageRate != null)
            {
                _context.WageRates.Remove(wageRate);
                await _context.SaveChangesAsync();
            }
        }
    }
}
