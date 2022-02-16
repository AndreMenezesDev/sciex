using System;
using System.Collections.Generic;
using System.Linq;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IMinhaSolicitacaoAlteracaoDetalheBll
	{
		PagedItems<PRCSolicDetalheVM> ListarPaginado(PRCSolicDetalheVM objeto);
		DetalhesMinhaSolicitacaoAlteracaoVM BuscarInfoDetalhes(int idSolicitacaoAlteracao);
		int ApagarDetalheSolicitacaoAlteracao(int id);
	}
}
