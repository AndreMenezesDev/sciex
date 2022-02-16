using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_CONSULTA_PUBLICA")]
    public partial class ConsultaPublicaEntity : BaseEntity
    {
        public virtual ArquivoEntity Arquivo { get; set; }

        [Column("CPU_DH_RESTRICAO")]
        public DateTime? DataRestricao { get; set; }

        [Column("ARQ_ID")]
        [ForeignKey(nameof(Arquivo))]
        public int? IdArquivo { get; set; }

        [Key]
        [Column("CPU_ID")]
        public int IdConsultaPublica { get; set; }

        [Column("PFI_ID")]
        [ForeignKey(nameof(PessoaFisica))]
        public int? IdPessoaFisica { get; set; }

        [Column("PJU_ID")]
        [ForeignKey(nameof(PessoaJuridica))]
        public int? IdPessoaJuridica { get; set; }

        [Column("ADM_ID")]
        [ForeignKey(nameof(PessoaJuridicaAdministrador))]
        public int? IdPessoaJuridicaAdministrador { get; set; }

        [Column("SOC_ID")]
        [ForeignKey(nameof(PessoaJuridicaSocio))]
        public int? IdPessoaJuridicaSocio { get; set; }

        [Column("PRT_ID")]
        [ForeignKey(nameof(Protocolo))]
        public int? IdProtocolo { get; set; }

        [Column("TCP_ID")]
        [ForeignKey(nameof(TipoConsultaPublica))]
        public int? IdTipoConsultaPublica { get; set; }

        [StringLength(100)]
        [Column("CPU_NM_CONSULTA")]
        public string NomeConsulta { get; set; }

        public virtual PessoaFisicaEntity PessoaFisica { get; set; }

        public virtual PessoaJuridicaEntity PessoaJuridica { get; set; }

        public virtual PessoaJuridicaAdministradorEntity PessoaJuridicaAdministrador { get; set; }

        public virtual PessoaJuridicaSocioEntity PessoaJuridicaSocio { get; set; }

        public virtual ProtocoloEntity Protocolo { get; set; }

        [Column("CPU_ST_RESTRICAO")]
        public bool? StatusRestricao { get; set; }

        public virtual TipoConsultaPublicaEntity TipoConsultaPublica { get; set; }

        [Column("CPU_TP_ORIGEM")]
        public int TipoOrigem { get; set; }
    }
}