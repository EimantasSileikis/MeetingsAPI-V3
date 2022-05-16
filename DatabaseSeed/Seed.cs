using MeetingsAPI_V3.Data;
using MeetingsAPI_V3.Entities;
using Microsoft.EntityFrameworkCore;

namespace MeetingsAPI_V3.DatabaseSeed
{
    public static class Seed
    {
        public static void PrepSeed(IApplicationBuilder app)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<DataContext>());
            }
        }

        public static void SeedData(DataContext context)
        {

            context.Database.Migrate();

            if (!context.Meetings.Any())
            {
                context.Meetings.AddRange(
                    new Meeting()
                    {
                        Name = "Calculus Exam",
                        Users = "12345"
                    },
                    new Meeting()
                    {
                        Name = "Algebra First Meeting",
                        Users = "12345"
                    },
                    new Meeting()
                    {
                        Name = "Web programming Meeting"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
