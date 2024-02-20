using System.ComponentModel.DataAnnotations;

namespace Sibers_test_app.Domain.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string MiddleName { get; set; } = "";
        public string Email { get; set; } = "";

        // Проекты, над которыми работает сотрудник
        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
