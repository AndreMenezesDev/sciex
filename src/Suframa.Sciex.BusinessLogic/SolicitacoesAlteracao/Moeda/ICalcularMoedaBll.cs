using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.BusinessLogic
{
	public interface ICalcularMoedaBll
	{
		bool Salvar(SolicitacoesAlteracaoVM objeto);
		CalcularMoedaVM CalcularMoeda(SolicitacoesAlteracaoVM objeto);
		PRCInsumoEntity CalcularMoedaPRCInsumo(PRCInsumoEntity PRCInsumoEntity);
		PRCDetalheInsumoEntity CalcularMoedaPRCDetalheInsumo(PRCDetalheInsumoEntity PRCDetalheInsumoEntity);

	}
}
