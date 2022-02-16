using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface ITipoUsuarioBll
	{
		IEnumerable<TipoUsuarioVM> Listar(TipoUsuarioVM tipoUsuario);
	}
}