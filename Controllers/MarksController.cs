﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebTestAssesment.Models;

namespace WebTestAssesment.Controllers
{
    public class MarksController : Controller
    {
        private readonly TestAssesmentDbContext _context;

        private Student _student { get; set; } = new Student();  
        private Course _course { get; set; } = new Course();
        private List<Course> _courseList { get; set; }   
        private List<Student> _studentList { get; set; }
        public MarksController(TestAssesmentDbContext context)
        {
            _context = context;
        }

        // GET: Marks
        public async Task<IActionResult> Index()
        {
            var testAssesmentDbContext = _context.Marks.Include(m => m.Course).Include(m => m.Student);
            return View(await testAssesmentDbContext.ToListAsync());
        }

        // GET: Marks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Marks == null)
            {
                return NotFound();
            }

            var mark = await _context.Marks
                .Include(m => m.Course)
                .Include(m => m.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mark == null)
            {
                return NotFound();
            }

            return View(mark);
        }

        // GET: Marks/Create
        public async Task<IActionResult> CreateAsync()
        {
            _courseList =  _context.Courses.ToList();
            _studentList =  _context.Students.ToList();

			ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "CourseName");
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "FullName");

            ViewData["Course"] = _courseList.FirstOrDefault();
            ViewData["Student"] = _studentList.FirstOrDefault();
            return View();
        }

        // POST: Marks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CourseId,StudentId,Marks,Course,Student")] Mark mark)
        {
            //var s = await _context.Students.FindAsync(mark.StudentId);
            //var c = await _context.Courses.FindAsync(mark.StudentId);
            ////mark.Student = s;
            ////mark.Course = c;
            if (ModelState.IsValid)
            {
               int res = await CreateStudentMarkSqlRawAsync(_context, mark.StudentId, mark.CourseId, mark.Marks);
                //_context.Add(mark);
                //await _context.SaveChangesAsync();
                //if (res == -1)
                //{
                    return RedirectToAction(nameof(Index));
                //}
               
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "CourseName", mark.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "FullName", mark.StudentId);
            return View(mark);
        }

        // GET: Marks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Marks == null)
            {
                return NotFound();
            }

            var mark = await _context.Marks.FindAsync(id);
            if (mark == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "CourseName", mark.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "FullName", mark.StudentId);
            return View(mark);
        }

        // POST: Marks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseId,StudentId,Marks")] Mark mark)
        {
            if (id != mark.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mark);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MarkExists(mark.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "CourseName", mark.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "FullName", mark.StudentId);
            return View(mark);
        }

        // GET: Marks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Marks == null)
            {
                return NotFound();
            }

            var mark = await _context.Marks
                .Include(m => m.Course)
                .Include(m => m.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mark == null)
            {
                return NotFound();
            }

            return View(mark);
        }

        // POST: Marks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Marks == null)
            {
                return Problem("Entity set 'TestAssesmentDbContext.Marks'  is null.");
            }
            var mark = await _context.Marks.FindAsync(id);
            if (mark != null)
            {
                _context.Marks.Remove(mark);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MarkExists(int id)
        {
          return (_context.Marks?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async static Task<int> CreateStudentMarkSqlRawAsync(TestAssesmentDbContext context, int studentId,int courseId, int mark)
        {
            return await context.Database.ExecuteSqlRawAsync("dbo.Sp_CreateMarks @StudentId,@CourseId, @Marks",
                new SqlParameter("@StudentId", studentId),
                new SqlParameter("@CourseId", courseId),
                new SqlParameter("@Marks", mark)
                );
        }
    }
}