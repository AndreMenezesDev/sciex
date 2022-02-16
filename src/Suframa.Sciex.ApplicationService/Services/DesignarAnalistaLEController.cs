using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class DesignarAnalistaLEController : ApiController
	{
		private readonly IDesignarPliBll _designarPliBll;

		public DesignarAnalistaLEController(IDesignarPliBll designarPliBll)
		{
			_designarPliBll = designarPliBll;
		}

		public LEProdutoVM Put([FromBody]ListaLeVM pliVM)
		{
			return _designarPliBll.DesignarAnalistaLe(pliVM);
		}

		public void Delete(int id)
		{
			_designarPliBll.Deletar(id);
		}
	}
}