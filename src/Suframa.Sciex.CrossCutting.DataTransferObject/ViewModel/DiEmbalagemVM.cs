using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class DiEmbalagemVM : PagedOptions
	{
		public long Id { get; set; }
		public int CodigoTipoEmbalagem { get; set; }
		public int QuantidadeVolumeCarga { get; set; }
		public long IdDi { get; set; }

		#region Descriao Embalagem
		public string Descricao { get; set; }
		#endregion

	}
}
