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


        /// <summary>
        /// Get a course by the Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>a course corresponding to a provided Id</returns>
        public Course? GetCourseById(string id)
        {
            return _context.Courses
                .Where(c => c.CourseId == id)
                .FirstOrDefault();
        }

        /// <summary>
        /// Get a list of all courses
        /// </summary>
        /// <returns>list of all courses </returns>
        public List<Course> GetAllCourses()
        {
            return _context.Courses
                .OrderBy(c => c.CourseName)
                .ToList<Course>();
        }

    }
}
