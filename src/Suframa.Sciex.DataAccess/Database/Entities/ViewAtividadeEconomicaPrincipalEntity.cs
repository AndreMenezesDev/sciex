using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
    [Table("[VW_SCIEX_ATIV_ECONOMICA_PRINCIPAL]")]
    public partial class ViewAtividadeEconomicaPrincipalEntity : BaseEntity
    {
		[Key]
		[Column("SET_ID")]
		public int IdSetor { get; set; }

		[Column("SBC_CO_CONCLA")]
		public string CodigoConcla { get; set; }

		[Column("SBC_DS")]		
		[StringLength(200)]
		public string Descricao { get; set; }
		
		[Column("ATV_TP")]
		public int TipoAtividade { get; set; }

		[Column("ATV_ST_ATUANTE")]
		public bool StatusAtividadeAtuante { get; set; }
		
		[Column("ATV_ST_ATUANTE_DS")]
		public string DescricaoStatusAtividadeAtuante { get; set; }

		[Column("SET_DS")]
		public string DescricaoSetor { get; set; }

		[Column("IMP_NU_CNPJ")]
		public string CNPJ { get; set; }


	}
}