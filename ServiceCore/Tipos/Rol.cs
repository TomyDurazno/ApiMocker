
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCore.Tipos
{
    public class Rol
    {
        //- Rol Administrador de Seguridad
        public const string Administrador = "Administrador";

        //- Rol de Usuario
        public const string Usuario = "Usuario";

        public static List<string> ToList()
        {
            return typeof(Rol).GetFields(BindingFlags.Static | BindingFlags.Public)
                              .Select(t => t.Name)
                              .ToList();
        }
    }
}
