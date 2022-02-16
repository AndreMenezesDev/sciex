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
	public class LEInsumoVM : PagedOptions
	{
		public int IdLe { get; set; }
		public int IdLeInsumo { get; set; }
		public int CodigoProduto { get; set; }
		public int CodigoInsumo { get; set; }
		public string CodigoNCM { get; set; }
		public string CodigoNCMFormatado { get; set; }
		public int IdCodigoNCM { get; set; }
		public string TipoInsumo { get; set; }
		public int TipoInsumoAlteracao { get; set; }
		public string DescricaoTipoInsumo { get; set; }
		public string DescricaoTipoInsumoAlteracao { get; set; }
		public int CodigoUnidadeMedida { get; set; }
		public string DescricaoUnidadeMedida { get; set; }
		public int CodigoDetalhe { get; set; }
		public int IdCodigoDetalhe { get; set; }
		public string DescricaoInsumo { get; set; }
		public decimal ValorCoeficienteTecnico { get; set; }
		public int? SituacaoInsumo { get; set; }
		public string DescricaoSituacaoInsumo { get; set; }
		public string CodigoPartNumber { get; set; }
		public string DescricaoEspecTecnica { get; set; }
		

		/* complemento da classe */
		public string DescricaoCodigoProdutoSuframa { get; set; }
		public short CodigoProdutoSuframa { get; set; }
		public string Path { get; set; }
		public string Descricao { get; set; }
		public int Id { get; set; }
		public int IdProduto { get; set; }
		public string Mensagem { get; set; }
		public string MensagemErro { get; set; }
		public string DescricaoErro { get; set; }
		public bool IsAlteracao { get; set; }
		public int IdProcesso { get; set; }
		public string DescricaoUnidade { get; set; }
		public List<LEInsumoErroVM> listaInsumoErro { get; set; }
		public LEInsumoErroVM UltimoInsumoErro { get; set; }

		public bool Checkbox { get; set; } = false;
		public int? IdPEProduto { get; set; }
		public List<LEInsumoVM> ListaInsumosSelecionados { get; set; }

		// Modal Transferencia de Saldo de Insumo
		public bool ExisteNoProcesso { get; set; }
		public bool ExisteCopia { get; set; } = false;
	}

}
