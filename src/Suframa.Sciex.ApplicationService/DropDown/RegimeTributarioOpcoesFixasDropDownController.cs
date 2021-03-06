using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Drop down de classe atividade</summary>
	public class RegimeTributarioOpcoesFixasDropDownController : ApiController
	{
		private readonly IRegimeTributarioBll _regimeTributarioBll;
		private readonly IRegimeTributarioMercadoriaBll _regimeTributarioMercadoriaBll;

		/// <summary>Classe Atividade injetar regras de negócio</summary>
		/// <param name="regimeTributario"></param>
		public RegimeTributarioOpcoesFixasDropDownController(IRegimeTributarioBll regimeTributarioBll, IRegimeTributarioMercadoriaBll regimeTributarioMercadoriaBll)
		{
			_regimeTributarioBll = regimeTributarioBll;
			_regimeTributarioMercadoriaBll = regimeTributarioMercadoriaBll;
		}

		/// <summary>Obter ID e Descrição da SubClasse Atividade filtrada</summary>
		/// <returns></returns>
		public IEnumerable<object> Get([FromUri]RegimeTributarioVM regimeTributarioVM)
		{
			return _regimeTributarioBll.ListarDropOpcoesFixas();
		}

	}
}