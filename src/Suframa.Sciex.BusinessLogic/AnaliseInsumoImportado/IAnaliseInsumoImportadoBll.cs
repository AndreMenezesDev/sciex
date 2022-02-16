using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IAnaliseInsumoImportadoBll
	{
		List<PRCInsumoVM> ListarInsumosImportados(BuscarValoresInsumoVM parametros);
		decimal? CalcularParidadeValorPara();
		ResultadoMensagemProcessamentoVM AprovarInsumo(PRCSolicDetalheVM filtroDetalheInsumo);
		ResultadoMensagemProcessamentoVM ReprovarInsumo(PRCSolicDetalheVM filtro);
		void CalcularNovosValoresPorTipo(PRCSolicDetalheEntity regSolicDetalhe);

	}
}