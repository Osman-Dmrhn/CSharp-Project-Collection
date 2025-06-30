using FactoryEntitlementProgram.Interfaces;
using FactoryEntitlementProgram.Models;
using Microsoft.EntityFrameworkCore;

namespace FactoryEntitlementProgram.Services
{
    public class WorkDayService:IWorkDayService
    {
        private readonly AppDbContext _context;

        public WorkDayService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<WorkDay> GetByIdAsync(int id)
        {
            return await _context.WorkDays
                .Include(w => w.Employee)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<List<WorkDay>> GetByEmployeeAndMonthAsync(int employeeId, int year, int month)
        {
            return await _context.WorkDays
            .Where(wd => wd.EmployeeId == employeeId &&
                      wd.WorkDate.Year == year &&
                      wd.WorkDate.Month == month)
            .ToListAsync();
        }

        public async Task<IEnumerable<WorkDay>> GetAllAsync()
        {
            return await _context.WorkDays
                .Include(w => w.Employee)
                .ToListAsync();
        }
        public async Task<List<WorkDay>> GetWorkdaysByEmployeeIdAsync(int employeeId)
        {
            return await _context.WorkDays
                .Where(wd => wd.EmployeeId == employeeId)
                .OrderBy(wd => wd.WorkDate)
                .ToListAsync();
        }
        public async Task<IEnumerable<WorkDay>> GetByEmployeeAsync(int employeeId)
        {
            return await _context.WorkDays
                .Where(w => w.EmployeeId == employeeId)
                .OrderByDescending(w => w.WorkDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkDay>> GetByDateRangeAsync(int employeeId, DateTime start, DateTime end)
        {
            return await _context.WorkDays
                .Where(w => w.EmployeeId == employeeId && w.WorkDate >= start && w.WorkDate <= end)
                .OrderBy(w => w.WorkDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<WorkDay>> GetUnexcusedAbsencesAsync(int employeeId, DateTime start, DateTime end)
        {
            return await _context.WorkDays
                .Where(w =>
                    w.EmployeeId == employeeId &&
                    w.WorkDate >= start &&
                    w.WorkDate <= end &&
                    w.IsAbsent &&
                    !w.IsExcused)
                .ToListAsync();
        }

        public async Task AddAsync(WorkDay workDay)
        {
            _context.WorkDays.Add(workDay);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(WorkDay workDay)
        {
            _context.WorkDays.Update(workDay);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var workDay = await _context.WorkDays.FindAsync(id);
            if (workDay != null)
            {
                _context.WorkDays.Remove(workDay);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddRangeAsync(IEnumerable<WorkDay> workDays)
        {
            _context.WorkDays.AddRange(workDays);
            await _context.SaveChangesAsync();
        }
    }
}
