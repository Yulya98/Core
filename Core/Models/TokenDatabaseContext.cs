using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Core.Models
{
    public partial class TokenDatabaseContext : DbContext
    {
        public virtual DbSet<Album> Album { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<Images> Images { get; set; }
        public virtual DbSet<Table> Table { get; set; }
        public virtual DbSet<UserAlbum> UserAlbum { get; set; }
        public virtual DbSet<UserAlbumPosts> UserAlbumPosts { get; set; }
        public virtual DbSet<UsersImages> UsersImages { get; set; }
        public virtual DbSet<UsersImagesComments> UsersImagesComments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=TokenDatabase;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Album>(entity =>
            {
                entity.Property(e => e.NameOfAlbum)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.City)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FieldOfActivity)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.Pseudonym)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Surname)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.HasOne(d => d.User)
                    .WithOne(p => p.AspNetUserTokens)
                    .HasForeignKey<AspNetUserTokens>(d => d.UserId);
            });

            modelBuilder.Entity<Images>(entity =>
            {
                entity.Property(e => e.Path)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Table>(entity =>
            {
                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.Album)
                    .WithMany(p => p.Table)
                    .HasForeignKey(d => d.AlbumId)
                    .HasConstraintName("FK__Table__AlbumId__68487DD7");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Table)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Table__UserId__03F0984C");
            });

            modelBuilder.Entity<UserAlbum>(entity =>
            {
                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.Album)
                    .WithMany(p => p.UserAlbum)
                    .HasForeignKey(d => d.AlbumId)
                    .HasConstraintName("FK__UserAlbum__Album__6C190EBB");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserAlbum)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__UserAlbum__UserI__04E4BC85");
            });

            modelBuilder.Entity<UserAlbumPosts>(entity =>
            {
                entity.HasOne(d => d.IdPostNavigation)
                    .WithMany(p => p.UserAlbumPosts)
                    .HasForeignKey(d => d.IdPost)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__UserAlbum__IdPos__6FE99F9F");

                entity.HasOne(d => d.IdUsersAlbumNavigation)
                    .WithMany(p => p.UserAlbumPosts)
                    .HasForeignKey(d => d.IdUsersAlbum)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__UserAlbum__IdUse__6EF57B66");
            });

            modelBuilder.Entity<UsersImages>(entity =>
            {
                entity.Property(e => e.IdUser)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdImageNavigation)
                    .WithMany(p => p.UsersImages)
                    .HasForeignKey(d => d.IdImage)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UsersImag__IdIma__628FA481");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.UsersImages)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UsersImag__IdUse__02FC7413");
            });

            modelBuilder.Entity<UsersImagesComments>(entity =>
            {
                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.IdUser)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.IdPostNavigation)
                    .WithMany(p => p.UsersImagesComments)
                    .HasForeignKey(d => d.IdPost)
                    .HasConstraintName("FK__UsersImag__IdPos__72C60C4A");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.UsersImagesComments)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK__UsersImag__IdUse__05D8E0BE");
            });
        }
    }
}
