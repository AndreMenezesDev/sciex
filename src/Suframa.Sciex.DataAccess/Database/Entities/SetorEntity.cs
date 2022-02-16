using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_SETOR")]
    public partial class SetorEntity : BaseEntity
    {
        [Column("SET_CO")]
        public int Codigo { get; set; }

        [Column("SET_DT_ALTERACAO")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? DataAlteracao { get; set; }

        [Column("SET_DT_INCLUSAO")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? DataInclusao { get; set; }

        [Required]
        [StringLength(200)]
        [Column("SET_DS")]
        public string Descricao { get; set; }

        [Required]
        [StringLength(500)]
        [Column("SET_DS_OBSERVACAO")]
        public string DescricaoObservacao { get; set; }

        [Key]
        [Column("SET_ID")]
        public int IdSetor { get; set; }

        public virtual ICollection<SetorAtividadeEntity> SetorAtividade { get; set; }

        [Column("SET_ST")]
        public bool Status { get; set; }

        [Column("SET_TP")]
        public int Tipo { get; set; }

    }
}