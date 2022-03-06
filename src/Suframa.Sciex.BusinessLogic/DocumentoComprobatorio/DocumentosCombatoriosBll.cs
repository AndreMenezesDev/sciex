using System;
using System.Collections.Generic;
using System.Linq;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.BusinessLogic.Pss;

namespace Suframa.Sciex.BusinessLogic
{
	public class DocumentosComprobatoriosBll : IDocumentosComprobatoriosBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUsuarioPssBll _usuarioPssBll;

		public DocumentosComprobatoriosBll(IUnitOfWorkSciex uowSciex, IUsuarioPssBll usuarioPssBll)
		{
			_uowSciex = uowSciex;
			_usuarioPssBll = usuarioPssBll;
		}


		public PagedItems<PRCDueComplementoVM> ListarPaginado(PlanoExportacaoDUEComplementoVM objeto)
		{
			var listaIDProdutoPais = _uowSciex.QueryStackSciex.PRCProdutoPais.Listar(x => x.IdPrcProduto == objeto.IdPRCProduto).Select(x => (int?)x.IdProdutoPais).ToList();

			string sort = null;
			if ("DescricaoPais".Equals(objeto.Sort))
			{
				sort = "DescricaoPais";
				objeto.Sort = null;
			}
			var pagedItems = _uowSciex.QueryStackSciex.PRCDue.ListarPaginadoGrafo(o => new PRCDueComplementoVM()
			{
				Numero = o.Numero,
				CodigoPais = o.CodigoPais,
				DataAverbacao = o.DataAverbacao,
				ValorDolar = o.ValorDolar,
				Quantidade = o.Quantidade,
				IdPRCProdutoPais = o.IdPRCProdutoPais,
				IdDue = o.IdDue
			}, o => listaIDProdutoPais.Contains(o.IdPRCProdutoPais), objeto);

			if (pagedItems.Total > 0)
			{
				foreach (var item in pagedItems.Items)
				{
					string codigoPais = Convert.ToInt32(item.CodigoPais).ToString("D3");
					var pais = _uowSciex.QueryStackSciex.ViewPais.Selecionar(o => o.CodigoPais == codigoPais);
					item.DataAverbacaoFormatada = ((DateTime)item.DataAverbacao).ToShortDateString();
					item.DescricaoPais = pais.Descricao;

				}
				if (sort != null)
				{
					if ("DescricaoPais".Equals(objeto.Sort))
					{
						if (!objeto.Reverse)
						{
							pagedItems.Items = pagedItems.Items.OrderBy(x => x.DescricaoPais).ToList();
						}
						else
						{
							pagedItems.Items = pagedItems.Items.OrderByDescending(x => x.DescricaoPais).ToList();
						}
					}
				}


			}

			return pagedItems;
		}
	}

}
