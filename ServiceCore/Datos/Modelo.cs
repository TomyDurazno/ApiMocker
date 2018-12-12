using Microsoft.AspNet.Identity.EntityFramework;
using ServiceCore.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCore.Datos
{
    public partial class Modelo : DbContext
    {
        #region Constructor

        public Modelo()
             : base("name=bteam")
        {
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 40;
        }

        #endregion

        #region Métodos

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserLogin>().HasKey(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        #endregion

        #region Propiedades

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<IdentityUserRole> IdentityUserRole { get; set; }
        public DbSet<IdentityRole> IdentityRole { get; set; }

        #endregion

    }
}
