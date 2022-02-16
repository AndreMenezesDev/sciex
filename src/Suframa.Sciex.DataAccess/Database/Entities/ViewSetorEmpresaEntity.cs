using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("VW_SCIEX_SETOR_EMPRESA")]
    public partial class ViewSetorEmpresaEntity : BaseEntity
    {
		[Key]
		[Column("SET_CO")]
		public int Codigo { get; set; }

		[Column("SET_ID")]
		public int IdSetor { get; set; }
			
		[Column("SET_DS")]
		public string Descricao { get; set; }

		[Column("IMP_NU_CNPJ")]
		public string Cnpj { get; set; }
    }
}