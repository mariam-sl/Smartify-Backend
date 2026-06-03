using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smartify.Domain.Entities;


namespace Smartify.Infrastructure.Data.Configurations
{
    public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
         

            builder.HasKey(l => l.Id);

            builder.Property(l => l.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(l => l.Content)
                   .IsRequired()
                   .HasColumnType("nvarchar(max)");

            builder.Property(l => l.VideoUrl)
                   .HasMaxLength(1000);

            builder.Property(l => l.OrderNumber)
                   .IsRequired();

            builder.Property(l => l.CreatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("GETUTCDATE()");


            builder.HasOne(l => l.Course)
                   .WithMany(c => c.Lessons)
                   .HasForeignKey(l => l.CourseId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(l => new { l.CourseId, l.OrderNumber })
                    .IsUnique();

            builder.HasIndex(l => l.CourseId);
            builder.HasIndex(l => l.CreatedAt);


            builder.ToTable("Lessons", t =>
            {
                t.HasCheckConstraint("CK_Lesson_OrderNumber","[OrderNumber] > 0");
            });

        }
    }
}
