using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Simple_911.Models;

namespace Simple_911.Data
{
    public class ApplicationDbContext : IdentityDbContext<SimpleUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<IncidentNote> IncidentNotes { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<CallType> CallTypes { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<IncidentSupport> IncidentSupports { get; set; }
    }
}