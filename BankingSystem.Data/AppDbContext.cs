using BankingSystem.Model;
using BankingSystem.Model.Domain;
using MySql.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.Data
{
    public class AppDbContext : DbContext
    {
        //DI of connection type so that the UI decides which connection it needs
        public AppDbContext() : base("name=BankDbConnection") 
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<AppDbContext>());
        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<SavingAccount> SavingAccounts { get; set; }
        public virtual DbSet<SalaryAccount> SalaryAccounts { get; set; }

        public virtual DbSet<BankService> BankServices { get; set; }

        public virtual DbSet<Certificate> Certificatres { get; set; }
        public virtual DbSet<CreditCard> CreditCards { get; set; }

        public virtual DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Fluent API config
            modelBuilder.Entity<Customer>().ToTable("Customers").HasKey(c => c.Id);
            modelBuilder.Entity<Customer>().Property(c => c.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Customer>().Property(c => c.NationalId).IsRequired().HasMaxLength(20);

            //(TPH) Account inheritence
            modelBuilder.Entity<Account>()
                .Map<SavingAccount>(m => m.Requires("AccountType").HasValue((int)BankingSystem.Model.CrossCutting.AccountType.Saving))
                .Map<SalaryAccount>(m => m.Requires("AccountType").HasValue((int)BankingSystem.Model.CrossCutting.AccountType.Salary))
                .ToTable("Accounts");

            modelBuilder.Entity<Account>().HasRequired(a => a.Customer).WithMany().HasForeignKey(a => a.CustomerId);

            //(TPT) BankService Inheritence
            modelBuilder.Entity<BankService>().ToTable("BankService").HasKey(b => b.Id);
            modelBuilder.Entity<BankService>().HasRequired(b => b.Customer).WithMany().HasForeignKey(b => b.CustomerId);

            modelBuilder.Entity<Certificate>().ToTable("Certificates");
            modelBuilder.Entity<CreditCard>().ToTable("CreditCards");

            //Transaction
            modelBuilder.Entity<Transaction>().ToTable("Transactions").HasKey(t => t.Id);
            modelBuilder.Entity<Transaction>().HasRequired(t => t.Account).WithMany().HasForeignKey(t => t.AccountId);
        }
    }
}
