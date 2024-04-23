using Microsoft.AspNetCore.Components;
using StarTEDSystemDB.BLL;
using StarTEDSystemDB.Entities;

namespace StarTEDSystemWebApp.Components.Pages
{
    public partial class Query
    {
        [Inject]
        CourseServices CourseServices { get; set; }

        [Inject]
        ProgramServices ProgramServices { get; set; }

        [Inject]
        ProgramCourseServices ProgramCourseServices { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        //Required properties
        public List<StarTEDSystemDB.Entities.Program> Programs { get; set; } 
        public List<ProgramCourse> ProgramCourses { get; set; }

        public List<string> errorList = new List<string>();
        public string feedback { get; set; }
        

        [Parameter]
        public int ProgramId { get; set; }

        [Parameter]
        public string CourseId { get; set; }

        [Parameter]
        public int ProgramCourseId { get; set; }

        [Parameter]
        public string CourseName { get; set; }

        [Parameter]
        public bool Required { get; set; }

        [Parameter]
        public int SectionLimit { get; set; }

        [Parameter]
        public double Credits { get; set; }


        protected override Task OnInitializedAsync()
        {
            return Task.Run(() =>
            {
                Programs = ProgramServices.GetAllPrograms();
               
            });
        }

        private void HandleSelectedProgram()
        {
            if (ProgramId != 0)
            {
                ProgramCourses = ProgramCourseServices.GetAllProgramCourses(ProgramId);
                NavigationManager.NavigateTo($"/query");
            }
        }    
    }
}
