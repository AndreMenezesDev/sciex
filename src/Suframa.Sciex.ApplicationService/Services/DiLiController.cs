using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class DiLiController : ApiController
	{
		private readonly IDiBll _diBll;

		public DiLiController(IDiBll diBll)
		{
			_diBll = diBll;
		}
		
		public DiLiVM Get(int id)
		{
			return _diBll.SelecionarDiLi(id);
		}
	}
}