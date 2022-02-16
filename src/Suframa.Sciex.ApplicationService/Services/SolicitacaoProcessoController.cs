using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class SolicitacaoProcessoController : ApiController
	{
		private readonly IProcessoSolicitacaoAlteracaoBll _bll;

		public SolicitacaoProcessoController(IProcessoSolicitacaoAlteracaoBll bll)
		{
			_bll = bll;
		}

		public PRCSolicitacaoAlteracaoVM Get(int id)
		{
			return _bll.SelecionarSolicitacao(id);
		}

		public ResultadoMensagemProcessamentoVM Post([FromBody] PRCSolicitacaoAlteracaoVM vm)
		{
			return _bll.CriarSolicitacao(vm);
		}

		public ResultadoMensagemProcessamentoVM Put([FromBody] PRCSolicitacaoAlteracaoVM vm)
		{
			return _bll.EntregarSolicitacao(vm);
		}

		public bool Delete(int id)
		{
			return _bll.ExcluirSolicitacao(id); ;
		}


	}
}