using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_SOLIC_PLI")]
	public partial class SolicitacaoPliEntity : BaseEntity
	{		
		public virtual ICollection<SolicitacaoPliMercadoriaEntity> SolicitacaoPLIMeradoria { get; set; }
		public virtual ICollection<ErroProcessamentoEntity> ErroProcessamento { get; set; }
		public virtual EstruturaPropriaPliEntity EstruturaPropriaPli { get; set; }

		public SolicitacaoPliEntity()
		{			
			SolicitacaoPLIMeradoria = new HashSet<SolicitacaoPliMercadoriaEntity>();
			ErroProcessamento = new HashSet<ErroProcessamentoEntity>();
		}

		[Key]
		[Column("SPL_ID")]
		public long IdSolicitacaoPli { get; set; }

		[Required]
		[Column("SPL_NU_CNPJ")]
		[StringLength(14)]
		public string CnpjEmpresa { get; set; }

		[Column("PAP_ID")]
		public int? IdTipoAplicacaoPli { get; set; }

		[Column("SPL_NU_LI_REFERENCIA")]
		[StringLength(10)]
		public string NumeroLIReferencia { get; set; }	

		[Column("SPL_NU_PEXPAM")]
		public int? NumeroPEXPAM { get; set; }

		[Column("SPL_NU_ANO_PEXPAM")]
		public short? NumeroAnoPEXPAM { get; set; }
	
		[Required]
		[Column("SPL_NU_CPF_REP_LEGAL_SISCOMEX")]
		[StringLength(11)]
		public string NumeroCPFRepresentanteLegal { get; set; }

		[Column("SPL_CO_CNAE")]
		[StringLength(10)]
		public string CodigoCNAE { get; set; }

		[Column("INS_CO")]
		public int? InscricaoCadastral { get; set; }

		[Column("SET_CO")]
		public int? CodigoSetor { get; set; }

		[Column("SET_DS")]
		[StringLength(200)]
		public string DescricaoSetor { get; set; }

		[Required]
		[Column("SPL_TP_DOCUMENTO")]
		public byte TipoDocumento { get; set; }

		[Column("SPL_TP_ORIGEM")]
		public byte? TipoOrigem { get; set; }	

		[Required]
		[Column("IMP_DS_RAZAO_SOCIAL")]
		[StringLength(100)]
		public string RazaoSocialEmpresa { get; set; }

		[Column("PAP_CO")]
		public short? CodigoPliAplicacao { get; set; }

		[Column("SPL_ST_INDICACAO_PLI_EXIGENCIA")]
		[StringLength(1)]
		public string StatusIndicacaoPliExigencia { get; set; }

		[Column("SPL_NU_PLI_IMPORTADOR")]
		[StringLength(10)]
		public string NumeroPliImportador { get; set; }

		[ForeignKey(nameof(EstruturaPropriaPli))]
		[Column("ESP_ID")]
		public long? IdEstruturaPropriaPli { get; set; }

		[Column("SPL_ST_SOLICITACAO")]
		public byte? StatusSolicitacao { get; set; }

		[Column("SPL_ST_PLI_TECNOLOGIA_ASSISTIVA")]
		public byte? StatusPliTecnologiaAssistiva { get; set; }

		[Column("PLI_ID")]
		public long? IdPLI { get; set; }

		[Column("PLI_NU")]
		public long? NumeroPLI { get; set; }

		[Column("PLI_NU_ANO")]
		public long? AnoPLI { get; set; }
	}
}