using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net.Mail;
using System.Web.UI.WebControls;

namespace ServiceCore.Mailer.Models
{
    public abstract class IMailerModel
    {
        #region Propiedades

        /// <summary> Clases usada por el método que manda mails
        /// </summary>
        private MailDefinition Definition { get; set; }

        /// <summary> Clases usada por el método que manda mails
        /// </summary>
        public MailMessage Mensaje { get; set; }

        /// <summary> Lista de los receptores del Mail
        /// </summary>
        public ICollection<Receptor> Receptores { get; private set; }

        /// <summary> array de bytes que contiene la vista del mail en formato HTML
        /// </summary>
        private byte[] HtmlRecurso { get; set; }

        /// <summary> Cuerpo del mail en formato string
        /// </summary>
        private string HtmlBody { get; set; }

        /// <summary> Función opcional a ser implementada por las clases que heredan, usada para parsear el Html del cuerpo del mail
        /// </summary>
        public Func<Receptor, string, string> FuncHtmlParse { get; set; }

        #endregion

        /// <summary> Crea el cuerpo del Mail a ser enviado
        /// </summary>
        /// <param name="receptor"></param>
        /// <param name="replacements"></param>
        public void CreateMessage(Receptor receptor, ListDictionary replacements)
        {
            var html = HtmlBody.ToString();

            if (FuncHtmlParse != null) { html = FuncHtmlParse.Invoke(receptor, html); }

            Mensaje = Definition.CreateMailMessage(receptor.Email, replacements, html, new System.Web.UI.Control());

            //var header = new Attachment(Properties.Resources.logo_agc.ToStream(ImageFormat.Png), "header.png", "image/png")
            //{
            //    ContentId = "header"
            //};

            //var footer = new Attachment(Properties.Resources.footer_mail.ToStream(ImageFormat.Png), "footer.png", "image/png")
            //{
            //    ContentId = "footer"
            //};

            //Mensaje.Attachments.Add(header);
            //Mensaje.Attachments.Add(footer);

        }

        /// <summary> Instancia las clases usadas por el método que manda mail, las clases que hereden deben llamar a esta función en su constructor
        /// </summary>
        /// <param name="asunto"></param>
        /// <param name="recurso"></param>
        /// <param name="receptores"></param>
        public void InstanciarMailer(string asunto, byte[] recurso, List<Receptor> receptores, Func<Receptor, string, string> funcHtml = null)
        {
            HtmlRecurso = recurso;
            Receptores = receptores;

            FuncHtmlParse = funcHtml;

            Definition = new MailDefinition() { IsBodyHtml = true, Subject = asunto };

            using (var memStream = new MemoryStream(HtmlRecurso))
            {
                using (var strReader = new StreamReader(memStream))
                {
                    HtmlBody = strReader.ReadToEnd();
                }
            }
        }

        /// <summary> Método a ser implementado por las clases que heredan, 
        /// devuelve una lista con los reemplazos a ser realizados en el cuerpo del mail según el receptor que recibe por parámetro
        /// </summary>
        /// <param name="receptor"></param>
        /// <returns></returns>
        public abstract ListDictionary GetReplacements(Receptor receptor);

    }
}
