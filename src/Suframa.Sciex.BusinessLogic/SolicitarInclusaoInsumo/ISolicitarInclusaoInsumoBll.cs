using System;
using System.Collections.Generic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;

namespace Suframa.Sciex.BusinessLogic
{ 
	public interface ISolicitarInclusaoInsumoBll
	{
		PagedItems<LEInsumoVM> ListarPaginado(LEInsumoVM objeto);
		string Salvar(List<LEInsumoVM> objeto);
	}
}
