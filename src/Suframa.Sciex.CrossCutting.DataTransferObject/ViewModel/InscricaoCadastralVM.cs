using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using System;
using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class InscricaoCadastralVM : PagedOptions
	{
		public string Ano { get; set; }

		public InscricaoCadastralAtividadeVM AtividadePrincipal { get; set; }

		public IEnumerable<InscricaoCadastralAtividadeVM> AtividadesSecundarias { get; set; }
		public string Codigo { get; set; }
		public string CpfCnpj { get; set; }
		public string Data { get; set; }
		public DateTime? DataAbertura { get; set; }
		public string DataAtual { get { return DateTime.Now.ToString("dd/MM/yyyy"); } }
		public DateTime? DataValidade { get; set; }
		public string DescricaoPerfil { get; set; }
		public string DescricaoUnidadeCadastradora { get; set; }
		public string Email { get; set; }
		public InscricaoCadastralEnderecoVM Endereco { get; set; }
		public string HoraAtual { get { return DateTime.Now.ToString("HH:mm:ss"); } }
		public int? IdInscricaoCadastral { get; set; }
		public int? IdPessoaFisica { get; set; }
		public int? IdPessoaJuridica { get; set; }
		public EnumSituacaoInscricao? IdSituacaoInscricao { get; set; }
		public int? IdUnidadeCadastradora { get; set; }
		public bool? IsBloquearDesbloquear { get; set; }

		public bool? IsCancelar { get; set; }
		public bool? IsPessoaFisica { get { return !string.IsNullOrEmpty(CpfCnpj) && CpfCnpj.Length == 14; } }

		public bool? IsVisualizarHistorico { get; set; }

		public string NaturezaJuridicaCodigo { get; set; }

		public string NaturezaJuridicaDescricao { get; set; }

		public string NomeRazaoSocial { get; set; }

		public string SituacaoCadastralData { get; set; }

		public string SituacaoCadastralDescricao { get; set; }

		public string SituacaoInscricaoDescricaoMotivo { get; set; }

		public string Telefone { get; set; }

		public string TipoEstabelecimento { get; set; }

		public InscricaoCadastralTipoIncentivoVM TipoIncentivo { get; set; }
	}
}