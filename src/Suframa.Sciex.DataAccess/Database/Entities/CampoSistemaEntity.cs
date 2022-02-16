using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_CAMPO_SISTEMA")]
    public class CampoSistemaEntity : BaseEntity
    {
        [StringLength(50)]
        [Column("CAM_NM_CAMPO")]
        public string Campo { get; set; }

        [StringLength(50)]
        [Column("CAM_NM_CAMPO_OBJETO")]
        public string CampoObjeto { get; set; }

        [StringLength(50)]
        [Column("CAM_NM_CAMPO_TELA")]
        public string CampoTela { get; set; }

        [StringLength(50)]
        [Column("CAM_DS_CAMPO")]
        public string DescricaoCampo { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("CAM_ID")]
        public int IdCampoSistema { get; set; }

        [StringLength(50)]
        [Column("CAM_NM_SECAO")]
        public string Secao { get; set; }

        [StringLength(50)]
        [Column("CAM_NM_TABELA")]
        public string Tabela { get; set; }
    }
}