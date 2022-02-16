using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class DesignarAnalistaSolicitacaoController : ApiController
	{
		private readonly IDesignarPliBll _designarPliBll;

		public DesignarAnalistaSolicitacaoController(IDesignarPliBll designarPliBll)
		{
			_designarPliBll = designarPliBll;
		}

		public PlanoExportacaoVM Put([FromBody] ListaSolicitacaoVM vm)
		{
			return _designarPliBll.DesignarAnalistaSolicitacao(vm);
		}
	}
}