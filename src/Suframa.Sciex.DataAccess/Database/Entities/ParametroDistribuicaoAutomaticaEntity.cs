using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_PARAMETRO_DISTRIBUICAO_AUTOMATICA")]
    public partial class ParametroDistribuicaoAutomaticaEntity : BaseEntity
    {
        [Key]
        [Column("PDA_ID")]
        public int IdParametroDistribuicaoAutomatica { get; set; }

        [ForeignKey(nameof(UnidadeCadastradora))]
        [Column("UND_ID")]
        public int IdUnidadeCadastradora { get; set; }

        [Column("PDA_ST_DIST_AUTOMATICA")]
        public bool IsAtivo { get; set; }

        public virtual UnidadeCadastradoraEntity UnidadeCadastradora { get; set; }
    }
}