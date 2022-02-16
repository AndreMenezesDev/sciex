using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service natureza jurídica</summary>
	public class RegimeTributarioTesteController : ApiController
	{
		private readonly IRegimeTributarioTesteBll _regimeTributarioTesteBll;

		/// <summary>Regime Tributário injetar regras de negócio</summary>
		/// <param name="regimeTributarioTesteBll"></param>
		public RegimeTributarioTesteController(IRegimeTributarioTesteBll regimeTributarioTesteBll)
		{
			_regimeTributarioTesteBll = regimeTributarioTesteBll;
		}

		/// <summary>Deletar o regime tributário pelo ID</summary>
		/// <param name="id">ID regime tributário</param>
		public void Delete(int id)
		{
			_regimeTributarioTesteBll.Deletar(id);
		}

		/// <summary>Obter Regime Tributário pelo ID</summary>
		/// <param name="id">ID do regime tributário</param>
		/// <returns></returns>
		public RegimeTributarioTesteVM Get(int id)
		{
			return _regimeTributarioTesteBll.Visualizar(new RegimeTributarioTesteVM { IdRegimeTributario = id });
		}

		/// <summary>Persistir regime tributário</summary>
		/// <param name="regimeTributarioTesteVM">Objeto natureza jurídica a ser persistido</param>
		/// <returns></returns>
		public RegimeTributarioTesteVM Put(RegimeTributarioTesteVM regimeTributarioTesteVM)
		{
			_regimeTributarioTesteBll.Salvar(regimeTributarioTesteVM);
			return regimeTributarioTesteVM;
		}
	}
}