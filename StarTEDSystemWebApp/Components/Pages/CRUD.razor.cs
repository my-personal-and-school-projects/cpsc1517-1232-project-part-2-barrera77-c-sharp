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
        CourseServices CourseServices { get; set; }

        [Inject]
        ProgramCourseServices ProgramCourseServices { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }
        
        //ProgramCourse Properties
        [Parameter]
        public int ProgramCourseId { get; set; }

        [Parameter]
        public bool Required { get; set; } = false;

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
        public string? Description { get; set; }

        [Parameter]
        public int ProgramId { get; set; }
        
        //Program Properties
        [Parameter]
        public int PCourseId { get; set; }


        public List<StarTEDSystemDB.Entities.Program> Programs { get; set; }
        
        //Get all Courses by ProgramCourseId
        public List<ProgramCourse> ProgramCourses { get; set; }

        //Get a list of all courses
        public List<Course> CoursesList { get; set; }

        public ProgramCourse ProgramCourse { get; set; }
        public Course Course { get; set; }

        public List<string> errorList = new List<string>();
        public string feedback { get; set; }

        public bool IsNewProgramCourse { get; set; } = false;


        /// <summary>
        /// Initialize processes
        /// </summary>
        /// <returns></returns>
        protected override Task OnInitializedAsync()
        {            
            IsNewProgramCourse = true;

            Programs = ProgramServices.GetAllPrograms();
            CoursesList = CourseServices.GetAllCourses();

            if (ProgramCourseId != 0)
            {
                IsNewProgramCourse = false;                
                ProgramCourses = ProgramCourseServices.GetAllProgramCourses(ProgramCourseId);
                DisplayProgramCourse(ProgramCourseId);
                ProgramId = ProgramCourse.ProgramId;
                CourseId = ProgramCourse.CourseId;
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
        }

        /// <summary>
        /// Handles the selcted course passing the course Id to get all details relataed to that course
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private async Task HandleSelectedCourse(ChangeEventArgs e)
        {
            CourseId = Convert.ToString(e.Value);          

            //Display course details
            DisplayProgramCourse(ProgramCourseId);
        }

        /// <summary>
        /// Deactivate a ProgramCourse and display an alert for confirmation
        /// </summary>
        /// <returns></returns>
        private async Task HandleDeactivate()
        {                       
            if (ProgramCourseId != 0)
            {
                // Make a JS call to confirm whether to deactivate or not
                object[] message = new[] { "Are you sure you want to deactivate this course?"};

                if(await JSRuntime.InvokeAsync<bool>("confirm", message))
                {
                    try
                    {
                        ProgramCourseServices.DeactivateProgramCourse(ProgramCourse);
                        ClearFields();
                        feedback = "Course succesfully deactivated";
                        
                    }
                    catch (Exception e)
                    {
                        errorList.Add(e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Fill up the fields with the program course information based on the access method (NavBar Link, Create New button, etc.).
        /// If the ProgramCourse is null create a course using the CourseId value from the dropdown course list
        /// Otherwise use the CourseId from the ProgramCourse 
        /// </summary>
        /// <param name="programCourseId"></param>
        private void DisplayProgramCourse(int programCourseId)
        {
            ProgramCourse = ProgramCourseServices.GetProgramCourseById(ProgramCourseId);            

            if (ProgramCourse == null)
            {
                Course = CourseServices.GetCourseById(CourseId);
            }
            else
            {
                Course = CourseServices.GetCourseById(ProgramCourse.Course.CourseId);                
                
                if(ProgramCourseId != 0)
                {
                    SectionLimit = ProgramCourse.SectionLimit;
                    Active = ProgramCourse.Active;
                    Required = ProgramCourse.Required;
                    Comments = ProgramCourse.Comments;
                }
            }
            //ProgramCourseId = ProgramCourse.ProgramCourseId;                
            CourseName = Course.CourseName;
            Credits = Convert.ToDouble(Course.Credits);
            Description = Course.Description;
            ClassroomType = Convert.ToInt32(Course.ClassroomType);
            Term = Convert.ToInt32(Course.Term);
            Tuition = Convert.ToDouble(Course.Tuition);
            TotalHours = Convert.ToInt32(Course.TotalHours);
        }

        /// <summary>
        /// Save the newly created ProgramCourse in the DB
        /// </summary>
        private void HandleSaveProgramCourse()
        {
            if (IsValidProgramCourse())
            {
                ProgramCourse = new();

                if (ProgramCourseId == 0)
                {
                    // check if the course already belongs to that program
                    if (ProgramCourseServices.ProgramCourseExists(ProgramId, CourseId))
                    {
                        errorList.Add("Course already included in the current program");
                    }
                    else
                    {
                        try
                        {
                            CreateProgramCourse();
                            ProgramCourseId = ProgramCourse.ProgramCourseId;
                            feedback = "Program Course succesfully added";
                            NavigationManager.NavigateTo($"/crud/{ProgramCourseId}");
                            ClearFields();
                        }
                        catch (Exception e)
                        {
                            errorList.Add(e.Message);
                        }
                    }
                }
                else
                {
                    errorList.Add($"Program course already exists {ProgramCourseId}");
                }
            }            
        }

        /// <summary>
        /// Create a new ProgramCourse extracting the values from the form fields
        /// </summary>
        private void CreateProgramCourse()
        {
            ProgramCourse = new();

            ProgramCourse.Active = Active;
            ProgramCourse.Required = Required;
            ProgramCourse.Comments = Comments;
            ProgramCourse.ProgramId = ProgramId;
            ProgramCourse.CourseId = CourseId;
            ProgramCourse.SectionLimit = SectionLimit;

            ProgramCourseServices.AddProgramCourse(ProgramCourse);
        }

        /// <summary>
        /// Validate fields for the ProgramCourse
        /// </summary>
        private bool IsValidProgramCourse()
        {
            errorList.Clear();

            if (ProgramId == 0)
            {
                errorList.Add("Program Id cannot be null");
            }
            if (string.IsNullOrWhiteSpace(CourseId))
            {
                errorList.Add("Course Id cannot be null");
            }
            if (SectionLimit == 0)
            {
                errorList.Add("Please provide the limit for the section");
            }
            if (!Active)
            {
                errorList.Add("Program Course cannot be inactive");
            }       

            //Required and Comments fields seem to be optional in the DB so, no validation was implemented

            return errorList.Count == 0;
        }

        /// <summary>
        /// Clear all form fields and initialize variables
        /// </summary>
        private void ClearFields()
        {
            ProgramCourseId = 0;
            ProgramId = 0;
            CourseId = "";
            CourseName = "";
            Credits = 0;
            Description = "";
            ClassroomType = 0;
            Term = 0;
            Tuition = 0;
            Active = false;
            Required = false;
            Comments = "";  
            SectionLimit = 0;
        }
    }
}
