namespace Sibers_test_app.Domain.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string CustomerCompanyName { get; set; } = "";
        public string ExecutorCompanyName { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Priority { get; set; }
        public int LeaderId { get; set; }
   
        // Сотрудники, работающие на проекте
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    }
}
