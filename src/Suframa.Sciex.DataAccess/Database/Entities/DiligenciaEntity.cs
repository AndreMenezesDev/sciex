using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_DILIGENCIA")]
    public partial class DiligenciaEntity : BaseEntity, IData
    {
        [Column("DLG_DS_ANALISTA_RESPONSAVEL")]
        public string AnalistaResponsavel { get; set; }

        [Column("DLG_DT_ALTERACAO")]
        public DateTime DataAlteracao { get; set; }

        [Column("DLG_DT_DILIGENCIA")]
        public DateTime? DataDiligenciaUtc { get; set; }

        [Column("DLG_DT_INCLUSAO")]
        public DateTime DataInclusao { get; set; }

        public virtual ICollection<DiligenciaAtividadesEntity> DiligeciaAtividade { get; set; }

        public virtual ICollection<DiligenciaAnexosEntity> DiligenciaAnexo { get; set; }

        [Key]
        [Column("DLG_ID")]
        public int IdDiligencia { get; set; }

        [Column("PFI_ID")]
        [ForeignKey(nameof(PessoaFisica))]
        public int? IdPessoaFisica { get; set; }

        [Column("PJU_ID")]
        [ForeignKey(nameof(PessoaJuridica))]
        public int? IdPessoaJuridica { get; set; }

        [Column("PRT_ID")]
        [ForeignKey(nameof(Protocolo))]
        public int? IdProtocolo { get; set; }

        [Column("UND_ID")]
        [ForeignKey(nameof(UnidadeCadastradora))]
        public int? IdUnidadeCadastradora { get; set; }

        [Column("DLG_DS_MOTIVO")]
        public string Motivo { get; set; }

        [Column("DLG_ME_PARECER")]
        public string Parecer { get; set; }

        public virtual PessoaFisicaEntity PessoaFisica { get; set; }

        public virtual PessoaJuridicaEntity PessoaJuridica { get; set; }

        [Column("DLG_DS_PESSOA_RESPONSAVEL")]
        public string PessoaResponsavel { get; set; }

        public virtual ProtocoloEntity Protocolo { get; set; }

        [Column("DLG_ST")]
        public int? Status { get; set; }

        [Column("DLG_ST_REALIZADA")]
        public bool? StatusRealizada { get; set; }

        public virtual UnidadeCadastradoraEntity UnidadeCadastradora { get; set; }

        public DiligenciaEntity()
        {
            DiligeciaAtividade = new HashSet<DiligenciaAtividadesEntity>();
            DiligenciaAnexo = new HashSet<DiligenciaAnexosEntity>();
        }
    }
}