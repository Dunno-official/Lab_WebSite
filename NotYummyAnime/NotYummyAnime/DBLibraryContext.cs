using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace NotYummyAnime
{
    public partial class DBLibraryContext : DbContext
    {
        public DBLibraryContext()
        {
        }

        public DBLibraryContext(DbContextOptions<DBLibraryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Anime> Animes { get; set; }
        public virtual DbSet<AnimeGenre> AnimeGenres { get; set; }
        public virtual DbSet<AnimeInfo> AnimeInfos { get; set; }
        public virtual DbSet<AnimeTranslation> AnimeTranslations { get; set; }
        public virtual DbSet<AnimeUser> AnimeUsers { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Translation> Translations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server= DUNNO; Database=DBLibrary; Trusted_Connection=True; ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Anime>(entity =>
            {
                entity.ToTable("Anime");

                entity.Property(e => e.AnimeId).HasColumnName("Anime_ID");

                entity.Property(e => e.AnimeInfoId).HasColumnName("AnimeInfo_ID");

                entity.Property(e => e.AnimeName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Poster)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.AnimeInfo)
                    .WithMany(p => p.Animes)
                    .HasForeignKey(d => d.AnimeInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Anime_AnimeInfo");
            });

            modelBuilder.Entity<AnimeGenre>(entity =>
            {
                entity.ToTable("Anime_Genre");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AnimeInfoId).HasColumnName("AnimeInfo_ID");

                entity.Property(e => e.GenreId).HasColumnName("Genre_ID");

                entity.HasOne(d => d.AnimeInfo)
                    .WithMany(p => p.AnimeGenres)
                    .HasForeignKey(d => d.AnimeInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Anime_Genre_AnimeInfo");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.AnimeGenres)
                    .HasForeignKey(d => d.GenreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Anime_Genre_Genre");
            });

            modelBuilder.Entity<AnimeInfo>(entity =>
            {
                entity.ToTable("AnimeInfo");

                entity.Property(e => e.AnimeInfoId).HasColumnName("AnimeInfo_ID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("ntext");

                entity.Property(e => e.Season)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.AgeRating)
                    .IsRequired()
                    .HasColumnType("ntext");

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.StudioName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<AnimeTranslation>(entity =>
            {
                entity.ToTable("Anime_Translation");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AnimeInfoId).HasColumnName("AnimeInfo_ID");

                entity.Property(e => e.TranslationId).HasColumnName("Translation_ID");

                entity.HasOne(d => d.AnimeInfo)
                    .WithMany(p => p.AnimeTranslations)
                    .HasForeignKey(d => d.AnimeInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Anime_Translation_AnimeInfo1");

                entity.HasOne(d => d.Translation)
                    .WithMany(p => p.AnimeTranslations)
                    .HasForeignKey(d => d.TranslationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Anime_Translation_Translation");
            });

            modelBuilder.Entity<AnimeUser>(entity =>
            {
                entity.ToTable("AnimeUser");

                entity.Property(e => e.AnimeUserId).HasColumnName("AnimeUser_ID");

                entity.Property(e => e.EMail)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("E_mail");

                entity.Property(e => e.NickName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsFixedLength(true);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");

                entity.Property(e => e.CommentId).HasColumnName("Comment_ID");

                entity.Property(e => e.AnimeId).HasColumnName("Anime_ID");

                entity.Property(e => e.AnimeUserId).HasColumnName("AnimeUser_ID");

                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasColumnType("ntext");

                entity.HasOne(d => d.Anime)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.AnimeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comment_Anime");

                entity.HasOne(d => d.AnimeUser)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.AnimeUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comment_AnimeUser");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("Genre");

                entity.Property(e => e.GenreId).HasColumnName("Genre_ID");

                entity.Property(e => e.GenreName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Description)
                   .HasColumnType("ntext");
            });

            modelBuilder.Entity<Translation>(entity =>
            {
                entity.ToTable("Translation");

                entity.Property(e => e.TranslationId).HasColumnName("Translation_ID");

                entity.Property(e => e.OrganizationName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.TranslationType)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
