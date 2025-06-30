namespace FactoryEntitlementProgram.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string Department { get; set; }

        public decimal SaatlikUcret { get; set; }

        // Navigation
        public ICollection<WorkDay> WorkDays { get; set; }
        public AppUser AppUser { get; set; }
    }
}
