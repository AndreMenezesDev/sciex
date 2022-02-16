using System;
using System.Collections.Generic;
using System.Linq;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;
using Suframa.Sciex.DataAccess;

namespace Suframa.Sciex.BusinessLogic
{
	public class MenuBll : IMenuBll
	{
		private readonly IUnitOfWorkSciex _uow;
		private readonly IUsuarioLogado _usuarioLogado;

		public MenuBll(IUnitOfWorkSciex uow, IUsuarioLogado usuarioLogado)
		{
			_uow = uow;
			_usuarioLogado = usuarioLogado;
		}

		/// <summary>
		/// Listar os menus com base em perfil ou papel do usuário
		/// </summary>
		/// <param name="idPerfil"></param>
		/// <param name="idPapel"></param>
		/// <returns></returns>
		public IEnumerable<IGrouping<string, MenuDto>> Listar()
		{
			var idPerfil = _usuarioLogado.Usuario.Perfis != null ? _usuarioLogado.Usuario.Perfis.Cast<int>().ToList() : new List<int>();
			var idPapel = _usuarioLogado.Usuario.Papeis != null ? _usuarioLogado.Usuario.Papeis.Cast<int>().ToList() : new List<int>();

			/*var menus = _uow.QueryStackSciex.MenuPerfil.ListarGrafo(s => new MenuDto
			{
				Grupo = s.Menu.MenuGrupo.Descricao,
				IdPapel = s.IdPapel,
				IdTipoUsuario = s.IdTipoUsuario,
				Menu = s.Menu.Descricao,
				Rota = s.Menu.Rota,
			},
			w =>
				(idPerfil.Count > 0 && w.IdTipoUsuario.HasValue && idPerfil.Contains(w.IdTipoUsuario.Value)) ||
				(idPapel.Count > 0 && w.IdPapel.HasValue && idPapel.Contains(w.IdPapel.Value)));

			return menus.GroupBy(g => g.Grupo);*/
			return null;
		}
	}
}