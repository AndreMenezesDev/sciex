using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class ConsultarNcmController : ApiController
	{
		private readonly INcmBll _ncmBll;

		public ConsultarNcmController(INcmBll ncmBll)
		{
			_ncmBll = ncmBll;
		}

		public NcmVM Get(int id)
		{
			return _ncmBll.Selecionar(id);
		}

		[AllowAnonymous]
		public NcmVM Get([FromUri]NcmVM ncmVM)
		{
			return _ncmBll.Selecionar(ncmVM);
		}
	}
}