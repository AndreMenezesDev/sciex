using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class DiController : ApiController
	{
		private readonly IDiBll _diBll;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="pliBll"></param>
		public DiController(IDiBll diBll)
		{
			_diBll = diBll;
		}
		
		/// <summary>Seleciona a Pli pelo ID</summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public PliVM Get(int id)
		{
			return _diBll.Selecionar(id);
		}

		/// <summary>Seleciona uma lista de Pli</summary>
		/// <param name="pliVM"></param>
		/// <returns></returns>
		public IEnumerable<PliVM> Get([FromUri]PliVM pliVM)
		{
			return null;
		}

		/// <summary>Valida a regra de cadastro do PLI</summary>
		/// <param name="pliVM"></param>
		/// <returns></returns>
		public PliVM Put([FromBody]PliVM pliVM)
		{
			return null;
		}


		/// <summary>Deletar Pli pelo ID</summary>
		/// <param name="id">ID Pli</param>
		public void Delete(int id)
		{
			
		}
	}
}