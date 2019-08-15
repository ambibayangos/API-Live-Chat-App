using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LiveChatAPp.Model
{
    public partial class liveChatAppContext : DbContext
    {
        public liveChatAppContext()
        {
        }

        public liveChatAppContext(DbContextOptions<liveChatAppContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Player> Player { get; set; }
        public virtual DbSet<Video> Video { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=tcp:live-chat-app.database.windows.net,1433;Initial Catalog=LiveChatApp;Persist Security Info=False;User ID=ambibayangos;Password=MsaPhase2;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Player>(entity =>
            {
                entity.Property(e => e.PlayerName).IsUnicode(false);

                entity.HasOne(d => d.Video)
                    .WithMany(p => p.Player)
                    .HasForeignKey(d => d.VideoId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("VideoId");
            });

            modelBuilder.Entity<Video>(entity =>
            {
                entity.Property(e => e.ThumbnailUrl).IsUnicode(false);

                entity.Property(e => e.VideoTitle).IsUnicode(false);

                entity.Property(e => e.WebUrl).IsUnicode(false);
            });
        }
    }
}
