namespace Projects.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FullName = c.String(),
                        CompanyName = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                        Fax = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Project",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        CustomerId = c.Guid(nullable: false),
                        DateStart = c.DateTime(nullable: false),
                        DateEnd = c.DateTime(),
                        Priority = c.Int(nullable: false),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customer", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Employee",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FullName = c.String(),
                        ExecutorCompanyId = c.Guid(nullable: false),
                        PositionId = c.Guid(nullable: false),
                        Phone = c.String(),
                        Email = c.String(),
                        DateBorn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ExecutorCompany", t => t.ExecutorCompanyId, cascadeDelete: true)
                .ForeignKey("dbo.Position", t => t.PositionId, cascadeDelete: true)
                .Index(t => t.ExecutorCompanyId)
                .Index(t => t.PositionId);
            
            CreateTable(
                "dbo.ExecutorCompany",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Address = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                        Fax = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Position",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProjectEmployee",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProjectId = c.Guid(nullable: false),
                        EmployeeId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employee", t => t.EmployeeId, cascadeDelete: true)
                .ForeignKey("dbo.Project", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId)
                .Index(t => t.EmployeeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectEmployee", "ProjectId", "dbo.Project");
            DropForeignKey("dbo.ProjectEmployee", "EmployeeId", "dbo.Employee");
            DropForeignKey("dbo.Employee", "PositionId", "dbo.Position");
            DropForeignKey("dbo.Employee", "ExecutorCompanyId", "dbo.ExecutorCompany");
            DropForeignKey("dbo.Project", "CustomerId", "dbo.Customer");
            DropIndex("dbo.ProjectEmployee", new[] { "EmployeeId" });
            DropIndex("dbo.ProjectEmployee", new[] { "ProjectId" });
            DropIndex("dbo.Employee", new[] { "PositionId" });
            DropIndex("dbo.Employee", new[] { "ExecutorCompanyId" });
            DropIndex("dbo.Project", new[] { "CustomerId" });
            DropTable("dbo.ProjectEmployee");
            DropTable("dbo.Position");
            DropTable("dbo.ExecutorCompany");
            DropTable("dbo.Employee");
            DropTable("dbo.Project");
            DropTable("dbo.Customer");
        }
    }
}
