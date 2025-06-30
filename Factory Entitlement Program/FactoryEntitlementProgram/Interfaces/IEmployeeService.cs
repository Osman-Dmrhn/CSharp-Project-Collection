using FactoryEntitlementProgram.Models;

namespace FactoryEntitlementProgram.Interfaces
{
    public interface IEmployeeService
    {
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task AddEmployeeAsync(Employee employee);
        Task UpdateEmployeeAsync(Employee employee, string fullName, string email, string department, string role, decimal saatlikUcret);
        Task DeleteEmployeeAsync(int id);

    }
}
