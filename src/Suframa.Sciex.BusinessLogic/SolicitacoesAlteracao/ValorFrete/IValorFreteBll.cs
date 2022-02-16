using System;
using System.Collections.Generic;
using System.Linq;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.BusinessLogic.Pss;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IValorFreteBll
	{
		SolicitacoesAlteracaoVM BuscarInfo(SolicitacoesAlteracaoVM dados);
		ValorFreteVM Calcular(SolicitacoesAlteracaoVM objeto);
		decimal CalculaQuantidadeMaxima(SolicitacoesAlteracaoVM vm);
		int Salvar(SolicitacoesAlteracaoVM objeto);
		void CalcularValoresInsumo(bool isPara, SolicitacoesAlteracaoVM objeto, ref PRCInsumoEntity PRCInsumoEntity);
		PRCDetalheInsumoEntity CalcularValoresDetalhe(bool isPara, SolicitacoesAlteracaoVM objeto, PRCInsumoEntity PRCInsumoEntity, PRCDetalheInsumoEntity PRCDetalheInsumoEntity);
	}
}
