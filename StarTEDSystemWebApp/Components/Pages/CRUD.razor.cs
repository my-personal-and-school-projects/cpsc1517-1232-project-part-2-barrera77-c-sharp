using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StarTEDSystemDB.BLL;
using StarTEDSystemDB.Entities;

namespace StarTEDSystemWebApp.Components.Pages
{
    public partial class CRUD
    {
        [Inject]
        IJSRuntime JSRuntime { get; set; }

        [Inject]
        ProgramServices ProgramServices { get; set; }
        [Inject]
        ProgramCourseServices ProgramCourseServices { get; set; }
        [Inject]
        NavigationManager NavigationManager { get; set; }
        
        //ProgramCourse Properties
        [Parameter]
        public int ProgramCourseId { get; set; }
        
        [Parameter]
        public bool Required { get; set; }

        [Parameter]
        public string Comments { get; set; }

        [Parameter]
        public int SectionLimit { get; set; }

        [Parameter]
        public bool Active { get; set; }

        //Course properties
        [Parameter]
        public string CourseId { get; set; }

        [Parameter]
        public string CourseName { get; set; }

        [Parameter]
        public double Credits { get; set; }

        [Parameter]
        public int TotalHours { get; set; }

        [Parameter]
        public int ClassroomType { get; set; }

        [Parameter]
        public int Term { get; set;}
        
        [Parameter]
        public double Tuition { get; set; }

        [Parameter]
        public string Description { get; set; }

        [Parameter]
        public int ProgramId { get; set; }
        
        //Program Properties
        [Parameter]
        public int PCourseId { get; set; }


        public List<StarTEDSystemDB.Entities.Program> Programs { get; set; }
        
        //Get all Courses by ProgramCourseId
        public List<ProgramCourse> ProgramCourses { get; set; }

        //Get a list of all courses
        public List<ProgramCourse> CoursesList { get; set; }

        private ProgramCourse ProgramCourse { get; set; }

        public List<string> errorList = new List<string>();
        public string feedback { get; set; }


        /// <summary>
        /// Initialize processes
        /// </summary>
        /// <returns></returns>
        protected override Task OnInitializedAsync()
        {           
            Programs = ProgramServices.GetAllPrograms();
            CoursesList = ProgramCourseServices.GetAllProgramCourses();


            if (ProgramCourseId != 0)
            {
                ProgramCourses = ProgramCourseServices.GetAllProgramCourses(ProgramCourseId);
                ProgramCourse = ProgramCourseServices.GetProgramCourseById(ProgramCourseId);

                if (ProgramCourse == null)
                {
                    errorList.Add($"No course found for id {ProgramCourseId}");
                }
                else
                {
                    ProgramCourseId = ProgramCourse.ProgramCourseId;
                    ProgramId = ProgramCourse.ProgramId;
                    CourseId = ProgramCourse.CourseId;
                    CourseName = ProgramCourse.Course.CourseName;
                    Credits = Convert.ToDouble(ProgramCourse.Course.Credits);
                    Description = ProgramCourse.Course.Description;
                    ClassroomType = Convert.ToInt32(ProgramCourse.Course.ClassroomType);
                    Term = Convert.ToInt32(ProgramCourse.Course.Term);
                    Tuition = Convert.ToDouble(ProgramCourse.Course.Tuition);
                    Active = ProgramCourse.Active;
                    Required = ProgramCourse.Required;
                    Comments = ProgramCourse.Comments;
                }
            }

            return base.OnInitializedAsync();
        }

        /// <summary>
        /// Handles the selected program passing the program Id to get all courses related to that program
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private async Task HandleSelectedProgram(ChangeEventArgs e)
        {
            ProgramId = Convert.ToInt32(e.Value);
            if (ProgramId != 0)
            {
                CoursesList = ProgramCourseServices.GetAllProgramCourses(ProgramId);
                await InvokeAsync(StateHasChanged);
            }
        }

        /// <summary>
        /// Handles the selcted course passing the course Id to get all details relataed to that course
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private async Task HandleSelectedCourse(ChangeEventArgs e)
        {
            ProgramCourseId = Convert.ToInt32(e.Value);
            if (PCourseId != 0)
            {
                //Course = CourseServices.GetCourseById(CourseId);
                ProgramCourse = ProgramCourseServices.GetProgramCourseById(PCourseId);

                await InvokeAsync(StateHasChanged);
            }
        }

        /// <summary>
        /// Deactivate a ProgramCourse and display an alert for confirmation
        /// </summary>
        /// <returns></returns>
        private async Task HandleDeactivate()
        {
            if (ProgramCourse.ProgramCourseId != 0)
            {
                // Make a JS call to confirm whether to deactivate or not
                object[] message = new[] { "Are you sure you want to deactivate this course?"};

                if(await JSRuntime.InvokeAsync<bool>("confirm", message))
                {
                    try
                    {
                        feedback = "Course succesfully deactivated";
                    }
                    catch (Exception e)
                    {
                        errorList.Add(e.Message);
                    }
                }

            }
        }
    }
}
