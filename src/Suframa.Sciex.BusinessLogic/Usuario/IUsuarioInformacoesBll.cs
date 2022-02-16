using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IUsuarioInformacoesBll
	{
		string ObterCNPJ();
		IEnumerable<EnumPapel> ListarPapeis(int idUsuarioInterno);
		IEnumerable<EnumPerfil> ListarPerfis(int? idPessoaFisica, int? idPessoaJuridica);
		int? ObterIdPessoaFisica(string cpf);
		int? ObterIdPessoaJuridica(string cnpj);
		string ObterSetorUsuarioInterno(string cpf);
		UsuarioInternoEntity ObterUsuarioInterno(string cpf);
		string ObterDadosImportador(string cnpj);
		string ObterDadosPreposto(string cpf);
		//IEnumerable<RepresentacaoVM> ListaEmpresaRepresentadas();
		//TokenDto ObterDadosUsuario();
	}
}