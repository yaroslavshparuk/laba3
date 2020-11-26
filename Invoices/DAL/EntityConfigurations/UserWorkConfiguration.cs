using Invoices.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invoices.DAL.EntityConfigurations
{
    public class UserWorkConfiguration : IEntityTypeConfiguration<UserWork>
    {
        public void Configure(EntityTypeBuilder<UserWork> builder)
        {
            builder.ToTable("UserWorks").HasKey(x => x.Id);
        }
    }
}
