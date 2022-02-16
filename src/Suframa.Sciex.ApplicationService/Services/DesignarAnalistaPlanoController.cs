using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class DesignarAnalistaPlanoController : ApiController
	{
		private readonly IDesignarPliBll _designarPliBll;

		public DesignarAnalistaPlanoController(IDesignarPliBll designarPliBll)
		{
			_designarPliBll = designarPliBll;
		}

		public PlanoExportacaoVM Put([FromBody]ListaPlanoVM vm)
		{
			return _designarPliBll.DesignarAnalistaPlano(vm);
		}
	}
}