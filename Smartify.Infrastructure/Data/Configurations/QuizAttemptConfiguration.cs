using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smartify.Domain.Entities;
using Smartify.Infrastructure.Identity;


namespace Smartify.Infrastructure.Data.Configurations
{
    public class QuizAttemptConfiguration : IEntityTypeConfiguration<QuizAttempt>
    {
        public void Configure(EntityTypeBuilder<QuizAttempt> builder)
        {
            

            builder.HasKey(x => x.Id);

            
            builder.Property(x => x.Score)
                .HasDefaultValue(0)
                   .IsRequired();

            builder.Property(x => x.AttemptDate)
                   .IsRequired()
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(x => x.DurationSeconds)
                   .IsRequired()
                   .HasDefaultValue(0);

            builder.Property(x => x.UserId)
                   .IsRequired()
                   .HasMaxLength(450);

            
            builder.HasOne(x => x.Quiz)
                   .WithMany(q => q.Attempts)
                   .HasForeignKey(x => x.QuizId)
                   .OnDelete(DeleteBehavior.Restrict);

            
            builder.HasOne<ApplicationUser>()
                   .WithMany(q => q.QuizAttempts)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.UserId, x.QuizId });
            builder.HasIndex(x => new { x.QuizId, x.AttemptDate });

            builder.HasIndex(x => x.QuizId);
            builder.HasIndex(x => x.AttemptDate);

            builder.ToTable("QuizAttempts", t =>
            {
                t.HasCheckConstraint("CK_QuizAttempt_Score", "[Score] >= 0 AND [Score] <= 100");
                t.HasCheckConstraint("CK_QuizAttempt_Duration", "[DurationSeconds] >= 0");
            });


        }
    }
}
