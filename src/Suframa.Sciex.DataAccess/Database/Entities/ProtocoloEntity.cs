using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_PROTOCOLO")]
    public partial class ProtocoloEntity : BaseEntity, IData
    {
        [Column("PRT_NU_ANO")]
        public int Ano { get; set; }

        public virtual ICollection<ConsultaPublicaEntity> ConsultaPublica { get; set; }

        [Column("PRT_DT_ALTERACAO")]
        public DateTime DataAlteracao { get; set; }

        [Column("PRT_DT_CANCELAMENTO")]
        public DateTime? DataCancelamento { get; set; }

        [Column("PRT_DT_INCLUSAO")]
        public DateTime DataInclusao { get; set; }

        public virtual ICollection<DiligenciaEntity> Diligencia { get; set; }

        [Key]
        [Column("PRT_ID")]
        public int IdProtocolo { get; set; }

        [Column("REQ_ID")]
        [ForeignKey(nameof(Requerimento))]
        public int IdRequerimento { get; set; }

        [Column("SPR_ID")]
        [ForeignKey(nameof(StatusProtocolo))]
        public int IdStatusProtocolo { get; set; }

        [Column("USI_ID")]
        [ForeignKey(nameof(UsuarioInterno))]
        public int? IdUsuarioInterno { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("PRT_NU_SEQ")]
        public int NumeroSequencial { get; set; }

        public virtual ICollection<RecursoEntity> Recurso { get; set; }

        public virtual RequerimentoEntity Requerimento { get; set; }

        public virtual StatusProtocoloEntity StatusProtocolo { get; set; }

        public virtual ICollection<TaxaServicoEntity> TaxaServico { get; set; }

        [Column("PRT_TP_ORIGEM")]
        public int TipoOrigem { get; set; }

        public virtual UsuarioInternoEntity UsuarioInterno { get; set; }

        public virtual ICollection<WorkflowProtocoloEntity> WorkflowProtocolo { get; set; }

        public ProtocoloEntity()
        {
            ConsultaPublica = new HashSet<ConsultaPublicaEntity>();
            TaxaServico = new HashSet<TaxaServicoEntity>();
            WorkflowProtocolo = new HashSet<WorkflowProtocoloEntity>();
            Recurso = new HashSet<RecursoEntity>();
            Diligencia = new HashSet<DiligenciaEntity>();
        }
    }
}