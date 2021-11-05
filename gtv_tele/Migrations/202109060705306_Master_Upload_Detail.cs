namespace gtv_tele.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Master_Upload_Detail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Master_Upload_Detail", "Body_Message", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Master_Upload_Detail", "Body_Message");
        }
    }
}
