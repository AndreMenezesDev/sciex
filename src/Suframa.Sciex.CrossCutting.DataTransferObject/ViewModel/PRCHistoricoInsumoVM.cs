using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class PRCHistoricoInsumoVM : PagedOptions
	{
		#region CampoTabela
		public int? IdPRCHistoricoInsumo { get; set; }
		public int? IdProduto { get; set; }
		public int? IdSolicitacaoAlteracao { get; set; }
		public DateTime? DataHistorico { get; set; }
		public string NomeResponsavel { get; set; }
		public string DescricaoInsumo { get; set; }
		public string DescricaoEmpresa { get; set; }
		public string DescricaoProcesso { get; set; }
		public string DescricaoSolicitacao { get; set; }
		public string DescricaoProduto { get; set; }
		public string CodigoInsumo { get; set; }
		#endregion

		#region Complemento
		public List<PRCHistoricoDetalheInsumoVM> ListaAlteracao { get; set; }
		public List<PRCHistoricoDetalheInsumoVM> ListaValorAfetado { get; set; }
		public List<PRCHistoricoDetalheInsumoVM> ListaAcrescimo { get; set; }
		public List<PRCHistoricoDetalheInsumoVM> ListaDescrescimo { get; set; }
		public bool IsInsumo { get; set; }
		public bool IsProduto { get; set; }
		public int Id { get; set; }
		public string DataHistoricoFormatada { get; set; }
		public int ContadorListaHistorico { get; set; }
		#endregion
	}
}
