using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;


namespace Suframa.Sciex.BusinessLogic
{
	public interface IManterBeneficioNCMBll
	{

		PagedItems<TaxaNCMBeneficioVM> ListarPaginado(TaxaNCMBeneficioVM pagedFilter);
		TaxaGrupoBeneficioVM Selecionar(int? idAladi);
		void Deletar(int id);
		int Salvar(TaxaNCMBeneficioVM taxaGrupoBeneficioVM);
		PagedItems<TaxaEmpresaAtuacaoVM> ListarEmpresaPDI(TaxaEmpresaAtuacaoVM parametros);
	}
}
