using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_TIPO_CONSULTA_PUBLICA")]
    public partial class TipoConsultaPublicaEntity : BaseEntity
    {
        public virtual ICollection<ConsultaPublicaEntity> ConsultaPublica { get; set; }

        [Required]
        [StringLength(200)]
        [Column("TCP_DS")]
        public string Descricao { get; set; }

        [Key]
        [Column("TCP_ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdTipoConsultaPublica { get; set; }

        [Column("TCP_ST")]
        public int Status { get; set; }

        [Required]
        [StringLength(20)]
        [Column("TCP_NM")]
        public string TipoConsulta { get; set; }

        public TipoConsultaPublicaEntity()
        {
            ConsultaPublica = new HashSet<ConsultaPublicaEntity>();
        }
    }
}