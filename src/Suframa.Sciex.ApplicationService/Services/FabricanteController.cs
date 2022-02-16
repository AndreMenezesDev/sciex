using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service fabricante</summary>
	public class FabricanteController : ApiController
	{
		private readonly IFabricanteBll _fabricanteBll;
		private readonly IPaisBll _paisBll;

		/// <summary>Fabricante injetar regras de negócio</summary>
		/// <param name="fabricanteBll"></param>
		public FabricanteController(IFabricanteBll fabricanteBll, IPaisBll paisBll)
		{
			_fabricanteBll = fabricanteBll;
			_paisBll = paisBll;
		}


		/// <summary>Seleciona a Aladi pelo ID</summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public FabricanteVM Get(int id)
		{
			var fabricante = _fabricanteBll.Selecionar(id);

			if (fabricante != null)
			{
				fabricante.DescricaoPais = _paisBll.ListarDescricaoPais(fabricante.CodigoPais);
			}

			return fabricante;

		}

		/// <summary>Deletar o fabricante pelo ID</summary>
		/// <param name="id">ID fabricante</param>
		public void Delete(int id)
		{
			_fabricanteBll.Deletar(id);
		}

		/// <summary>Persistir fabricante</summary>
		/// <param name="fabricanteVM">Objeto fabricante a ser persistido</param>
		/// <returns></returns>
		public FabricanteVM Put([FromBody]FabricanteVM fabricanteVM)
		{
			return _fabricanteBll.Salvar(fabricanteVM);

		}

		public FabricanteVM Post([FromBody]FabricanteVM fabricanteVM)
		{
			return _fabricanteBll.Salvar(fabricanteVM);

		}
	}
}