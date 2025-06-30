using FactoryEntitlementProgram.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Uygulama veri tabloları
    public DbSet<Employee> Employees { get; set; }
    public DbSet<WorkDay> WorkDays { get; set; }
    public DbSet<Holiday> Holidays { get; set; }
    public DbSet<WageRate> WageRates { get; set; }
    public DbSet<MonthlyEarnings> MonthlyEarnings { get; set; }


    // Fluent API ayarları (isteğe bağlı)
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Identity için gerekli

        // Employee ile AppUser bire bir ilişki
        modelBuilder.Entity<Employee>()
            .HasOne(e => e.AppUser)
            .WithOne(u => u.Employee)
            .HasForeignKey<AppUser>(u => u.EmployeeId)
            .OnDelete(DeleteBehavior.SetNull);

        // WorkDay - Employee ilişkisi
        modelBuilder.Entity<WorkDay>()
            .HasOne(w => w.Employee)
            .WithMany(e => e.WorkDays)
            .HasForeignKey(w => w.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}