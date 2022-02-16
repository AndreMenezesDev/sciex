using FluentValidation;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Suframa.Sciex.BusinessLogic
{
	public class PliMercadoriaBll : IPliMercadoriaBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;
		private readonly IPliDetalheMercadoriaBll _iPliDetalheMercadoriaBll;
		private readonly IPliProcessoAnuenteBll _iPliProcessoAnuenteBll;
		private readonly IPliBll _iPliBll;

		public PliMercadoriaBll(IUnitOfWorkSciex uowSciex,
			IPliDetalheMercadoriaBll iPliDetalheMercadoriaBll,
			IPliProcessoAnuenteBll iPliProcessoAnuenteBll,
			IPliBll iPliBll)
		{
			_uowSciex = uowSciex;
			_iPliDetalheMercadoriaBll = iPliDetalheMercadoriaBll;
			_iPliProcessoAnuenteBll = iPliProcessoAnuenteBll;
			_iPliBll = iPliBll;
			_validation = new Validation();
		}

		public IEnumerable<PliMercadoriaVM> Listar(PliMercadoriaVM pliMercadoriaVM)
		{

			var pliMercadoria = _uowSciex.QueryStackSciex.PliMercadoria.Listar(o =>
				o.IdPLI == pliMercadoriaVM.IdPLI && pliMercadoriaVM.IdPliProduto == null || o.IdPliProduto == pliMercadoriaVM.IdPliProduto);
			return AutoMapper.Mapper.Map<IEnumerable<PliMercadoriaVM>>(pliMercadoria);
		}

		public IEnumerable<object> Listar()
		{
			return _uowSciex.QueryStackSciex.PliMercadoria
				.Listar()
				.OrderBy(o => o.DescricaoInformacaoComplementar)
				.Select(
					s => new
					{
						id = s.IdPliMercadoria,
						text = s.IdPliMercadoria + " - " + s.DescricaoInformacaoComplementar
					});
		}

		public PagedItems<PliMercadoriaVM> ListarPaginado(PliMercadoriaVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<PliMercadoriaVM>(); }

			PagedItems<PliMercadoriaVM> pliMercadoria = null;

			pliMercadoria = _uowSciex.QueryStackSciex.PliMercadoria.ListarPaginado<PliMercadoriaVM>(o =>
					(
						(
							pagedFilter.IdPLI == -1 || o.IdPLI == pagedFilter.IdPLI
						) &&
						(
							pagedFilter.IdPliProduto == -1 || o.IdPliProduto == pagedFilter.IdPliProduto
						) &&
						(
							string.IsNullOrEmpty(pagedFilter.CodigoNCMMercadoria) ||
							o.CodigoNCMMercadoria.Contains(pagedFilter.CodigoNCMMercadoria)
						) &&
						(
							string.IsNullOrEmpty(pagedFilter.DescricaoNCMMercadoria) ||
							o.DescricaoNCMMercadoria.Contains(pagedFilter.DescricaoNCMMercadoria)
						)
					),
					pagedFilter);

			if (pliMercadoria != null)
			{
				foreach (var item in pliMercadoria.Items)
					item.IdPLIAplicacao = pagedFilter.IdPLIAplicacao;
			}

			return pliMercadoria;
		}

		public PliMercadoriaVM RegrasSalvar(PliMercadoriaVM pliMercadoria)
		{
			if (pliMercadoria == null) { return null; }

			if (pliMercadoria.AplicarParametros)
			{
				if (pliMercadoria.IdPliMercadoria != null)
				{
					int idParametro = pliMercadoria.IdParametro;
					bool confirmaAplicacao = pliMercadoria.ConfirmacaoClienteParametro;
					pliMercadoria = Selecionar(pliMercadoria.IdPliMercadoria);
					//verificar se ja existe mercadoria com o fornecedor
					if (!confirmaAplicacao) // verifica se o cliente confirma a aplicação dos parametros
					{
						if (VerificarMercadoriaFornecedor(pliMercadoria.IdPLI, pliMercadoria.IdPliProduto, pliMercadoria.CodigoNCMMercadoria, pliMercadoria.IdFornecedor, pliMercadoria.IdPliMercadoria, true, idParametro))
						{
							pliMercadoria.MensagemErro = "Operação não realizada. Fornecedor já existente para a mesma mercadoria.";
							return pliMercadoria;
						}
					}
					pliMercadoria.IdParametro = idParametro;
					pliMercadoria.ConfirmacaoClienteParametro = confirmaAplicacao;
				}

				if (AplicarParametros(pliMercadoria, pliMercadoria.ConfirmacaoClienteParametro))
				{
					pliMercadoria.ParametroAplicado = true;
					return pliMercadoria;
				}
				else
				{
					pliMercadoria.MensagemErro = "Erro ao aplicar parametros no PLI.";
					return pliMercadoria;
				}
			}

			if (pliMercadoria.IdPliMercadoria == null)
			{
				if (QuantidadePLIMercadoriaDoPLI(pliMercadoria.IdPLI) == 400)
				{
					pliMercadoria.MensagemErro = "Um PLI não pode ter mais de 400 mercadorias.";
					return pliMercadoria;
				}
			}

			if (pliMercadoria.IdPliMercadoria != null)
			{
				if (pliMercadoria.TipoFornecedor == 1 || pliMercadoria.TipoFornecedor == 2)
				{
					if (VerificarMercadoriaFornecedor(pliMercadoria.IdPLI, pliMercadoria.IdPliProduto, pliMercadoria.CodigoNCMMercadoria, pliMercadoria.IdFornecedor, pliMercadoria.IdPliMercadoria, false, 0))
					{
						pliMercadoria.MensagemErro = "Operação não realizada. Fornecedor já existente para a mesma mercadoria.";
						return pliMercadoria;
					}
				}
			}
			// Salva PliMercadoria	

			var pliMercadoriaEntity = AutoMapper.Mapper.Map<PliMercadoriaEntity>(pliMercadoria);
			if (pliMercadoriaEntity == null) { return null; }

			if (pliMercadoria.IdPliMercadoria.HasValue)
			{
				pliMercadoriaEntity = _uowSciex.QueryStackSciex.PliMercadoria.Selecionar(x => x.IdPliMercadoria == pliMercadoria.IdPliMercadoria);
				byte[] a = pliMercadoriaEntity.RowVersion;
				pliMercadoriaEntity = AutoMapper.Mapper.Map(pliMercadoria, pliMercadoriaEntity);
				pliMercadoriaEntity.RowVersion = a;

				if (pliMercadoriaEntity.TipoFornecedor == 0)
				{
					pliMercadoriaEntity.IdFornecedor = null;
					pliMercadoriaEntity.IdFabricante = null;
					pliMercadoriaEntity.CodigoPaisOrigemFabricante = null;
				}
				else if (pliMercadoriaEntity.TipoFornecedor == 1)
				{
					pliMercadoriaEntity.IdFabricante = null;
					pliMercadoriaEntity.CodigoPaisOrigemFabricante = null;
				}
				else if (pliMercadoriaEntity.TipoFornecedor == 2)
				{
					pliMercadoriaEntity.CodigoPaisOrigemFabricante = null;
				}
				else
				{
					pliMercadoriaEntity.IdFornecedor = null;
					pliMercadoriaEntity.IdFabricante = null;
				}

				decimal valorTotalVenda = 0;

				if (pliMercadoria.ValorTotalCondicaoVenda.HasValue && pliMercadoria.ValorTotalCondicaoVenda.Value > (decimal)999999999999999.99)
				{
					pliMercadoria.MensagemErro = "Operação não realizada. Valor da condição de venda ultrapassa o limite de 15 inteiros e 2 decimais";
					return pliMercadoria;
				}

				if (pliMercadoria.ListaPliDetalheMercadoriaVM != null && pliMercadoria.ListaPliDetalheMercadoriaVM.Count > 0)
				{
					foreach (var item in pliMercadoria.ListaPliDetalheMercadoriaVM)
					{
						if (item.ValorCondicaoVenda.HasValue && item.ValorCondicaoVenda.Value > (decimal)99999999.999999999999)
						{
							pliMercadoria.MensagemErro = "Operação não realizada. Valor da condição de venda ultrapassa o limite de 8 inteiros e 12 decimais";
							return pliMercadoria;
						}
					}

					foreach (var item in pliMercadoria.ListaPliDetalheMercadoriaVM)
					{
						if (item.Excluir)
						{
							if (item.IdPliDetalheMercadoria.HasValue)
								_iPliDetalheMercadoriaBll.Deletar(item.IdPliDetalheMercadoria.Value);
						}
						else
						{
							item.QuantidadeComercializada = Convert.ToDecimal(item.QuantidadeComercializadaFormatada != null && item.QuantidadeComercializadaFormatada.Length > 0 ? item.QuantidadeComercializadaFormatada.Replace(".", "") : "0");
							item.ValorUnitarioCondicaoVenda = Convert.ToDecimal(item.ValorUnitarioCondicaoVendaFormatada != null && item.ValorUnitarioCondicaoVendaFormatada.Length > 0 ? item.ValorUnitarioCondicaoVendaFormatada.Replace(".", "") : "0");

							valorTotalVenda = valorTotalVenda + (item.ValorCondicaoVenda.HasValue ? item.ValorCondicaoVenda.Value : 0);

							item.IdPliMercadoria = pliMercadoriaEntity.IdPliMercadoria;
							_iPliDetalheMercadoriaBll.Salvar(item);
						}
					}
				}

				pliMercadoriaEntity.ValorTotalCondicaoVenda = valorTotalVenda;

				if (pliMercadoria.ListaPliProcessoAnuenteVM != null && pliMercadoria.ListaPliProcessoAnuenteVM.Count > 0)
				{
					foreach (var item in pliMercadoria.ListaPliProcessoAnuenteVM)
					{
						if (item.Excluir)
						{
							if (item.IdPliProcessoAnuente.HasValue)
								_iPliProcessoAnuenteBll.Deletar(item.IdPliProcessoAnuente.Value);
						}
						else
						{
							item.IdPliMercadoria = pliMercadoriaEntity.IdPliMercadoria;
							_iPliProcessoAnuenteBll.Salvar(item);
						}

					}
				}
			}

			if(pliMercadoria.IdPLIAplicacao == 1 || pliMercadoria.IdPLIAplicacao == 3 || pliMercadoria.IdPLIAplicacao == 4)
			{
				var viewProdutoEmpresaMercadoria = _uowSciex.QueryStackSciex.Ncm.Selecionar(o => o.IdNcm == pliMercadoria.IdMercadoria);
				if(viewProdutoEmpresaMercadoria != null)
				{
					pliMercadoriaEntity.CodigoNCMMercadoria = viewProdutoEmpresaMercadoria.CodigoNCM;
					pliMercadoriaEntity.DescricaoNCMMercadoria = viewProdutoEmpresaMercadoria.Descricao;
				}
			}
			else if(pliMercadoria.IdPLIAplicacao == 2)
			{
				var viewProdutoEmpresaMercadoriaVM = ConsultaMercadoria(pliMercadoria.IdMercadoria);

				if (viewProdutoEmpresaMercadoriaVM != null)
				{
					pliMercadoriaEntity.CodigoNCMMercadoria = viewProdutoEmpresaMercadoriaVM.CodigoNCMMercadoria;
					pliMercadoriaEntity.DescricaoNCMMercadoria = viewProdutoEmpresaMercadoriaVM.Descricao;

					var pliMercadoriaValida = _uowSciex.QueryStackSciex.PliMercadoria
						.Listar(o => o.IdPLI == pliMercadoria.IdPLI && o.CodigoNCMMercadoria == viewProdutoEmpresaMercadoriaVM.CodigoNCMMercadoria && o.CodigoProduto == viewProdutoEmpresaMercadoriaVM.CodigoProdutoMercadoria);
				}
			}
			

			pliMercadoriaEntity.PesoLiquido = Convert.ToDecimal((pliMercadoria.PesoLiquidoString != null && pliMercadoria.PesoLiquidoString.Length > 0 ? pliMercadoria.PesoLiquidoString.Replace(".", "") : "0"));
			pliMercadoriaEntity.QuantidadeUnidadeMedidaEstatistica =
				Convert.ToDecimal(pliMercadoria.QuantidadeEstatisticaString != null && pliMercadoria.QuantidadeEstatisticaString.Length > 0 ? pliMercadoria.QuantidadeEstatisticaString.Replace(".", "") : "0");

			_uowSciex.CommandStackSciex.PliMercadoria.Salvar(pliMercadoriaEntity);
			_uowSciex.CommandStackSciex.Save();


			var pli = AutoMapper.Mapper.Map<PliVM>(_uowSciex.QueryStackSciex.Pli.Selecionar(o => o.IdPLI == pliMercadoria.IdPLI));
			if (pliMercadoriaEntity.ValorTotalCondicaoVenda != null && pliMercadoria.ValorTotalCondicaoVenda > 0)
			{
				if (pli != null)
				{
					pli.ValorTotalCondicaoVenda = pliMercadoriaEntity.ValorTotalCondicaoVenda;
					_iPliBll.Salvar(pli);
				}
			}

			//salvar fabricante e/ou fornecedor
			if (pli != null)
			{
				PliFornecedorFabricanteEntity pliFornecedorFabricante = _uowSciex.QueryStackSciex.PliFornecedorFabricante.Selecionar(o => o.IdPliMercadoria == pliMercadoria.IdPliMercadoria);

				if (pliFornecedorFabricante != null)
				{
					pliFornecedorFabricante.CodigoAusenciaFabricante = pliMercadoria.DadosFabricanteFornecedor.CodigoAusenciaFabricante;

					if (pliMercadoria.TipoFornecedor == 0)
					{
						pliFornecedorFabricante.CodigoPaisFornecedor = "";
						pliFornecedorFabricante.DescricaoCidadeFornecedor = "";
						pliFornecedorFabricante.DescricaoComplementoFornecedor = "";
						pliFornecedorFabricante.DescricaoEstadoFornecedor = "";
						pliFornecedorFabricante.DescricaoFornecedor = "";
						pliFornecedorFabricante.DescricaoLogradouroFornecedor = "";
						pliFornecedorFabricante.DescricaoPaisFornecedor = "";
						pliFornecedorFabricante.NumeroFornecedor = "";

						pliFornecedorFabricante.CodigoPaisFabricante = "";
						pliFornecedorFabricante.DescricaoCidadeFabricante = "";
						pliFornecedorFabricante.DescricaoComplementoFabricante = "";
						pliFornecedorFabricante.DescricaoEstadoFabricante = "";
						pliFornecedorFabricante.DescricaoFabricante = "";
						pliFornecedorFabricante.DescricaoLogradouroFabricante = "";
						pliFornecedorFabricante.DescricaoPaisFabricante = "";
						pliFornecedorFabricante.NumeroFabricante = "";
					}


					if (pliMercadoria.TipoFornecedor == 1 || pliMercadoria.TipoFornecedor == 2)
					{
						if (pliMercadoria.DadosFabricanteFornecedor.CodigoPaisFornecedor != null)
							pliFornecedorFabricante.CodigoPaisFornecedor = pliMercadoria.DadosFabricanteFornecedor.CodigoPaisFornecedor.Split('|')[0].Trim();
						if (pliMercadoria.DadosFabricanteFornecedor.DescricaoCidadeFornecedor != null)
							pliFornecedorFabricante.DescricaoCidadeFornecedor = pliMercadoria.DadosFabricanteFornecedor.DescricaoCidadeFornecedor;
						if (pliMercadoria.DadosFabricanteFornecedor.DescricaoComplementoFornecedor != null)
							pliFornecedorFabricante.DescricaoComplementoFornecedor = pliMercadoria.DadosFabricanteFornecedor.DescricaoComplementoFornecedor;
						if (pliMercadoria.DadosFabricanteFornecedor.DescricaoEstadoFornecedor != null)
							pliFornecedorFabricante.DescricaoEstadoFornecedor = pliMercadoria.DadosFabricanteFornecedor.DescricaoEstadoFornecedor;
						if (pliMercadoria.DadosFabricanteFornecedor.DescricaoFornecedor != null)
							pliFornecedorFabricante.DescricaoFornecedor = pliMercadoria.DadosFabricanteFornecedor.DescricaoFornecedor;
						if (pliMercadoria.DadosFabricanteFornecedor.DescricaoLogradouroFornecedor != null)
							pliFornecedorFabricante.DescricaoLogradouroFornecedor = pliMercadoria.DadosFabricanteFornecedor.DescricaoLogradouroFornecedor;
						if (pliMercadoria.DadosFabricanteFornecedor.CodigoPaisFornecedor != null)
							pliFornecedorFabricante.DescricaoPaisFornecedor = pliMercadoria.DadosFabricanteFornecedor.CodigoPaisFornecedor.Split('|')[1].TrimStart();
						if (pliMercadoria.DadosFabricanteFornecedor.NumeroFornecedor != null)
							pliFornecedorFabricante.NumeroFornecedor = pliMercadoria.DadosFabricanteFornecedor.NumeroFornecedor;

						pliFornecedorFabricante.CodigoPaisFabricante = "";
						pliFornecedorFabricante.DescricaoCidadeFabricante = "";
						pliFornecedorFabricante.DescricaoComplementoFabricante = "";
						pliFornecedorFabricante.DescricaoEstadoFabricante = "";
						pliFornecedorFabricante.DescricaoFabricante = "";
						pliFornecedorFabricante.DescricaoLogradouroFabricante = "";
						pliFornecedorFabricante.DescricaoPaisFabricante = "";
						pliFornecedorFabricante.NumeroFabricante = "";

						if (pliMercadoria.DadosFabricanteFornecedor.DescricaoFornecedor == null || pliMercadoria.DadosFabricanteFornecedor.DescricaoFornecedor == "")
						{
							pliFornecedorFabricante.CodigoPaisFornecedor = "";
							pliFornecedorFabricante.DescricaoCidadeFornecedor = "";
							pliFornecedorFabricante.DescricaoComplementoFornecedor = "";
							pliFornecedorFabricante.DescricaoEstadoFornecedor = "";
							pliFornecedorFabricante.DescricaoFornecedor = "";
							pliFornecedorFabricante.DescricaoLogradouroFornecedor = "";
							pliFornecedorFabricante.DescricaoPaisFornecedor = "";
							pliFornecedorFabricante.NumeroFornecedor = "";
						}

						if (pliMercadoria.TipoFornecedor == 2)
						{
							if (pliMercadoria.DadosFabricanteFornecedor.CodigoPaisFabricante != null)
								pliFornecedorFabricante.CodigoPaisFabricante = pliMercadoria.DadosFabricanteFornecedor.CodigoPaisFabricante.Split('|')[0].Trim();
							if (pliMercadoria.DadosFabricanteFornecedor.DescricaoCidadeFabricante != null)
								pliFornecedorFabricante.DescricaoCidadeFabricante = pliMercadoria.DadosFabricanteFornecedor.DescricaoCidadeFabricante;
							if (pliMercadoria.DadosFabricanteFornecedor.DescricaoComplementoFabricante != null)
								pliFornecedorFabricante.DescricaoComplementoFabricante = pliMercadoria.DadosFabricanteFornecedor.DescricaoComplementoFabricante;
							if (pliMercadoria.DadosFabricanteFornecedor.DescricaoEstadoFabricante != null)
								pliFornecedorFabricante.DescricaoEstadoFabricante = pliMercadoria.DadosFabricanteFornecedor.DescricaoEstadoFabricante;
							if (pliMercadoria.DadosFabricanteFornecedor.DescricaoFabricante != null)
								pliFornecedorFabricante.DescricaoFabricante = pliMercadoria.DadosFabricanteFornecedor.DescricaoFabricante;
							if (pliMercadoria.DadosFabricanteFornecedor.DescricaoLogradouroFabricante != null)
								pliFornecedorFabricante.DescricaoLogradouroFabricante = pliMercadoria.DadosFabricanteFornecedor.DescricaoLogradouroFabricante;
							if (pliMercadoria.DadosFabricanteFornecedor.CodigoPaisFabricante != null)
								pliFornecedorFabricante.DescricaoPaisFabricante = pliMercadoria.DadosFabricanteFornecedor.CodigoPaisFabricante.Split('|')[1].TrimStart();
							if (pliMercadoria.DadosFabricanteFornecedor.NumeroFabricante != null)
								pliFornecedorFabricante.NumeroFabricante = pliMercadoria.DadosFabricanteFornecedor.NumeroFabricante;
						}

					}

					if (pliMercadoria.TipoFornecedor == 3)
					{
						pliFornecedorFabricante.CodigoPaisFornecedor = "";
						pliFornecedorFabricante.DescricaoCidadeFornecedor = "";
						pliFornecedorFabricante.DescricaoComplementoFornecedor = "";
						pliFornecedorFabricante.DescricaoEstadoFornecedor = "";
						pliFornecedorFabricante.DescricaoFornecedor = "";
						pliFornecedorFabricante.DescricaoLogradouroFornecedor = "";
						pliFornecedorFabricante.DescricaoPaisFornecedor = "";
						pliFornecedorFabricante.NumeroFornecedor = "";

						pliFornecedorFabricante.CodigoPaisFabricante = pliMercadoria.CodigoPaisOrigemFabricante;
						pliFornecedorFabricante.DescricaoCidadeFabricante = "";
						pliFornecedorFabricante.DescricaoComplementoFabricante = "";
						pliFornecedorFabricante.DescricaoEstadoFabricante = "";
						pliFornecedorFabricante.DescricaoFabricante = "";
						pliFornecedorFabricante.DescricaoLogradouroFabricante = "";
						pliFornecedorFabricante.DescricaoPaisFabricante = pliMercadoria.DescricaoPaisOrigemFabricante; ;
						pliFornecedorFabricante.NumeroFabricante = "";
					}

					_uowSciex.CommandStackSciex.PliFornecedorFabricante.Salvar(pliFornecedorFabricante);
					_uowSciex.CommandStackSciex.Save();
				}
				else
				{
					if (pliMercadoria.DadosFabricanteFornecedor != null)
					{
						pliFornecedorFabricante = new PliFornecedorFabricanteEntity();

						pliFornecedorFabricante.CodigoAusenciaFabricante = pliMercadoria.DadosFabricanteFornecedor.CodigoAusenciaFabricante;
						pliFornecedorFabricante.PliMercadoria = pliMercadoriaEntity;

						if (pliMercadoria.TipoFornecedor == 1 || pliMercadoria.TipoFornecedor == 2)
						{
							if (pliMercadoria.DadosFabricanteFornecedor.CodigoPaisFornecedor != null)
								pliFornecedorFabricante.CodigoPaisFornecedor = pliMercadoria.DadosFabricanteFornecedor.CodigoPaisFornecedor.Split('|')[0].Trim();
							if (pliMercadoria.DadosFabricanteFornecedor.DescricaoCidadeFornecedor != null)
								pliFornecedorFabricante.DescricaoCidadeFornecedor = pliMercadoria.DadosFabricanteFornecedor.DescricaoCidadeFornecedor;
							if (pliMercadoria.DadosFabricanteFornecedor.DescricaoComplementoFornecedor != null)
								pliFornecedorFabricante.DescricaoComplementoFornecedor = pliMercadoria.DadosFabricanteFornecedor.DescricaoComplementoFornecedor;
							if (pliMercadoria.DadosFabricanteFornecedor.DescricaoEstadoFornecedor != null)
								pliFornecedorFabricante.DescricaoEstadoFornecedor = pliMercadoria.DadosFabricanteFornecedor.DescricaoEstadoFornecedor;
							if (pliMercadoria.DadosFabricanteFornecedor.DescricaoFornecedor != null)
								pliFornecedorFabricante.DescricaoFornecedor = pliMercadoria.DadosFabricanteFornecedor.DescricaoFornecedor;
							if (pliMercadoria.DadosFabricanteFornecedor.DescricaoLogradouroFornecedor != null)
								pliFornecedorFabricante.DescricaoLogradouroFornecedor = pliMercadoria.DadosFabricanteFornecedor.DescricaoLogradouroFornecedor;
							if (pliMercadoria.DadosFabricanteFornecedor.CodigoPaisFornecedor != null)
								pliFornecedorFabricante.DescricaoPaisFornecedor = pliMercadoria.DadosFabricanteFornecedor.CodigoPaisFornecedor.Split('|')[1].TrimStart();
							if (pliMercadoria.DadosFabricanteFornecedor.NumeroFornecedor != null)
								pliFornecedorFabricante.NumeroFornecedor = pliMercadoria.DadosFabricanteFornecedor.NumeroFornecedor;


							if (pliMercadoria.DadosFabricanteFornecedor.DescricaoFornecedor == null)
							{
								pliFornecedorFabricante.CodigoPaisFornecedor = "";
								pliFornecedorFabricante.DescricaoCidadeFornecedor = "";
								pliFornecedorFabricante.DescricaoComplementoFornecedor = "";
								pliFornecedorFabricante.DescricaoEstadoFornecedor = "";
								pliFornecedorFabricante.DescricaoFornecedor = "";
								pliFornecedorFabricante.DescricaoLogradouroFornecedor = "";
								pliFornecedorFabricante.DescricaoPaisFornecedor = "";
								pliFornecedorFabricante.NumeroFornecedor = "";
							}

							if (pliMercadoria.TipoFornecedor == 2)
							{
								if (pliMercadoria.DadosFabricanteFornecedor.CodigoPaisFabricante != null)
									pliFornecedorFabricante.CodigoPaisFabricante = pliMercadoria.DadosFabricanteFornecedor.CodigoPaisFabricante.Split('|')[0].Trim();
								if (pliMercadoria.DadosFabricanteFornecedor.DescricaoCidadeFabricante != null)
									pliFornecedorFabricante.DescricaoCidadeFabricante = pliMercadoria.DadosFabricanteFornecedor.DescricaoCidadeFabricante;
								if (pliMercadoria.DadosFabricanteFornecedor.DescricaoComplementoFabricante != null)
									pliFornecedorFabricante.DescricaoComplementoFabricante = pliMercadoria.DadosFabricanteFornecedor.DescricaoComplementoFabricante;
								if (pliMercadoria.DadosFabricanteFornecedor.DescricaoEstadoFabricante != null)
									pliFornecedorFabricante.DescricaoEstadoFabricante = pliMercadoria.DadosFabricanteFornecedor.DescricaoEstadoFabricante;
								if (pliMercadoria.DadosFabricanteFornecedor.DescricaoFabricante != null)
									pliFornecedorFabricante.DescricaoFabricante = pliMercadoria.DadosFabricanteFornecedor.DescricaoFabricante;
								if (pliMercadoria.DadosFabricanteFornecedor.DescricaoLogradouroFabricante != null)
									pliFornecedorFabricante.DescricaoLogradouroFabricante = pliMercadoria.DadosFabricanteFornecedor.DescricaoLogradouroFabricante;
								if (pliMercadoria.DadosFabricanteFornecedor.CodigoPaisFabricante != null)
									pliFornecedorFabricante.DescricaoPaisFabricante = pliMercadoria.DadosFabricanteFornecedor.CodigoPaisFabricante.Split('|')[1].TrimStart();
								if (pliMercadoria.DadosFabricanteFornecedor.NumeroFabricante != null)
									pliFornecedorFabricante.NumeroFabricante = pliMercadoria.DadosFabricanteFornecedor.NumeroFabricante;

								if (pliMercadoria.DadosFabricanteFornecedor.DescricaoFabricante == null)
								{
									pliFornecedorFabricante.CodigoPaisFabricante = "";
									pliFornecedorFabricante.DescricaoCidadeFabricante = "";
									pliFornecedorFabricante.DescricaoComplementoFabricante = "";
									pliFornecedorFabricante.DescricaoEstadoFabricante = "";
									pliFornecedorFabricante.DescricaoFabricante = "";
									pliFornecedorFabricante.DescricaoLogradouroFabricante = "";
									pliFornecedorFabricante.DescricaoPaisFabricante = "";
									pliFornecedorFabricante.NumeroFabricante = "";
								}

							}
						}

						if (pliMercadoria.TipoFornecedor == 3)
						{
							pliFornecedorFabricante.CodigoPaisFornecedor =
							pliFornecedorFabricante.DescricaoCidadeFornecedor = "";
							pliFornecedorFabricante.DescricaoComplementoFornecedor = "";
							pliFornecedorFabricante.DescricaoEstadoFornecedor = "";
							pliFornecedorFabricante.DescricaoFornecedor = "";
							pliFornecedorFabricante.DescricaoLogradouroFornecedor = "";
							pliFornecedorFabricante.DescricaoPaisFornecedor = "";
							pliFornecedorFabricante.NumeroFornecedor = "";

							pliFornecedorFabricante.CodigoPaisFabricante = pliMercadoria.DadosFabricanteFornecedor.CodigoPaisFornecedor != null ? pliMercadoria.DadosFabricanteFornecedor.CodigoPaisFornecedor.Split('|')[0].Trim() : pliMercadoria.CodigoPaisOrigemFabricante;
							pliFornecedorFabricante.DescricaoCidadeFabricante = "";
							pliFornecedorFabricante.DescricaoComplementoFabricante = "";
							pliFornecedorFabricante.DescricaoEstadoFabricante = "";
							pliFornecedorFabricante.DescricaoFabricante = "";
							pliFornecedorFabricante.DescricaoLogradouroFabricante = "";
							pliFornecedorFabricante.DescricaoPaisFabricante = pliMercadoria.DadosFabricanteFornecedor.CodigoPaisFornecedor != null ? pliMercadoria.DadosFabricanteFornecedor.CodigoPaisFornecedor.Split('|')[1].TrimStart() : pliMercadoria.DescricaoPaisOrigemFabricante;
							pliFornecedorFabricante.NumeroFabricante = "";
						}

						_uowSciex.CommandStackSciex.PliFornecedorFabricante.Salvar(pliFornecedorFabricante);
						_uowSciex.CommandStackSciex.Save();
					}
				}

			}


			if (!pliMercadoria.IsValidarItemPli)
			{
				if(pliMercadoria.IdPLIAplicacao == 1 || pliMercadoria.IdPLIAplicacao == 3 || pliMercadoria.IdPLIAplicacao == 4)
				{
					pliMercadoria = AutoMapper.Mapper.Map<PliMercadoriaComercializacaoVM>(_uowSciex.QueryStackSciex.PliMercadoria.Selecionar(o => o.IdPliMercadoria == pliMercadoriaEntity.IdPliMercadoria));
				}
				else if (pliMercadoria.IdPLIAplicacao == 2)
				{
					pliMercadoria = AutoMapper.Mapper.Map<PliMercadoriaVM>(_uowSciex.QueryStackSciex.PliMercadoria.Selecionar(o => o.IdPliMercadoria == pliMercadoriaEntity.IdPliMercadoria));
				}
			}
			else
			{
				Int32 idPliAplicacao = Convert.ToInt32(pliMercadoria.IdPLIAplicacao);
				pli = _iPliBll.RegrasValidar(null, idPliAplicacao, null, pliMercadoriaEntity.IdPliMercadoria, false);
				pliMercadoria = pli.ListaMercadorias.FirstOrDefault();
			}
			return pliMercadoria;
		}

		public PliMercadoriaVM AtualizarNCM(PliMercadoriaVM pliMercadoria)
		{
			if (pliMercadoria == null)
				return new PliMercadoriaVM();

			var valid1 = _uowSciex.QueryStackSciex.PliMercadoria.Selecionar(o => o.IdPLI == pliMercadoria.IdPLI && o.IdPliProduto == pliMercadoria.IdPliProduto && o.CodigoNCMMercadoria == pliMercadoria.CodigoNCMMercadoria && o.DescricaoNCMMercadoria == pliMercadoria.DescricaoNCMMercadoria);

			if (valid1 != null)
			{
				pliMercadoria.MensagemErro = "A NCM selecionada já foi adicionada";
				return pliMercadoria;
			}

			var ncm = _uowSciex.QueryStackSciex.PliMercadoria.Selecionar(o => o.IdPliMercadoria == pliMercadoria.IdPliMercadoria);

			ncm.CodigoNCMMercadoria = pliMercadoria.CodigoNCMMercadoria;
			ncm.DescricaoNCMMercadoria = pliMercadoria.DescricaoNCMMercadoria;

			_uowSciex.CommandStackSciex.PliMercadoria.Salvar(ncm);
			_uowSciex.CommandStackSciex.Save();

			if(pliMercadoria.IdPLIAplicacao == 2)
			{
				List<long> ids = new List<long>();
				foreach (var item in ncm.PliDetalheMercadoria)
				{
					ids.Add(item.IdPliDetalheMercadoria);
				}

				foreach (var item in ids)
				{
					_uowSciex.CommandStackSciex.PliDetalheMercadoria.Apagar(item);
					_uowSciex.CommandStackSciex.Save();
				}
			}

			pliMercadoria.IdMercadoria = 0;

			return pliMercadoria;
		}

		private ViewProdutoEmpresaVM ConsultaProdutoMercadoria(int IdProdutoEmpresa)
		{
			var viewProdutoEmpresaEntity = _uowSciex.QueryStackSciex.ViewProdutoEmpresa.Selecionar(o => o.IdProdutoEmpresa == IdProdutoEmpresa);

			return AutoMapper.Mapper.Map<ViewProdutoEmpresaVM>(viewProdutoEmpresaEntity);

		}

		private ViewMercadoriaVM ConsultaMercadoria(int IdMercadoria)
		{
			var viewMercadoriaEntity = _uowSciex.QueryStackSciex.ViewMercadoria.Selecionar(o => o.IdMercadoria == IdMercadoria);
			return AutoMapper.Mapper.Map<ViewMercadoriaVM>(viewMercadoriaEntity);
		}

		public PliMercadoriaVM Salvar(PliMercadoriaVM pliMercadoriaVM)
		{
			var a = RegrasSalvar(pliMercadoriaVM);
			return a;
		}

		public PliMercadoriaVM Selecionar(long? idPliMercadoria)
		{
			var pliMercadoriaVM = new PliMercadoriaVM();
			if (!idPliMercadoria.HasValue) { return pliMercadoriaVM; }

			var pliMercadoria = _uowSciex.QueryStackSciex.PliMercadoria.Selecionar(x => x.IdPliMercadoria == idPliMercadoria);
			pliMercadoriaVM = AutoMapper.Mapper.Map<PliMercadoriaVM>(pliMercadoria);
			return pliMercadoriaVM;
		}

		public void Deletar(long id)
		{
			var pliMercadoria = _uowSciex.QueryStackSciex.PliMercadoria.Selecionar(s => s.IdPliMercadoria == id);
			if (pliMercadoria != null)
			{
				_uowSciex.CommandStackSciex.PliMercadoria.Apagar(pliMercadoria.IdPliMercadoria);
			}
			_uowSciex.CommandStackSciex.Save();
		}

		public bool AplicarParametros(PliMercadoriaVM pliMercadoriaVM, bool confirmaAplicacao)
		{
			try
			{
				var pliMercadoriaRetornado = _uowSciex.QueryStackSciex.PliMercadoria.Selecionar(o => o.IdPLI == pliMercadoriaVM.IdPLI && o.IdPliMercadoria == pliMercadoriaVM.IdPliMercadoria && o.IdPliProduto == pliMercadoriaVM.IdPliProduto);
				var parametroEscolhido = _uowSciex.QueryStackSciex.Parametros.Selecionar(o => o.IdParametro == pliMercadoriaVM.IdParametro);

				pliMercadoriaRetornado.IdMoeda = parametroEscolhido.IdMoeda;
				pliMercadoriaRetornado.IdIncoterms = parametroEscolhido.IdIncoterms;
				pliMercadoriaRetornado.IdRegimeTributario = parametroEscolhido.IdRegimeTributario;
				pliMercadoriaRetornado.IdFundamentoLegal = parametroEscolhido.IdFundamentoLegal;
				pliMercadoriaRetornado.IdInstituicaoFinanceira = parametroEscolhido.IdInstituicaoFinanceira;
				pliMercadoriaRetornado.IdMotivo = parametroEscolhido.IdMotivo;
				pliMercadoriaRetornado.IdModalidadePagamento = parametroEscolhido.IdModalidadePagamento;
				pliMercadoriaRetornado.IdAladi = parametroEscolhido.IdAladi;
				pliMercadoriaRetornado.IdNaladi = parametroEscolhido.IdNaladi;
				pliMercadoriaRetornado.IdURFEntrada = parametroEscolhido.IdUnidadeReceitaFederalEntrada;
				pliMercadoriaRetornado.IdURFDespacho = parametroEscolhido.IdUnidadeReceitaFederalDespacho;
				pliMercadoriaRetornado.CodigoPais = parametroEscolhido.CodigoPaiMercadoria;
				pliMercadoriaRetornado.DescricaoPais = parametroEscolhido.DescricaoPaiMercadoria;
				pliMercadoriaRetornado.CodigoPaisOrigemFabricante = parametroEscolhido.CodigoPaisOrigemFabricante;
				pliMercadoriaRetornado.DescricaoPaisOrigemFabricante = parametroEscolhido.DescricaoPaisOrigemFabricante;
				pliMercadoriaRetornado.TipoCOBCambial = parametroEscolhido.TipoCorbeturaCambial;
				pliMercadoriaRetornado.NumeroCOBCambialLimiteDiasPagamento = parametroEscolhido.QuantidadeDiaLimite;
				pliMercadoriaRetornado.TipoAcordoTarifario = parametroEscolhido.TipoAcordoTarifario;
				if (!confirmaAplicacao)
				{
					pliMercadoriaRetornado.IdFabricante = parametroEscolhido.IdFabricante;
					pliMercadoriaRetornado.IdFornecedor = parametroEscolhido.IdFornecedor;
					pliMercadoriaRetornado.TipoFornecedor = parametroEscolhido.TipoFornecedor;

					PliFornecedorFabricanteEntity _pliFF = new PliFornecedorFabricanteEntity();

					var temp = _uowSciex.QueryStackSciex.PliFornecedorFabricante.Selecionar(o => o.IdPliMercadoria == pliMercadoriaRetornado.IdPliMercadoria);
					if (temp != null)
					{
						_pliFF = temp;
					}
					else
					{
						_pliFF.PliMercadoria = pliMercadoriaRetornado;
					}
					_pliFF.CodigoAusenciaFabricante = Convert.ToByte(pliMercadoriaRetornado.TipoFornecedor);

					if (pliMercadoriaRetornado.IdFornecedor != null)
					{
						FornecedorEntity _fornecedorEntity = _uowSciex.QueryStackSciex.Fornecedor.Selecionar(o => o.IdFornecedor == pliMercadoriaRetornado.IdFornecedor);

						_pliFF.CodigoPaisFornecedor = _fornecedorEntity.CodigoPais;
						_pliFF.DescricaoCidadeFornecedor = _fornecedorEntity.Cidade;
						_pliFF.DescricaoComplementoFornecedor = _fornecedorEntity.Complemento;
						_pliFF.DescricaoEstadoFornecedor = _fornecedorEntity.Estado;
						_pliFF.DescricaoFornecedor = _fornecedorEntity.RazaoSocial;
						_pliFF.DescricaoLogradouroFornecedor = _fornecedorEntity.Logradouro;
						_pliFF.DescricaoPaisFornecedor = _fornecedorEntity.DescricaoPais;
						_pliFF.NumeroFornecedor = _fornecedorEntity.Numero;
					}

					if (pliMercadoriaRetornado.IdFabricante != null)
					{
						FabricanteEntity _fabricanteEntity = _uowSciex.QueryStackSciex.Fabricante.Selecionar(o => o.IdFabricante == pliMercadoriaRetornado.IdFabricante);

						_pliFF.CodigoPaisFabricante = _fabricanteEntity.CodigoPais;
						_pliFF.DescricaoCidadeFabricante = _fabricanteEntity.Cidade;
						_pliFF.DescricaoComplementoFabricante = _fabricanteEntity.Complemento;
						_pliFF.DescricaoEstadoFabricante = _fabricanteEntity.Estado;
						_pliFF.DescricaoFabricante = _fabricanteEntity.RazaoSocial;
						_pliFF.DescricaoLogradouroFabricante = _fabricanteEntity.Logradouro;
						_pliFF.DescricaoPaisFabricante = _fabricanteEntity.DescricaoPais;
						_pliFF.NumeroFabricante = _fabricanteEntity.Numero;
					}

					_uowSciex.CommandStackSciex.PliFornecedorFabricante.Salvar(_pliFF);
					_uowSciex.CommandStackSciex.Save();

				}

				_uowSciex.CommandStackSciex.PliMercadoria.Salvar(pliMercadoriaRetornado);
				_uowSciex.CommandStackSciex.Save();

				return true;
			}
			catch { return false; }
		}

		public int QuantidadePLIMercadoriaDoPLI(long idPLI)
		{
			return _uowSciex.QueryStackSciex.PliMercadoria.Listar(s => s.IdPLI == idPLI).Count;
		}

		public bool VerificarMercadoriaFornecedor(long? idPliProduto, long? idPli , string CodigoNCMMercadoria, int? IdFornecedor, long? IdPliMercadoria, bool? aplicarParametro, int? idParametro)
		{
			int qtd = 0;
			if (aplicarParametro != null)
			{
				if (aplicarParametro.Value)
				{
					var parametroEscolhido = _uowSciex.QueryStackSciex.Parametros.Selecionar(o => o.IdParametro == idParametro);
					qtd = _uowSciex.QueryStackSciex.PliMercadoria.Listar(o => o.IdPLI == idPli && o.IdPliProduto == idPliProduto && o.CodigoNCMMercadoria.Equals(CodigoNCMMercadoria) && o.IdFornecedor == parametroEscolhido.IdFornecedor && o.IdPliMercadoria != IdPliMercadoria).Count;
				}
				else
				{
					qtd = _uowSciex.QueryStackSciex.PliMercadoria.Listar(o => o.IdPLI == idPli && o.IdPliProduto == idPliProduto && o.CodigoNCMMercadoria.Equals(CodigoNCMMercadoria) && o.IdFornecedor == IdFornecedor && o.IdPliMercadoria != IdPliMercadoria).Count;
				}
			}
			return (qtd > 0);
		}
	}
}