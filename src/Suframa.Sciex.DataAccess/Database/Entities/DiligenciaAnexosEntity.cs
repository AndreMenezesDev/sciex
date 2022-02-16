using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_DILIGENCIA_ANEXOS")]
    public class DiligenciaAnexosEntity : BaseEntity
    {
        public virtual ArquivoEntity Arquivo { get; set; }

        public virtual DiligenciaEntity Diligencia { get; set; }

        [Column("ARQ_ID")]
        [ForeignKey(nameof(Arquivo))]
        public int IdArquivo { get; set; }

        [Column("DLG_ID")]
        [ForeignKey(nameof(Diligencia))]
        public int IdDiligencia { get; set; }

        [Key]
        [Column("DAN_ID")]
        public int IdDiligenciaAnexo { get; set; }
    }
}