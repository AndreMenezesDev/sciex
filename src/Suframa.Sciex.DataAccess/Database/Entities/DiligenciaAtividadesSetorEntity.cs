using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_DILIGENCIA_ATIVIDADES_SETOR")]
    public partial class DiligenciaAtividadesSetorEntity : BaseEntity
    {
        [Column("DAS_CO_SETOR")]
        public int? Codigo { get; set; }

        public virtual DiligenciaAtividadesEntity DiligenciaAtividade { get; set; }

        [Column("DGA_ID")]
        [ForeignKey(nameof(DiligenciaAtividade))]
        public int IdDiligenciaAtividade { get; set; }

        [Key]
        [Column("DAS_ID")]
        public int IdDiligenciaAtividadeSetor { get; set; }

        [Column("DAS_DS_SETOR")]
        public string Setor { get; set; }
    }
}