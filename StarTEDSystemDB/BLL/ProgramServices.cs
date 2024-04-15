using StarTEDSystemDB.DAL;
using StarTEDSystemDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarTEDSystemDB.BLL
{
    public class ProgramServices
    {

        private readonly StarTEDContext _context;

        internal ProgramServices(StarTEDContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Create a list of programs
        /// </summary>
        /// <returns></returns>
        public List<Program> GetAllPrograms()
        {
            return _context.Programs
                .OrderBy(p => p.ProgramName)
                .ToList<Program>();
        }
    }
}
