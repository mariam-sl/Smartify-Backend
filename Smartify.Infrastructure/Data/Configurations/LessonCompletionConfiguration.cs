using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smartify.Domain.Entities;
using Smartify.Infrastructure.Identity;


namespace Smartify.Infrastructure.Data.Configurations
{
    public class LessonCompletionConfiguration : IEntityTypeConfiguration<LessonCompletion>
    {
        public void Configure(EntityTypeBuilder<LessonCompletion> builder)
        {
            builder.ToTable("LessonCompletions");

            builder.HasKey(lc => lc.Id);

            builder.Property(lc => lc.UserId)
                   .IsRequired()
                   .HasMaxLength(450);

            builder.Property(lc => lc.CompletedAt)
                    .IsRequired()
                    .HasDefaultValueSql("GETUTCDATE()");


            builder.HasOne(lc => lc.Lesson)
                   .WithMany(l => l.LessonCompletions)
                   .HasForeignKey(lc => lc.LessonId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<ApplicationUser>()
                   .WithMany(u => u.LessonCompletions)
                   .HasForeignKey(lc => lc.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(lc => new { lc.UserId, lc.LessonId })
                    .IsUnique();

            builder.HasIndex(lc => lc.LessonId);
            builder.HasIndex(lc => lc.CompletedAt);



        }
    }
}
