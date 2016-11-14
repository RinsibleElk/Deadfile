using System;
using System.Data.Entity;
using System.Linq;

namespace Deadfile.Entity
{
    /// <summary>
    /// Access to the Entity framework's storage of the DeadfileContext.
    /// </summary>
    public class DeadfileContext : DbContext
    {
        public DeadfileContext()
            : base("name=Deadfile")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Disable cascade delete from job/invoice to job-invoice mappings.
            modelBuilder.Entity<Job>().HasMany(i => i.JobInvoiceMappings).WithOptional().WillCascadeOnDelete(false);
            modelBuilder.Entity<Invoice>().HasMany(i => i.JobInvoiceMappings).WithOptional().WillCascadeOnDelete(false);
        }

        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<BillableHour> BillableHours { get; set; }
        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<Quotation> Quotations { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<LocalAuthority> LocalAuthorities { get; set; }
        public virtual DbSet<JobInvoiceMapping> JobInvoiceMappings { get; set; }
    }
}