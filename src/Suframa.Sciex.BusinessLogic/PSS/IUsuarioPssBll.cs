using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.BusinessLogic.Pss
{
	public interface IUsuarioPssBll
	{
		UsuarioPssVM ObterUsuarioLogado();
		UsuarioPssVM PossuiUsuarioLogado();
		UsuarioPssVM configurarUsuarioMock(String cnpjUsuarioLogado);
		UsuarioPssVM configurarUsuario(String cnpjUsuarioLogado);
		UsuarioPssVM obterUsuarioInternoPorLogin(string loginUsuario);
		string obterMenuUsuarioLogado();
		UsuarioPssVM configurarRepresentacao(RepresentacaoVM representacao);
		IEnumerable<RepresentacaoVM> ObterRepresentacoesUsuarioLogado();
		IEnumerable<UsuarioPssVM> ListarUsuariosInternos();
		IEnumerable<UsuarioPssVM> ListarUsuariosInternosPorUC();
		IEnumerable<RepresentacaoVM> ListaEmpresaRepresentadas();
		UsuarioPssVM configurarRepresentacaoMock(RepresentacaoVM representacao);
		IEnumerable<UsuarioPssVM> ObterListaUsuariosPerfilAnalista();
	}
}
