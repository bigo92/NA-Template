using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NA.DataAccess.Bases;
using Newtonsoft.Json;
using static NA.DataAccess.Contexts.Template;

namespace NA.DataAccess.Contexts
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
            modelBuilder.HasDbFunction(() => DbFunction.JsonValue(default, default));

            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity<Template>(entity =>
            {
                entity.Property(e => e.id)
                     .HasColumnName("id")
                     .ValueGeneratedNever();

                entity.Property(e => e.data).HasColumnName("data").IsJson();

                entity.Property(e => e.data_db).HasColumnName("data_db").IsJson();

                entity.Property(e => e.files).HasColumnName("files").IsJson();

                entity.Property(e => e.language).HasColumnName("language");

                entity.Property(e => e.tag)
                    .HasColumnName("tag")
                    .HasMaxLength(25);
            });

        }
    }
}
