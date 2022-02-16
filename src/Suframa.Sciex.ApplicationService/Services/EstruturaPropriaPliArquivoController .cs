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
	public class EstruturaPropriaPliArquivoController : ApiController
	{
		private readonly IEstruturaPropriaPliArquivoBll _estruturapropriaarquivoBll;

		/// <summary>Construtor</summary>
		/// <param name="estruturaPropriaPliarquivoBll"></param>
		public EstruturaPropriaPliArquivoController(
			IEstruturaPropriaPliArquivoBll estruturaPropriaPliarquivoBll)
		{
			_estruturapropriaarquivoBll = estruturaPropriaPliarquivoBll;
		}

		/// <summary>Seleciona o Importador pela INSCRICAO</summary>
		/// <param name="estrutura"></param>
		/// <returns></returns>	
		public string Put([FromBody]EstruturaPropriaPLIArquivoVM estrutura)
		{
			estrutura.LocalPastaEstruturaArquivo = HttpContext.Current.Server.MapPath("EstruturaPropriaPLI");
			return _estruturapropriaarquivoBll.ValidarArquivo(estrutura);
			
		}
	}
}