using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IPliAnaliseVisualBll
	{
		IEnumerable<PliVM> Listar(PliVM pliVM);

		PagedItems<PliVM> ListarPaginado(PliVM pagedFilter);
		PagedItems<PliVM> ListarPaginadoSql(PliVM pagedFilter);
		IEnumerable<RelatorioRetificacoesVM> ListarRelatorio(long? idPli);
		PliVM SalvarResposta(PliVM pliVM);
		PliVM Salvar(PliVM pliVM);
		PliVM Selecionar(long? idPli);

		void Deletar(long id);

		IEnumerable<object> Listar();
	}
}