using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smartify.Domain.Entities;
using Smartify.Infrastructure.Identity;

namespace Smartify.Infrastructure.Data.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Courses");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(c => c.ShortDescription)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(c => c.LongDescription)
                   .IsRequired()
                   .HasColumnType("nvarchar(max)");

            builder.Property(c => c.Category)
                   .IsRequired()
                   .HasConversion<int>();

            builder.Property(c => c.Difficulty)
                   .IsRequired()
                   .HasConversion<int>();

            builder.Property(c => c.Thumbnail)
                   .HasMaxLength(1000);

            builder.Property(c => c.CreatedById)
                   .IsRequired()
                   .HasMaxLength(450); 

            builder.Property(c => c.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("GETUTCDATE()"); 

            builder.Property(c => c.IsPublished)
                .HasDefaultValue(false);

            builder.HasOne<ApplicationUser>()
                    .WithMany(u => u.CreatedCourses)
                    .HasForeignKey(c => c.CreatedById)
                    .OnDelete(DeleteBehavior.Restrict);

            
             builder.HasIndex(c => c.Title);
             builder.HasIndex(c => c.Category);
             builder.HasIndex(c => c.IsPublished);
             builder.HasIndex(c => c.CreatedById);
             builder.HasIndex(c => c.CreatedAt);
        }

    }
}
