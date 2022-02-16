using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	public class SetorArmazenamentoController : ApiController
	{
		ISetorArmazenamentoBll _setorArmazenamentoBll;

		public SetorArmazenamentoController(ISetorArmazenamentoBll setorArmazenamentoBll)
		{
			_setorArmazenamentoBll = setorArmazenamentoBll;
		}

		public SetorArmazenamentoVM Get([FromUri]SetorArmazenamentoVM obejto)
		{
			return _setorArmazenamentoBll.VerificaCodigoCadastrado(obejto);
		}

		public SetorArmazenamentoVM Get(int id)
		{
			return _setorArmazenamentoBll.SelecionarArmazenamento(id);
		}

		public int Post([FromBody]SetorArmazenamentoVM objeto)
		{
			return _setorArmazenamentoBll.Salvar(objeto);
		}

		public int Put([FromBody]SetorArmazenamentoVM objeto)
		{
			return _setorArmazenamentoBll.AtualizarStatus(objeto);
		}

	}
}