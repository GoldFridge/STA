using Microsoft.AspNetCore.Mvc;
using Sibers_test_app.Models;
using Sibers_test_app.Services;
using System.Threading.Tasks;
using Sibers_test_app.Domain.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Sibers_test_app.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public async Task<IActionResult> Index(SortState sortOrder = SortState.PriorityAsc,string? name = null, int? priority = null, DateTime? startDate = null, DateTime? EndDate = null)
        {
            var projects = await _projectService.GetAllProjectsAsync();

            if (priority != null && priority != 0)
            {
                projects = projects.Where(p => p.Priority == priority);
            }
            if (!string.IsNullOrEmpty(name))
            {
                projects = projects.Where(p => p.Name!.Contains(name));
            }
            if (startDate != null)
            {
                projects = projects.Where(p => p.StartDate >= startDate);
            }
            if(EndDate != null)
            {
                projects = projects.Where(p => p.EndDate <= EndDate);
            }
            

            ViewData["NameSort"] = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            ViewData["CustomerSort"] = sortOrder == SortState.CustomerCompanyNameAsc ? SortState.CustomerCompanyNameDesc : SortState.CustomerCompanyNameAsc;
            ViewData["ExecutorSort"] = sortOrder == SortState.ExecutorCompanyNameAsc ? SortState.ExecutorCompanyNameDesc : SortState.ExecutorCompanyNameAsc;
            ViewData["StartDateSort"] = sortOrder == SortState.StartDateAsc ? SortState.StartDateDesc : SortState.StartDateAsc;
            ViewData["EndDateSort"] = sortOrder == SortState.EndDateAsc ? SortState.EndDateDesc : SortState.EndDateAsc;
            ViewData["PrioritySort"] = sortOrder == SortState.PriorityAsc ? SortState.PriorityDesc : SortState.PriorityAsc;

            projects = sortOrder switch
            {
                SortState.NameAsc => projects.OrderBy(s => s.Name),
                SortState.NameDesc => projects.OrderByDescending(s => s.Name),
                SortState.CustomerCompanyNameAsc => projects.OrderBy(s => s.CustomerCompanyName),
                SortState.CustomerCompanyNameDesc => projects.OrderByDescending(s => s.CustomerCompanyName),
                SortState.ExecutorCompanyNameAsc => projects.OrderBy(s => s.ExecutorCompanyName),
                SortState.ExecutorCompanyNameDesc => projects.OrderByDescending(s => s.ExecutorCompanyName),
                SortState.StartDateAsc => projects.OrderBy(s => s.StartDate),
                SortState.StartDateDesc => projects.OrderByDescending(s => s.StartDate),
                SortState.EndDateAsc => projects.OrderBy(s => s.EndDate),
                SortState.EndDateDesc => projects.OrderByDescending(s => s.EndDate),
                SortState.PriorityAsc => projects.OrderBy(s => s.Priority),
                SortState.PriorityDesc => projects.OrderByDescending(s => s.Priority),
                _ => projects,
            };

            return View(projects);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Project project)
        {
            if (ModelState.IsValid)
            {
                await _projectService.CreateProjectAsync(project);
                return RedirectToAction("Index");
            }
            return View(project);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                await _projectService.DeleteProjectAsync(id.Value);
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                var project = await _projectService.GetProjectByIdAsync(id.Value);
                if (project != null)
                {
                    return View(project);
                }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Project project)
        {
            if (ModelState.IsValid)
            {
                await _projectService.UpdateProjectAsync(project);
                return RedirectToAction("Index");
            }
            return View(project);
        }

        public async Task<IActionResult> Employees(int? id)
        {
            if (id != null)
            {
                var project = await _projectService.GetProjectByIdAsync(id.Value);
                if (project != null)
                {
                    return View(project);
                }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddToProject(int id, int projectId)
        {
            await _projectService.AddEmployeeToProjectAsync(id, projectId);
            return RedirectToAction("Employees", new { id = projectId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFromProject(int id, int projectId)
        {
            await _projectService.RemoveEmployeeFromProjectAsync(id, projectId);
            return RedirectToAction("Employees", new { id = projectId });
        }

        [HttpPost]
        public async Task<IActionResult> DoLeader(int id, int projectId)
        {
            await _projectService.SetProjectLeaderAsync(id, projectId);
            return RedirectToAction("Employees", new { id = projectId });
        }

        [HttpPost]
        public async Task<IActionResult> DoNoLeader(int id, int projectId)
        {
            await _projectService.RemoveProjectLeaderAsync(projectId);
            return RedirectToAction("Employees", new { id = projectId });
        }
    }
}
