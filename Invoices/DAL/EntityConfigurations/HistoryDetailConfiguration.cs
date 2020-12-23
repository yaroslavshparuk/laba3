using Invoices.Data.Entities.TicketAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invoices.DAL.EntityConfigurations
{
    public class HistoryDetailConfiguration : IEntityTypeConfiguration<HistoryDetail>
    {
        public void Configure(EntityTypeBuilder<HistoryDetail> builder)
        {
            builder.HasKey(x => new { x.WorkItemId, x.RevisionDateTime }); 
            builder.ToTable("HistoryDetails");
        }
    }
}

