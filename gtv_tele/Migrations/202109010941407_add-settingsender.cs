namespace gtv_tele.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addsettingsender : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SettingSenders",
                c => new
                    {
                        SettingSenderId = c.Int(nullable: false, identity: true),
                        Number_Sender = c.String(),
                        ApiId_Sender = c.Int(nullable: false),
                        ApiHash_Sender = c.String(),
                    })
                .PrimaryKey(t => t.SettingSenderId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SettingSenders");
        }
    }
}
