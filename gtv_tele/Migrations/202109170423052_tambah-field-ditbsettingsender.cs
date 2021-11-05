namespace gtv_tele.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tambahfieldditbsettingsender : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SettingSenders", "SettingSenderName", c => c.String(null,150));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SettingSenders", "SettingSenderName");
        }
    }
}
