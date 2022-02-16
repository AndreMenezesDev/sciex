using System.IO;
using System.Net;
using System.Text;

namespace Suframa.Sciex.CrossCutting.Ftp
{
	public static class Ftp
	{

		public static string EnviarArquivo(string arquivo, string usuario, string senha, string conteudo)
		{

			try
			{
				FtpWebRequest request = (FtpWebRequest)WebRequest.Create(arquivo);
				request.Method = WebRequestMethods.Ftp.UploadFile;
				request.Credentials = new NetworkCredential(usuario, senha);

				byte[] fileContents;
				fileContents = Encoding.UTF8.GetBytes(conteudo);

				request.ContentLength = fileContents.Length;

				using (Stream requestStream = request.GetRequestStream())
				{
					requestStream.Write(fileContents, 0, fileContents.Length);
				}

				using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
				{
					return "enviado";
				}
			}
			catch (System.Exception ex)
			{
				return ex.Message;
			}


		}

		public static bool VerificarSeExisteArquivo(string arquivo, string usuario, string senha)
		{
			var request = (FtpWebRequest)WebRequest.Create(arquivo);
			request.Credentials = new NetworkCredential(usuario, senha);
			request.Method = WebRequestMethods.Ftp.GetFileSize;

			try
			{
				FtpWebResponse response = (FtpWebResponse)request.GetResponse();
				return true;
			}
			catch (WebException ex)
			{
				FtpWebResponse response = (FtpWebResponse)ex.Response;
				if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
					return false;
			}
			return false;
		}

		public static byte[] ReceberArquivo(string arquivo, string usuario, string senha)
		{
			try
			{
				// Get the object used to communicate with the server.
				FtpWebRequest request = (FtpWebRequest)WebRequest.Create(arquivo);
				request.Method = WebRequestMethods.Ftp.DownloadFile;

				// This example assumes the FTP site uses anonymous logon.
				request.Credentials = new NetworkCredential(usuario, senha);

				FtpWebResponse response = (FtpWebResponse)request.GetResponse();

				Stream responseStream = response.GetResponseStream();
				StreamReader reader = new StreamReader(responseStream);

				byte[] streamInByte;

				using (MemoryStream ms = new MemoryStream())
				{
					responseStream.CopyTo(ms);
					streamInByte = ms.ToArray();
				}

				response.Close();
				reader.Close();

				return streamInByte;
			}
			catch (System.Exception)
			{
				return null;
			}
		}

		public static string DeleteFile(string arquivo, string usuario, string senha)
		{
			FtpWebRequest request = (FtpWebRequest)WebRequest.Create(arquivo);
			request.Method = WebRequestMethods.Ftp.DeleteFile;
			request.Credentials = new NetworkCredential(usuario, senha);

			using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
			{
				return response.StatusDescription;
			}
		}


	}
}