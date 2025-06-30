namespace FactoryEntitlementProgram.Models
{
    public class EmployeeCreateViewModel
    {
        // Employee bilgileri
        public string Department { get; set; }

        // Kullanıcı bilgileri
        public string FullName { get; set; }
        public string Role { get; set; } // Admin veya User
        public string Email { get; set; } // Kullanıcı emaili (login için)
        public string Password { get; set; } // Parola

        public decimal SaatlikUcret { get; set; }

        public List<Employee> Employees { get; set; } = new();// Tabloda listelenecek personeller
    }
}
