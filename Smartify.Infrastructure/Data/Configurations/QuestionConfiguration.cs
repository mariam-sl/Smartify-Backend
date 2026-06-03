using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Smartify.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Smartify.Infrastructure.Data.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
           

            builder.HasKey(x => x.Id);

            builder.Property(x => x.QuestionText)
                   .IsRequired()
                   .HasMaxLength(1000);

            builder.Property(x => x.QuestionType)
                   .IsRequired()
                   .HasConversion<int>();

            builder.Property(x => x.OrderNumber)
                   .IsRequired();

            builder.HasOne(x => x.Quiz)
                   .WithMany(q => q.Questions)
                   .HasForeignKey(x => x.QuizId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new { x.QuizId, x.OrderNumber })
                .IsUnique();
            builder.HasIndex(x => x.QuizId);

            builder.ToTable("Questions", t =>
            {
                t.HasCheckConstraint("CK_Question_OrderNumber", "[OrderNumber] > 0");
            });

        }
    }
}
