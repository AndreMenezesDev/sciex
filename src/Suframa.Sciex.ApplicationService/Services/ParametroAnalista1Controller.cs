using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class ParametroAnalista1Controller : ApiController
	{
		private readonly IParametroAnalista1Bll _parametroAnalista1Bll;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="parametroAnalista1Bll"></param>
		public ParametroAnalista1Controller(IParametroAnalista1Bll parametroAnalista1Bll)
		{
			_parametroAnalista1Bll = parametroAnalista1Bll;
		}
		
		/// <summary>Seleciona a ParametroAnalista1 pelo ID</summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ParametroAnalista1VM Get(int id)
		{
			return _parametroAnalista1Bll.Selecionar(id);
		}

		/// <summary>Seleciona uma lista de ParametroAnalista1</summary>
		/// <param name="parametroAnalista1VM"></param>
		/// <returns></returns>
		public IEnumerable<ParametroAnalista1VM> Get([FromUri]ParametroAnalista1VM parametroAnalista1VM)
		{
			return _parametroAnalista1Bll.Listar(parametroAnalista1VM);
		}

		/// <summary>Salva a ParametroAnalista1</summary>
		/// <param name="parametroAnalista1VM"></param>
		/// <returns></returns>
		public ParametroAnalista1VM Put([FromBody]ParametroAnalista1VM parametroAnalista1VM)
		{
			_parametroAnalista1Bll.Salvar(parametroAnalista1VM);
			return parametroAnalista1VM;
		}


	}
}