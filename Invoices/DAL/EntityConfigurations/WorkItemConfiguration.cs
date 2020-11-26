using Invoices.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invoices.DAL.EntityConfigurations
{
    public class WorkItemConfiguration : IEntityTypeConfiguration<WorkItem>
    {
        public void Configure(EntityTypeBuilder<WorkItem> builder)
        {
            builder.ToTable("WorkItems").HasKey(x => x.Id);
            builder.HasMany(x => x.History).WithOne(x=>x.WorkItem);
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.Title).IsRequired();
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.CreatedDate).IsRequired();
        }
    }
}
