using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class DesignarAnalistaController : ApiController
	{
		private readonly IDesignarPliBll _designarPliBll;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="pliBll"></param>
		public DesignarAnalistaController(IDesignarPliBll designarPliBll)
		{
			_designarPliBll = designarPliBll;
		}

		/// <summary>Valida a regra de cadastro do PLI</summary>
		/// <param name="pliVM"></param>
		/// <returns></returns>
		public PliVM Put([FromBody]ListaPliVM pliVM)
		{
			return _designarPliBll.Salvar(pliVM);
		}


		/// <summary>Deletar Pli pelo ID</summary>
		/// <param name="id">ID Pli</param>
		public void Delete(int id)
		{
			_designarPliBll.Deletar(id);
		}
	}
}