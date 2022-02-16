using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Dropdown RegimeTributarioMercadoria</summary>
	public class RegimeTributarioMercadoriaDropDownController : ApiController
	{
		private readonly IRegimeTributarioMercadoriaBll _regimeTributarioMercadoriaBll;
		private readonly IMunicipioBll _municipioBll;
		private readonly IRegimeTributarioBll _regimeTributarioBll;

		/// <summary>regimeTributarioMercadoriaBll injetar regras de negócio</summary>
		/// <param name="_regimeTributarioMercadoriaBll"></param>
		public RegimeTributarioMercadoriaDropDownController(IRegimeTributarioMercadoriaBll regimeTributarioMercadoriaBll, IMunicipioBll municipioBll, IRegimeTributarioBll regimeTributarioBll)
		{
			_regimeTributarioMercadoriaBll = regimeTributarioMercadoriaBll;
			_municipioBll = municipioBll;
			_regimeTributarioBll = regimeTributarioBll;
		}

		/// <summary>Obter ID e Descrição dos municípios filtrados</summary>
		/// <returns></returns>
		public IEnumerable<object> Get([FromUri]ViewMunicipioVM municipioVM)
		{
			return _municipioBll.ListarChave(municipioVM);
		}

		/// <summary>Obter ID e Descrição dos Regimes Tributários filtrados</summary>
		/// <returns></returns>
		public IEnumerable<object> Get()
		{
			return _regimeTributarioBll.ListarDrop();

		}
	}

}