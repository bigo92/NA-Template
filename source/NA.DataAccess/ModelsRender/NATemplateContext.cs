using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace NA.DataAccess.ModelsRender
{
    public partial class NATemplateContext : DbContext
    {
        public NATemplateContext()
        {
        }

        public NATemplateContext(DbContextOptions<NATemplateContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Template> Template { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.;Database=NATemplate;user id=sa;password=123456;Trusted_Connection=False;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity<Template>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Data).HasColumnName("data");

                entity.Property(e => e.DataDb).HasColumnName("data_db");

                entity.Property(e => e.Files).HasColumnName("files");

                entity.Property(e => e.Language).HasColumnName("language");

                entity.Property(e => e.Tag)
                    .HasColumnName("tag")
                    .HasMaxLength(25);
            });
        }
    }
}
