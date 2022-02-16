using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	public class ST_ParecerTecnicoEntity : BaseEntity
	{
		[Key]
		public virtual decimal? QT_PRODUTO_APROV { get; set; }
		public virtual decimal? VL_PRODUTO_APROV { get; set; }
		public virtual string PCO_NU { get; set; }
		public virtual string PCO_ANO { get; set; }

	}
}
