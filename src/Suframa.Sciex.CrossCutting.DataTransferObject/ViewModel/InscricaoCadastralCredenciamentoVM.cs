using System;
using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class InscricaoCadastralCredenciamentoVM : PagedOptions
	{
		public string CpfCnpj { get; set; }

		public DateTime? DataAbertura { get; set; }

		public DateTime? DataCadastro { get; set; }

		public DateTime? DataCadastroAte { get; set; }

		public DateTime? DataValidade { get; set; }

		public DateTime? DataValidadeAte { get; set; }

		public string DescricaoStatus { get; set; }

		public string DescricaoTipoUsuario { get; set; }

		public string DescricaoUnidadeCadastradora { get; set; }

		public IEnumerable<HistoricoSituacaoInscricaoVM> HistoricoSituacaoInscricao { get; set; }

		public int? IdCredenciamento { get; set; }

		public int? IdInscricaoCadastral { get; set; }

		public int? IdPessoaFisica { get; set; }

		public int? IdPessoaJuridica { get; set; }

		public int? IdProtocolo { get; set; }

		public int? IdRequerimento { get; set; }

		public int? IdSituacaoInscricao { get; set; }

		public int? IdTipoUsuario { get; set; }

		public int? IdUnidadeCadastradora { get; set; }

		public int? InscricaoCadastral { get; set; }

		public bool? IsBloquearDesbloquear { get; set; }

		public bool? IsCancelar { get; set; }

		public bool? IsVisualizarHistorico { get; set; }

		public string Justificativa { get; set; }

		public string NomeRazaoSocial { get; set; }
	}
}