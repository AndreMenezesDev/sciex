using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("CADSUF_UNIDADE_SECUNDARIA")]
    public partial class UnidadeSecundariaEntity : BaseEntity
    {
        [Column("MUN_ID")]
        public int IdMunicipio { get; set; }

        [Column("UND_ID")]
        public int IdUnidadeCadastradora { get; set; }

        [Key]
        [Column("USE_ID")]
        public int IdUnidadeSecundaria { get; set; }

        public virtual MunicipioEntity Municipio { get; set; }

        public virtual UnidadeCadastradoraEntity UnidadeCadastradora { get; set; }
    }
}