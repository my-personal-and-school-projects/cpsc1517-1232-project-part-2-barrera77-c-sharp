using Microsoft.EntityFrameworkCore;
using StarTEDSystemDB.DAL;
using StarTEDSystemDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarTEDSystemDB.BLL
{
    public class CourseServices
    {
        private readonly StarTEDContext _context;

        internal CourseServices( StarTEDContext context )
        {
            _context = context;
        }

        public Course? GetCourseById(string id)
        {
            return _context.Courses
                .Where(c => c.CourseId == id)
                .FirstOrDefault();
        }

        public List<Course> GetAllCourses()
        {
            return _context.Courses
                .OrderBy(c => c.CourseName)
                .ToList<Course>();
        }

    }
}
