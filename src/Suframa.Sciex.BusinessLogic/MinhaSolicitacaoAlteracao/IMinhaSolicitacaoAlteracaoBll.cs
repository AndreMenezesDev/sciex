using System;
using System.Collections.Generic;
using System.Linq;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IMinhaSolicitacaoAlteracaoBll
	{
		PagedItems<PRCSolicitacaoAlteracaoVM> ListarPaginado(PRCSolicitacaoAlteracaoVM objeto);
		string getDescricaoStatus(int? Status);
		string BuscarNumeroAnoProcessoPorIdProcesso(int idProcesso);
	}
}
