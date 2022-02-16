using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_TIPO_INCENTIVO")]
    public partial class TipoIncentivoEntity : BaseEntity
    {
        [Required]
        [StringLength(20)]
        [Column("INC_DS")]
        public string Descricao { get; set; }

        [Required]
        [StringLength(200)]
        [Column("INC_DS_FUNDAMENTO")]
        public string DescricaoFundamento { get; set; }

        [Key]
        [Column("INC_ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdIncentivo { get; set; }

        public virtual ICollection<InscricaoCadastralEntity> InscricaoCadastral { get; set; }

        public TipoIncentivoEntity()
        {
            InscricaoCadastral = new HashSet<InscricaoCadastralEntity>();
        }
    }
}