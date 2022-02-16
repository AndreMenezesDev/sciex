using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.RestService.ApiDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.DataAccess.RestService
{
	public interface IIntegracaoPssApi
	{
		UsuarioPSSResDto obterUsuarioInternoPorLogin(string loginUsuario);
		string obterMenuUsuarioLogado(string loginUsuario);
		IEnumerable<RepresentacaoVM> ObterRepresentacoesUsuarioLogado(string loginUsuario);
	}
}
