using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class PliFornecedorFabricanteVM : PagedOptions
	{

		public long? IdPliMercadoria { get; set; }
		public string DescricaoFornecedor { get; set; }
		public string DescricaoLogradouroFornecedor { get; set; }
		public string NumeroFornecedor { get; set; }
		public string DescricaoComplementoFornecedor { get; set; }
		public string DescricaoCidadeFornecedor { get; set; }
		public string DescricaoEstadoFornecedor { get; set; }
		public string CodigoPaisFornecedor { get; set; }
		public string DescricaoPaisFornecedor { get; set; }
		public byte? CodigoAusenciaFabricante { get; set; }
		public string DescricaoFabricante { get; set; }
		public string DescricaoLogradouroFabricante { get; set; }
		public string NumeroFabricante { get; set; }
		public string DescricaoComplementoFabricante { get; set; }
		public string DescricaoCidadeFabricante { get; set; }
		public string DescricaoEstadoFabricante { get; set; }
		public string CodigoPaisFabricante { get; set; }
		public string DescricaoPaisFabricante { get; set; }

		//Varíavel de apoio
		public string CodigoDescricaoPaisFornecedorConcatenado { get; set; }
		public string CodigoDescricaoPaisFabricanteConcatenado { get; set; }
	}

}
