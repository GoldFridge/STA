using Sibers_test_app.Domain.Entities;
using Sibers_test_app.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sibers_test_app.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> GetAllProjectsAsync();
        Task<Project> GetProjectByIdAsync(int id);
        Task CreateProjectAsync(Project project);
        Task UpdateProjectAsync(Project project);
        Task DeleteProjectAsync(int id);
        Task AddEmployeeToProjectAsync(int employeeId, int projectId);
        Task RemoveEmployeeFromProjectAsync(int employeeId, int projectId);
        Task SetProjectLeaderAsync(int employeeId, int projectId);
        Task RemoveProjectLeaderAsync(int projectId);
    }
}