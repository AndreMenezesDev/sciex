using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class NcmExcecaoVM : PagedOptions
	{
		public int? IdNcmExcecao { get; set; }
		public string Codigo { get; set; }
		public string DescricaoNcm { get; set; }
		public byte Status { get; set; }
		public int CodigoMunicipio { get; set; }
		public string DescricaoMunicipio { get; set; }
		public int CodigoSetor { get; set; }
		public string DescricaoSetor { get; set; }
		public DateTime? DataInicioVigencia { get; set; }		
		public byte?[] RowVersion { get; set; }
		public string UF { get; set; }

		// Complemento de Classe
		public List<NcmExcecaoVM> ListaMunicipios { get; set; }
		public string MensagemErro { get; set; }
		public bool BuscarPorMunicipiosAssociados { get; set; }
		public string DataInicioVigenciaFormatado { get; set; }
		public string CodigoNCM { get; set; }
		public DateTime? DataVigenciaInicio { get; set; }
		public DateTime? DataVigenciaFim { get; set; }
		public string DescricaoMunicipioCodigo { get; set; }
	}
}