using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCore.Mailer.Models
{
    public class Receptor
    {
        /// <summary> Posee el nombre completo del receptor, sea un string con el Apellido + Nombre, la Razón Social u otro caso
        /// </summary>
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }
}
