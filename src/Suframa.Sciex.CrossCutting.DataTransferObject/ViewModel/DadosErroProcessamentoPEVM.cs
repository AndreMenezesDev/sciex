using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class DadosErroProcessamentoPEVM : PagedOptions
	{
		public PagedItems<ErroProcessamentoVM> listaErroProcessamentoPaginada { get; set; }
		public EstruturaPropriaPEVM EstruturaPropriaPE { get; set; }
	}
}
