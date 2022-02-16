using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	public class RecintoAlfandegaController : ApiController
	{
		IRecintoAlfandegaBll _recintoalfandega;
		public RecintoAlfandegaController(IRecintoAlfandegaBll recintoalfandega)
		{
			_recintoalfandega = recintoalfandega;
		}

		public RecintoAlfandegaVM Get([FromUri]RecintoAlfandegaVM obejto)
		{
			return _recintoalfandega.VerificaCodigoCadastrado(obejto);
		}

		public RecintoAlfandegaVM Get(int id)
		{
			return _recintoalfandega.SelecionarRecintoAlfandega(id);
		}

		public int Post([FromBody]RecintoAlfandegaVM objeto)
		{
			return _recintoalfandega.Salvar(objeto);
		}

		public int Put([FromBody]RecintoAlfandegaVM objeto)
		{
			return _recintoalfandega.AtualizarStatus(objeto);
		}

	}
}