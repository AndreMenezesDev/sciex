using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class PliFornecedorFabricanteController : ApiController
	{
		private readonly IPliFornecedorFabricanteBll _pliFornecedorFabricanteBll;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="pliBll"></param>
		public PliFornecedorFabricanteController(IPliFornecedorFabricanteBll pliBll)
		{
			_pliFornecedorFabricanteBll = pliBll;
		}
		
		/// <summary>Seleciona a Pli pelo ID</summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public PliFornecedorFabricanteVM Get(int id)
		{
			return _pliFornecedorFabricanteBll.Selecionar(id);
		}
	}
}