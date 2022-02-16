using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using System;
using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ProtocoloVM : PagedOptions
	{
		public int? Ano { get; set; }

		public string CpfCnpj { get; set; }

		public DateTime? DataAlteracao { get; set; }

		public DateTime? DataCancelamento { get; set; }

		public DateTime? DataDesignacao { get; set; }

		public DateTime? DataDesignacaoFinal { get; set; }

		public DateTime? DataDesignacaoInicial { get; set; }

		public DateTime? DataInclusao { get; set; }

		public DateTime? DataInclusaoFinal { get; set; }

		public DateTime? DataInclusaoInicial { get; set; }

		public string DescricaoServico { get; set; }

		public string DescricaoUsuario { get; set; }

		public int? DiasRestantes { get; set; }

		public int? IdProtocolo { get; set; }

		public int? IdRequerimento { get; set; }

		public int? IdServico { get; set; }

		public int? IdSituacao { get; set; }

		public EnumStatusProtocolo? IdStatusProtocolo { get; set; }

		public int? IdUnidadeCadastradora { get; set; }

		public int? IdUsuarioInterno { get; set; }

		public int? InscricaoCadastral { get; set; }

		public bool IsAnalise { get; set; }

		public bool IsComPendencia { get; set; }

		public bool IsContinuarAnalise { get; set; }

		public bool IsFiltroUsuarioInterno { get; set; }

		public bool IsGeraCobranca { get; set; }

		public bool IsInicioAnalise { get; set; }

		public IEnumerable<ResumoVM> ItensCorrigidos { get; set; }

		public IEnumerable<ResumoVM> ItensCorrigir { get; set; }

		public string Justificativa { get; set; }

		public IEnumerable<MensagemPadraoVM> MensagensPadrao { get; set; }

		public string NomeRazaoSocial { get; set; }

		public string NomeUnidadeCadastradora { get; set; }

		public string NomeUsuarioInterno { get; set; }

		public string NumeroProtocolo { get; set; }

		public int? NumeroSequencial { get; set; }

		public string ProtocoloHtml { get; set; }

		public int? Rank { get; set; }

		public string Situacao { get; set; }

		public EnumStatusProtocoloGrupo StatusProtocoloGrupo { get; set; }

		public EnumStatusTaxaServico StatusServico { get; set; }

		public EnumTipoOrigemRequisicao TipoOrigem { get; set; }

		public IEnumerable<TipoDocumentoVM> TiposDocumentos { get; set; }

		public int? Total { get; set; }
		public string UsuarioInterno { get; set; }

		public IEnumerable<WorkflowProtocoloVM> Workflows { get; set; }
	}
}