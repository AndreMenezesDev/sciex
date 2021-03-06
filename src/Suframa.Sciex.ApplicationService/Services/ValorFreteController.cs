using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	public class ValorFreteController : ApiController
	{
		private readonly IValorFreteBll _bll;

		public ValorFreteController(IValorFreteBll bll)
		{
			_bll = bll;
		}
		
		public ValorFreteVM Post([FromBody] SolicitacoesAlteracaoVM objeto)
		{
			return _bll.Calcular(objeto);
		}

		//public SolicitacoesAlteracaoVM Get([FromUri] SolicitacoesAlteracaoVM objeto) =>
		//		_bll.BucarInfo(objeto);

		//public bool Put([FromBody] SolicitacoesAlteracaoVM objeto) =>
		//		_bll.Salvar(objeto);



	}
}