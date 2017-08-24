namespace Role_Again.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asas : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FileUploads", "Approve", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FileUploads", "Approve", c => c.Boolean());
        }
    }
}
