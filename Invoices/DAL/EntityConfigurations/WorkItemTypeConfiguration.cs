using Invoices.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invoices.DAL.EntityConfigurations
{
    public class WorkItemTypeConfiguration : IEntityTypeConfiguration<WorkItemType>
    {
        public void Configure(EntityTypeBuilder<WorkItemType> builder)
        {
            builder.ToTable("WorkItemTypes").HasKey(x => x.Id);
            builder.Property(x => x.TypeName).IsRequired();
        }
    }

}
