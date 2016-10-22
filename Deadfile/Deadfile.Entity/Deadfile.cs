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
    }
}