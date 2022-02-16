using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class SolicitacaoLeInsumoVM : PagedOptions
	{
		public int IdSolicitacaoLeInsumo { get; set; }
		public long? IdEstruturaPropriaLe { get; set; }
		public int CodigoDestaque { get; set; }
		public string CodigoNCM { get; set; }
		public string TipoInsumo { get; set; }
		public decimal? CodigoUnidade { get; set; }
		public string DescricaoInsumo { get; set; }
		public decimal? ValorCoeficienteTecnico { get; set; }
		public int SituacaoInsumo { get; set; }
		public string SituacaoInsumoDesc { get; set; }
		public string CodigoPartNumber { get; set; }
		public string DescricaoEspecificacaoTecnica { get; set; }
		public int NumeroLinha { get; set; }

		//Campos Complementares
		public string DataValidacao { get; set; }
		public int QtdErros { get; set; }
		public string DescricaoUnidade { get; set; }

	}
}
