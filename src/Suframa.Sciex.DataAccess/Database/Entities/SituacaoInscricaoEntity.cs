using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_SITUACAO_INSCRICAO")]
    public partial class SituacaoInscricaoEntity : BaseEntity
    {
        [Column("STI_CO")]
        public int? Codigo { get; set; }

        [Column("STI_DS")]
        [StringLength(40)]
        public string Descricao { get; set; }

        [Key]
        [Column("STI_ID")]
        public int IdSituacaoInscricao { get; set; }

        public virtual ICollection<InscricaoCadastralEntity> InscricaoCadastral { get; set; }

        public virtual ICollection<MotivoSituacaoInscricaoEntity> MotivoSituacaoInscricao { get; set; }

        public SituacaoInscricaoEntity()
        {
            InscricaoCadastral = new HashSet<InscricaoCadastralEntity>();
            MotivoSituacaoInscricao = new HashSet<MotivoSituacaoInscricaoEntity>();
        }
    }
}