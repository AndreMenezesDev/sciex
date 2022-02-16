using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ControleImportacaoVM : PagedOptions
	{
		public int? IdControleImportacao { get; set; }
		public int IdCodigoConta { get; set; }
		public int IdCodigoUtilizacao { get; set; }
		public byte Status { get; set; }
		public int IdPliAplicacao { get; set; }
		public int CodigoSetor { get; set; }
		public string DescricaoSetor { get; set; }
		public byte[] RowVersion { get; set; }

		public short CodigodaConta { get; set; }
		public string DescricaoCodigoConta { get; set; }
		public string DescricaoCodigoUtilizacao { get; set; }
		public string DescricaoPliAplicacao { get; set; }
		public string MensagemErro { get; set; }
		public int isEditStatus { get; set; }
		public byte Ativar { get; set; }
		public string MensagemErroVinculo { get; set; }



	}
}
