namespace Deadfile.Entity
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class Deadfile : DbContext
    {
        public Deadfile()
            : base("name=Deadfile")
        {
        }

        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<PlanningApplication> PlanningApplications { get; set; }
        public virtual DbSet<Expense> Expenses { get; set; }
    }
}