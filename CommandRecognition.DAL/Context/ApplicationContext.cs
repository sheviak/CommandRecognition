using CommandRecognition.CORE;
using CommandRecognition.DAL.Interface;
using System.Data.Entity;

namespace CommandRecognition.DAL.Context
{
    public class ApplicationContext : DbContext, IApplicationContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<VoiceCommand> VoiceCommands { get; set; }

        public ApplicationContext() : base("CommandRecognition")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(x => x.Id)
                .HasMany(x => x.VoiceCommands)
                .WithRequired(x => x.User)
                .HasForeignKey(x => x.UserId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<VoiceCommand>()
                .HasKey(x => x.Id);
        }
    }
}
