using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sibers_test_app.Domain.Entities;
using Sibers_test_app.Domain;

namespace Sibers_test_app.Controllers
{
    public class EmployeeController : Controller
    {
        AppDbContext db;
        public EmployeeController(AppDbContext context)
        {
            db = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await db.Employees.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            db.Employees.Add(employee);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Employee employee = new Employee { Id = id.Value };
                db.Entry(employee).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                Employee? employee = await db.Employees.FirstOrDefaultAsync(p => p.Id == id);
                if (employee != null) return View(employee);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Employee employee)
        {
            db.Employees.Update(employee);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ListOfProject(int? id)
        {
            if (id != null)
            {
                Employee? employee = await db.Employees.Include(p => p.Projects).FirstOrDefaultAsync(p => p.Id == id);
                if (employee != null) return View(employee);
            }
            return NotFound();
        }
    }
}
