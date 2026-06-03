using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smartify.Domain.Entities;


namespace Smartify.Infrastructure.Data.Configurations
{
    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.ToTable("Answers");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.AnswerText)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(x => x.IsCorrect)
                .IsRequired();

            
            builder.HasOne(x => x.Question)
                   .WithMany(q => q.Answers)
                   .HasForeignKey(x => x.QuestionId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);


            builder.HasIndex(x => x.QuestionId);

        }
    }
}
