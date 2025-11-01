using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations
{
    public class UserQuizConfig : IEntityTypeConfiguration<UserQuiz>
    {

        public void Configure(EntityTypeBuilder<UserQuiz> builder)
        {
            builder.HasKey(uq => new { uq.UserId, uq.QuizId });
            builder.HasOne(uq=>uq.Quiz).WithMany(q=>q.UserQuizzes).HasForeignKey(q=>q.QuizId);
            builder.HasOne(uq=>uq.User).WithMany(u=>u.UserQuizzes).HasForeignKey(u=>u.QuizId);
        }
    }
}
