using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Update_Transaction.Models
{
    public class TransactionAppContext : DbContext
    {
        public TransactionAppContext(DbContextOptions<TransactionAppContext> options)
         : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            foreach (var relationship in modelbuilder.Model.GetEntityTypes().SelectMany(f => f.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            base.OnModelCreating(modelbuilder);
        }
        public DbSet<User> USER { get; set; }
        public DbSet<Transaction> TRANSACTION { get; set; }

    }
}
