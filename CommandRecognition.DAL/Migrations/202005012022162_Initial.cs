namespace CommandRecognition.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Login = c.String(),
                        Password = c.String(),
                        Question = c.String(),
                        Answer = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VoiceCommands",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Command = c.String(),
                        Path = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VoiceCommands", "UserId", "dbo.Users");
            DropIndex("dbo.VoiceCommands", new[] { "UserId" });
            DropTable("dbo.VoiceCommands");
            DropTable("dbo.Users");
        }
    }
}
