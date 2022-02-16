using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_AGENDA_ATENDIMENTO")]
    public class AgendaAtendimentoEntity : BaseEntity, IData
    {
        public virtual ICollection<CalendarioHoraEntity> CalendarioHora { get; set; }

        [StringLength(14)]
        [Column("AGD_CO_CPF_CNPJ")]
        public string CpfCnpj { get; set; }

        public virtual DadosSolicitanteEntity DadosSolicitante { get; set; }

        [Column("AGD_DT_AGENDAMENTO")]
        public DateTime DataAgendamento { get; set; }

        [Column("AGD_DT_ALTERACAO")]
        public DateTime DataAlteracao { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("AGD_DT_INCLUSAO")]
        public DateTime DataInclusao { get; set; }

        [Key]
        [Column("AGD_ID")]
        public int IdAgendaAtendimento { get; set; }

        [Column("SRV_ID")]
        [ForeignKey(nameof(Servico))]
        public int? IdServico { get; set; }

        [Column("DSO_ID")]
        [ForeignKey(nameof(DadosSolicitante))]
        public int? IdSolicitante { get; set; }

        [Column("UND_ID")]
        [ForeignKey(nameof(UnidadeCadastradora))]
        public int? IdUnidadeCadastradora { get; set; }

        [StringLength(100)]
        [Column("AGD_DS_NOME_RAZAO_SOCIAL")]
        public string NomeRazaoSocial { get; set; }

        public virtual ServicoEntity Servico { get; set; }

        [Column("AGD_TP")]
        public int Tipo { get; set; }

        public virtual UnidadeCadastradoraEntity UnidadeCadastradora { get; set; }

        public AgendaAtendimentoEntity()
        {
            CalendarioHora = new HashSet<CalendarioHoraEntity>();
        }
    }
}