using Microsoft.EntityFrameworkCore;
using StarTEDSystemDB.DAL;
using StarTEDSystemDB.Entities;


namespace StarTEDSystemDB.BLL
{
    public class ProgramCourseServices
    {
        private readonly StarTEDContext _context;

        internal ProgramCourseServices(StarTEDContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all program courses
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ProgramCourse> GetAllProgramCourses()
        {
            return _context.ProgramCourses                
                .Include(pc => pc.Course)
               .OrderBy(c => c.Course.CourseName)
               .ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns> list of courses associated with a specific program.</returns>
        public List<ProgramCourse> GetAllProgramCourses(int id)
        {
            return _context.ProgramCourses
                .Where(pc => pc.ProgramId == id)
                .Include(pc => pc.Course)              
               .OrderBy(c => c.Course.CourseName)
               .ToList();
        }

        /// <summary>
        /// Get all Courses by Course ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>all ProgramCourses associated with a specific CourseId</returns>
        public List<ProgramCourse> GetAllCourses(string id)
        {
            return _context.ProgramCourses
                .Where(c => c.CourseId == id)
                .Include(c => c.Course)
                .Include(p => p.Program)
               .OrderBy(c => c.Course.CourseName)
               .ToList();
        }

        /// <summary>
        /// Get a single ProgramCourse by course ID including all its details from the Course Table
        /// </summary>
        /// <param name="id"></param>
        /// <returns>a single program associated with a given Id</returns>
        public ProgramCourse? GetProgramCourseById(int id)
        {
            return _context.ProgramCourses
                .Where(pc => pc.ProgramCourseId == id)
                .Include(c => c.Course)
                .Include (p => p.Program)
                .FirstOrDefault();
        }


        //public void DeactivateProgramCourse(int id, bool isActive)
        //{
        //    ProgramCourse UpdatedProgramCourse = _context.ProgramCourses
        //        .FirstOrDefault(pc => pc.ProgramCourseId == id);
        //    UpdatedProgramCourse.Active = isActive;

        //    _context.SaveChanges();
        //}

       /// <summary>
       /// Update a program course
       /// </summary>
       /// <param name="programCourse"></param>
       /// <exception cref="ArgumentNullException"></exception>
        public void UpdateProgramCourse(ProgramCourse programCourse)
        {
            if (programCourse == null)
            {
                throw new ArgumentNullException("Course cannot be null", new ArgumentNullException());
            }
            _context.ProgramCourses.Update(programCourse);
            _context.SaveChanges();
        }


        /// <summary>
        /// Deactivates a ProgramCourse associated with a given ID by updating its "Active" status to false
        /// </summary>
        /// <param name="programCourse"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void DeactivateProgramCourse(ProgramCourse programCourse)
        {
            if (programCourse == null)
            {
                throw new ArgumentNullException("Course cannot be null", new ArgumentNullException());
            }
            programCourse.Active = false;
            UpdateProgramCourse(programCourse);
        }
    }
}
