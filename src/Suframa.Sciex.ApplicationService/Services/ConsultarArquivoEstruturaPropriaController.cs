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
	public class ConsultarArquivoEstruturaPropriaController : ApiController
	{
		private readonly IEstruturaPropriaLEArquivoBll _estruturapropriaarquivoBll;

		public ConsultarArquivoEstruturaPropriaController(
			IEstruturaPropriaLEArquivoBll estruturaPropriaPliarquivoBll)
		{
			_estruturapropriaarquivoBll = estruturaPropriaPliarquivoBll;
		}

		public EstruturaPropriaLEVM Get([FromUri]EstruturaPropriaLEVM estrutura)
		{
			return _estruturapropriaarquivoBll.BuscarArquivo(estrutura);
		}
	}
}