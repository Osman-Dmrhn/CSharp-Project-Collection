namespace FactoryEntitlementProgram.Models
{
    public class EmployeeEditViewModel
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Department { get; set; }
        public string? Role { get; set; }

        public decimal SaatlikUcret { get; set; }
    }
}
