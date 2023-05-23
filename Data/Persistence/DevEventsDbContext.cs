using AwesomeDevEvents.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AwesomeDevEvents.API.Data.Persistence
{
    public class DevEventsDbContext : DbContext
    {
        public DevEventsDbContext(DbContextOptions<DevEventsDbContext> options) : base(options)
        {

        }

        public DbSet<DevEvent> DevEvents { get; set; }
        public DbSet<DevEventSpeaker> DevEventsSpeakers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DevEvent>(e =>
            {
                e.HasKey(de => de.Id);

                e.Property(de => de.Title).IsRequired();

                e.Property(de => de.Description).HasMaxLength(250).HasColumnType("varchar(250)");

                e.Property(de => de.StartDate).HasColumnName("Start_Date");

                e.Property(de => de.EndDate).HasColumnName("End_Date");

                //e.HasMany(de => de.Speakers)
                //.WithOne()
                //.HasForeignKey(de => de.DevEventId);

                e.HasMany(de => de.Speakers)
                .WithOne()
                .HasForeignKey(s => s.DevEventId);
            });

            builder.Entity<DevEventSpeaker>(e =>
            {
                e.HasKey(de => de.Id);
            });
        }
    }
}
