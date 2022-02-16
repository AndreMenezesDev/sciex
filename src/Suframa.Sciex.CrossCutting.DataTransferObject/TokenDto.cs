using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject
{
	[Serializable]
	public class TokenDto
	{
		public string Cnpj { get; set; }
		public string Cpf { get; set; }
		public string CpfCnpj { get; set; }
		public double exp { get; set; }
		public int? IdPessoaFisica { get; set; }
		public int? IdPessoaJuridica { get; set; }
		public int? IdUsuarioInterno { get; set; }
		public string NomeUsuario { get; set; }
		public IEnumerable<EnumPapel> Papeis { get; set; }
		public IEnumerable<EnumPerfil> Perfis { get; set; }
		public string Setor { get; set; }
		public EnumTipoPessoa? TipoPessoa { get; set; }
		public bool UsuarioInterno { get; set; }
		public IEnumerable<RepresentacaoVM> ListaEmpresaRepresentadas { get; set; }
		public string EmpresaRepresentacao { get; set; }
		public string CNPJRepresentacao { get; set; }

	}


		
}