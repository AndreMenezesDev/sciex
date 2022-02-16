using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.BusinessLogic.Pss;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class ParametrizarAnalistaController : ApiController
	{
		private readonly IPliAnaliseVisualBll _pliAnaliseVisualBll;
		private readonly IUsuarioPssBll _usuarioPssBll;
		private readonly IParametrizarAnalistaBll _parametrizarAnalistaBll;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="pliBll"></param>
		public ParametrizarAnalistaController(IPliAnaliseVisualBll pliAnaliseVisualBll, IUsuarioPssBll usuarioPssBll, IParametrizarAnalistaBll parametrizarAnalistaBll)
		{
			_pliAnaliseVisualBll = pliAnaliseVisualBll;
			_usuarioPssBll = usuarioPssBll;
			_parametrizarAnalistaBll = parametrizarAnalistaBll;
		}

		public PagedItems<AnalistaVM> Get([FromUri]AnalistaVM pagedFilter)
		{
			return _parametrizarAnalistaBll.ListarPaginado(pagedFilter);
		}

		/// <summary>Valida a regra de cadastro do PLI</summary>
		/// <param name="pliVM"></param>
		/// <returns></returns>
		public AnalistaVM Put([FromBody]AnalistaVM analistaVM)
		{
			var ret = _parametrizarAnalistaBll.Salvar(analistaVM);
			return ret;
		}
	}
}