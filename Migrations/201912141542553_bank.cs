namespace MateuszSliwkaLab4ZadDom.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bank : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 250),
                        LastName = c.String(nullable: false, maxLength: 250),
                        PESEL = c.String(nullable: false, maxLength: 250),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountId = c.Int(nullable: false),
                        Amount = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.AccountId);
            
            CreateTable(
                "dbo.Transfers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SenderId = c.Int(nullable: false),
                        RecipientId = c.Int(nullable: false),
                        Amount = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.RecipientId)
                .ForeignKey("dbo.Accounts", t => t.SenderId)
                .Index(t => t.SenderId)
                .Index(t => t.RecipientId);
            
            CreateTable(
                "dbo.Withdrawals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountId = c.Int(nullable: false),
                        Amount = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.AccountId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Withdrawals", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.Transfers", "SenderId", "dbo.Accounts");
            DropForeignKey("dbo.Transfers", "RecipientId", "dbo.Accounts");
            DropForeignKey("dbo.Payments", "AccountId", "dbo.Accounts");
            DropIndex("dbo.Withdrawals", new[] { "AccountId" });
            DropIndex("dbo.Transfers", new[] { "RecipientId" });
            DropIndex("dbo.Transfers", new[] { "SenderId" });
            DropIndex("dbo.Payments", new[] { "AccountId" });
            DropTable("dbo.Withdrawals");
            DropTable("dbo.Transfers");
            DropTable("dbo.Payments");
            DropTable("dbo.Accounts");
        }
    }
}
