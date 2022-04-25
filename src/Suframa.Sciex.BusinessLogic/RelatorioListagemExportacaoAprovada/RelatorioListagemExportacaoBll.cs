using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.BusinessLogic
{
	public class RelatorioListagemExportacaoBll : IRelatorioListagemExportacaoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;

		public RelatorioListagemExportacaoBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
		} 

		public LEProdutoComplementoVM ListarRelatorioListagemExportacao(LEProdutoVM filtro)
		{
			var retorno = new LEProdutoComplementoVM();
			retorno.DataInicio = filtro.DataInicio != null ? new DateTime(filtro.DataInicio.Value.Year, filtro.DataInicio.Value.Month, filtro.DataInicio.Value.Day): filtro.DataInicio;
			retorno.DataFim = filtro.DataFim != null ? new DateTime(filtro.DataFim.Value.Year, filtro.DataFim.Value.Month, filtro.DataFim.Value.Day) : filtro.DataFim;

			if(filtro == null) { return null; }
			var listaLEProtudo = _uowSciex.QueryStackSciex.LEProduto.Listar(o => 
					(
					   filtro.InscricaoCadastral == 0 || o.InscricaoCadastral == filtro.InscricaoCadastral
					)
					&&
					(
					String.IsNullOrEmpty(filtro.RazaoSocial) || o.RazaoSocial.ToUpper().Contains(filtro.RazaoSocial.ToUpper())
					)
					&&
					(
						(filtro.DataInicio == null) || (o.DataAprovacao >= retorno.DataInicio && o.DataAprovacao <= retorno.DataFim)
					)
					);
			if (listaLEProtudo.Count == 0)
			{
				return null;
			}

			foreach (var lEProduto in listaLEProtudo)
			{
				var reg = new LEProdutoVM()
				{
					CodigoProduto = lEProduto.CodigoProduto,
					CodigoProdutoSuframa = lEProduto.CodigoProdutoSuframa,
					CodigoNCM = lEProduto.CodigoNCM,
					DescricaoModelo = lEProduto.DescricaoModelo,
				};
				retorno.listaLEProduto.Add(reg);
			}
			retorno.RazaoSocial = listaLEProtudo.FirstOrDefault().RazaoSocial;
			retorno.InscricaoCadastral = listaLEProtudo.FirstOrDefault().InscricaoCadastral;
			return retorno;
		}

	}
}
