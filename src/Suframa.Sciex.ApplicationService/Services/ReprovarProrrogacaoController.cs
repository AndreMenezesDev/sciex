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
	public class ReprovarProrrogacaoController : ApiController
	{
		private readonly IConsultarProcessoExportacaoBll _consultarProcessoExportacaoBll;

		public ReprovarProrrogacaoController(
			IConsultarProcessoExportacaoBll consultarProcessoExportacaoBll)
		{
			_consultarProcessoExportacaoBll = consultarProcessoExportacaoBll;
		}

		public PRCSolicProrrogacaoVM Get([FromUri]PRCSolicProrrogacaoVM filtro)
		{
			return _consultarProcessoExportacaoBll.ReprovarProrrogacao(filtro);
		}
	}
}