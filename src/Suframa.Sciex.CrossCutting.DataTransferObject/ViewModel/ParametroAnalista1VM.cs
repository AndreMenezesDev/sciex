using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class ParametroAnalista1VM : PagedOptions
	{
		public int? IdParametroAnalista { get; set; }
		public int IdAnalista { get; set; }
		public string CPF { get; set; }
		public string Nome { get; set; }
		public int StatusAnaliseVisual { get; set; }
		public DateTime? DataAnaliseVisualInicio { get; set; }
		public TimeSpan? HoraAnaliseVisualInicio { get; set; }
		public TimeSpan? HoraAnaliseVisualFim { get; set; }
		public int StatusAnaliseVisualBloqueio { get; set; }
		public DateTime? DataAnaliseVisualBloqueioInicio { get; set; }
		public TimeSpan? HoraAnaliseVisualBloqueioInicio { get; set; }
		public TimeSpan? HoraAnaliseVisualBloqueioFim { get; set; }
		public string DescricaoAnaliseVisualBloqueioFim { get; set; }
		public int StatusAnaliseLoteListagem { get; set; }
		public DateTime? DataAnaliseLoteListagemInicio { get; set; }
		public TimeSpan? HoraAnaliseLoteListagemInicio { get; set; }
		public TimeSpan? HoraAnaliseLoteListagemFim { get; set; }
		public int StatusAnaliseLoteListagemBloqueio { get; set; }
		public DateTime? DataAnaliseListagemLoteBloqueioInicio { get; set; }
		public TimeSpan? HoraAnaliseLoteListagemBloqueioInicio { get; set; }
		public TimeSpan? HoraAnaliseLoteListagemBloqueioFim { get; set; }
		public string DescricaoAnaliseLoteListagemBloqueioFim { get; set; }
		public int TipoSistema { get; set; }
		public AnalistaVM Analista { get; set; }

	}
}