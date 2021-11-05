namespace gtv_tele.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        CustomerName = c.String(),
                        CustomerPhone = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        Region = c.String(),
                        PostalCode = c.String(),
                        Country = c.String(),
                        Created_at = c.DateTime(nullable: false),
                        Updated_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CustomerId);
            
            CreateTable(
                "dbo.History_Read",
                c => new
                    {
                        History_ReadId = c.Int(nullable: false, identity: true),
                        Master_Upload_DetailId = c.Int(nullable: false),
                        Ip = c.String(),
                        Read_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.History_ReadId);
            
            CreateTable(
                "dbo.Master_Upload",
                c => new
                    {
                        Master_UploadId = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        ProjectId = c.Int(nullable: false),
                        Cycle = c.String(),
                        Part = c.Int(nullable: false),
                        FileName = c.String(),
                        FilePath = c.String(),
                        UserId = c.Int(nullable: false),
                        Created_at = c.DateTime(nullable: false),
                        Updated_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Master_UploadId);
            
            CreateTable(
                "dbo.Master_Upload_Detail",
                c => new
                    {
                        Master_Upload_DetailId = c.Int(nullable: false, identity: true),
                        Master_UploadId = c.Int(nullable: false),
                        Nama = c.String(),
                        Account = c.String(),
                        Send = c.Int(nullable: false),
                        Send_at = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        Message_Status = c.String(),
                        Read_Total = c.Int(nullable: false),
                        Read_at = c.DateTime(nullable: false),
                        Keterangan = c.String(),
                    })
                .PrimaryKey(t => t.Master_Upload_DetailId);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ProjectId = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        ProjectName = c.String(),
                        ProjectDesc = c.String(),
                        Created_at = c.DateTime(nullable: false),
                        Updated_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ProjectId);
            
            CreateTable(
                "dbo.Verify_Code",
                c => new
                    {
                        Verify_CodeId = c.Guid(nullable: false),
                        Master_Upload_DetailId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Verify_CodeId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Verify_Code");
            DropTable("dbo.Projects");
            DropTable("dbo.Master_Upload_Detail");
            DropTable("dbo.Master_Upload");
            DropTable("dbo.History_Read");
            DropTable("dbo.Customers");
        }
    }
}
