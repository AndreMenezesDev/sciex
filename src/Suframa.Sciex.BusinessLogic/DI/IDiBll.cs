using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IDiBll
	{
		string GerarArquivoSimulacaoDI();

		void LerAquivoDI();

		string ProcessarDI();

		PliVM Selecionar(long? idPli);

		DiLiVM SelecionarDiLi(long? idDi);

		PagedItems<DiLiVM> ListarPaginado(DiLiVM pagedFilter);
		IEnumerable<object> ListarLiAdicoes(DiVM diVM);
	}
}
