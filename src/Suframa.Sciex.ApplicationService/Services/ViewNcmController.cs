using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class ViewNcmController : ApiController
	{
		private readonly IViewNcmBll _viewNcmBll;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="viewNcmBll"></param>
		public ViewNcmController(IViewNcmBll viewNcmBll)
		{
			_viewNcmBll = viewNcmBll;
		}

		public ViewNcmVM Get(int id)
		{
			return _viewNcmBll.Selecionar(id);
		}
	}
}