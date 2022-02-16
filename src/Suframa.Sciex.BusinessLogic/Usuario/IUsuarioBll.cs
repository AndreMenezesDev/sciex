using System.Collections.Generic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IUsuarioBll
	{
		void AssociarDesassociarProtocolo(UsuarioInternoAssociacaoProtocoloVM usuarioInternoAssociacaoProtocoloVM);

		void AutenticarUsuarioSenha(string usuario, string senha);

		bool ConsultarSituacaoUsuario(string cpfCnpj);

		bool ConsultarSituacaoUsuarioComValidacao(string cpfCnpj);

		IEnumerable<UsuarioInternoVM> Listar(UsuarioInternoVM usuarioInternoVM);

		IEnumerable<UsuarioInternoVM> ListarAgendamento(UsuarioInternoVM usuarioInternoVM);

		IEnumerable<UsuarioInternoVM> ListarParaParametroAnalista(UsuarioInternoVM usuarioInternoVM);

		IEnumerable<UsuarioInternoVM> ListarParaUsuarioInterno();

		UsuarioInternoVM Selecionar(UsuarioInternoVM usuarioInternoVM);

		int SelecionarQuantidadeProtocolos(UsuarioInternoVM usuarioInternoVM);

		UsuarioInternoVM SelecionarUsuarioLogado();

		void Salvar(UsuarioInternoVM usuarioInternoVM);
	}
}