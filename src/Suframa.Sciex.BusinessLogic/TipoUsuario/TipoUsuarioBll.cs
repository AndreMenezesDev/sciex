using System.Collections.Generic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;

namespace Suframa.Sciex.BusinessLogic
{
	public class TipoUsuarioBll : ITipoUsuarioBll
	{
		private readonly IUnitOfWork _uow;

		public TipoUsuarioBll(IUnitOfWork uow)
		{
			_uow = uow;
		}

		public IEnumerable<TipoUsuarioVM> Listar(TipoUsuarioVM tipoUsuario)
		{
			return _uow.QueryStack.TipoUsuario.ListarGrafo(s => new TipoUsuarioVM
			{
				Descricao = s.Descricao,
				IdTipoUsuario = s.IdTipoUsuario
			}
			, w =>
				(!tipoUsuario.IdTipoUsuario.HasValue || w.IdTipoUsuario == tipoUsuario.IdTipoUsuario)
			);
		}
	}
}