using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service natureza jurídica</summary>
	public class RegimeTributarioController : ApiController
	{
		private readonly IRegimeTributarioBll _regimeTributarioBll;

		/// <summary>Regime Tributário injetar regras de negócio</summary>
		/// <param name="regimeTributarioBll"></param>
		public RegimeTributarioController(IRegimeTributarioBll regimeTributarioBll)
		{
			_regimeTributarioBll = regimeTributarioBll;
		}

		/// <summary>Deletar o regime tributário pelo ID</summary>
		/// <param name="id">ID regime tributário</param>
		public void Delete(int id)
		{
			_regimeTributarioBll.Deletar(id);
		}

		/// <summary>Obter Regime Tributário pelo ID</summary>
		/// <param name="id">ID do regime tributário</param>
		/// <returns></returns>
		public RegimeTributarioVM Get(int id)
		{
			return _regimeTributarioBll.Visualizar(new RegimeTributarioVM { IdRegimeTributario = id });
		}

		/// <summary>Persistir regime tributário</summary>
		/// <param name="regimeTributarioVM">Objeto natureza jurídica a ser persistido</param>
		/// <returns></returns>
		public RegimeTributarioVM Put(RegimeTributarioVM regimeTributarioVM)
		{
			_regimeTributarioBll.Salvar(regimeTributarioVM);
			return regimeTributarioVM;
		}
	}
}