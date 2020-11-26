using Invoices;
using Invoices.DAL.EntityConfigurations;
using Invoices.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.EF
{
   public class InvoiceContext:DbContext
    {
        public InvoiceContext()
        {
            Database.EnsureCreated();
        }

        public InvoiceContext(DbContextOptions<InvoiceContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<HistoryDetail> HistoryDetails { get; set; }
        public virtual DbSet<WorkItem> WorkItems { get; set; }
        public virtual DbSet<UserWork> UserWorks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new WorkItemConfiguration());
            modelBuilder.ApplyConfiguration(new UserWorkConfiguration());
            modelBuilder.ApplyConfiguration(new HistoryDetailConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
