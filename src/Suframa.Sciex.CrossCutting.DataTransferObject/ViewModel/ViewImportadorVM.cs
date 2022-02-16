using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ViewImportadorVM : PagedOptions
	{		
		public int IdPessoaJuridica { get; set; }
		public string Cnpj { get; set; }
		public string RazaoSocial { get; set; }
		public string NomeFantasia { get; set; }
		public string TipoEntidadeRegistro { get; set; }
		public string TipoEstabelecimento { get; set; }
		public string NumeroRegistroConstituicao { get; set; }
		public bool? StatusOptanteSimples { get; set; }
		public string StatusOptanteSimplesDescricao { get; set; }
		public double? ValorCapitalSocial { get; set; }
		public decimal CEP { get; set; }
		public string Logradouro { get; set; }
		public string Endereco { get; set; }
		public string Bairro { get; set; }
		public string Complemento { get; set; }
		public string Numero { get; set; }
		public string PontoReferencia { get; set; }
		public decimal CodigoMunicipio { get; set; }
		public string Municipio { get; set; }
		public int? CodigoUF { get; set; }
		public string UF { get; set; }
		public string Telefone { get; set; }
		public string Ramal { get; set; }
		public string Email { get; set; }
		public int IdNaturezaJuridica { get; set; }
		public string DescricaoNaturezaJuridica { get; set; }
		public int IdNaturezaGrupo { get; set; }
		public int CodigoNaturezaGrupo { get; set; }
		public string DescricaoNaturezaGrupo { get; set; }
		public int? IdCEP { get; set; }
		public int? IdPorteEmpresa { get; set; }
		public string DescricaoPorteEmpresa { get; set; }
		public string NumeroInscricaoMunicipal { get; set; }
		public int InscricaoCadastral { get; set; }
		public int IdUnidadeCadastradora { get; set; }
		public int? CodigoUnidadeCadastradora { get; set; }
		public string DescricaoUnidadeCadastradora { get; set; }
		public int IdSituacaoInscricao { get; set; }
		public string DescricaoSituacaoInscricao { get; set; }
		public DateTime DataInscricaoCadastral { get; set; }
		public int? StatusInscricaoCadastalProjetoAprovado { get; set; }
		public string TemProjetoAprovado { get; set; }
		public int CobrarTCIF { get; set; }
	}
}
