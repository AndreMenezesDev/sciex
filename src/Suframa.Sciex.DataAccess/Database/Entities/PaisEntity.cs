using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("VW_SCIEX_PAIS")]
    public partial class PaisEntity : BaseEntity
    {
		[Key]
		[Column("PAI_ID")]
		public int IdPais { get; set; }

		[Required]
        [StringLength(50)]
        [Column("PAI_DS")]
        public string Descricao { get; set; }

		[Column("PAI_CO")]
		[StringLength(3)]
		public string CodigoPais { get; set; }

		public virtual ICollection<PessoaJuridicaSocioEntity> PessoaJuridicaSocio { get; set; }

        public PaisEntity()
        {
            PessoaJuridicaSocio = new HashSet<PessoaJuridicaSocioEntity>();
        }
    }
}