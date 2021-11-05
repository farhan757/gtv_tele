namespace gtv_tele.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class editmasteruploaddet : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Master_Upload_Detail",
                c => new
                {
                    Master_Upload_DetailId = c.Int(nullable: false, identity: true),
                    Master_UploadId = c.Int(nullable: false),
                    Nama = c.String(),
                    Account = c.String(),
                    Send = c.Int(nullable: true, false, 0),
                    Send_at = c.DateTime(nullable: true),
                    Status = c.Int(nullable: true),
                    Message_Status = c.String(),
                    Read_Total = c.Int(nullable: true, false, 0),
                    Read_at = c.DateTime(nullable: true),
                    Keterangan = c.String(),
                })
                .PrimaryKey(t => t.Master_Upload_DetailId);

            AddColumn("dbo.Master_Upload_Detail", "NoHP", c => c.String());
            AddColumn("dbo.Master_Upload_Detail", "Body_Message", c => c.String());
        }

        public override void Down()
        {
            DropTable("dbo.Master_Upload_Detail");
        }
    }
}
