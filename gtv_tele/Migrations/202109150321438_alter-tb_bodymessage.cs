namespace gtv_tele.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class altertb_bodymessage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Body_Message", "Nama_Body", c => c.String(true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Body_Message", "Nama_Body");
        }
    }
}
