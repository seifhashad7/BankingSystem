namespace BankingSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IntialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedAt = c.DateTime(nullable: false, precision: 0),
                        CustomerId = c.Int(nullable: false),
                        AccountType = c.Int(nullable: false),
                        EmployerName = c.String(unicode: false),
                        LastSalaryCreditDate = c.DateTime(precision: 0),
                        IsZeroBalancedAccount = c.Boolean(),
                        InterestRate = c.Decimal(precision: 18, scale: 2),
                        MinimumBalance = c.Decimal(precision: 18, scale: 2),
                        Discriminator = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                        Age = c.Int(nullable: false),
                        Gender = c.Int(nullable: false),
                        Address = c.String(unicode: false),
                        PhoneNumber = c.String(unicode: false),
                        NationalId = c.String(nullable: false, maxLength: 20, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BankService",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PeriodInYears = c.Int(nullable: false),
                        IssueDate = c.DateTime(nullable: false, precision: 0),
                        CustomerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountId = c.Int(nullable: false),
                        TransactionType = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TransactionDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.AccountId);
            
            CreateTable(
                "dbo.Certificates",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        PrincipalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InterestRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BankService", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.CreditCards",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        CashLimit = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BankService", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CreditCards", "Id", "dbo.BankService");
            DropForeignKey("dbo.Certificates", "Id", "dbo.BankService");
            DropForeignKey("dbo.Transactions", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.BankService", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Accounts", "CustomerId", "dbo.Customers");
            DropIndex("dbo.CreditCards", new[] { "Id" });
            DropIndex("dbo.Certificates", new[] { "Id" });
            DropIndex("dbo.Transactions", new[] { "AccountId" });
            DropIndex("dbo.BankService", new[] { "CustomerId" });
            DropIndex("dbo.Accounts", new[] { "CustomerId" });
            DropTable("dbo.CreditCards");
            DropTable("dbo.Certificates");
            DropTable("dbo.Transactions");
            DropTable("dbo.BankService");
            DropTable("dbo.Customers");
            DropTable("dbo.Accounts");
        }
    }
}
