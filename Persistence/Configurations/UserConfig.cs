using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Persistence.Configurations
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        void IEntityTypeConfiguration<User>.Configure(EntityTypeBuilder<User> builder)
        {
           builder.HasKey(u => u.Id);
           builder.HasMany(u=>u.UserGroups).WithOne(ug => ug.User).HasForeignKey(ug => ug.UserId);
           builder.HasMany(u=>u.Payments).WithOne(p=>p.User).HasForeignKey(p => p.UserId);
            builder.HasMany(u => u.UserQuizzes).WithOne(uq => uq.User).HasForeignKey(uq => uq.UserId);
            builder.HasMany(u=>u.Chats).WithOne(c => c.User).HasForeignKey(c => c.UserId);
            builder.HasMany(u => u.Materials).WithMany(m => m.Users).UsingEntity(j=>j.ToTable("UserMaterials"));

        }
    }
}
