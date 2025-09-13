using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using DSED.REC.Entity;

namespace DSED.REC.DataAccesLayer;

public class ApplicationDbContexte : DbContext
{
    public ApplicationDbContexte(DbContextOptions<ApplicationDbContexte> options) : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
    }

    public DbSet<LeadDTO> LeadsDtos { get; set; } = null;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LeadDTO>(entity =>
            {
                entity.ToTable("Leads");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(CONST.MAX_NAME_LENGTH);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(CONST.MAX_NAME_LENGTH);
            
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(CONST.MAX_EMAIL_LENGTH);
        });
        base.OnModelCreating(modelBuilder);
    }
}