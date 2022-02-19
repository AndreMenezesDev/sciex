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


		public PagedItems<PlanoExportacaoDUEComplementoVM> ListarPaginado(PlanoExportacaoDUEComplementoVM objeto)
		{


			var pagedItems = _uowSciex.QueryStackSciex.PlanoExportacaoDue.ListarPaginadoGrafo(o => new PlanoExportacaoDUEComplementoVM()
			{
				Numero = o.Numero,
				CodigoPais = o.CodigoPais,
				DataAverbacao = o.DataAverbacao,
				ValorDolar = o.ValorDolar,
				Quantidade = o.Quantidade,
				IdPEProdutoPais = o.IdPEProdutoPais,
				IdDue = o.IdDue,


			}, o => o.IdPEProdutoPais == objeto.IdPEProdutoPais, objeto);
			if (pagedItems.Total > 0)
			{
				foreach (var item in pagedItems.Items)
				{
					string codigoPais = Convert.ToInt32(item.CodigoPais).ToString("D3");
					var pais = _uowSciex.QueryStackSciex.ViewPais.Selecionar(o => o.CodigoPais == codigoPais);
					item.DataAverbacaoFormatada = ((DateTime)item.DataAverbacao).ToShortDateString();
					item.DescricaoPais = pais.Descricao;

				}


			}

			return pagedItems;
		}
	}

}
