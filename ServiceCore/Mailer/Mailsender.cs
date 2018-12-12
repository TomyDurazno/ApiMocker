using ServiceCore.Mailer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ServiceCore.Mailer
{
    public class MailSender
    {
        private IMailerModel model;
        public MailSender(IMailerModel model)
        {
            this.model = model;
        }

        public void SendMail()
        {
            try
            {
                SmtpClient sc = new SmtpClient();
                foreach (var receptor in model.Receptores)
                {
                    model.CreateMessage(receptor, model.GetReplacements(receptor));
                    sc.Send(model.Mensaje);
                }
            }
            catch (HttpException ex)
            {
                throw ex;
            }
        }

        public async Task SendMailAsync()
        {
            try
            {
                SmtpClient sc = new SmtpClient();
                foreach (var receptor in model.Receptores)
                {
                    model.CreateMessage(receptor, model.GetReplacements(receptor));
                    await sc.SendMailAsync(model.Mensaje);
                }
            }
            catch (HttpException ex)
            {
                throw ex;
            }
        }
    }

    public static class MailerExtensions
    {

        /// <summary> Método usado para mandar múltiples mails de forma paralela
        /// </summary>
        /// <param name="mailers"></param>
        public static void SendMultiple(this IEnumerable<MailSender> mailers)
        {
            Parallel.ForEach(mailers.ToList(), m => m.SendMail());
        }

        /// <summary> Método usado para mandar múltiples mails de forma asíncrona
        /// </summary>
        /// <param name="mailers"></param>
        public static async Task SendMultipleAsync(this IEnumerable<MailSender> mailers)
        {
            await Task.WhenAll(mailers.ToList().Select(m => Task.Run(() => m.SendMailAsync())));
        }
    }
}
