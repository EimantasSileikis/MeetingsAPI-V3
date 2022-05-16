using MeetingsAPI_V3.Entities;
using Microsoft.EntityFrameworkCore;

namespace MeetingsAPI_V3.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Meeting> Meetings { get; set; } = null!;

    }
}
