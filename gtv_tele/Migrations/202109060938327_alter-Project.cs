namespace gtv_tele.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterProject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Projects", "Body_MessageId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Projects", "Body_MessageId");
        }
    }
}
