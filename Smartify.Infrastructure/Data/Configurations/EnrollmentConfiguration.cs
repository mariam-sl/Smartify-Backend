using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smartify.Domain.Entities;
using Smartify.Domain.Enums;
using Smartify.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Infrastructure.Data.Configurations
{
    public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            

            builder.HasKey(e => e.Id);

            builder.Property(e => e.UserId)
                   .IsRequired()
                   .HasMaxLength(450);

            builder.Property(e => e.Progress)
                   .IsRequired()
                   .HasDefaultValue(0);

            builder.Property(e => e.EnrolledAt)
                   .IsRequired()
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(e => e.Status)
                   .IsRequired()
                   .HasConversion<int>()
                   .HasDefaultValue(EnrollmentStatus.Active); 

            builder.Property(e => e.CompletedAt)
                   .IsRequired(false);

            builder.HasOne(e => e.Course)
                   .WithMany(c => c.Enrollments)
                   .HasForeignKey(e => e.CourseId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<ApplicationUser>()
                   .WithMany(u => u.Enrollments)
                   .HasForeignKey(e => e.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => new { e.UserId, e.CourseId })
                     .IsUnique();


            // check constraint for Progress (0 to 100)
            builder.ToTable("Enrollments", t =>
            {
                t.HasCheckConstraint("CK_Enrollment_Progress", "[Progress] >= 0 AND [Progress] <= 100");
            });


            builder.HasIndex(e => e.CourseId);
            builder.HasIndex(e => e.Status);
            builder.HasIndex(e => e.EnrolledAt);
        }
    }
}
