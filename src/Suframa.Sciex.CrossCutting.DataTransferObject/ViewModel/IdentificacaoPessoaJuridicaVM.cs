using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class IdentificacaoPessoaJuridicaVM
	{
		public CepVM Cep { get; set; }
		public string Cnpj { get; set; }
		public string Codigo { get; set; }
		public string Complemento { get; set; }
		public DateTime? DataAlteracao { get; set; }
		public DateTime? DataInclusao { get; set; }
		public DateTime? DataRegistro { get; set; }
		public IEnumerable<DocumentoComprobatorioVM> DocumentosComprobatorios { get; set; }
		public string Email { get; set; }
		public ManterEnderecoVM Endereco { get; set; }
		public FiltroCadastroPessoaJuridicaVM FiltroCadastroPessoaJuridica { get; set; }
		public int? IdCep { get; set; }
		public int? IdPessoaJuridica { get; set; }
		public int? IdPorteEmpresa { get; set; }
		public int? IdRequerimento { get; set; }
		public int? IdTipoRequerimento { get; set; }
		public int? IdUnidadeCadastradora { get; set; }
		public bool IsCredenciamento { get; set; }
		public bool IsCredenciamentoTransportador { get; set; }
		public bool IsEntidadeEmpresarial { get; set; }
		public bool IsQuadroSocietario { get; set; }
		public EnumTipoTransportador? ModalidadeTransportador { get; set; }
		public string NomeFantasia { get; set; }
		public string NumeroEndereco { get; set; }
		public string NumeroInscricaoMunicipal { get; set; }
		public string NumeroRegistroConstituicao { get; set; }
		public IEnumerable<PessoaJuridicaInscricaoEstadualVM> PessoaJuridicaInscricaoEstadual { get; set; } = Enumerable.Empty<PessoaJuridicaInscricaoEstadualVM>();
		public string PontoReferencia { get; set; }
		public decimal? Ramal { get; set; }
		public string RazaoSocial { get; set; }
		public bool? StatusOptanteSimples { get; set; }
		public decimal? Telefone { get; set; }
		public double? ValorCapitalSocial { get; set; }
	}
}