using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCore.Mailer.Models
{
    public class Welcome : IMailerModel
    {
        public string Direccion { get; set; }

        public Welcome(string email, string password, string direccion)
        {
            this.Direccion = direccion;

            var auxListaReceptores = new List<Receptor>();
            auxListaReceptores.Add(new Receptor()
            {
                Email = email,
                Password = password
            });
            //byte array es el recurso html
            InstanciarMailer("Bienvenido", new byte[8], auxListaReceptores);
        }

        public override ListDictionary GetReplacements(Receptor receptor)
        {
            var replacements = new ListDictionary();
            replacements.Add("{{Email}}", receptor.Email);
            replacements.Add("{{Password}}", receptor.Password);
            replacements.Add("{{Direccion}}", Direccion);
            return replacements;
        }
    }
}
