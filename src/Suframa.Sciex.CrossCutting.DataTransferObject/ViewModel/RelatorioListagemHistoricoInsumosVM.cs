using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class RelatorioListagemHistoricoInsumosVM
	{
		public int InscricaoCadastral { get; set; }
		public string NomeEmpresa { get; set; }
		public int CodigoProduto { get; set; }		
		public string DescricaoModelo { get; set; }		
		public string DataImpressao { get; set; }
		public List<InsumosRelatorioListagemHistoricoVM> Insumos { get; set; }
	}

	public class InsumosRelatorioListagemHistoricoVM
	{
		public int CodigoInsumo { get; set; }
		public string DescricaoInsumo { get; set; }
		public string DescricaoTipoAlteracao { get; set; }
		public string SituacaoInsumo { get; set; }
		public string Justificativa { get; set; }
	}

}
