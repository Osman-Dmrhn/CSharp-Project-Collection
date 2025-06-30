using FactoryEntitlementProgram.Interfaces;
using FactoryEntitlementProgram.Models;
using Microsoft.EntityFrameworkCore;

namespace FactoryEntitlementProgram.Services
{
    public class MonthlyEarningsService : IMonthlyEarningsService
    {
        private readonly AppDbContext _context;

        public MonthlyEarningsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MonthlyEarnings> GetByIdAsync(int id)
        {
            return await _context.MonthlyEarnings
                .Include(me => me.Employee)
                .FirstOrDefaultAsync(me => me.Id == id);
        }

        public async Task<IEnumerable<MonthlyEarnings>> GetAllAsync()
        {
            return await _context.MonthlyEarnings
                .Include(me => me.Employee)
                .ToListAsync();
        }

        public async Task<IEnumerable<MonthlyEarnings>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _context.MonthlyEarnings
                .Where(me => me.EmployeeId == employeeId)
                .OrderByDescending(me => me.Year)
                .ThenByDescending(me => me.Month)
                .ToListAsync();
        }

        public async Task<MonthlyEarnings> GetByEmployeeAndMonthAsync(int employeeId, int year, int month)
        {
            var employee = await _context.Employees
            .Include(e => e.AppUser)
            .FirstOrDefaultAsync(e => e.Id == employeeId);

            var holidays = await _context.Holidays
            .Where(h => h.Date.Year == year && h.Date.Month == month)
            .Select(h => h.Date.Date)
            .ToListAsync();
            
                var workDays = await _context.WorkDays
                    .Where(w => w.EmployeeId == employee.Id && w.WorkDate.Year == year && w.WorkDate.Month == month)
                    .ToListAsync();

                double totalNormalHours = 0;
                double totalOvertimeHours = 0;
                int unexcusedAbsenceCount = 0;
                decimal holidayHoursPayment = 0m;

                foreach (var day in workDays)
                {
                    bool isHoliday = holidays.Contains(day.WorkDate.Date);

                    if (day.IsAbsent && !day.IsExcused)
                    {
                        unexcusedAbsenceCount++;
                    }
                    else if (day.StartTime.HasValue && day.EndTime.HasValue)
                    {
                        double hoursWorked = (day.EndTime.Value - day.StartTime.Value).TotalHours;

                        if (isHoliday)
                        {
                            holidayHoursPayment += (decimal)hoursWorked * employee.SaatlikUcret * 2m;
                        }
                        else
                        {
                            totalNormalHours += hoursWorked;
                        }
                    }

                    if (day.OvertimeHours.HasValue)
                    {
                        totalOvertimeHours += day.OvertimeHours.Value.TotalHours;
                    }
                }

                decimal overtimeRate = 1.5m;
                decimal penaltyPerHour = 1m;

                double penaltyHours = unexcusedAbsenceCount * 8;
                decimal penaltyAmount = (decimal)penaltyHours * penaltyPerHour;

                decimal totalPayment =
                    ((decimal)totalNormalHours * employee.SaatlikUcret) +
                    ((decimal)totalOvertimeHours * employee.SaatlikUcret * overtimeRate) +
                    holidayHoursPayment;

                decimal netPayment = totalPayment - penaltyAmount;


                var monthlyEarning = new MonthlyEarnings
                {
                    EmployeeId = employee.Id,
                    Employee = employee,
                    Year = year,
                    Month = month,
                    TotalNormalHours = totalNormalHours,
                    TotalOvertimeHours = totalOvertimeHours,
                    UnexcusedAbsenceCount = unexcusedAbsenceCount,
                    AbsencePenaltyHours = penaltyHours,
                    AbsencePenaltyAmount = penaltyAmount,
                    TotalPayment = totalPayment,
                    NetPayment = netPayment,
                    CalculatedAt = DateTime.Now
                };


            return monthlyEarning;
        }
    

        public async Task AddAsync(MonthlyEarnings earnings)
        {
            _context.MonthlyEarnings.Add(earnings);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MonthlyEarnings earnings)
        {
            _context.MonthlyEarnings.Update(earnings);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _context.MonthlyEarnings.FindAsync(id);
            if (existing != null)
            {
                _context.MonthlyEarnings.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<MonthlyEarnings>> CalculateMonthlyEarningsAsync(int year, int month)
        {
            var employees = await _context.Employees.Include(e => e.AppUser).ToListAsync();

            var holidays = await _context.Holidays
            .Where(h => h.Date.Year == year && h.Date.Month == month)
            .Select(h => h.Date.Date)
            .ToListAsync();

            var results = new List<MonthlyEarnings>();

            foreach (var employee in employees)
            {
                var workDays = await _context.WorkDays
                    .Where(w => w.EmployeeId == employee.Id && w.WorkDate.Year == year && w.WorkDate.Month == month)
                    .ToListAsync();

                double totalNormalHours = 0;
                double totalOvertimeHours = 0;
                int unexcusedAbsenceCount = 0;
                decimal holidayHoursPayment = 0m;

                foreach (var day in workDays)
                {
                    bool isHoliday = holidays.Contains(day.WorkDate.Date);

                    if (day.IsAbsent && !day.IsExcused)
                    {
                        unexcusedAbsenceCount++;
                    }
                    else if (day.StartTime.HasValue && day.EndTime.HasValue)
                    {
                        double hoursWorked = (day.EndTime.Value - day.StartTime.Value).TotalHours;

                        if (isHoliday)
                        {
                            holidayHoursPayment += (decimal)hoursWorked * employee.SaatlikUcret * 2m;
                        }
                        else
                        {
                            totalNormalHours += hoursWorked;
                        }
                    }

                    if (day.OvertimeHours.HasValue)
                    {
                        totalOvertimeHours += day.OvertimeHours.Value.TotalHours;
                    }
                }

                decimal overtimeRate = 1.5m;
                decimal penaltyPerHour = 1m;

                double penaltyHours = unexcusedAbsenceCount * 8;
                decimal penaltyAmount = (decimal)penaltyHours * penaltyPerHour;

                decimal totalPayment =
                    ((decimal)totalNormalHours * employee.SaatlikUcret) +
                    ((decimal)totalOvertimeHours * employee.SaatlikUcret * overtimeRate) +
                    holidayHoursPayment;

                decimal netPayment = totalPayment - penaltyAmount;


                var monthlyEarning = new MonthlyEarnings
                {
                    EmployeeId = employee.Id,
                    Employee = employee,
                    Year = year,
                    Month = month,
                    TotalNormalHours = totalNormalHours,
                    TotalOvertimeHours = totalOvertimeHours,
                    UnexcusedAbsenceCount = unexcusedAbsenceCount,
                    AbsencePenaltyHours = penaltyHours,
                    AbsencePenaltyAmount = penaltyAmount,
                    TotalPayment = totalPayment,
                    NetPayment = netPayment,
                    CalculatedAt = DateTime.Now
                };

                results.Add(monthlyEarning);
            }

            return results;
        }
    }
}