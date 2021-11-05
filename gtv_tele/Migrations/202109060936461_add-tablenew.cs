namespace gtv_tele.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtablenew : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Body_Message",
                c => new
                    {
                        Body_MessageId = c.Int(nullable: false, identity: true),
                        VariableToBodyId = c.Int(nullable: false),
                        Body = c.String(),
                    })
                .PrimaryKey(t => t.Body_MessageId);
            
            CreateTable(
                "dbo.VariableToBodies",
                c => new
                    {
                        VariableToBodyId = c.Int(nullable: false, identity: true),
                        VariableToBodyName = c.String(),
                    })
                .PrimaryKey(t => t.VariableToBodyId);
            
            CreateTable(
                "dbo.VariableToBodyDetails",
                c => new
                    {
                        VariableToBodyDetailId = c.Int(nullable: false, identity: true),
                        VariableToBodyId = c.Int(nullable: false),
                        VariableName = c.String(),
                        VariableField = c.String(),
                    })
                .PrimaryKey(t => t.VariableToBodyDetailId);
            
            AddColumn("dbo.Master_Upload_Detail", "Short_Link", c => c.String());
            AddColumn("dbo.Master_Upload_Detail", "Path_PDF", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Master_Upload_Detail", "Path_PDF");
            DropColumn("dbo.Master_Upload_Detail", "Short_Link");
            DropTable("dbo.VariableToBodyDetails");
            DropTable("dbo.VariableToBodies");
            DropTable("dbo.Body_Message");
        }
    }
}
