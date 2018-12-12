using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ServiceCore.Datos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCore.Entidades
{
    public class Usuario : IdentityUser, IEntidad
    {
        public Usuario()
        {

        }

        #region Propiedades

        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }

        #endregion

        #region Entidad

        public bool Eliminado { get; set; }

        [Column(TypeName = "DateTime2")]
        public DateTime? FechaCreacion { get; set; }


        [Column(TypeName = "DateTime2")]
        public DateTime? FechaEdicion { get; set; }

        public int GetId()
        {
            throw new NotImplementedException();
        }

        public void ReinicializarLista(string[] nombres)
        {
            foreach (string nombre in nombres)
            {
                var tipo = GetType().GetProperty(nombre);
                if (tipo == null)
                {
                    throw new Exception("La propiedad " + nombre + " no existe o no es publica.");
                }
                if (tipo.PropertyType.FullName.StartsWith("System.Collections.Generic.ICollection"))
                {
                    switch (nombre)
                    {
                        case "Proyectos":

                            break;
                        default: throw new NotImplementedException();
                    }
                }
            }
        }

        #endregion

        #region Métodos Públicos


        /// <summary> Trae todos los roles asociados al usuario
        /// </summary>
        /// <returns></returns>
        public List<string> TraerRoles()
        {
            // Esta funcion deberia devolver todos los roles asociados al usuario que la llama.
            using (var userManager = new UserManager<Usuario>(new UserStore<Usuario>(new Modelo())))
            {
                return userManager.GetRoles(Id).ToList();
            }
        }
        #endregion
    }
}
