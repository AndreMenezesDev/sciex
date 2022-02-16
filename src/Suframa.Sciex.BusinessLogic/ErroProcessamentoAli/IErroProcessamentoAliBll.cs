﻿using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IErroProcessamentoAliBll
	{
		IEnumerable<ErroProcessamentoVM> Listar(ErroProcessamentoVM ErroProcessamentoVM);

		PagedItems<ErroProcessamentoVM> ListarPaginado(ErroProcessamentoVM pagedFilter);

	}
}