using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IPliBll
	{
		IEnumerable<PliVM> Listar(PliVM pliVM);

		PagedItems<PliVM> ListarPaginado(PliVM pagedFilter);

		PliVM Salvar(PliVM pliVM);

		bool Validar(PliVM pliVM);

		PliVM Selecionar(long? idPli);

		void Deletar(long id);

		IEnumerable<object> Listar();

		long GerarSequence(string cnpj, int ano);

		PliVM RegrasValidar(long? IdPLI, int idPLIAplicacao, long? IdPliProduto, long? IdPliMercadoria, bool EntregarPli);

		string ProcessarPLI();

		bool CopiarPliParaCancelamentoLi(List<long> ListaPliMercadoriasSelecionados);

		PliVM PliAddLi(PliVM pliVM);
	}
}