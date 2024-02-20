using Microsoft.EntityFrameworkCore;
using Sibers_test_app.Domain.Entities;
using Sibers_test_app.Domain;
using Sibers_test_app.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sibers_test_app.Services
{
    public class ProjectService : IProjectService
    {
        private readonly AppDbContext _db;

        public ProjectService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            return await _db.Projects.ToListAsync();
        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            return await _db.Projects.Include(p => p.Employees).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task CreateProjectAsync(Project project)
        {
            _db.Projects.Add(project);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateProjectAsync(Project project)
        {
            _db.Entry(project).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task DeleteProjectAsync(int id)
        {
            var project = await _db.Projects.FindAsync(id);
            _db.Projects.Remove(project);
            await _db.SaveChangesAsync();
        }

        public async Task AddEmployeeToProjectAsync(int employeeId, int projectId)
        {
            var employee = await _db.Employees.FindAsync(employeeId);
            var project = await _db.Projects.FindAsync(projectId);

            project.Employees.Add(employee);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveEmployeeFromProjectAsync(int employeeId, int projectId)
        {
            var project = await _db.Projects.Include(p => p.Employees).FirstOrDefaultAsync(p => p.Id == projectId);
            var employee = project.Employees.FirstOrDefault(e => e.Id == employeeId);

            if (employee != null)
            {
                project.Employees.Remove(employee);
                await _db.SaveChangesAsync();
            }
        }

        public async Task SetProjectLeaderAsync(int employeeId, int projectId)
        {
            var project = await _db.Projects.FindAsync(projectId);
            project.LeaderId = employeeId;
            await _db.SaveChangesAsync();
        }

        public async Task RemoveProjectLeaderAsync(int projectId)
        {
            var project = await _db.Projects.FindAsync(projectId);
            project.LeaderId = 0;
            await _db.SaveChangesAsync();
        }
    }
}