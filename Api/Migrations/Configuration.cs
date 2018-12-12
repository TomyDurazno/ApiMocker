using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ServiceCore.Datos;
using ServiceCore.Entidades;
using ServiceCore.Tipos;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace Api.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<Modelo>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(Modelo context)
        {
            #region Implementation

            /*
            #region Usuarios

            // Creo roles
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            foreach (string rol in Rol.ToList())
            {
                if (roleManager.FindByName(rol) == null)
                    roleManager.Create(new IdentityRole(rol));
            }

            // Creo el usuario admin
            var userManager = new UserManager<Usuario>(new UserStore<Usuario>(context));
            var usuarioAdmin = new Usuario()
            {
                Nombre = "Administrador",
                Apellido = "Richards",
                UserName = "administrador@tzkapp.com",
                Email = "administrador@tzkapp.com",
                FechaCreacion = DateTime.Now
            };
            var password = "123456";

            if (userManager.FindByName(usuarioAdmin.UserName) == null)
            {
                userManager.Create(usuarioAdmin, password);
            }
            usuarioAdmin = userManager.FindByName(usuarioAdmin.UserName);

            // usuarioAdmin es Rol.Administrador
            if (!userManager.IsInRole(usuarioAdmin.Id, Rol.Administrador))
                userManager.AddToRole(usuarioAdmin.Id, Rol.Administrador);

            var usuario2 = new Usuario()
            {
                Nombre = "Usuario",
                Apellido = "Williams",
                UserName = "usuario@tzkapp.com",
                Email = "usuario@tzkapp.com",
                FechaCreacion = DateTime.Now
            };

            if (userManager.FindByName(usuario2.UserName) == null)
            {
                userManager.Create(usuario2, password);
            }
            usuario2 = userManager.FindByName(usuario2.UserName);

            // usuario2 es Rol.Usuario
            if (!userManager.IsInRole(usuario2.Id, Rol.Usuario))
                userManager.AddToRole(usuario2.Id, Rol.Usuario);

            //Mesas
            var usuario3 = new Usuario()
            {
                Nombre = "Juan",
                Apellido = "Gonzalez",
                UserName = "jgonzalez@tzkapp.com",
                Email = "jgonzalez@tzkapp.com",
                FechaCreacion = DateTime.Now
            };

            if (userManager.FindByName(usuario3.UserName) == null)
            {
                userManager.Create(usuario3, password);
            }

            usuario3 = userManager.FindByName(usuario3.UserName);

            if (!userManager.IsInRole(usuario3.Id, Rol.Usuario))
                userManager.AddToRole(usuario3.Id, Rol.Usuario);

            //Rampa
            var usuario4 = new Usuario()
            {
                Nombre = "Agustina",
                Apellido = "Fernandez",
                UserName = "afernandez@tzkapp.com",
                Email = "afernandez@tzkapp.com",
                FechaCreacion = DateTime.Now
            };

            if (userManager.FindByName(usuario4.UserName) == null)
            {
                userManager.Create(usuario4, password);
            }

            usuario4 = userManager.FindByName(usuario4.UserName);

            if (!userManager.IsInRole(usuario4.Id, Rol.Usuario))
                    userManager.AddToRole(usuario4.Id, Rol.Usuario);

            //Pared
            var usuario5 = new Usuario()
            {
                Nombre = "Eugenia",
                Apellido = "Vigosky",
                UserName = "evigosky@tzkapp.com",
                Email = "evigosky@tzkapp.com",
                FechaCreacion = DateTime.Now
            };


            if (userManager.FindByName(usuario5.UserName) == null)
            {
                userManager.Create(usuario5, password);
            }

            usuario5 = userManager.FindByName(usuario5.UserName);

            if (!userManager.IsInRole(usuario5.Id, Rol.Usuario))
                userManager.AddToRole(usuario5.Id, Rol.Usuario);

            #endregion

            if(!context.Ubicaciones.Any())
            {
                context.Ubicaciones.AddRange(GeneradorEntidades.GetUbicaciones());
            }

            context.SaveChanges();

            var almagro = context.Ubicaciones.Where(u => u.Nombre == "Almagro").First();
            var balvanera = context.Ubicaciones.Where(u => u.Nombre == "Balvanera").First();
            var lugano = context.Ubicaciones.Where(u => u.Nombre == "Villa Lugano").First();

            #region Campanias

            if (!context.Campanias.Any())
            {
                context.Campanias.Add(GeneradorEntidades.GetCampaniaMesas(usuario3, almagro, password));
                context.Campanias.Add(GeneradorEntidades.GetCampaniaRampa(usuario4, balvanera, password));
                context.Campanias.Add(GeneradorEntidades.GetCampaniaPared(usuario5, lugano, password));
            }

            #endregion

            context.SaveChanges();
            */

            #endregion
        }
    }
}