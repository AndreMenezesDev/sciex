using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class NcmVM : PagedOptions
	{
		public int? IdNcm { get; set; }
		public string Descricao { get; set; }
		public short Status { get; set; }
		public string CodigoNCM { get; set; }
		public byte[] RowVersion { get; set; }
		public byte? IsAmazoniaOcidental { get; set; }
		public string IsAmazoniaOcidentalString { get; set; }
		public string Justificativa { get; set; }

		//complemento de classe
		public int? Id { get; set; }
		public string MensagemErro { get; set; }
		public int isEditStatus { get; set; }
		public string CodigoMascaradoNCM { get; set; }
		public bool CheckboxSomenteAmazoniaOcidental { get; set; }
		public string DescricaoAlteracoes { get; set; }
		public int AcaoTela { get; set; }

	}
}