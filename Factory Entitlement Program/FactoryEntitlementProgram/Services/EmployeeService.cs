using FactoryEntitlementProgram.Interfaces;
using FactoryEntitlementProgram.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FactoryEntitlementProgram.Services
{
    public class EmployeeService:IEmployeeService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public EmployeeService(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees
            .Include(e => e.AppUser)
            .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees
                         .Include(e => e.AppUser)
                         .ToListAsync();
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEmployeeAsync(Employee employee, string fullName, string email, string department, string role, decimal saatlikUcret)
        {
            if (employee == null || employee.AppUser == null)
                return;

            // 1. Departman ve SaatlikUcret güncelle
            employee.Department = department;
            employee.SaatlikUcret = saatlikUcret;

            // 2. AppUser verilerini güncelle
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == employee.AppUser.Id);
            if (user != null)
            {
                user.FullName = fullName;
                user.Email = email;
                user.UserName = email;
                user.NormalizedEmail = email.ToUpper();
                user.NormalizedUserName = email.ToUpper();
                user.Role = role;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    throw new Exception("Kullanıcı güncellenirken hata oluştu.");
                }
            }

            // 3. Employee tablosunu güncelle
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employees
        .Include(e => e.AppUser)
        .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
                return;

            if (employee.AppUser != null)
            {
                await _userManager.DeleteAsync(employee.AppUser);
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }

    }
}
