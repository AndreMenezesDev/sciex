using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class SolicitacoesAlteracaoVM
	{
		public int IdMoeda { get; set; }
		public string DescricaoMoedaDE { get; set; }
		public string DescricaoMoedaPARA { get; set; }

		public int codigoPaisDe { get; set; }
		public int codigoPaisPara { get; set; }
		public bool flagExisteRegistroDuplicado { get; set; }
		public int IdProcesso { get; set; }
		public int IdProduto { get; set; }
		public decimal QuantidadeMaxima { get; set; }
		public decimal ValorParidade { get; set; }

		public PRCInsumoTableColunsVM PRCInsumoDE { get; set; }
		public PRCDetalheInsumoTableColunsVM PRCDetalheInsumoDE { get; set; }		
		public PRCInsumoTableColunsVM PRCInsumoPara { get; set; }
		public PRCDetalheInsumoTableColunsVM PRCDetalheInsumoPara { get; set; }		
		public QuantidadeCoefTecnicoVM QuantidadeCoefTecnicoPara { get; set; }	
		public ValorUnitarioVM ValorUnitarioAlteracaoDe { get; set; }	
		public ValorUnitarioVM ValorUnitarioAlteracaoPara { get; set; }
		public ValorFreteVM ValorFreteAlteracaoDe { get; set; }
		public ValorFreteVM ValorFreteAlteracaoPara { get; set; }
		public CalcularMoedaVM CalcularMoedaPara { get; set; }
	}
}
