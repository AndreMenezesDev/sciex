using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	[Serializable]
	public class UsuarioPssVM : PagedOptions
	{
		public string usuCpfCnpjEmpresaOuLogado { get; set; }
		public string usuNomeEmpresaOuLogado { get; set; }
		public string usuarioLogadoCpfCnpj { get; set; }
		public string usuarioLogadoNome { get; set; }
		public string empresaRepresentadaCnpj { get; set; }
		public string empresaRepresentadaRazaoSocial { get; set; }
		public string usuSetor { get; set; }
		public string usuUnidadeAdministrativa { get; set; }
		public string usuSituacao { get; set; }
		public string usuNomeUsuario { get; set; }
		public int usuInscricaoCadastral { get; set; }
		public int usuCodmunicipio { get; set; }
		public double exp { get; set; }

		public string usuCnpjRepresentanteLogado { get; set; }//representante
		public string usuNomeRepresentanteLogado { get; set; }
		
		public IList<EnumPerfil> Perfis { get; set; }
	}
}
