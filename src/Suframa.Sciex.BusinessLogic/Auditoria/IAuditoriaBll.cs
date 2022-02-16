using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IAuditoriaBll
	{
		PagedItems<AuditoriaVM> ListarPorIdNcm(NcmVM ncm);

		PagedItems<AuditoriaVM> ListarPorIdBeneficio(TaxaGrupoBeneficioVM ncm);
	}
}