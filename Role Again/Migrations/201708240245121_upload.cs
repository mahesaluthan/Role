namespace Role_Again.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class upload : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FileUploads",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Filename = c.String(),
                        Filepath = c.String(),
                        Userid = c.String(),
                        Approve = c.Boolean(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FileUploads");
        }
    }
}
