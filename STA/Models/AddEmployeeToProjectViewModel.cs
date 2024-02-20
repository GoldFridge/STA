using Sibers_test_app.Domain.Entities;

namespace Sibers_test_app.ViewModels
{
    public class AddEmployeeToProjectViewModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = "";
        public List<Employee> Employees { get; set; } = new List<Employee>();
    }
}