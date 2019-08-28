using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NA.DataAccess.Bases;
using Newtonsoft.Json;
using static NA.DataAccess.Models.Template;

namespace NA.DataAccess.Models
{
    public partial class NATemplateContext : IdentityDbContext<ApplicationUser, ApplicationRole, long>
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
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            #region aut
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("Users", "aut");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccessFailedCount).HasColumnName("accessFailedCount");

                entity.Property(e => e.ConcurrencyStamp).HasColumnName("concurrencyStamp");

                entity.Property(e => e.data).HasColumnName("data").IsJson();

                entity.Property(e => e.data_db).HasColumnName("data_db").IsJson();

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(256);

                entity.Property(e => e.EmailConfirmed).HasColumnName("emailConfirmed");

                entity.Property(e => e.files).HasColumnName("files");

                entity.Property(e => e.LockoutEnabled).HasColumnName("lockoutEnabled");

                entity.Property(e => e.LockoutEnd).HasColumnName("lockoutEnd");

                entity.Property(e => e.NormalizedEmail)
                    .HasColumnName("normalizedEmail")
                    .HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName)
                    .HasColumnName("normalizedUserName")
                    .HasMaxLength(256);

                entity.Property(e => e.PasswordHash).HasColumnName("passwordHash");

                entity.Property(e => e.PhoneNumber).HasColumnName("phoneNumber");

                entity.Property(e => e.PhoneNumberConfirmed).HasColumnName("phoneNumberConfirmed");

                entity.Property(e => e.SecurityStamp).HasColumnName("securityStamp");

                entity.Property(e => e.TwoFactorEnabled).HasColumnName("twoFactorEnabled");

                entity.Property(e => e.UserName).HasColumnName("userName").HasMaxLength(256);
            });

            modelBuilder.Entity<ApplicationRole>(entity =>
            {
                entity.ToTable("Roles", "aut");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ConcurrencyStamp).HasColumnName("concurrencyStamp");

                entity.Property(e => e.data).HasColumnName("data").IsJson();

                entity.Property(e => e.data_db).HasColumnName("data_db").IsJson();

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(256);

                entity.Property(e => e.NormalizedName)
                    .HasColumnName("normalizedName")
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<IdentityUserRole<long>>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.ToTable("UserRoles", "aut");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.RoleId).HasColumnName("roleId");
            });

            modelBuilder.Entity<IdentityUserClaim<long>>(entity =>
            {
                entity.ToTable("UserClaims", "aut");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClaimType).HasColumnName("claimType");

                entity.Property(e => e.ClaimValue).HasColumnName("claimValue");

                entity.Property(e => e.UserId).HasColumnName("userId");

            });

            modelBuilder.Entity<IdentityUserLogin<long>>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.ToTable("UserLogins", "aut");

                entity.Property(e => e.LoginProvider).HasColumnName("loginProvider");

                entity.Property(e => e.ProviderKey).HasColumnName("providerKey");

                entity.Property(e => e.ProviderDisplayName).HasColumnName("providerDisplayName");

                entity.Property(e => e.UserId).HasColumnName("userId");

            });

            modelBuilder.Entity<IdentityUserToken<long>>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.ToTable("UserTokens", "aut");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.LoginProvider).HasColumnName("loginProvider");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Value).HasColumnName("value");
            });

            modelBuilder.Entity<IdentityRoleClaim<long>>(entity =>
            {
                entity.ToTable("RoleClaims", "aut");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClaimType).HasColumnName("claimType");

                entity.Property(e => e.ClaimValue).HasColumnName("claimValue");

                entity.Property(e => e.RoleId).HasColumnName("roleId");
            });
            #endregion

            #region dbo
            modelBuilder.Entity<Template>(entity =>
            {
                entity.Property(e => e.id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.address).HasColumnName("address").IsJson();

                entity.Property(e => e.data_db).HasColumnName("data_db");

                entity.Property(e => e.files).HasColumnName("files");

                entity.Property(e => e.info).HasColumnName("info").IsJson();
            });
            #endregion

        }
    }
}
