using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IAliHistoricoBll
	{
		IEnumerable<AliHistoricoVM> Listar(AliHistoricoVM AliHistoricoVM);
		IEnumerable<object> ListarChave(AliHistoricoVM AliHistoricoVM);
		PagedItems<AliHistoricoVM> ListarPaginado(AliHistoricoVM pagedFilter);

		void Salvar(AliHistoricoVM AliHistoricoVM);
		AliHistoricoVM Selecionar(int? numeroAliHistorico);
		void Deletar(int id);

		//AliHistoricoVM VisuAliHistoricozar(AliHistoricoVM AliHistoricoVM);
		//PagedItems<AliHistoricoVM> ListarPaginado(AliHistoricoVM pagedFilter);
		//void GerarAquivoEnvio();
		//bool DowwloadArquivoAliHistorico();
		//string EnviarArquivoAliHistorico();
		//PagedItems<AliHistoricoVM> ListarPaginadoRelatorioAliHistorico(AliHistoricoVM pagedFilter);
		//void GerarArquivoCancelamento();
		//string EnviarArquivoAliHistoricoCancelamento();
	}
}
