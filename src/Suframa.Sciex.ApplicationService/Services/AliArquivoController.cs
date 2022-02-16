using FluentValidation;
using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service de AliArquivo</summary>
	public class AliArquivoController : ApiController
	{
		private readonly IAliArquivoBll _aliArquivoBll;

		/// <summary>AliArquivo injetar regras de negócio</summary>
		/// <param name="AliArquivoBll"></param>
		public AliArquivoController(IAliArquivoBll AliArquivoBll)
		{
			_aliArquivoBll = AliArquivoBll;
		}

		//public IHttpActionResult GetAliArquivo(long idAliArquivo)
		//{
		//	var AliArquivo = _aliArquivoBll.Selecionar(idAliArquivo);
		//	var response = new HttpResponseMessage(HttpStatusCode.OK)
		//	{
		//		Content = new StreamContent(new MemoryStream(AliArquivo.Arquivo))
		//	};
		//	response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
		//	{
		//		FileName = "Eu"
		//	};
		//	response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
		//	response.Content.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
		//	ResponseMessageResult result = ResponseMessage(response);
		//	return result;
		//}

		public string GetAliArquivo(long idAliArquivo)
		{
			// Create a new WebClient instance.
			WebClient myWebClient = new WebClient();
			// Download home page data.

			// Download the Web resource and save it into a data buffer.
			byte[] myDataBuffer = myWebClient.DownloadData("https://www.google.com/url?sa=i&rct=j&q=&esrc=s&source=images&cd=&cad=rja&uact=8&ved=2ahUKEwiwu4rcsf3hAhVnFLkGHVKjCVcQjRx6BAgBEAU&url=https%3A%2F%2Fjovemnerd.com.br%2Fnerdbunker%2Fsonic-o-filme-imagens-vazadas-mostram-um-ourico-saido-de-pesadelos%2F&psig=AOvVaw0RzOcnx-BfRIKfmy2hYfLT&ust=1556905128216713");

			// Display the downloaded data.
			string download = Encoding.ASCII.GetString(myDataBuffer);

			return download;
		}

	}
}