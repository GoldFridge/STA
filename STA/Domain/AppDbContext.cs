using Microsoft.EntityFrameworkCore;
using Sibers_test_app.Domain.Entities;

namespace Sibers_test_app.Domain
{
    public class AppDbContext : DbContext
    {   

        public DbSet<Project> Projects { get; set; }
        public DbSet<Employee> Employees { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Добавление тестовых данных
            modelBuilder.Entity<Employee>().HasData(new Employee
            {
                Id = 1,
                FirstName = "Ryan",
                LastName = "Gosling",
                MiddleName = "Ryanovich"
            });

            modelBuilder.Entity<Project>().HasData(new Project
            {
                Id=1,
                Name = "SibersTest"
            });
            // Определение связи между проектами и сотрудниками
            modelBuilder.Entity<Project>()
             .HasMany(p => p.Employees)
             .WithMany(e => e.Projects)
             .UsingEntity<Dictionary<string, object>>(
                 "ProjectEmployees",
                 j => j.HasOne<Employee>().WithMany().HasForeignKey("EmployeeId"),
                 j => j.HasOne<Project>().WithMany().HasForeignKey("ProjectId"),
                 j =>
                 {
                     j.HasKey("ProjectId", "EmployeeId");
                     j.ToTable("ProjectEmployees");
                 });
        }
    }
}
