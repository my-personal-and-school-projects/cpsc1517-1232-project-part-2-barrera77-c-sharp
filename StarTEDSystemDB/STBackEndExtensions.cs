using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StarTEDSystemDB.BLL;
using StarTEDSystemDB.DAL;


namespace StarTEDSystemDB
{
    public static class STBackEndExtensions
    {
        public static void STBackEndDependencies(this IServiceCollection services,
            Action<DbContextOptionsBuilder> options)
        {
            services.AddDbContext<StarTEDContext>(options);

            services.AddTransient<CourseServices>((ServiceProvider) =>
            {
                var context = ServiceProvider.GetService<StarTEDContext>();
                return new CourseServices(context!);
            });

            services.AddTransient<ProgramServices>((ServiceProvider) =>
            {
                var context = ServiceProvider.GetService<StarTEDContext>();
                return new ProgramServices(context!);
            });

            services.AddTransient<ProgramCourseServices>((ServiceProvider) =>
            {
                var context = ServiceProvider.GetService<StarTEDContext>();
                return new ProgramCourseServices(context!);
            });

        }

    }
}
