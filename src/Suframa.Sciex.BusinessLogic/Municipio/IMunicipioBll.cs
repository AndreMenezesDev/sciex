using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
    public interface IMunicipioBll
    {
        IEnumerable<MunicipioDto> Listar(MunicipioDto municipioDto);

        MunicipioDto Selecionar(MunicipioDto municipioDto);

		IEnumerable<object> ListarChave(ViewMunicipioVM municipioVM);
	}
}