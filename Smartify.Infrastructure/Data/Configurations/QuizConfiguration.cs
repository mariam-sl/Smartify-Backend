using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smartify.Domain.Entities;


namespace Smartify.Infrastructure.Data.Configurations
{
    public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {

          
            builder.HasKey(q => q.Id);

            builder.Property(q => q.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(q => q.PassingScore)
                   .HasDefaultValue(50)
                   .IsRequired();

            builder.Property(q => q.TimeLimit)
                   .IsRequired();

            builder.Property(q => q.IsRetakeAllowed)
                .HasDefaultValue(false);


            builder.HasOne(q => q.Course)
                   .WithMany(c => c.Quizzes)
                   .HasForeignKey(q => q.CourseId)
                   .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.Lesson)
                  .WithOne(l => l.Quiz)
                  .HasForeignKey<Quiz>(x => x.LessonId)
                  .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.LessonId)
                   .IsUnique()
                   .HasFilter("[LessonId] IS NOT NULL");

            builder.HasIndex(q => q.CourseId);
            builder.HasIndex(q => q.Title);

            builder.ToTable("Quizzes",t =>
            {
                t.HasCheckConstraint("CK_Quiz_PassingScore", "[PassingScore] BETWEEN 0 AND 100");
                t.HasCheckConstraint("CK_Quiz_TimeLimit", "[TimeLimit] > 0");
            });

        }
    }
}
