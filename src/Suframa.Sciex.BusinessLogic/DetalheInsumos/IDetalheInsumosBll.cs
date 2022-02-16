using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IDetalheInsumosBll
	{
		bool Deletar(PRCInsumoVM objeto);
		int SalvarNovoDetalhe(SalvarDetalhePRCInsumoVM vm);
		bool Deletar(int PRCDetalheInsumo);
	}
}
