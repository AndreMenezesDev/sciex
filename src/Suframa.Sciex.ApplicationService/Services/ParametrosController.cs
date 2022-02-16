using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class ParametrosController : ApiController
	{
		private readonly IParametrosBll _parametrosBll;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="parametrosBll"></param>
		public ParametrosController(IParametrosBll parametrosBll)
		{
			_parametrosBll = parametrosBll;
		}

		/// <summary>Seleciona a parametros pelo ID</summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ParametrosVM Get(int id)
		{
			return _parametrosBll.Selecionar(id);
		}

		/// <summary>Seleciona uma lista de parametros</summary>
		/// <param name="parametrosVM"></param>
		/// <returns></returns>
		public IEnumerable<ParametrosVM> Get([FromUri]ParametrosVM parametrosVM)
		{
			return _parametrosBll.Listar(parametrosVM);
		}

		/// <summary>Salva a parametros</summary>
		/// <param name="parametrosVM"></param>
		/// <returns></returns>
		public ParametrosVM Put([FromBody]ParametrosVM parametrosVM)
		{
			_parametrosBll.Salvar(parametrosVM);
			return parametrosVM;
		}

		/// <summary>Deletar parametros pelo ID</summary>
		/// <param name="id">ID parametros</param>
		public void Delete(int id)
		{
			_parametrosBll.Deletar(id);
		}
	}
}