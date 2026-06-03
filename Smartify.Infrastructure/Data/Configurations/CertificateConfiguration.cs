using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smartify.Domain.Entities;
using Smartify.Infrastructure.Identity;


namespace Smartify.Infrastructure.Data.Configurations
{
    public class CertificateConfiguration : IEntityTypeConfiguration<Certificate>
    {
        public void Configure(EntityTypeBuilder<Certificate> builder)
        {
            builder.ToTable("Certificates");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.UserId)
                   .IsRequired()
                   .HasMaxLength(450); 

            builder.Property(c => c.DownloadUrl)
                   .HasMaxLength(1000);

            builder.Property(c => c.GeneratedAt)
                   .IsRequired()
                   .HasDefaultValueSql("GETUTCDATE()");

            
            builder.HasOne(c => c.Course)
                   .WithMany(course => course.Certificates)
                   .HasForeignKey(c => c.CourseId)
                   .OnDelete(DeleteBehavior.Restrict);

            
            builder.HasOne<ApplicationUser>()
                   .WithMany(u => u.Certificates)
                   .HasForeignKey(c => c.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(c => new { c.UserId, c.CourseId })
                    .IsUnique();


            builder.HasIndex(c => c.GeneratedAt);

            builder.HasIndex(c => c.CourseId);

        }
    }
}
