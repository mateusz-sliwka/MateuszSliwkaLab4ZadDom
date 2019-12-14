using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MateuszSliwkaLab4ZadDom.Models
{
    class BankContext : DbContext
    {
        public BankContext() : base("AppContext")
        {

        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Transfer> Transfers { get; set; }
        public virtual DbSet<Withdrawal> Withdrawals { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transfer>()
                        .HasRequired(m => m.Sender)
                        .WithMany(t => t.TransfersBy)
                        .HasForeignKey(m => m.SenderId)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<Transfer>()
                        .HasRequired(m => m.Recipient)
                        .WithMany(t => t.TransfersTo)
                        .HasForeignKey(m => m.RecipientId)
                        .WillCascadeOnDelete(false);
        }
    }
}
