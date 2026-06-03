using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smartify.Domain.Entities;

namespace Smartify.Infrastructure.Data.Configurations
{
    public class QuizAttemptAnswerConfiguration : IEntityTypeConfiguration<QuizAttemptAnswer>
    {
        public void Configure(EntityTypeBuilder<QuizAttemptAnswer> builder)
        {
            builder.ToTable("QuizAttemptAnswers");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.IsCorrect)
                   .IsRequired();

            builder.HasOne(x => x.QuizAttempt)
                   .WithMany(q => q.Answers)
                   .HasForeignKey(x => x.QuizAttemptId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Question)
                   .WithMany(q => q.AttemptAnswers)
                   .HasForeignKey(x => x.QuestionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SelectedAnswer)
                   .WithMany(q => q.AttemptAnswers)
                   .HasForeignKey(x => x.SelectedAnswerId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Allow multiple selections per question 
            // Prevent duplicate selection of same answer for same attempt+question.
            builder.HasIndex(x => new { x.QuizAttemptId, x.QuestionId, x.SelectedAnswerId })
                   .IsUnique();

            builder.HasIndex(x => x.QuestionId);
            builder.HasIndex(x => x.SelectedAnswerId);
        }
    }
}
