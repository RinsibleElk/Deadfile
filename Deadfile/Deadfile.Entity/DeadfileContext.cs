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
        public DeadfileContext(string connectionString)
            : base(connectionString)
        {
        }

        public DeadfileContext() : this(@"Server =.\SQLEXPRESS; Database=Deadfile;Integrated Security = True")
        {
        }

        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<BillableHour> BillableHours { get; set; }
        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<JobTask> JobTasks { get; set; }
        public virtual DbSet<Quotation> Quotations { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<LocalAuthority> LocalAuthorities { get; set; }
        public virtual DbSet<InvoiceItem> InvoiceItems { get; set; }
    }
}
