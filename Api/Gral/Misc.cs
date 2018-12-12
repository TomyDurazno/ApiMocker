using ServiceCore;
using ServiceCore.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Api.Gral
{
    public static class Misc
    {
        public static Usuario GetUser()
        {
            var servicio = DependencyResolver.Current.GetService<IServicio<Usuario>>();

            return servicio.BuscarTodos().Where(s => s.Apellido == "Gonzalez").First();
        }
    }
}