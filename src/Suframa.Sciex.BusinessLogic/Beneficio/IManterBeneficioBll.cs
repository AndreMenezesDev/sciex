using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;


namespace Suframa.Sciex.BusinessLogic
{
	public interface IManterBeneficioBll
	{

		void Salvar(TaxaGrupoBeneficioVM taxaGrupoBeneficioVM);
		void RegrasSalvar(TaxaGrupoBeneficioVM taxaGrupoBeneficioVM);
		PagedItems<TaxaGrupoBeneficioVM> ListarPaginado(TaxaGrupoBeneficioVM pagedFilter);
		TaxaGrupoBeneficioVM Selecionar(int? idAladi);
	}
}
