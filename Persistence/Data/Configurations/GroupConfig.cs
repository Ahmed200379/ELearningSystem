using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Configurations
{
    public class GroupConfig : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasMany(b=>b.Materials).WithMany(m=>m.Groups).UsingEntity(j=>j.ToTable("MaterialGroup"));
            builder.HasMany(g => g.Chats).WithOne(c => c.Group).HasForeignKey(c => c.GroupId);
            builder.HasMany(g => g.UserGroups).WithOne(ug => ug.Group).HasForeignKey(ug => ug.GroupId);
        }
    }
}
