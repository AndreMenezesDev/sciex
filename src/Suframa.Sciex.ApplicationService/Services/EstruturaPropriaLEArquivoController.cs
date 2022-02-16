using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.Compressor;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.Mensagens;
using System;
using System.IO;
using System.Security.AccessControl;
using System.Text;
using System.Web;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service EstruturaPropriaPLI</summary>
	public class EstruturaPropriaLEArquivoController : ApiController
	{
		private readonly IEstruturaPropriaLEArquivoBll _estruturapropriaarquivoBll;

		public EstruturaPropriaLEArquivoController(
			IEstruturaPropriaLEArquivoBll estruturaPropriaPliarquivoBll)
		{
			_estruturapropriaarquivoBll = estruturaPropriaPliarquivoBll;
		}

		public string Put([FromBody]EstruturaPropriaLEVM estrutura)
		{
			estrutura.LocalPastaEstruturaArquivo = HttpContext.Current.Server.MapPath("EstruturaPropriaLE");
			return _estruturapropriaarquivoBll.ValidarArquivo(estrutura);
			
		}
	}
}