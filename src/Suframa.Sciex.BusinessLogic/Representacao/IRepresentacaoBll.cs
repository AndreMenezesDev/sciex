using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
    public interface IRepresentacaoBll
	{
		IEnumerable<RepresentacaoVM> Listar(RepresentacaoVM representacaoVM);


	}
}