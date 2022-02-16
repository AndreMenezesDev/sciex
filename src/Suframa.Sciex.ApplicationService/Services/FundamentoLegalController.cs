using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Fundamento Legal</summary>
	public class FundamentoLegalController : ApiController
	{
		private readonly IFundamentoLegalBll _fundamentoLegalBll;

		/// <summary>Fundamento Legal injetar regras de negócio</summary>
		/// <param name="fundamentoLegalBll"></param>
		public FundamentoLegalController(IFundamentoLegalBll fundamentoLegalBll)
		{
			_fundamentoLegalBll = fundamentoLegalBll;
		}

		/// <summary>Deletar o Fundamento Legal pelo ID</summary>
		/// <param name="id">ID Fundamento Legal</param>
		public void Delete(int id)
		{
			_fundamentoLegalBll.Deletar(id);
		}

		/// <summary>Obter Fundamento Legal pelo ID</summary>
		/// <param name="id">ID do Fundamento Legal</param>
		/// <returns></returns>
		public FundamentoLegalVM Get(int id)
		{
			return _fundamentoLegalBll.Visualizar(new FundamentoLegalVM { IdFundamentoLegal = id });
		}

		/// <summary>Persistir Fundamento Legal</summary>
		/// <param name="fundamentoLegalVM">Objeto Fundamento Legal a ser persistido</param>
		/// <returns></returns>
		public FundamentoLegalVM Put(FundamentoLegalVM fundamentoLegalVM)
		{
			_fundamentoLegalBll.Salvar(fundamentoLegalVM);
			return fundamentoLegalVM;
		}
	}
}