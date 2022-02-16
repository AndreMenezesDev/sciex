using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class ConsultarProtocoloEnvioController : ApiController
	{
		private readonly IConsultarProtocoloEnvioBll _ConsultarProtocoloEnvioBll;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="ConsultarProtocoloEnvioBll"></param>
		public ConsultarProtocoloEnvioController(IConsultarProtocoloEnvioBll ConsultarProtocoloEnvioBll)
		{
			_ConsultarProtocoloEnvioBll = ConsultarProtocoloEnvioBll;
		}

		/// <summary>Seleciona a ConsultarProtocoloEnvio pelo ID</summary>
		/// <param name="id"></param>
		/// <param name="idSolicitacaoPli"></param>
		/// <returns></returns>
		public EstruturaPropriaPLIVM Get(long id)
		{
			return _ConsultarProtocoloEnvioBll.Selecionar(id);
		}

	}
}