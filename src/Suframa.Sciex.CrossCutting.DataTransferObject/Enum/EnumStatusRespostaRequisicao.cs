using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.Enum
{
	public enum EnumStatusRetornoRequisicao
	{
		ERRO = 0,
		SUCESSO = 1,
		PARIDADE_CAMBIAL_NAO_CADASTRADA = 2,
		NAO_EXISTE_SOLICITACAO_ALTERACAO_CADASTRADA = 3
	}
}
