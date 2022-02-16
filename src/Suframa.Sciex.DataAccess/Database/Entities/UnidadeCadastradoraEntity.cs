using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_UNIDADE_CADASTRADORA")]
    public partial class UnidadeCadastradoraEntity : BaseEntity
    {
        public virtual ICollection<AgendaAtendimentoEntity> AgendaAtendimento { get; set; }

        public virtual ICollection<CalendarioAgendamentoEntity> CalendarioAgendamento { get; set; }

        [Column("UND_CO")]
        public int Codigo { get; set; }

        public virtual ICollection<CredenciamentoEntity> Credenciamento { get; set; }

        [Column("UND_DT_ALTERACAO")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? DataAlteracao { get; set; }

        [Column("UND_DT_INCLUSAO")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? DataInclusao { get; set; }

        [Required]
        [StringLength(50)]
        [Column("UND_DS")]
        public string Descricao { get; set; }

        public virtual ICollection<DiligenciaEntity> Diligencia { get; set; }

        [ForeignKey(nameof(Municipio))]
        [Column("MUN_ID")]
        public int IdMunicipio { get; set; }

        [Key]
        [Column("UND_ID")]
        public int IdUnidadeCadastradora { get; set; }

        public virtual MunicipioEntity Municipio { get; set; }

        public virtual ICollection<ParametroAnalistaEntity> ParametroAnalista { get; set; }

        public virtual ICollection<ParametroDistribuicaoAutomaticaEntity> ParametroDistribuicaoAutomatica { get; set; }

        public virtual ICollection<RequerimentoEntity> Requerimento { get; set; }

        public virtual ICollection<SeqInscricaoCadastralEntity> SeqInscricaoCadastral { get; set; }

        public virtual ICollection<UnidadeSecundariaEntity> UnidadeSecundaria { get; set; }

        public virtual ICollection<UsuarioInternoEntity> UsuarioInterno { get; set; }

        public UnidadeCadastradoraEntity()
        {
            AgendaAtendimento = new HashSet<AgendaAtendimentoEntity>();
            CalendarioAgendamento = new HashSet<CalendarioAgendamentoEntity>();
            Credenciamento = new HashSet<CredenciamentoEntity>();
            Diligencia = new HashSet<DiligenciaEntity>();
            ParametroAnalista = new HashSet<ParametroAnalistaEntity>();
            ParametroDistribuicaoAutomatica = new HashSet<ParametroDistribuicaoAutomaticaEntity>();
            Requerimento = new HashSet<RequerimentoEntity>();
            UnidadeSecundaria = new HashSet<UnidadeSecundariaEntity>();
            UsuarioInterno = new HashSet<UsuarioInternoEntity>();
            SeqInscricaoCadastral = new HashSet<SeqInscricaoCadastralEntity>();
        }
    }
}