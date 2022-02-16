using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{

	public class ProrrogarSolicitacaoController : ApiController
	{
		private readonly IProrrogarSolicitacaoBll _bll;

		public ProrrogarSolicitacaoController(IProrrogarSolicitacaoBll bll)
		{
			_bll = bll;
		}


		public ProrrogarSolicitacaoVM Post([FromBody] ProrrogarSolicitacaoVM view)
		{
			return _bll.SalvarProrrogacao(view);
		}

	}
}