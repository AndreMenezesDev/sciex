using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class DadosErroProcessamentoVM : PagedOptions
	{
		public PagedItems<ErroProcessamentoVM> listaErroProcessamentoPaginada { get; set; }
		public EstruturaPropriaPLIGrafoVM EstruturaPropriaLE { get; set; }
		public SolicitacaoLeInsumoVM SolicitacaoLeInsumo { get; set; }
	}
}
