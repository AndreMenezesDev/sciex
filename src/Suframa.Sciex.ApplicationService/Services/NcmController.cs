using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class NcmController : ApiController
	{
		private readonly INcmBll _ncmBll;

		public NcmController(INcmBll ncmBll)
		{
			_ncmBll = ncmBll;
		}

		public NcmVM Get(int id)
		{
			return _ncmBll.Selecionar(id);
		}

		[AllowAnonymous]
		public IEnumerable<object> Get([FromUri] NcmVM ncmvm)
		{
			return _ncmBll.ListarChave(ncmvm);
		}
		[AllowAnonymous]
		public NcmVM Put([FromBody]NcmVM ncmVM)
		{
			_ncmBll.Salvar(ncmVM);
			return ncmVM;
		}

		public void Delete(int id)
		{
			_ncmBll.Deletar(id);
		}
	}
}