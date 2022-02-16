using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class CancelarLiController : ApiController
	{
		private readonly ICancelarLiBll _CancelarLiBll;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="CancelarLiBll"></param>
		public CancelarLiController(ICancelarLiBll CancelarLiBll)
		{
			_CancelarLiBll = CancelarLiBll;
		}

		/// <summary>Salva a ConsultarPli</summary>
		/// <param name="LiVM"></param>
		/// <returns></returns>
		public LiVM Put([FromBody]LiVM LiVM)
		{
			_CancelarLiBll.Salvar(LiVM);
			return LiVM;
		}

		/// <summary>Salva a ConsultarPli</summary>
		/// <param name="numeroLI"></param>
		/// <returns></returns>
		public LiVM Get(long numeroLI)
		{			
			return _CancelarLiBll.Selecionar(numeroLI); 
		}


	}
}