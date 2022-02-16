using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_REQUERIMENTO")]
    public partial class RequerimentoEntity : BaseEntity, IData
    {
        [Column("REQ_CO")]
        [StringLength(18)]
        public string Codigo { get; set; }

        public virtual DadosSolicitanteEntity DadosSolicitante { get; set; }

        [Column("REQ_DT_ALTERACAO")]
        public DateTime DataAlteracao { get; set; }

        [Column("REQ_DT_FECHAMENTO")]
        public DateTime? DataFechamento { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("REQ_DT_INCLUSAO")]
        public DateTime DataInclusao { get; set; }

        [Column("DSO_ID")]
        [ForeignKey(nameof(DadosSolicitante))]
        public int? IdDadosSolicitante { get; set; }

        [Column("PFI_ID")]
        [ForeignKey(nameof(PessoaFisica))]
        public int? IdPessoaFisica { get; set; }

        [Column("PJU_ID")]
        [ForeignKey(nameof(PessoaJuridica))]
        public int? IdPessoaJuridica { get; set; }

        [Key]
        [Column("REQ_ID")]
        public int IdRequerimento { get; set; }

        [Column("TRE_ID")]
        [ForeignKey(nameof(TipoRequerimento))]
        public int IdTipoRequerimento { get; set; }

        [Column("UND_ID")]
        [ForeignKey(nameof(UnidadeCadastradora))]
        public int? IdUnidadeCadastradora { get; set; }

        [Column("REQ_DS_MOTIVO")]
        public string Motivo { get; set; }

        public virtual PessoaFisicaEntity PessoaFisica { get; set; }

        public virtual PessoaJuridicaEntity PessoaJuridica { get; set; }

        public virtual ICollection<ProtocoloEntity> Protocolo { get; set; }

        public virtual ICollection<RequerimentoDocumentoEntity> RequerimentoDocumento { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("REQ_ST_CONCLUIDO")]
        public bool? StatusConcluido { get; set; }

        [Column("REQ_TP_COBRANCA")]
        public int? TipoCobranca { get; set; }

		[Column("REQ_ST_COMUNICACAO")]
		public int? StatusComunicacao { get; set; }

		public virtual TipoRequerimentoEntity TipoRequerimento { get; set; }

        public virtual UnidadeCadastradoraEntity UnidadeCadastradora { get; set; }

        public RequerimentoEntity()
        {
            Protocolo = new HashSet<ProtocoloEntity>();
            RequerimentoDocumento = new HashSet<RequerimentoDocumentoEntity>();
        }
    }
}