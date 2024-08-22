using CPW219_CRUD_Troubleshooting.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CPW219_CRUD_Troubleshooting.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SchoolContext context;

        public StudentsController(SchoolContext dbContext)
        {
            context = dbContext;
        }

        public async Task<IActionResult> StudentRoster()
        {
            List<Student> students = await context.Students.ToListAsync();
            return View(students);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Student s)
        {
            if (ModelState.IsValid)
            {
                context.Students.Add(s);
                await context.SaveChangesAsync();

                TempData["Message"] = $"{s.Name} was added!";
                return RedirectToAction("StudentRoster");
            }

            //Show web page with errors
            return View(s);
        }

        public IActionResult Edit(int id)
        {
            //get the product by id
            Student p = StudentDb.GetStudent(context, id);
            if (p == null)
            {
                return NotFound();
            }

            //show it on web page
            return View(p);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Student s)
        {
            if (ModelState.IsValid)
            {
                context.Students.Update(s);
                await context.SaveChangesAsync();

                ViewData["Message"] = "Product Updated!";
                return RedirectToAction("StudentRoster");
            }
            //return view with errors
            return View(s);
        }

        public IActionResult Delete(int id)
        {
            Student p = StudentDb.GetStudent(context, id);
            return View(p);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirm(int id)
        {
            //Get Product from database
            Student p = StudentDb.GetStudent(context, id);

            StudentDb.Delete(context, p);

            return RedirectToAction("Index");
        }
    }
}
