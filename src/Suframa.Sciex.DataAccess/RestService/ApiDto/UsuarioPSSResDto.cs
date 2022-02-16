using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.DataAccess.RestService.ApiDto
{
	public class UsuarioPSSResDto
	{
		public string codUsuarioExterno { get; set; }

		public string nome { get; set; }

		public string unidadeAdministrativaId { get; set; }

		public string unidadeAdministrativaNome { get; set; }

		public string setorId { get; set; }

		public string setorNome { get; set; }

		public string municipioId { get; set; }

		public string cnpjRepresentante { get; set; }

		public string nomeRepresentante { get; set; }

		public IEnumerable<UsuarioPSSPerfilVO> perfis { get; set; } = Enumerable.Empty<UsuarioPSSPerfilVO>();

		public class UsuarioPSSPerfilVO
		{
			public string nome { get; set; }
			public string descricao { get; set; }
		}

		public UsuarioPSSResDto()
		{
		}
	}
}
