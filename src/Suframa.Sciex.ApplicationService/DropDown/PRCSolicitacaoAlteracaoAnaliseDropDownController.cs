using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	public class SolicitacaoAlteracaoAnaliseDropDownController : ApiController
	{
		private readonly ISolicitacaoAlteracaoBll _bll;

		/// <summary>Município injetar regras de negócio</summary>
		/// <param name="solicitaçãoAnalise"></param>
		public SolicitacaoAlteracaoAnaliseDropDownController(ISolicitacaoAlteracaoBll bll)
		{
			_bll = bll;
		}

		public IEnumerable<object> Get([FromUri] PRCSolicitacaoAlteracaoVM view)
		{
			var ret = _bll.ListarChave(view);

			return ret;
		}
	}
}