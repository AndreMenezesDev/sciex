using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IConsultarProcessoExportacaoBll
	{
		PRCStatusVM AprovarProrogacao(PRCStatusVM filtro);
		PRCSolicProrrogacaoVM ReprovarProrrogacao(PRCSolicProrrogacaoVM filtro);
		PRCSolicProrrogacaoVM ListarRegistroAlteracao(int IdProcesso);

	}
}
