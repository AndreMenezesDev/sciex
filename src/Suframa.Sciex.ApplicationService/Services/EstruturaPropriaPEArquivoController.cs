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
	public class EstruturaPropriaPEArquivoController : ApiController
	{
		private readonly IEstruturaPropriaPEArquivoBll _bll;

		public EstruturaPropriaPEArquivoController(IEstruturaPropriaPEArquivoBll bll)
		{
			_bll = bll;
		}

		public string Put([FromBody]EstruturaPropriaPLIArquivoVM estrutura)
		{
			estrutura.LocalPastaEstruturaArquivo = HttpContext.Current.Server.MapPath("EstruturaPropriaPE");
			return _bll.ValidarArquivo(estrutura);
			
		}
	}
}