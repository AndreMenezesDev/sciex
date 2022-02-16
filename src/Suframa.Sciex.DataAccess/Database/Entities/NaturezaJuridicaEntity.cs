using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_NATUREZA_JURIDICA")]
    public partial class NaturezaJuridicaEntity : BaseEntity
    {
        [Column("NJU_CO", TypeName = "numeric")]
        public decimal Codigo { get; set; }

        [Column("NJU_DT_ALTERACAO")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? DataAlteracao { get; set; }

        [Column("NJU_DT_INCLUSAO")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? DataInclusao { get; set; }

        [Required]
        [StringLength(500)]
        [Column("NJU_DS")]
        public string Descricao { get; set; }

        [Column("NGR_ID")]
        public int IdNaturezaGrupo { get; set; }

        [Key]
        [Column("NJU_ID")]
        public int IdNaturezaJuridica { get; set; }

        public virtual NaturezaGrupoEntity NaturezaGrupo { get; set; }

        public virtual ICollection<NaturezaQualificacaoEntity> NaturezaQualificacao { get; set; }

        public virtual ICollection<PessoaJuridicaSocioEntity> PessoaJuridicaSocio { get; set; }

        [Column("NJU_ST")]
        public bool Status { get; set; }

        [Column("NJU_ST_QUADRO_SOCIO")]
        public bool StatusQuadroSocial { get; set; }

        public NaturezaJuridicaEntity()
        {
            NaturezaQualificacao = new HashSet<NaturezaQualificacaoEntity>();
            PessoaJuridicaSocio = new HashSet<PessoaJuridicaSocioEntity>();
        }
    }
}