using NLog;
using Suframa.Sciex.CrossCutting.Configuration;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Suframa.Sciex.CrossCutting.SuperStructs
{
	public static class Email
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();

		public static Encoding Nothing { get; private set; }

		public static void Enviar(string corpo, string titulo, string emailPara, ParidadeCambialGenerator other = null)
		{
			if (string.IsNullOrEmpty(emailPara)) { return; }

			try
			{				
				var mail = new MailMessage();

				ICredentialsByHost credentials = null;

				// Verifica se é necessário autenticar no SMTP
				if (!string.IsNullOrEmpty(PrivateSettings.MAIL_CREDENTIALS_EMAIL)
					&& !string.IsNullOrEmpty(PrivateSettings.PASS_CREDENTIALS_EMAIL))
				{
					credentials = new System.Net.NetworkCredential(PrivateSettings.MAIL_CREDENTIALS_EMAIL, PrivateSettings.PASS_CREDENTIALS_EMAIL, PrivateSettings.HOST_EMAIL);
				}

				var client = new SmtpClient
				{
					Port = PrivateSettings.PORT_EMAIL,
					Host = PrivateSettings.HOST_EMAIL,
					DeliveryMethod = SmtpDeliveryMethod.Network,
					UseDefaultCredentials = false,
					Credentials =  new NetworkCredential(PrivateSettings.MAIL_CREDENTIALS_EMAIL, PrivateSettings.PASS_CREDENTIALS_EMAIL),
					EnableSsl = PrivateSettings.SSL_HOST_EMAIL,						
				};
							
				mail.From = new MailAddress(PrivateSettings.FROM_EMAIL, "SCIEX");
				mail.To.Add(emailPara);
				mail.Subject = titulo;
				if(other != null)
					mail.Body = corpo.Replace("@dia", other.DataParidade.ToShortDateString());
				else
					mail.Body = corpo.Replace("@dia", DateTime.Now.AddDays(1).ToShortDateString());
				mail.IsBodyHtml = true;
				mail.BodyEncoding = Encoding.UTF8;
				mail.SubjectEncoding = System.Text.Encoding.Default;
				
				client.Send(mail);
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Erro ao tentar enviar email");
			}
		}
	}
}