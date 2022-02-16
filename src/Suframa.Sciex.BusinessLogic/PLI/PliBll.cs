using FluentValidation;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Configuration;
using Suframa.Sciex.BusinessLogic.Pss;
using System.Text;
using Suframa.Sciex.CrossCutting.Extension;
using System.Net.Mail;

namespace Suframa.Sciex.BusinessLogic
{
	public class PliBll : IPliBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUnitOfWork _uowCadsuf;
		private readonly IUsuarioPssBll _usuarioPssBll;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;
		private readonly IViewImportadorBll _IViewImportadorBll;
		private readonly IComplementarPLIBll _complementarPLIBll;


		private long _idPLiRetorno;

		public PliBll(IUnitOfWorkSciex uowSciex, IUnitOfWork uowCadsuf,
			 IViewImportadorBll viewImportadorBll, IComplementarPLIBll complementarPLIBll,
			 IUsuarioPssBll usuarioPssBll, IUsuarioInformacoesBll usuarioInformacoesBll)
		{
			_uowSciex = uowSciex;
			_uowCadsuf = uowCadsuf;
			_IViewImportadorBll = viewImportadorBll;
			_complementarPLIBll = complementarPLIBll;
			_usuarioPssBll = usuarioPssBll;
			_usuarioInformacoesBll = usuarioInformacoesBll;
		}

		public IEnumerable<PliVM> Listar(PliVM pliVM)
		{
			var pli = _uowSciex.QueryStackSciex.Pli.Listar<PliVM>();
			return AutoMapper.Mapper.Map<IEnumerable<PliVM>>(pli);
		}

		public IEnumerable<object> Listar()
		{
			return _uowSciex.QueryStackSciex.Pli
				.Listar()
				.OrderBy(o => o.NumeroPli)
				.Select(
					s => new
					{
						id = s.IdPLI,
						text = s.CodigoCNAE + " - " + s.NumeroPli
					});
		}

		public PagedItems<PliVM> ListarPaginado(PliVM pagedFilter)
		{
			var dataInicio = pagedFilter.DataInicio == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataInicio.Value.Year, pagedFilter.DataInicio.Value.Month, pagedFilter.DataInicio.Value.Day);
			var dataFim = pagedFilter.DataFim == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataFim.Value.Year, pagedFilter.DataFim.Value.Month, pagedFilter.DataFim.Value.Day, 23, 59, 59);
			var usuCpfCnpjEmpresaOuLogado = _usuarioInformacoesBll.ObterCNPJ().Replace(".", "").Replace("-", "").Replace("/", ""); ;

			if (pagedFilter == null) { return new PagedItems<PliVM>(); }

			var pli = new PagedItems<PliVM>();

			if (pagedFilter.StatusPli == 0)
			{
				pli = _uowSciex.QueryStackSciex.Pli.ListarPaginado<PliVM>(o =>
				(
					(
						pagedFilter.NumeroPli == -1 || o.NumeroPli == pagedFilter.NumeroPli
					) 
					&&
					(
						pagedFilter.Ano == -1 || o.Ano == pagedFilter.Ano
					) 
					&&
					(
						pagedFilter.IdPLIAplicacao == 0 || o.IdPLIAplicacao == pagedFilter.IdPLIAplicacao
					) 
					&&
					(
						pagedFilter.TipoDocumento == 0 || o.TipoDocumento == pagedFilter.TipoDocumento
					)
					&&
					(
						pagedFilter.InscricaoCadastral == null || o.InscricaoCadastral == pagedFilter.InscricaoCadastral
					)
					&&
					(
						o.StatusPli == (byte)EnumPliStatus.EM_ELABORAÇÃO || o.StatusPli == (byte)EnumPliStatus.ENTREGUE || (o.StatusPli == (byte)EnumPliStatus.AGUARDANDO_ANÁLISE_VISUAL && o.StatusAnaliseVisual == 1 && o.PliAnaliseVisual.StatusAnalise == 9)
					)
					&&
					(
						(pagedFilter.DataInicio == null) || (o.DataCadastro >= dataInicio && o.DataCadastro <= dataFim)
					)
					&& 
					(
						(o.Cnpj == usuCpfCnpjEmpresaOuLogado && usuCpfCnpjEmpresaOuLogado.Length == 14) ||
						(o.Cnpj == usuCpfCnpjEmpresaOuLogado && usuCpfCnpjEmpresaOuLogado == o.NumeroResponsavelRegistro)
					)
				),
				pagedFilter);
			}
			else
			{
				pli = _uowSciex.QueryStackSciex.Pli.ListarPaginado<PliVM>(o =>
				(
					(
						pagedFilter.NumeroPli == -1 || o.NumeroPli == pagedFilter.NumeroPli
					) 
					&&
					(
						pagedFilter.Ano == -1 || o.Ano == pagedFilter.Ano
					) 
					&&
					(
						pagedFilter.IdPLIAplicacao == 0 || o.IdPLIAplicacao == pagedFilter.IdPLIAplicacao
					)
					&&
					(
						pagedFilter.TipoDocumento == 0 || o.TipoDocumento == pagedFilter.TipoDocumento
					)
					&&
					(
						pagedFilter.InscricaoCadastral == null || o.InscricaoCadastral == pagedFilter.InscricaoCadastral
					)
					&&
					(
						(pagedFilter.DataInicio == null) || (o.DataCadastro >= dataInicio && o.DataCadastro <= dataFim)
					)
					&&
					(
						pagedFilter.StatusPli == null || (pagedFilter.StatusPli == 19 && (o.StatusPli == (byte)EnumPliStatus.AGUARDANDO_ANÁLISE_VISUAL && o.StatusAnaliseVisual == 1 && o.PliAnaliseVisual.StatusAnalise == 9)) || o.StatusPli == pagedFilter.StatusPli
					)
					&& (
						(o.Cnpj == usuCpfCnpjEmpresaOuLogado && usuCpfCnpjEmpresaOuLogado.Length == 14) ||
						(o.Cnpj == usuCpfCnpjEmpresaOuLogado && usuCpfCnpjEmpresaOuLogado == o.NumeroResponsavelRegistro)
					)
				),
				pagedFilter);
			}

			foreach (var item in pli.Items)
			{
				var obj = _uowSciex.QueryStackSciex.PliHistorico.Selecionar(o => o.IdPLI == item.IdPLI && o.StatusPli == 9);
				if (obj != null)
					item.DescricaoMotivo = obj.Observacao;

				if (item.AnaliseVisualStatus == 9 && item.StatusPli == 23 && item.StatusAnaliseVisual == 1)
					item.DescricaoStatus = "PENDENTE";
			}

			return pli;

		}

		private PliVM ConfiguraNovoPLI(PliVM pli)
		{
			pli.Cnpj = _usuarioPssBll.ObterUsuarioLogado().usuCpfCnpjEmpresaOuLogado.Replace(".", "").Replace("-", "").Replace("/", "");
			var viewImportador = _IViewImportadorBll.Selecionar(_usuarioPssBll.ObterUsuarioLogado().usuCpfCnpjEmpresaOuLogado.Replace(".", "").Replace("-", "").Replace("/", ""));  // falta integrar com o PSS
			if(viewImportador == null)
			{
				pli.Mensagem = "O usuário logado não está cadastrado no sistema CADSUF como importador, por favor verifique o cadastro e tente novamente.";
				return pli;
			}

			pli.InscricaoCadastral = viewImportador != null ? viewImportador.InscricaoCadastral : 0;  // falta integrar com o PSS 

			pli.TipoOrigem = Convert.ToByte(EnumPliTipoOrigem.PliWeb);

			var pliAplicacao = _uowSciex.QueryStackSciex.PliAplicacao.Selecionar(o => o.Codigo == pli.CodigoPliAplicao);
			pli.IdPLIAplicacao = pliAplicacao.IdPliAplicacao;

			//valida tipo de pli (substitutivo)
			if(pli.TipoDocumento == 2)
			{
				var numeroLi = Convert.ToInt64(pli.NumeroLIReferencia);

				var verificaLI = _uowSciex.QueryStackSciex.Li.Selecionar(o => o.NumeroLi == numeroLi);

				if(verificaLI == null)
				{
					pli.Mensagem = "LI de referência inexistente.";
					return pli;
				}

				if(verificaLI.PliMercadoria.Pli.IdPLIAplicacao != pli.IdPLIAplicacao)
				{
					pli.Mensagem = "A LI de referência informada não é da mesma Aplicação do novo PLI";
					return pli;
				}

				var resp = _uowSciex.QueryStackSciex.VerificaLiIndeferidoCancelado(pli.NumeroLIReferencia);

				if (resp != null && resp.Count > 0)
				{
					pli.Mensagem = "LI de referência não pode ser utilizada, pois está sendo referenciada em outro PLI.";
					return pli;
				}

				var liParaImportador = _uowSciex.QueryStackSciex.VerificaLiDoImportador(numeroLi, pli.Cnpj);

				if (liParaImportador.Count <= 0)
				{
					pli.Mensagem = "LI de referência inválida para substituição. Para ser referência, a LI deve estar Deferida, sem DI e pertencer ao mesmo importador do novo PLI.";
					return pli;
				}

				var li = _uowSciex.QueryStackSciex.Li.Selecionar(o => o.NumeroLi == numeroLi);
				var lis = _uowSciex.QueryStackSciex.LiSubstituida.Listar(o => o.IdLiSubstituida == li.IdPliMercadoria || o.IdLiSubstituta == li.IdPliMercadoria);
				if(lis.Count > 0)
				{
					var idLis = lis.FirstOrDefault().IdLiOrigem;
					var liOrigem = _uowSciex.QueryStackSciex.LiSubstituida.Listar(o => o.IdLiOrigem == idLis);

					if (liOrigem != null && liOrigem.LastOrDefault().NumeroLsu >= 3)
					{
						pli.Mensagem = "LI de referência não pode ser utilizada, pois excedeu o limite máximo de 3 substituições.";
						return pli;
					}
				}
			}

			if(pli.TipoDocumento == 3)
			{
				var numeroDi = Convert.ToInt64(pli.NumeroDIReferencia);

				var DI = _uowSciex.QueryStackSciex.Di.Selecionar(o => o.NumeroDi == numeroDi);
				
				if(DI == null)
				{
					pli.Mensagem = "DI de referência inexistente.";
					return pli;
				}


				var LisDi = _uowSciex.QueryStackSciex.DiLi.Listar(o => o.IdDi == DI.IdDi);

				List<LiEntity> lisAplicacao = new List<LiEntity>();
				foreach (var item in LisDi)
				{
					var li = _uowSciex.QueryStackSciex.Li.Selecionar(o => o.NumeroLi == item.NumeroLi);
					var pliMerc = _uowSciex.QueryStackSciex.PliMercadoria.Selecionar(o => o.IdPliMercadoria == li.IdPliMercadoria);
					if (pliMerc.Pli.IdPLIAplicacao == pli.IdPLIAplicacao)
						lisAplicacao.Add(li);
				}

				if(lisAplicacao.Count == 0)
				{
					pli.Mensagem = "A DI de referência informada não possui LI com a mesma Aplicação do novo PLI.";
					return pli;
				}
			}

			var empresa = _uowCadsuf.QueryStack.ViewSetorEmpresa.Listar(o => o.Cnpj == pli.Cnpj);
			if (pli.CodigoPliAplicao == 0)
			{
				if (empresa.Any())
				{
					ViewSetorEntity setor = new ViewSetorEntity();
					foreach (var item in empresa)
					{	
						setor = _uowCadsuf.QueryStack.ViewSetor.Selecionar(o => o.IdSetor == item.IdSetor && o.Codigo == 2);
						if (setor != null)
						{
							pli.DescricaoSetor = setor.Descricao;
							pli.CodigoSetor = setor.Codigo;
							break;
						}
					}
					if (setor == null)
					{
						pli.Mensagem = "Aplicação do PLI inválida. Empresa não cadastrada no setor Comércio";
						return pli;
					}
				}
				else
				{
					pli.Mensagem = "Aplicação do PLI inválida. Empresa não cadastrada no setor Comércio";
					return pli;
				}
			}
			else if(pli.CodigoPliAplicao == 1)
			{
				if (empresa.Any())
				{
					ViewSetorEntity setor = new ViewSetorEntity();
					foreach (var item in empresa)
					{
						setor = _uowCadsuf.QueryStack.ViewSetor.Selecionar(o => o.IdSetor == item.IdSetor && o.Codigo == 19);
						if (setor != null)
						{
							pli.DescricaoSetor = setor.Descricao;
							pli.CodigoSetor = setor.Codigo;
							break;
						}
					}
					if (setor == null)
					{
						pli.Mensagem = "Aplicação do PLI inválida. Empresa não cadastrada no setor Indústria";
						return pli;
					}
				}
				else
				{
					pli.Mensagem = "Aplicação do PLI inválida. Empresa não cadastrada no setor Indústria";
					return pli;
				}
			}
			else if (pli.CodigoPliAplicao == 2)
			{
				if (empresa.Any())
				{
					ViewSetorEntity setor = new ViewSetorEntity();
					foreach (var item in empresa)
					{
						setor = _uowCadsuf.QueryStack.ViewSetor.Selecionar(o => o.IdSetor == item.IdSetor && o.Codigo == 19);
						if (setor != null)
						{
							pli.DescricaoSetor = setor.Descricao;
							pli.CodigoSetor = setor.Codigo;
							break;
						}
					}
				}
			}
			else if (pli.CodigoPliAplicao == 3)
			{
				if (empresa.Any())
				{
					ViewSetorEntity setor = new ViewSetorEntity();
					
					foreach (var item in empresa)
					{
						setor = _uowCadsuf.QueryStack.ViewSetor.Selecionar(o => o.IdSetor == item.IdSetor && o.Codigo == 19);

						if (setor != null)
						{
							pli.DescricaoSetor = setor.Descricao;
							pli.CodigoSetor = setor.Codigo;
							break;
						}
					}
					if (setor == null)
					{
						pli.Mensagem = "Aplicação do PLI inválida. Empresa não cadastrada no setor Indústria";
						return pli;
					}
				}
				else
				{
					pli.Mensagem = "Aplicação do PLI inválida. Empresa não cadastrada no setor Indústria";
					return pli;
				}
			}

			var cnae = _uowCadsuf.QueryStack.ViewAtividadeEconomicaPrincipal.Selecionar(o => o.CNPJ == pli.Cnpj);
			if (cnae != null)
			{
				pli.CodigoCNAE = cnae.CodigoConcla;
			}

			pli.RazaoSocial = viewImportador.RazaoSocial.ToUpper();
			pli.NomeResponsavelRegistro = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoNome.Replace(".", "").Replace("-", "").Replace("/", "");
			pli.NumeroResponsavelRegistro = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoCpfCnpj.Replace(".", "").Replace("-", "").Replace("/", "");

			pli.StatusPli = (byte)EnumPliStatus.EM_ELABORAÇÃO;

			if(pli.TipoDocumento == 1)
			{
				pli.TipoDocumento = (byte)EnumPliTipoDocumento.NORMAL;
				pli.DescricaoTipoDocumento = "NORMAL";
			}

			if (pli.TipoDocumento == 2)
			{
				pli.TipoDocumento = (byte)EnumPliTipoDocumento.SUBSTITUTIVO;
				pli.DescricaoTipoDocumento = "SUBSTITUTIVO";
			}

			if (pli.TipoDocumento == 3)
			{
				pli.TipoDocumento = 3;
				pli.DescricaoTipoDocumento = "RETIFICADOR";
			}

			pli.DataCadastro = DateTime.Now;

			var sequence = this.GerarSequence(pli.Cnpj, DateTime.Now.Year);
			pli.Ano = DateTime.Now.Year;
			pli.NumeroPli = sequence;

			return pli;
		}

		public PliVM RegrasSalvar(PliVM pli)
		{
			try
			{
				bool novoPli = false;
				if (pli == null)
				{
					return null;
				}
				else
					novoPli = pli.IdPLI.HasValue;

				if (pli.CopiaPli)
				{
					//regra para 
					var empresa = _uowCadsuf.QueryStack.ViewSetorEmpresa.Listar(o => o.Cnpj == pli.Cnpj);
					if (empresa != null)
					{
						ViewSetorEntity setor = new ViewSetorEntity();
						foreach (var item in empresa)
						{
							// RN 08 - Verifica se a empresa se enquadra na categoria de industria	
							setor = _uowCadsuf.QueryStack.ViewSetor.Selecionar(o => o.IdSetor == item.IdSetor && o.Codigo == 19);
							if (setor != null)
							{
								pli.DescricaoSetor = setor.Descricao;
								pli.CodigoSetor = setor.Codigo;
								break;
							}
						}

						if (setor == null)
						{
							pli.Mensagem = "Aplicação do PLI inválida. Empresa não cadastrada no setor indústria";
							return pli;
						}
					}

					if (!CopiarPli(pli.IdPLI))
					{
						pli.MensagemErro = "Erro ao copiar PLI";
					}

					if (_idPLiRetorno > 0)
						pli = Selecionar(_idPLiRetorno);

					return pli;
				}

				if (!pli.IdPLI.HasValue)
				{
					pli = ConfiguraNovoPLI(pli);

					if (!String.IsNullOrEmpty(pli.Mensagem))
					{
						return pli;
					}
				}

				if (String.IsNullOrEmpty(pli.Mensagem))
				{
					var pliEntity = AutoMapper.Mapper.Map<PliEntity>(pli);

					if (pliEntity == null) { return null; }

					if (pli.IdPLI.HasValue)
					{
						pliEntity = _uowSciex.QueryStackSciex.Pli.Selecionar(x => x.IdPLI == pli.IdPLI);
						pliEntity = AutoMapper.Mapper.Map(pli, pliEntity);
					}

					if (!pli.CopiaPli)
					{
						pliEntity.NumCPFRepLegalSISCO = pliEntity.NumCPFRepLegalSISCO.CnpjCpfUnformat();
						pliEntity.Cnpj = pliEntity.Cnpj.CnpjCpfUnformat();
						_uowSciex.CommandStackSciex.Pli.Salvar(pliEntity);
						_uowSciex.CommandStackSciex.Save();
					}

					if(!novoPli && pli.TipoDocumento == 2)
					{
						var liNumero = Convert.ToInt64(pli.NumeroLIReferencia);
						var liRef = _uowSciex.QueryStackSciex.Li.Selecionar(o => o.NumeroLi == liNumero);
						var liMercadoria = _uowSciex.QueryStackSciex.PliMercadoria.Selecionar(o => o.IdPliMercadoria == liRef.IdPliMercadoria);
						var liProduto = _uowSciex.QueryStackSciex.PliProduto.Selecionar(o=>o.IdPliProduto == liMercadoria.IdPliProduto);
						var fornecedorFabricante = _uowSciex.QueryStackSciex.PliFornecedorFabricante.Selecionar(o => o.IdPliMercadoria == liMercadoria.IdPliMercadoria);
						PliProdutoEntity produto = new PliProdutoEntity();
						if (pli.IdPLIAplicacao == 2)
						{
							produto.CodigoModeloProduto = liProduto.CodigoModeloProduto;
							produto.CodigoProduto = liProduto.CodigoProduto;
							produto.CodigoTipoProduto = liProduto.CodigoTipoProduto;
							produto.Descricao = liProduto.Descricao;
							produto.IdPLI = pliEntity.IdPLI;
							produto.IdPliProduto = 0;
							produto.Pli = null;
							produto.PliMercadoria = null;
							_uowSciex.CommandStackSciex.PliProduto.Salvar(produto);
							_uowSciex.CommandStackSciex.Save();
						}

						PliMercadoriaEntity NCM = new PliMercadoriaEntity();

						NCM.IdPLI = pliEntity.IdPLI;
						if (pli.IdPLIAplicacao == 2)
							NCM.IdPliProduto = produto.IdPliProduto;
						else
							NCM.IdPliProduto = null;
						NCM.IdCodigoConta = liMercadoria.IdCodigoConta;
						NCM.IdCodigoUtilizacao = liMercadoria.IdCodigoUtilizacao;

						NCM.IdFabricante = liMercadoria.IdFabricante;
						NCM.IdFornecedor = liMercadoria.IdFornecedor;

						NCM.IdFundamentoLegal = liMercadoria.IdFundamentoLegal;
						NCM.IdIncoterms = liMercadoria.IdIncoterms;
						NCM.IdInstituicaoFinanceira = liMercadoria.IdInstituicaoFinanceira;
						NCM.IdModalidadePagamento = liMercadoria.IdModalidadePagamento;
						NCM.IdMoeda = liMercadoria.IdMoeda;
						NCM.IdMotivo = liMercadoria.IdMotivo;
						NCM.IdRegimeTributario = liMercadoria.IdRegimeTributario;
						NCM.IdURFDespacho = liMercadoria.IdURFDespacho;
						NCM.IdURFEntrada = liMercadoria.IdURFEntrada;
						NCM.IdAladi = liMercadoria.IdAladi;
						NCM.IdNaladi = liMercadoria.IdNaladi;

						NCM.CodigoModeloProduto = liMercadoria.CodigoModeloProduto;
						NCM.CodigoNCMMercadoria = liMercadoria.CodigoNCMMercadoria;
						NCM.CodigoPais = liMercadoria.CodigoPais;
						NCM.CodigoPaisOrigemFabricante = liMercadoria.CodigoPaisOrigemFabricante;
						NCM.CodigoProduto = liMercadoria.CodigoProduto;
						NCM.CodigoTipoProduto = liMercadoria.CodigoTipoProduto;
						NCM.DescricaoInformacaoComplementar = liMercadoria.DescricaoInformacaoComplementar;
						NCM.DescricaoNCMMercadoria = liMercadoria.DescricaoNCMMercadoria;
						NCM.DescricaoPais = liMercadoria.DescricaoPais;
						NCM.DescricaoPaisOrigemFabricante = liMercadoria.DescricaoPaisOrigemFabricante;
						NCM.DescricaoProduto = liMercadoria.DescricaoProduto;
						NCM.NumeroComunicadoCompra = liMercadoria.NumeroComunicadoCompra;
						NCM.NumeroAtoDrawback = liMercadoria.NumeroAtoDrawback;
						NCM.NumeroAgenciaSecex = liMercadoria.NumeroAgenciaSecex;
						NCM.NumeroNCMDestaque = liMercadoria.NumeroNCMDestaque;
						NCM.RowVersion = liMercadoria.RowVersion;
						NCM.ValorTotalCondicaoVenda = liMercadoria.ValorTotalCondicaoVenda;


						NCM.TipoCOBCambial = liMercadoria.TipoCOBCambial;
						NCM.TipoFornecedor = liMercadoria.TipoFornecedor;
						NCM.PesoLiquido = liMercadoria.PesoLiquido;
						
						NCM.TipoAcordoTarifario = liMercadoria.TipoAcordoTarifario;
						NCM.QuantidadeUnidadeMedidaEstatistica = liMercadoria.QuantidadeUnidadeMedidaEstatistica;
						NCM.NumeroCOBCambialLimiteDiasPagamento = liMercadoria.NumeroCOBCambialLimiteDiasPagamento;

						_uowSciex.CommandStackSciex.PliMercadoria.Salvar(NCM);
						_uowSciex.CommandStackSciex.Save();
						_uowSciex.CommandStackSciex.DetachEntries();

						PliFornecedorFabricanteEntity fornecedorefabricante = new PliFornecedorFabricanteEntity();
						fornecedorefabricante.IdPliMercadoria = NCM.IdPliMercadoria;
						fornecedorefabricante.CodigoAusenciaFabricante = fornecedorFabricante.CodigoAusenciaFabricante;
						fornecedorefabricante.CodigoPaisFabricante = fornecedorFabricante.CodigoPaisFabricante;
						fornecedorefabricante.CodigoPaisFornecedor = fornecedorFabricante.CodigoPaisFornecedor;
						fornecedorefabricante.DescricaoCidadeFabricante = fornecedorFabricante.DescricaoCidadeFabricante;
						fornecedorefabricante.DescricaoCidadeFornecedor = fornecedorFabricante.DescricaoCidadeFornecedor;
						fornecedorefabricante.DescricaoComplementoFabricante = fornecedorFabricante.DescricaoComplementoFabricante;
						fornecedorefabricante.DescricaoComplementoFornecedor = fornecedorFabricante.DescricaoComplementoFornecedor;
						fornecedorefabricante.DescricaoEstadoFabricante = fornecedorFabricante.DescricaoEstadoFabricante;
						fornecedorefabricante.DescricaoEstadoFornecedor = fornecedorFabricante.DescricaoEstadoFornecedor;
						fornecedorefabricante.DescricaoFabricante = fornecedorFabricante.DescricaoFabricante;
						fornecedorefabricante.DescricaoFornecedor = fornecedorFabricante.DescricaoFornecedor;
						fornecedorefabricante.DescricaoLogradouroFabricante = fornecedorFabricante.DescricaoLogradouroFabricante;
						fornecedorefabricante.DescricaoLogradouroFornecedor = fornecedorFabricante.DescricaoLogradouroFornecedor;
						fornecedorefabricante.DescricaoPaisFabricante = fornecedorFabricante.DescricaoPaisFabricante;
						fornecedorefabricante.DescricaoPaisFornecedor = fornecedorFabricante.DescricaoPaisFornecedor;
						fornecedorefabricante.NumeroFabricante = fornecedorFabricante.NumeroFabricante;
						fornecedorefabricante.NumeroFornecedor = fornecedorFabricante.NumeroFornecedor;

						_uowSciex.CommandStackSciex.PliFornecedorFabricante.Salvar(fornecedorefabricante, true);
						_uowSciex.CommandStackSciex.Save();

						foreach (var item in liMercadoria.PliDetalheMercadoria)
						{
							PliDetalheMercadoriaEntity detalheMercadoria = new PliDetalheMercadoriaEntity();
							detalheMercadoria.CodigoDetalheMercadoria = item.CodigoDetalheMercadoria;
							detalheMercadoria.DescricaoComplemento = item.DescricaoComplemento;
							detalheMercadoria.DescricaoDetalhe = item.DescricaoDetalhe;
							detalheMercadoria.DescricaoMateriaPrimaBasica = item.DescricaoMateriaPrimaBasica;
							detalheMercadoria.DescricaoPartNumber = item.DescricaoPartNumber;
							detalheMercadoria.DescricaoREFFabricante = item.DescricaoREFFabricante;
							detalheMercadoria.DescricaoUnidadeMedida = item.DescricaoUnidadeMedida;

							detalheMercadoria.IdPliMercadoria = NCM.IdPliMercadoria;
							detalheMercadoria.IdUnidadeMedida = item.IdUnidadeMedida;
							detalheMercadoria.QuantidadeComercializada = item.QuantidadeComercializada;
							detalheMercadoria.RowVersion = item.RowVersion;
							detalheMercadoria.SiglaUnidadeMedida = item.SiglaUnidadeMedida;

							detalheMercadoria.ValorCondicaoVenda = item.ValorCondicaoVenda;
							detalheMercadoria.ValorTotalCondicaoVendaDolar = item.ValorTotalCondicaoVendaDolar;
							detalheMercadoria.ValorTotalCondicaoVendaReal = item.ValorTotalCondicaoVendaReal;
							detalheMercadoria.ValorUnitarioCondicaoVenda = item.ValorUnitarioCondicaoVenda;
							detalheMercadoria.ValorUnitarioCondicaoVendaDolar = item.ValorUnitarioCondicaoVendaDolar;

							_uowSciex.CommandStackSciex.PliDetalheMercadoria.Salvar(detalheMercadoria);
							_uowSciex.CommandStackSciex.Save();
						}

						foreach (var item in liMercadoria.PliProcessoAnuente)
						{
							PliProcessoAnuenteEntity detalheProcessoAnuente = new PliProcessoAnuenteEntity();
							detalheProcessoAnuente.IdOrgaoAnuente = item.IdOrgaoAnuente;
							detalheProcessoAnuente.IdPliMercadoria = NCM.IdPliMercadoria;
							detalheProcessoAnuente.NumeroProcesso = item.NumeroProcesso;
							//detalheProcessoAnuente.OrgaoAnuente = item.OrgaoAnuente;
							//detalheProcessoAnuente.PLIMercadoria = item.PLIMercadoria;

							_uowSciex.CommandStackSciex.PliProcessoAnuente.Salvar(detalheProcessoAnuente);
							_uowSciex.CommandStackSciex.Save();
						}

					}

					if (!novoPli) //caso seja um novo pli, inserir o cadastro de historico
					{
						 
						PliHistoricoEntity pliHistoricoEntidade = new PliHistoricoEntity();
						pliHistoricoEntidade.IdPLI = pliEntity.IdPLI;
						pliHistoricoEntidade.StatusPli = (byte)EnumPliStatus.EM_ELABORAÇÃO;
						pliHistoricoEntidade.DescricaoStatusPli = EnumPliStatus.EM_ELABORAÇÃO.ToString().Replace("_", " ");
						pliHistoricoEntidade.DataEvento = DateTime.Now;
						if (pli.TipoDocumento == 1)
							pliHistoricoEntidade.Observacao = "CADASTRO DO PLI";
						if (pli.TipoDocumento == 2)
							pliHistoricoEntidade.Observacao = "CADASTRO DO PLI SUBSTITUTIVO";
						if (pli.TipoDocumento == 3)
							pliHistoricoEntidade.Observacao = "CADASTRO DO PLI RETIFICADOR";
						pliHistoricoEntidade.CPFCNPJ = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoCpfCnpj.Replace(".", "").Replace("-", "").Replace("/", "");
						pliHistoricoEntidade.NomeResponsavel = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoNome.Replace(".", "").Replace("-", "").Replace("/", ""); ;

						_uowSciex.CommandStackSciex.PliHistorico.Salvar(pliHistoricoEntidade);
						_uowSciex.CommandStackSciex.Save();
					}

					return AutoMapper.Mapper.Map<PliVM>(pliEntity);
				}

				return pli;
			}
			catch (Exception ex)
			{
				throw;
			}

		}

		public PliVM Salvar(PliVM pliVM)
		{
			if (!pliVM.CopiaPli)
			{
				if (pliVM.EntregarPli)
				{
					return RegrasEntregar(pliVM);
				}

				if (pliVM.ValidarPli)
				{
					return RegrasValidar(pliVM.IdPLI.Value, pliVM.IdPLIAplicacao, null, null, false);
				}
			}
			return RegrasSalvar(pliVM);

		}

		public PliVM PliAddLi(PliVM pliVM)
		{
			var liRef = _uowSciex.QueryStackSciex.Li.Selecionar(o => o.IdPliMercadoria == pliVM.IdLiReferencia);

			var valid1 = _uowSciex.QueryStackSciex.PliMercadoria.Selecionar(o => o.NumeroLiRetificador == liRef.NumeroLi && o.IdPLI == pliVM.IdPLI);
			if (valid1 != null)
			{
				//var liProduto = _uowSciex.QueryStackSciex.PliProduto.Selecionar(o => o.IdPliProduto == liMercadoria.IdPliProduto && o.IdPLI == pliVM.IdPLI);
				pliVM.Mensagem = "Operação não realizada. Este registro já foi adicionado.";
				return pliVM;
			}
			var valid2 = _uowSciex.QueryStackSciex.PliMercadoria.Selecionar(o => o.NumeroLiRetificador == liRef.NumeroLi && o.Pli.TipoDocumento == 3);
			if(valid2 != null)
			{
				pliVM.Mensagem = "Operação não realizada. Este registro já foi adicionado em outro PLI Retificador.";
				return pliVM;
			}


			var merc = _uowSciex.QueryStackSciex.PliMercadoria.Selecionar(o => o.IdPliMercadoria == liRef.IdPliMercadoria);
			var prod = _uowSciex.QueryStackSciex.PliProduto.Selecionar(o => o.IdPliProduto == merc.IdPliProduto);
			var fornecedorFabricante = _uowSciex.QueryStackSciex.PliFornecedorFabricante.Selecionar(o => o.IdPliMercadoria == merc.IdPliMercadoria);


			PliProdutoEntity produto = new PliProdutoEntity();
			if (pliVM.IdPLIAplicacao == 2)
			{
				produto.CodigoModeloProduto = prod.CodigoModeloProduto;
				produto.CodigoProduto = prod.CodigoProduto;
				produto.CodigoTipoProduto = prod.CodigoTipoProduto;
				produto.Descricao = prod.Descricao;
				produto.IdPLI = pliVM.IdPLI.Value;
				produto.IdPliProduto = 0;
				produto.Pli = null;
				produto.PliMercadoria = null;
				_uowSciex.CommandStackSciex.PliProduto.Salvar(produto);
				_uowSciex.CommandStackSciex.Save();
			}

			PliMercadoriaEntity NCM = new PliMercadoriaEntity();
			NCM.IdPLI = pliVM.IdPLI.Value;
			if (pliVM.IdPLIAplicacao == 2)
				NCM.IdPliProduto = produto.IdPliProduto;
			else
				NCM.IdPliProduto = null;
			NCM.IdCodigoConta = merc.IdCodigoConta;
			NCM.IdCodigoUtilizacao = merc.IdCodigoUtilizacao;

			NCM.IdFabricante = merc.IdFabricante;
			NCM.IdFornecedor = merc.IdFornecedor;

			NCM.IdFundamentoLegal = merc.IdFundamentoLegal;
			NCM.IdIncoterms = merc.IdIncoterms;
			NCM.IdInstituicaoFinanceira = merc.IdInstituicaoFinanceira;
			NCM.IdModalidadePagamento = merc.IdModalidadePagamento;
			NCM.IdMoeda = merc.IdMoeda;
			NCM.IdMotivo = merc.IdMotivo;
			NCM.IdRegimeTributario = merc.IdRegimeTributario;
			NCM.IdURFDespacho = merc.IdURFDespacho;
			NCM.IdURFEntrada = merc.IdURFEntrada;
			NCM.IdAladi = merc.IdAladi;
			NCM.IdNaladi = merc.IdNaladi;

			NCM.CodigoModeloProduto = merc.CodigoModeloProduto;
			NCM.CodigoNCMMercadoria = merc.CodigoNCMMercadoria;
			NCM.CodigoPais = merc.CodigoPais;
			NCM.CodigoPaisOrigemFabricante = merc.CodigoPaisOrigemFabricante;
			NCM.CodigoProduto = merc.CodigoProduto;
			NCM.CodigoTipoProduto = merc.CodigoTipoProduto;
			NCM.DescricaoInformacaoComplementar = merc.DescricaoInformacaoComplementar;
			NCM.DescricaoNCMMercadoria = merc.DescricaoNCMMercadoria;
			NCM.DescricaoPais = merc.DescricaoPais;
			NCM.DescricaoPaisOrigemFabricante = merc.DescricaoPaisOrigemFabricante;
			NCM.DescricaoProduto = merc.DescricaoProduto;
			NCM.NumeroComunicadoCompra = merc.NumeroComunicadoCompra;
			NCM.NumeroAtoDrawback = merc.NumeroAtoDrawback;
			NCM.NumeroAgenciaSecex = merc.NumeroAgenciaSecex;
			NCM.NumeroNCMDestaque = merc.NumeroNCMDestaque;
			NCM.RowVersion = merc.RowVersion;
			NCM.ValorTotalCondicaoVenda = merc.ValorTotalCondicaoVenda;


			NCM.TipoCOBCambial = merc.TipoCOBCambial;
			NCM.TipoFornecedor = merc.TipoFornecedor;
			NCM.PesoLiquido = merc.PesoLiquido;

			NCM.TipoAcordoTarifario = merc.TipoAcordoTarifario;
			NCM.QuantidadeUnidadeMedidaEstatistica = merc.QuantidadeUnidadeMedidaEstatistica;
			NCM.NumeroCOBCambialLimiteDiasPagamento = merc.NumeroCOBCambialLimiteDiasPagamento;
			NCM.NumeroLiRetificador = Convert.ToInt32(liRef.NumeroLi.Value);

			_uowSciex.CommandStackSciex.PliMercadoria.Salvar(NCM);
			_uowSciex.CommandStackSciex.Save();
			_uowSciex.CommandStackSciex.DetachEntries();

			PliFornecedorFabricanteEntity fornecedorefabricante = new PliFornecedorFabricanteEntity();
			fornecedorefabricante.IdPliMercadoria = NCM.IdPliMercadoria;
			fornecedorefabricante.CodigoAusenciaFabricante = fornecedorFabricante.CodigoAusenciaFabricante;
			fornecedorefabricante.CodigoPaisFabricante = fornecedorFabricante.CodigoPaisFabricante;
			fornecedorefabricante.CodigoPaisFornecedor = fornecedorFabricante.CodigoPaisFornecedor;
			fornecedorefabricante.DescricaoCidadeFabricante = fornecedorFabricante.DescricaoCidadeFabricante;
			fornecedorefabricante.DescricaoCidadeFornecedor = fornecedorFabricante.DescricaoCidadeFornecedor;
			fornecedorefabricante.DescricaoComplementoFabricante = fornecedorFabricante.DescricaoComplementoFabricante;
			fornecedorefabricante.DescricaoComplementoFornecedor = fornecedorFabricante.DescricaoComplementoFornecedor;
			fornecedorefabricante.DescricaoEstadoFabricante = fornecedorFabricante.DescricaoEstadoFabricante;
			fornecedorefabricante.DescricaoEstadoFornecedor = fornecedorFabricante.DescricaoEstadoFornecedor;
			fornecedorefabricante.DescricaoFabricante = fornecedorFabricante.DescricaoFabricante;
			fornecedorefabricante.DescricaoFornecedor = fornecedorFabricante.DescricaoFornecedor;
			fornecedorefabricante.DescricaoLogradouroFabricante = fornecedorFabricante.DescricaoLogradouroFabricante;
			fornecedorefabricante.DescricaoLogradouroFornecedor = fornecedorFabricante.DescricaoLogradouroFornecedor;
			fornecedorefabricante.DescricaoPaisFabricante = fornecedorFabricante.DescricaoPaisFabricante;
			fornecedorefabricante.DescricaoPaisFornecedor = fornecedorFabricante.DescricaoPaisFornecedor;
			fornecedorefabricante.NumeroFabricante = fornecedorFabricante.NumeroFabricante;
			fornecedorefabricante.NumeroFornecedor = fornecedorFabricante.NumeroFornecedor;

			_uowSciex.CommandStackSciex.DetachEntries();
			_uowSciex.CommandStackSciex.PliFornecedorFabricante.Salvar(fornecedorefabricante, true);
			_uowSciex.CommandStackSciex.Save();

			foreach (var item in merc.PliDetalheMercadoria)
			{
				PliDetalheMercadoriaEntity detalheMercadoria = new PliDetalheMercadoriaEntity();
				detalheMercadoria.CodigoDetalheMercadoria = item.CodigoDetalheMercadoria;
				detalheMercadoria.DescricaoComplemento = item.DescricaoComplemento;
				detalheMercadoria.DescricaoDetalhe = item.DescricaoDetalhe;
				detalheMercadoria.DescricaoMateriaPrimaBasica = item.DescricaoMateriaPrimaBasica;
				detalheMercadoria.DescricaoPartNumber = item.DescricaoPartNumber;
				detalheMercadoria.DescricaoREFFabricante = item.DescricaoREFFabricante;
				detalheMercadoria.DescricaoUnidadeMedida = item.DescricaoUnidadeMedida;

				detalheMercadoria.IdPliMercadoria = NCM.IdPliMercadoria;
				detalheMercadoria.IdUnidadeMedida = item.IdUnidadeMedida;
				detalheMercadoria.QuantidadeComercializada = item.QuantidadeComercializada;
				detalheMercadoria.RowVersion = item.RowVersion;
				detalheMercadoria.SiglaUnidadeMedida = item.SiglaUnidadeMedida;

				detalheMercadoria.ValorCondicaoVenda = item.ValorCondicaoVenda;
				detalheMercadoria.ValorTotalCondicaoVendaDolar = item.ValorTotalCondicaoVendaDolar;
				detalheMercadoria.ValorTotalCondicaoVendaReal = item.ValorTotalCondicaoVendaReal;
				detalheMercadoria.ValorUnitarioCondicaoVenda = item.ValorUnitarioCondicaoVenda;
				detalheMercadoria.ValorUnitarioCondicaoVendaDolar = item.ValorUnitarioCondicaoVendaDolar;

				_uowSciex.CommandStackSciex.DetachEntries();
				_uowSciex.CommandStackSciex.PliDetalheMercadoria.Salvar(detalheMercadoria);
				_uowSciex.CommandStackSciex.Save();
			}

			foreach (var item in merc.PliProcessoAnuente)
			{
				PliProcessoAnuenteEntity detalheProcessoAnuente = new PliProcessoAnuenteEntity();
				detalheProcessoAnuente.IdOrgaoAnuente = item.IdOrgaoAnuente;
				detalheProcessoAnuente.IdPliMercadoria = NCM.IdPliMercadoria;
				detalheProcessoAnuente.NumeroProcesso = item.NumeroProcesso;

				_uowSciex.CommandStackSciex.DetachEntries();
				_uowSciex.CommandStackSciex.PliProcessoAnuente.Salvar(detalheProcessoAnuente);
				_uowSciex.CommandStackSciex.Save();
			}

			return pliVM;
		}

		public bool Validar(PliVM pliVM)
		{
			if (pliVM.CodigoPliAplicao != -1 && (!string.IsNullOrEmpty(pliVM.NumCPFRepLegalSISCO)))
			{
				var cnpjUsuario = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoCpfCnpj.Replace(".", "").Replace("-", "").Replace("/", ""); ;
				return true;
			}
			else
				return false;
		}

		public PliVM Selecionar(long? idPli)
		{
			var pliVM = new PliVM();
			if (!idPli.HasValue)
			{
				return pliVM;
			}

			var pli = _uowSciex.QueryStackSciex.Pli.Selecionar(x => x.IdPLI == idPli);
			var pliTaxaDebito= _uowSciex.QueryStackSciex.TaxaPliDebito.Listar(p => p.IdPli == pli.IdPLI).OrderBy(k => k.IdDebito).FirstOrDefault();
			var pliTaxa = _uowSciex.QueryStackSciex.TaxaPli.Selecionar(p => p.IdPli == pli.IdPLI);

			if (pli == null)
			{
				return null;
			}
			
			var importador = _uowSciex.QueryStackSciex.ViewImportador.Selecionar(o => o.Cnpj == pli.Cnpj);
			var cnae = _uowCadsuf.QueryStack.ViewAtividadeEconomicaPrincipal.Selecionar(o => o.CNPJ == pli.Cnpj);

			var uticon = _uowSciex.QueryStackSciex.ControleImportacao.Listar(o => o.IdPliAplicacao == pli.IdPLIAplicacao).FirstOrDefault();

			pliVM = AutoMapper.Mapper.Map<PliVM>(pli);

			if(pli.PLIMercadoria != null && pli.PLIMercadoria.FirstOrDefault() != null && pli.PLIMercadoria.FirstOrDefault().Ali == null)
			{
				var erroProc = _uowSciex.QueryStackSciex.ErroProcessamento.Selecionar(o => o.IdPli == pli.IdPLI);
				if(erroProc != null)
					pliVM.DataProcessamento = erroProc.DataProcessamento.Value.ToShortDateString();
			}

			pliVM.Endereco = importador.Endereco;
			pliVM.Numero = importador.Numero;
			pliVM.Complemento = importador.Complemento;
			pliVM.Bairro = importador.Bairro;
			pliVM.CodigoMunicipio = importador.CodigoMunicipio.ToString();

			if (pliVM.DataDebitoGeracao == null)
			{
				pliVM.DescricaoDebito = " - ";
				pliVM.Situacao = " - ";
				pliVM.DescricaoValorGeralTcif = " - ";
			}
			else
			{
				pliVM.DescricaoDebito = pliTaxaDebito == null ? " - " : pliTaxaDebito.NumeroDebito.ToString() + "/" + pliTaxaDebito.AnoDebito.ToString();
				if (pliTaxaDebito == null)
				{
					pliVM.Situacao = " - ";
				}
				else
				{
					if (pliTaxaDebito.NumeroControleCobrancaTCIF == 0)
						pliVM.Situacao = "Cobrar Débito";
					if (pliTaxaDebito.NumeroControleCobrancaTCIF == 1)
						pliVM.Situacao = "Não Cobrar Débito";
					if (pliTaxaDebito.NumeroControleCobrancaTCIF == 2)
						pliVM.Situacao = "Suspender Débito";
				}
				pliVM.DescricaoValorGeralTcif = pliTaxa == null ? " - " : pliTaxa.ValorGeralTCIF.ToString();
			}

			//DI
			long? idDi = null;
			var li = pli.PLIMercadoria.Where(o => o.Li != null);
			if (li.Any())
				idDi = li.FirstOrDefault().Li.IdDI;

			if (idDi != null)
			{
				pliVM.UtilizadaDI = "Sim";
				pliVM.IdDI = idDi.ToString();
				var di = _uowSciex.QueryStackSciex.Di.Selecionar(p => p.IdDi == idDi);
				pliVM.NumeroDI = di.NumeroDi.ToString();
				pliVM.DataDiFormatada = di.DataRegistro.Value.ToShortDateString();
			}
			else
			{
				pliVM.UtilizadaDI = "Não";
			}
			


			pliVM.Municipio = importador.Municipio;
			pliVM.UF = importador.UF;
			pliVM.CEP = string.Format("{0:00000-000}", importador.CEP);
			pliVM.DescricaoCNAE = cnae.Descricao;
			pliVM.PaisCodigo = importador.CodigoPais;
			pliVM.PaisDescricao = importador.DescricaoPais;
			pliVM.Telefone = (importador.Telefone.Length == 10 ? string.Format("{0:(00) 0000-0000}", Convert.ToDecimal(importador.Telefone)) : string.Format("{0:(##) #####-####}", Convert.ToDecimal(importador.Telefone)));
			if(uticon != null)
			{
				pliVM.CodigoUtilizacao = uticon.CodigoUtilizacao.Codigo.ToString();
				pliVM.DescricaoUtilizacao = uticon.CodigoUtilizacao.Descricao.ToString();
				pliVM.CodigoConta = uticon.CodigoConta.Codigo.ToString();
				pliVM.DescricaoConta = uticon.CodigoConta.Descricao.ToString();
			}
			if (pli.NumeroLIReferencia != null)
			{
				if (!String.IsNullOrEmpty(pli.NumeroLIReferencia.Trim()))
				{
					var numLi = Int64.Parse(pli.NumeroLIReferencia);

					var idLi = _uowSciex.QueryStackSciex.Li.Selecionar(o => o.NumeroLi == numLi).IdPliMercadoria;
					pliVM.NumeroALISubstitutiva = _uowSciex.QueryStackSciex.Ali.Selecionar(o => o.IdPliMercadoria == idLi).NumeroAli;

					pliVM.IdLiReferencia = idLi;

					var idPLi = _uowSciex.QueryStackSciex.PliMercadoria.Selecionar(o => o.IdPliMercadoria == idLi).IdPLI;
					pliVM.NumeroPLISubstitutivo = _uowSciex.QueryStackSciex.Pli.Selecionar(o => o.IdPLI == idPLi).NumeroPli;

					pliVM.IdPLISubstitutivo = idPLi;
					pliVM.IdPliMercadoriaSubstitutivo = _uowSciex.QueryStackSciex.Ali.Selecionar(o => o.IdPliMercadoria == idLi).IdPliMercadoria;

					var NumeroPLISubstitutivoInt = Convert.ToInt32(pliVM.NumeroPLISubstitutivo);

					pliVM.AnoPliSubstitutivo = _uowSciex.QueryStackSciex.Pli.Selecionar(o => o.IdPLI == idPLi).Ano;
					pliVM.NumeroPliSubstitutivoConcatenado = (pliVM.AnoPliSubstitutivo + "/" + NumeroPLISubstitutivoInt.ToString("d6"));
				} 
			}

			if(pliVM.StatusAnaliseVisual == 1)
			{
				var plianalise = _uowSciex.QueryStackSciex.PliAnaliseVisual.Selecionar(o => o.IdPLI == pliVM.IdPLI);

				if (plianalise == null || plianalise.IdPLI == null || plianalise.StatusAnalise == 02)
				{
					pliVM.StatusPliAnalise = 2;
					pliVM.StatusPliAnaliseFormatado = "EM ANÁLISE VISUAL";
				}
				else if (plianalise.StatusAnalise == 07)
				{
					pliVM.StatusPliAnalise = plianalise.StatusAnalise;
					pliVM.StatusPliAnaliseFormatado = "ANÁLISE VISUAL OK";
				}
				else if (plianalise.StatusAnalise == 08)
				{
					pliVM.StatusPliAnalise = plianalise.StatusAnalise;
					pliVM.StatusPliAnaliseFormatado = "ANÁLISE VISUAL NÃO OK";
				}
				else if (plianalise.StatusAnalise == 09)
				{
					pliVM.StatusPliAnalise = plianalise.StatusAnalise;
					pliVM.StatusPliAnaliseFormatado = "ANÁLISE VISUAL PENDENTE";
				}
			}

			return pliVM;
		}

		public void Deletar(long id)
		{
			var pli = _uowSciex.QueryStackSciex.Pli.Selecionar(s => s.IdPLI == id);


			if (pli != null)
			{
				_uowSciex.CommandStackSciex.Pli.Apagar(pli.IdPLI);
			}
			_uowSciex.CommandStackSciex.Save();
		}

		public long GerarSequence(string cnpj, int ano)
		{
			long sequencia = 1;
			cnpj = cnpj.CnpjCpfUnformat();
			var pli = _uowSciex.QueryStackSciex.Pli.ListarGrafo(s => new PliVM
			{
				Cnpj = s.Cnpj,
				NumeroPli = s.NumeroPli,
				Ano = s.Ano
			},
			w => w.Cnpj == cnpj && w.Ano == ano).OrderByDescending(w => w.NumeroPli).Take(1);
			if (pli.Count() > 0)
			{
				sequencia = pli.FirstOrDefault().NumeroPli + 1;
			}
			return sequencia;
		}

		/// <summary>
		/// Rotina responsavel por gerar uma copia do PLI que está sendo passado o ID
		/// </summary>
		/// <param name="idPLi"></param>
		/// <returns>verdadeiro - copiado com sucesso, falso - problemas na copia</returns>
		public bool CopiarPli(long? idPLi)
		{
			try
			{

				int codigoStatusEmElaboracao = 20;
				PliVM pliAtual = Selecionar(idPLi);

				pliAtual.Ano = 0;
				pliAtual.NumeroPli = 0;
				pliAtual.IdPLI = null;
				pliAtual.NomeResponsavelRegistro = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoNome.Replace(".", "").Replace("-", "").Replace("/", ""); ;
				pliAtual.NumeroResponsavelRegistro = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoCpfCnpj.Replace(".", "").Replace("-", "").Replace("/", ""); ;
				pliAtual.DataCadastro = DateTime.Now;
				//buscar o status de Em Elaboracao
				pliAtual.StatusPli = (byte)EnumPliStatus.EM_ELABORAÇÃO;
				//gerar nova sequencia do pli junto com o ano atual
				var sequence = this.GerarSequence(pliAtual.Cnpj, DateTime.Now.Year);
				pliAtual.Ano = DateTime.Now.Year;
				pliAtual.NumeroPli = sequence;
				pliAtual.TipoOrigem = Convert.ToByte(EnumPliTipoOrigem.PliWeb);

				if(pliAtual.IdPLIAplicacao == 2)
				{
					_idPLiRetorno = _uowSciex.CommandStackSciex.Salvar(MontarCopiarPliSql(idPLi.ToString(),
					"'" + pliAtual.NumeroResponsavelRegistro + "'", "'" + pliAtual.NomeResponsavelRegistro + "'",
					pliAtual.NumeroPli.ToString(), pliAtual.Ano.ToString(), pliAtual.TipoOrigem.ToString()));
				}
				else
				{
					_idPLiRetorno = _uowSciex.CommandStackSciex.Salvar(MontarCopiarPliComercializacaoSql(idPLi.ToString(),
					"'" + pliAtual.NumeroResponsavelRegistro + "'", "'" + pliAtual.NomeResponsavelRegistro + "'",
					pliAtual.NumeroPli.ToString(), pliAtual.Ano.ToString(), pliAtual.TipoOrigem.ToString()));
				}

				return true;
			}
			catch (Exception ex)
			{ return false; }
		}

		private string MontarCopiarPliComercializacaoSql(string idPliAtual, string cpfResponsavel,
			string nomeResponsavel, string numeroPLI, string anoPLI, string tipoOrigem)
		{
			// BuildMyString.com generated code. Please enjoy your string responsibly.

			StringBuilder sb = new StringBuilder();

			sb.AppendLine("DECLARE @PLI_ID BIGINT; ");
			sb.AppendLine("DECLARE @PPR_ID BIGINT; ");
			sb.AppendLine("DECLARE @PME_ID BIGINT; ");
			sb.AppendLine("DECLARE @PDM_ID BIGINT; ");
			sb.AppendLine("DECLARE @PPA_ID INT; ");
			sb.AppendLine("DECLARE @PLI_ID_NOVO BIGINT; ");
			sb.AppendLine("DECLARE @PPR_ID_NOVO BIGINT; ");
			sb.AppendLine("DECLARE @PME_ID_NOVO BIGINT; ");
			sb.AppendLine("DECLARE @TIPO_ORIGEM_PLI INT; ");
			sb.AppendLine("DECLARE @ID_PLI_FORNECEDOR BIGINT; ");
			sb.AppendLine("CREATE TABLE #tabelamercadoria ");
			sb.AppendLine("  ( ");
			sb.AppendLine("     pme_id                              BIGINT, ");
			sb.AppendLine("     pli_id                              BIGINT, ");
			sb.AppendLine("     moe_id                              INT, ");
			sb.AppendLine("     inc_id                              INT, ");
			sb.AppendLine("     rtb_id                              INT, ");
			sb.AppendLine("     fle_id                              INT, ");
			sb.AppendLine("     fab_id                              INT, ");
			sb.AppendLine("     for_id                              INT, ");
			sb.AppendLine("     inf_id                              INT, ");
			sb.AppendLine("     mot_id                              INT, ");
			sb.AppendLine("     mop_id                              INT, ");
			sb.AppendLine("     ala_id                              INT, ");
			sb.AppendLine("     nld_id                              INT, ");
			sb.AppendLine("     ppr_id                              BIGINT, ");
			sb.AppendLine("     cut_id                              INT, ");
			sb.AppendLine("     cco_id                              INT, ");
			sb.AppendLine("     rfb_id_entrada                      INT, ");
			sb.AppendLine("     rfb_id_despacho                     INT, ");
			sb.AppendLine("     pai_co_mercadoria                   VARCHAR(3), ");
			sb.AppendLine("     pai_ds_mercadoria                   VARCHAR(50), ");
			sb.AppendLine("     pai_co_origem_fabricante            VARCHAR(3), ");
			sb.AppendLine("     pai_ds_origem_fabricante            VARCHAR(50), ");
			sb.AppendLine("     pme_nu_peso_liquido                 NUMERIC(15, 5), ");
			sb.AppendLine("     pme_qt_unid_medida_estatistica      NUMERIC(14, 5), ");
			sb.AppendLine("     pme_nu_comunicado_compra            VARCHAR(13), ");
			sb.AppendLine("     pme_nu_ato_drawback                 VARCHAR(13), ");
			sb.AppendLine("     pme_nu_agencia_secex                VARCHAR(5), ");
			sb.AppendLine("     pme_vl_cra                          NUMERIC(4, 2), ");
			sb.AppendLine("     pme_tp_cobcambial                   INT, ");
			sb.AppendLine("     pme_nu_cobcambial_limite_dias_pagto INT, ");
			sb.AppendLine("     pme_tp_acordo_tarifario             TINYINT, ");
			sb.AppendLine("     pme_ds_informacao_complementar      VARCHAR(4048), ");
			sb.AppendLine("     pme_tp_bem_encomenda                TINYINT, ");
			sb.AppendLine("     pme_tp_material_usado               TINYINT, ");
			sb.AppendLine("     pme_nu_ncm_destaque                 CHAR(3), ");
			sb.AppendLine("     pme_co_produto                      SMALLINT, ");
			sb.AppendLine("     pme_ds_produto                      VARCHAR(500), ");
			sb.AppendLine("     pme_co_tp_produto                   SMALLINT, ");
			sb.AppendLine("     pme_co_modelo_produto               SMALLINT, ");
			sb.AppendLine("     mer_co_ncm_mercadoria               CHAR(8), ");
			sb.AppendLine("     mer_ds_ncm_mercadoria               VARCHAR(120), ");
			sb.AppendLine("     pme_tp_fornecedor                   SMALLINT, ");
			sb.AppendLine("     pme_vl_total_condicao_venda         NUMERIC(15, 2), ");
			sb.AppendLine("     pme_vl_total_condicao_venda_real    NUMERIC(15, 2), ");
			sb.AppendLine("     pme_vl_total_condicao_venda_dolar   NUMERIC(15, 2), ");
			sb.AppendLine("     situacao                            INT ");
			sb.AppendLine("  ) ");
			sb.AppendLine("CREATE TABLE #tabeladetalhemercadoria ");
			sb.AppendLine("  ( ");
			sb.AppendLine("     pdm_id                      BIGINT, ");
			sb.AppendLine("     pme_id                      BIGINT, ");
			sb.AppendLine("     ume_co                      INT, ");
			sb.AppendLine("     ume_ds                      VARCHAR(40), ");
			sb.AppendLine("     ume_sg                      VARCHAR(5), ");
			sb.AppendLine("     dme_co_detalhe_mercadoria   INT, ");
			sb.AppendLine("     pdm_ds_detalhe              VARCHAR(254), ");
			sb.AppendLine("     pdm_ds_complemento          VARCHAR(3783), ");
			sb.AppendLine("     pdm_ds_mat_prima_basica     VARCHAR(20), ");
			sb.AppendLine("     pdm_ds_part_number          VARCHAR(20), ");
			sb.AppendLine("     pdm_ds_ref_fabricante       VARCHAR(20), ");
			sb.AppendLine("     pdm_qt_unid_comercializada  NUMERIC(14, 5), ");
			sb.AppendLine("     pdm_vl_unitario_cond_venda  NUMERIC(18, 7), ");
			sb.AppendLine("     pdm_vl_condicao_venda       NUMERIC(20, 12), ");
			sb.AppendLine("     pdm_vl_condicao_venda_real  NUMERIC(20, 12), ");
			sb.AppendLine("     pdm_vl_condicao_venda_dolar NUMERIC(20, 12), ");
			sb.AppendLine("     situacao                    INT ");
			sb.AppendLine("  ) ");
			sb.AppendLine("CREATE TABLE #tabelaprocessoanuente ");
			sb.AppendLine("  ( ");
			sb.AppendLine("     ppa_id          INT, ");
			sb.AppendLine("     pme_id          BIGINT, ");
			sb.AppendLine("     oan_id          INT, ");
			sb.AppendLine("     ppa_nu_processo VARCHAR(20), ");
			sb.AppendLine("     situacao        INT ");
			sb.AppendLine("  ) ");
			sb.AppendLine("BEGIN try -- INICIA TRATAMENTO DE ERRO ");
			sb.AppendLine("    BEGIN TRANSACTION; ");
			sb.AppendLine("   set @PLI_ID = idPliAtual;");
			sb.AppendLine("    INSERT INTO sciex_pli ");
			sb.AppendLine("    SELECT pap_id, ");
			sb.AppendLine("           NumeroPLI, ");
			sb.AppendLine("           AnoPLI, ");
			sb.AppendLine("           pli_nu_cnpj, ");
			sb.AppendLine("           ins_co, ");
			sb.AppendLine("           set_co, ");
			sb.AppendLine("           set_ds, ");
			sb.AppendLine("           pli_tp_documento, ");
			sb.AppendLine("           pli_st_analise_visual, ");
			sb.AppendLine("           pli_st_distribuicao, ");
			sb.AppendLine("           NULL, ");
			sb.AppendLine("           NULL, ");
			sb.AppendLine("           NULL, ");
			sb.AppendLine("           NULL, ");
			sb.AppendLine("           NULL, ");
			sb.AppendLine("           NULL, ");
			sb.AppendLine("           pli_nu_li_referencia, ");
			sb.AppendLine("           pli_nu_di_referencia, ");
			sb.AppendLine("           pli_nu_pexpam, ");
			sb.AppendLine("           pli_nu_ano_pexpam, ");
			sb.AppendLine("           pli_nu_lote_pexpam, ");
			sb.AppendLine("           NULL, ");
			sb.AppendLine("           TipoOrigem, ");
			sb.AppendLine("           CPFResponsavel, ");
			sb.AppendLine("           NomeResponsavel, ");
			sb.AppendLine("           Getdate(), ");
			sb.AppendLine("           NULL, ");
			sb.AppendLine("           pli_nu_cpf_rep_legal_siscomex, ");
			sb.AppendLine("           pli_co_cnae, ");
			sb.AppendLine("           pli_vl_total_condicao_venda, ");
			sb.AppendLine("           pli_vl_total_condicao_venda_real, ");
			sb.AppendLine("           pli_vl_total_condicao_venda_dolar, ");
			sb.AppendLine("           NULL, ");
			sb.AppendLine("           imp_ds_razao_social, ");
			sb.AppendLine("           20, ");
			sb.AppendLine("           NULL, ");
			sb.AppendLine("           NULL, ");
			sb.AppendLine("           NULL, ");
			sb.AppendLine("           NULL, ");
			sb.AppendLine("           NULL, ");
			sb.AppendLine("           NULL, ");
			sb.AppendLine("           NULL ");
			sb.AppendLine("    FROM   sciex_pli ");
			sb.AppendLine("    WHERE  pli_id = @PLI_ID");
			sb.AppendLine("    SELECT @TIPO_ORIGEM_PLI = pli_tp_origem ");
			sb.AppendLine("    FROM   sciex_pli ");
			sb.AppendLine("    WHERE  pli_id = @PLI_ID ");
			sb.AppendLine("    SELECT @PLI_ID_NOVO = @@IDENTITY; ");
			sb.AppendLine("    INSERT INTO sciex_pli_historico ");
			sb.AppendLine("                (pli_id, ");
			sb.AppendLine("                 phi_dh_evento, ");
			sb.AppendLine("                 phi_nu_cpfcnpj_responsavel, ");
			sb.AppendLine("                 phi_no_responsavel, ");
			sb.AppendLine("                 phi_ds_observacao, ");
			sb.AppendLine("                 pli_st_pli, ");
			sb.AppendLine("                 pli_st_pli_descricao) ");
			sb.AppendLine("    VALUES      ( @PLI_ID_NOVO, ");
			sb.AppendLine("                  Getdate(), ");
			sb.AppendLine("                  '04817052000106', ");
			sb.AppendLine("				  'UsuarioMock Nome', ");
			sb.AppendLine("                  'CADASTRO DO PLI', ");
			sb.AppendLine("                  20, ");
			sb.AppendLine("                  'EM ELABORAÇÃO' ) ");
			sb.AppendLine("INSERT INTO #tabelamercadoria ");
			sb.AppendLine("          SELECT pme_id, ");
			sb.AppendLine("                 pli_id, ");
			sb.AppendLine("                 moe_id, ");
			sb.AppendLine("                 inc_id, ");
			sb.AppendLine("                 rtb_id, ");
			sb.AppendLine("                 fle_id, ");
			sb.AppendLine("                 fab_id, ");
			sb.AppendLine("                 for_id, ");
			sb.AppendLine("                 inf_id, ");
			sb.AppendLine("                 mot_id, ");
			sb.AppendLine("                 mop_id, ");
			sb.AppendLine("                 ala_id, ");
			sb.AppendLine("                 nld_id, ");
			sb.AppendLine("                 ppr_id, ");
			sb.AppendLine("                 cut_id, ");
			sb.AppendLine("                 cco_id, ");
			sb.AppendLine("                 rfb_id_entrada, ");
			sb.AppendLine("                 rfb_id_despacho, ");
			sb.AppendLine("                 pai_co_mercadoria, ");
			sb.AppendLine("                 pai_ds_mercadoria, ");
			sb.AppendLine("                 pai_co_origem_fabricante, ");
			sb.AppendLine("                 pai_ds_origem_fabricante, ");
			sb.AppendLine("                 pme_nu_peso_liquido, ");
			sb.AppendLine("                 pme_qt_unid_medida_estatistica, ");
			sb.AppendLine("                 pme_nu_comunicado_compra, ");
			sb.AppendLine("                 pme_nu_ato_drawback, ");
			sb.AppendLine("                 pme_nu_agencia_secex, ");
			sb.AppendLine("                 pme_vl_cra, ");
			sb.AppendLine("                 pme_tp_cobcambial, ");
			sb.AppendLine("                 pme_nu_cobcambial_limite_dias_pagto, ");
			sb.AppendLine("                 pme_tp_acordo_tarifario, ");
			sb.AppendLine("                 pme_ds_informacao_complementar, ");
			sb.AppendLine("                 pme_tp_bem_encomenda, ");
			sb.AppendLine("                 pme_tp_material_usado, ");
			sb.AppendLine("                 pme_nu_ncm_destaque, ");
			sb.AppendLine("                 pme_co_produto, ");
			sb.AppendLine("                 pme_ds_produto, ");
			sb.AppendLine("                 pme_co_tp_produto, ");
			sb.AppendLine("                 pme_co_modelo_produto, ");
			sb.AppendLine("                 mer_co_ncm_mercadoria, ");
			sb.AppendLine("                 mer_ds_ncm_mercadoria, ");
			sb.AppendLine("                 pme_tp_fornecedor, ");
			sb.AppendLine("                 pme_vl_total_condicao_venda, ");
			sb.AppendLine("                 pme_vl_total_condicao_venda_real, ");
			sb.AppendLine("                 pme_vl_total_condicao_venda_dolar, ");
			sb.AppendLine("                 1 ");
			sb.AppendLine("          FROM   sciex_pli_mercadoria ");
			sb.AppendLine("          WHERE  pli_id = @PLI_ID ");
			sb.AppendLine("          WHILE EXISTS(SELECT * ");
			sb.AppendLine("                       FROM   #tabelamercadoria ");
			sb.AppendLine("                       WHERE  situacao = 1) ");
			sb.AppendLine("            BEGIN ");
			sb.AppendLine("                SELECT TOP 1 @PME_ID = pme_id ");
			sb.AppendLine("                FROM   #tabelamercadoria ");
			sb.AppendLine("                WHERE  situacao = 1 ");
			sb.AppendLine("                INSERT INTO sciex_pli_mercadoria ");
			sb.AppendLine("                            (pli_id, ");
			sb.AppendLine("                             moe_id, ");
			sb.AppendLine("                             inc_id, ");
			sb.AppendLine("                             rtb_id, ");
			sb.AppendLine("                             fle_id, ");
			sb.AppendLine("                             fab_id, ");
			sb.AppendLine("                             for_id, ");
			sb.AppendLine("                             inf_id, ");
			sb.AppendLine("                             mot_id, ");
			sb.AppendLine("                             mop_id, ");
			sb.AppendLine("                             ala_id, ");
			sb.AppendLine("                             nld_id, ");
			sb.AppendLine("                             ppr_id, ");
			sb.AppendLine("                             cut_id, ");
			sb.AppendLine("                             cco_id, ");
			sb.AppendLine("                             rfb_id_entrada, ");
			sb.AppendLine("                             rfb_id_despacho, ");
			sb.AppendLine("                             pai_co_mercadoria, ");
			sb.AppendLine("                             pai_ds_mercadoria, ");
			sb.AppendLine("                             pai_co_origem_fabricante, ");
			sb.AppendLine("                             pai_ds_origem_fabricante, ");
			sb.AppendLine("                             pme_nu_peso_liquido, ");
			sb.AppendLine("                             pme_qt_unid_medida_estatistica, ");
			sb.AppendLine("                             pme_nu_comunicado_compra, ");
			sb.AppendLine("                             pme_nu_ato_drawback, ");
			sb.AppendLine("                             pme_nu_agencia_secex, ");
			sb.AppendLine("                             pme_vl_cra, ");
			sb.AppendLine("                             pme_tp_cobcambial, ");
			sb.AppendLine("                             pme_nu_cobcambial_limite_dias_pagto, ");
			sb.AppendLine("                             pme_tp_acordo_tarifario, ");
			sb.AppendLine("                             pme_ds_informacao_complementar, ");
			sb.AppendLine("                             pme_tp_bem_encomenda, ");
			sb.AppendLine("                             pme_tp_material_usado, ");
			sb.AppendLine("                             pme_nu_ncm_destaque, ");
			sb.AppendLine("                             pme_co_produto, ");
			sb.AppendLine("                             pme_ds_produto, ");
			sb.AppendLine("                             pme_co_tp_produto, ");
			sb.AppendLine("                             pme_co_modelo_produto, ");
			sb.AppendLine("                             mer_co_ncm_mercadoria, ");
			sb.AppendLine("                             mer_ds_ncm_mercadoria, ");
			sb.AppendLine("                             pme_tp_fornecedor, ");
			sb.AppendLine("                             pme_vl_total_condicao_venda, ");
			sb.AppendLine("                             pme_vl_total_condicao_venda_real, ");
			sb.AppendLine("                             pme_vl_total_condicao_venda_dolar) ");
			sb.AppendLine("                SELECT @PLI_ID_NOVO, ");
			sb.AppendLine("                       moe_id, ");
			sb.AppendLine("                       inc_id, ");
			sb.AppendLine("                       rtb_id, ");
			sb.AppendLine("                       fle_id, ");
			sb.AppendLine("                       fab_id, ");
			sb.AppendLine("                       for_id, ");
			sb.AppendLine("                       inf_id, ");
			sb.AppendLine("                       mot_id, ");
			sb.AppendLine("                       mop_id, ");
			sb.AppendLine("                       ala_id, ");
			sb.AppendLine("                       nld_id,");
			sb.AppendLine("					   ppr_id, ");
			sb.AppendLine("                       cut_id, ");
			sb.AppendLine("                       cco_id, ");
			sb.AppendLine("                       rfb_id_entrada, ");
			sb.AppendLine("                       rfb_id_despacho, ");
			sb.AppendLine("                       pai_co_mercadoria, ");
			sb.AppendLine("                       pai_ds_mercadoria, ");
			sb.AppendLine("                       pai_co_origem_fabricante, ");
			sb.AppendLine("                       pai_ds_origem_fabricante, ");
			sb.AppendLine("                       pme_nu_peso_liquido, ");
			sb.AppendLine("                       pme_qt_unid_medida_estatistica, ");
			sb.AppendLine("                       pme_nu_comunicado_compra, ");
			sb.AppendLine("                       pme_nu_ato_drawback, ");
			sb.AppendLine("                       pme_nu_agencia_secex, ");
			sb.AppendLine("                       pme_vl_cra, ");
			sb.AppendLine("                       pme_tp_cobcambial, ");
			sb.AppendLine("                       pme_nu_cobcambial_limite_dias_pagto, ");
			sb.AppendLine("                       pme_tp_acordo_tarifario, ");
			sb.AppendLine("                       pme_ds_informacao_complementar, ");
			sb.AppendLine("                       pme_tp_bem_encomenda, ");
			sb.AppendLine("                       pme_tp_material_usado, ");
			sb.AppendLine("                       pme_nu_ncm_destaque, ");
			sb.AppendLine("                       pme_co_produto, ");
			sb.AppendLine("                       pme_ds_produto, ");
			sb.AppendLine("                       pme_co_tp_produto, ");
			sb.AppendLine("                       pme_co_modelo_produto, ");
			sb.AppendLine("                       mer_co_ncm_mercadoria, ");
			sb.AppendLine("                       mer_ds_ncm_mercadoria, ");
			sb.AppendLine("                       pme_tp_fornecedor, ");
			sb.AppendLine("                       pme_vl_total_condicao_venda, ");
			sb.AppendLine("                       pme_vl_total_condicao_venda_real, ");
			sb.AppendLine("                       pme_vl_total_condicao_venda_dolar ");
			sb.AppendLine("                FROM   #tabelamercadoria ");
			sb.AppendLine("                WHERE  pme_id = @PME_ID ");
			sb.AppendLine("                SELECT @PME_ID_NOVO = @@IDENTITY; ");
			sb.AppendLine("                INSERT INTO sciex_pli_fornecedor_fabricante ");
			sb.AppendLine("                SELECT @PME_ID_NOVO, ");
			sb.AppendLine("                       pff_ds_fornecedor, ");
			sb.AppendLine("                       pff_ds_logradouro_forn, ");
			sb.AppendLine("                       pff_nu_forn, ");
			sb.AppendLine("                       pff_ds_complemento_forn, ");
			sb.AppendLine("                       pff_ds_cidade_forn, ");
			sb.AppendLine("                       pff_ds_estado_forn, ");
			sb.AppendLine("                       pff_co_pais_forn, ");
			sb.AppendLine("                       pff_co_ausencia_fabricante, ");
			sb.AppendLine("                       pff_ds_fabricante, ");
			sb.AppendLine("                       pff_ds_logradouro_fab, ");
			sb.AppendLine("                       pff_nu_fab, ");
			sb.AppendLine("                       pff_ds_complemento_fab, ");
			sb.AppendLine("                       pff_ds_cidade_fab, ");
			sb.AppendLine("                       pff_ds_estado_fab, ");
			sb.AppendLine("                       pff_co_pais_fab, ");
			sb.AppendLine("                       pff_ds_pais_fab, ");
			sb.AppendLine("                       pff_ds_pais_forn ");
			sb.AppendLine("                FROM   sciex_pli_fornecedor_fabricante ");
			sb.AppendLine("                WHERE  pme_id = @PME_ID ");
			sb.AppendLine("                INSERT INTO #tabeladetalhemercadoria ");
			sb.AppendLine("                SELECT pdm_id, ");
			sb.AppendLine("                       pme_id, ");
			sb.AppendLine("                       ume_co, ");
			sb.AppendLine("                       ume_ds, ");
			sb.AppendLine("                       ume_sg, ");
			sb.AppendLine("                       dme_co_detalhe_mercadoria, ");
			sb.AppendLine("                       pdm_ds_detalhe, ");
			sb.AppendLine("                       pdm_ds_complemento, ");
			sb.AppendLine("                       pdm_ds_mat_prima_basica, ");
			sb.AppendLine("                       pdm_ds_part_number, ");
			sb.AppendLine("                       pdm_ds_ref_fabricante, ");
			sb.AppendLine("                       pdm_qt_unid_comercializada, ");
			sb.AppendLine("                       pdm_vl_unitario_cond_venda, ");
			sb.AppendLine("                       pdm_vl_condicao_venda, ");
			sb.AppendLine("                       NULL, ");
			sb.AppendLine("                       NULL, ");
			sb.AppendLine("                       1 ");
			sb.AppendLine("                FROM   sciex_pli_detalhe_mercadoria ");
			sb.AppendLine("                WHERE  pme_id = @PME_ID ");
			sb.AppendLine("                WHILE EXISTS(SELECT * ");
			sb.AppendLine("                             FROM   #tabeladetalhemercadoria ");
			sb.AppendLine("                             WHERE  situacao = 1) ");
			sb.AppendLine("                  BEGIN ");
			sb.AppendLine("                      SELECT TOP 1 @PDM_ID = pdm_id ");
			sb.AppendLine("                      FROM   #tabeladetalhemercadoria ");
			sb.AppendLine("                      WHERE  situacao = 1 ");
			sb.AppendLine("                      INSERT INTO sciex_pli_detalhe_mercadoria ");
			sb.AppendLine("                                  (pme_id, ");
			sb.AppendLine("                                   ume_co, ");
			sb.AppendLine("                                   ume_ds, ");
			sb.AppendLine("                                   ume_sg, ");
			sb.AppendLine("                                   dme_co_detalhe_mercadoria, ");
			sb.AppendLine("                                   pdm_ds_detalhe, ");
			sb.AppendLine("                                   pdm_ds_complemento, ");
			sb.AppendLine("                                   pdm_ds_mat_prima_basica, ");
			sb.AppendLine("                                   pdm_ds_part_number, ");
			sb.AppendLine("                                   pdm_ds_ref_fabricante, ");
			sb.AppendLine("                                   pdm_qt_unid_comercializada, ");
			sb.AppendLine("                                   pdm_vl_unitario_cond_venda, ");
			sb.AppendLine("                                   pdm_vl_condicao_venda, ");
			sb.AppendLine("                                   pdm_vl_condicao_venda_real, ");
			sb.AppendLine("                                   pdm_vl_condicao_venda_dolar) ");
			sb.AppendLine("                      SELECT @PME_ID_NOVO, ");
			sb.AppendLine("                             ume_co, ");
			sb.AppendLine("                             ume_ds, ");
			sb.AppendLine("                             ume_sg, ");
			sb.AppendLine("                             dme_co_detalhe_mercadoria, ");
			sb.AppendLine("                             pdm_ds_detalhe, ");
			sb.AppendLine("                             pdm_ds_complemento, ");
			sb.AppendLine("                             pdm_ds_mat_prima_basica, ");
			sb.AppendLine("                             pdm_ds_part_number, ");
			sb.AppendLine("                             pdm_ds_ref_fabricante, ");
			sb.AppendLine("                             pdm_qt_unid_comercializada, ");
			sb.AppendLine("                             pdm_vl_unitario_cond_venda, ");
			sb.AppendLine("                             pdm_vl_condicao_venda, ");
			sb.AppendLine("                             NULL, ");
			sb.AppendLine("                             NULL ");
			sb.AppendLine("                      FROM   #tabeladetalhemercadoria ");
			sb.AppendLine("                      WHERE  pdm_id = @PDM_ID ");
			sb.AppendLine("                      UPDATE #tabeladetalhemercadoria ");
			sb.AppendLine("                      SET    situacao = 2 ");
			sb.AppendLine("                      WHERE  pdm_id = @PDM_ID; ");
			sb.AppendLine("                  END ");
			sb.AppendLine("                DELETE FROM #tabeladetalhemercadoria ");
			sb.AppendLine("                INSERT INTO #tabelaprocessoanuente ");
			sb.AppendLine("                SELECT ppa_id, ");
			sb.AppendLine("                       pme_id, ");
			sb.AppendLine("                       oan_id, ");
			sb.AppendLine("                       ppa_nu_processo, ");
			sb.AppendLine("                       1 ");
			sb.AppendLine("                FROM   sciex_pli_processo_anuente ");
			sb.AppendLine("                WHERE  pme_id = @PME_ID; ");
			sb.AppendLine("                WHILE EXISTS(SELECT * ");
			sb.AppendLine("                             FROM   #tabelaprocessoanuente ");
			sb.AppendLine("                             WHERE  situacao = 1) ");
			sb.AppendLine("                  BEGIN ");
			sb.AppendLine("                      SELECT TOP 1 @PPA_ID = ppa_id ");
			sb.AppendLine("                      FROM   #tabelaprocessoanuente ");
			sb.AppendLine("                      WHERE  situacao = 1 ");
			sb.AppendLine("                      INSERT INTO sciex_pli_processo_anuente ");
			sb.AppendLine("                      SELECT @PME_ID_NOVO, ");
			sb.AppendLine("                             oan_id, ");
			sb.AppendLine("                             ppa_nu_processo ");
			sb.AppendLine("                      FROM   #tabelaprocessoanuente ");
			sb.AppendLine("                      WHERE  ppa_id = @PPA_ID ");
			sb.AppendLine("                      UPDATE #tabelaprocessoanuente ");
			sb.AppendLine("                      SET    situacao = 2 ");
			sb.AppendLine("                      WHERE  ppa_id = @PPA_ID ");
			sb.AppendLine("                  END ");
			sb.AppendLine("                DELETE FROM #tabelaprocessoanuente ");
			sb.AppendLine("                UPDATE #tabelamercadoria ");
			sb.AppendLine("                SET    situacao = 2 ");
			sb.AppendLine("                WHERE  pme_id = @PME_ID ");
			sb.AppendLine("            END");
			sb.AppendLine("DELETE FROM #tabelamercadoria ");
			sb.AppendLine("    COMMIT ");
			sb.AppendLine("END try ");
			sb.AppendLine("BEGIN catch ");
			sb.AppendLine("    IF @@TRANCOUNT > 0 ");
			sb.AppendLine("      ROLLBACK ");
			sb.AppendLine("END catch; ");
			sb.AppendLine("SELECT @PLI_ID_NOVO ");
			sb.AppendLine("DROP TABLE #tabelamercadoria ");
			sb.AppendLine("DROP TABLE #tabelaprocessoanuente ");
			sb.AppendLine("DROP TABLE #tabeladetalhemercadoria ");

			return sb.ToString().Replace("idPliAtual", idPliAtual)
				.Replace("NumeroPLI", numeroPLI)
				.Replace("AnoPLI", anoPLI)
				.Replace("CPFResponsavel", cpfResponsavel)
				.Replace("NomeResponsavel", nomeResponsavel)
				.Replace("TipoOrigem", tipoOrigem); ;
		}

		private string MontarCopiarPliSql(string idPliAtual, string cpfResponsavel,
			string nomeResponsavel, string numeroPLI, string anoPLI, string tipoOrigem)
		{

			string SQL =
			@"				
				DECLARE @PLI_ID bigint;
				DECLARE @PPR_ID bigint;
				DECLARE @PME_ID bigint;
				DECLARE @PDM_ID bigint;
				DECLARE @PPA_ID int;

				DECLARE @PLI_ID_NOVO bigint;
				DECLARE @PPR_ID_NOVO bigint;
				DECLARE @PME_ID_NOVO bigint;	

				DECLARE @TIPO_ORIGEM_PLI int;	
				DECLARE @ID_PLI_FORNECEDOR BIGINT;		

				CREATE TABLE #TabelaProduto
				(
					PPR_ID	bigint,	PLI_ID	bigint,	PPR_CO_PRODUTO	smallint, PPR_CO_TP_PRODUTO	smallint, 
					PPR_CO_MODELO_PRODUTO smallint, PPR_DS_PRODUTO varchar(400), SITUACAO int
				)

				
				CREATE TABLE #TabelaMercadoria
				(
					PME_ID	bigint, PLI_ID	bigint, MOE_ID	int, INC_ID	int, RTB_ID	int, FLE_ID	int, FAB_ID	int, FOR_ID	int, INF_ID	int,
					MOT_ID	int, MOP_ID	int, ALA_ID	int, NLD_ID	int, PPR_ID	bigint, CUT_ID int, CCO_ID int, RFB_ID_ENTRADA	int, RFB_ID_DESPACHO int, 
					PAI_CO_MERCADORIA varchar(3), PAI_DS_MERCADORIA	varchar(50), PAI_CO_ORIGEM_FABRICANTE varchar(3), 
					PAI_DS_ORIGEM_FABRICANTE varchar(50), PME_NU_PESO_LIQUIDO numeric(15,5), PME_QT_UNID_MEDIDA_ESTATISTICA numeric(14,5),
					PME_NU_COMUNICADO_COMPRA varchar(13), PME_NU_ATO_DRAWBACK varchar(13), PME_NU_AGENCIA_SECEX	varchar(5),
					PME_VL_CRA	numeric(4,2), PME_TP_COBCAMBIAL	int, PME_NU_COBCAMBIAL_LIMITE_DIAS_PAGTO int, PME_TP_ACORDO_TARIFARIO tinyint,
					PME_DS_INFORMACAO_COMPLEMENTAR	varchar(4048), PME_TP_BEM_ENCOMENDA	tinyint, PME_TP_MATERIAL_USADO	tinyint,
					PME_NU_NCM_DESTAQUE	char(3), PME_CO_PRODUTO	smallint, PME_DS_PRODUTO varchar(500), PME_CO_TP_PRODUTO smallint,
					PME_CO_MODELO_PRODUTO	smallint, MER_CO_NCM_MERCADORIA	char(8), MER_DS_NCM_MERCADORIA	varchar(120),
					PME_TP_FORNECEDOR smallint, PME_VL_TOTAL_CONDICAO_VENDA	numeric(15,2), PME_VL_TOTAL_CONDICAO_VENDA_REAL numeric(15,2),
					PME_VL_TOTAL_CONDICAO_VENDA_DOLAR numeric(15,2), SITUACAO int
				)

				CREATE TABLE #TabelaDetalheMercadoria
				(
				PDM_ID bigint, PME_ID bigint, UME_CO int, UME_DS varchar(40), UME_SG varchar(5), DME_CO_DETALHE_MERCADORIA int, 
				PDM_DS_DETALHE varchar(254),PDM_DS_COMPLEMENTO	varchar(3783), PDM_DS_MAT_PRIMA_BASICA varchar(20), 
				PDM_DS_PART_NUMBER varchar(20), PDM_DS_REF_FABRICANTE varchar(20), PDM_QT_UNID_COMERCIALIZADA numeric(14,5), 
				PDM_VL_UNITARIO_COND_VENDA numeric(18,7), PDM_VL_CONDICAO_VENDA numeric(20,12), PDM_VL_CONDICAO_VENDA_REAL numeric(20,12),
				PDM_VL_CONDICAO_VENDA_DOLAR	numeric(20,12), SITUACAO int
				)

				CREATE TABLE #TabelaProcessoAnuente
				(
					PPA_ID	int, PME_ID	bigint, OAN_ID int, PPA_NU_PROCESSO varchar(20), SITUACAO int
				)

				BEGIN TRY   -- INICIA TRATAMENTO DE ERRO

					BEGIN TRANSACTION; 

						INSERT INTO SCIEX_PLI						
						SELECT  PAP_ID, NumeroPLI, AnoPLI, PLI_NU_CNPJ, INS_CO, SET_CO, SET_DS, PLI_TP_DOCUMENTO,
							PLI_ST_ANALISE_VISUAL, PLI_ST_DISTRIBUICAO, NULL, NULL, NULL, NULL, NULL, NULL, 
							PLI_NU_LI_REFERENCIA, PLI_NU_DI_REFERENCIA, PLI_NU_PEXPAM, PLI_NU_ANO_PEXPAM, 
							PLI_NU_LOTE_PEXPAM, NULL, 1, CPFResponsavel, NomeResponsavel,
							GETDATE(), NULL, PLI_NU_CPF_REP_LEGAL_SISCOMEX, PLI_CO_CNAE,
							PLI_VL_TOTAL_CONDICAO_VENDA, PLI_VL_TOTAL_CONDICAO_VENDA_REAL, 
							PLI_VL_TOTAL_CONDICAO_VENDA_DOLAR, NULL, 
							IMP_DS_RAZAO_SOCIAL, 20,  NULL, NULL, NULL, NULL, NULL, NULL, NULL
						FROM SCIEX_PLI
						WHERE PLI_ID = idPliAtual

						SELECT
							@TIPO_ORIGEM_PLI = PLI_TP_ORIGEM
						FROM SCIEX_PLI
						WHERE PLI_ID = idPliAtual
				
						SELECT @PLI_ID_NOVO = @@IDENTITY; 

						INSERT INTO SCIEX_PLI_HISTORICO
						(PLI_ID, PHI_DH_EVENTO, PHI_NU_CPFCNPJ_RESPONSAVEL, PHI_NO_RESPONSAVEL, PHI_DS_OBSERVACAO, PLI_ST_PLI, PLI_ST_PLI_DESCRICAO)
						VALUES
						(
							@PLI_ID_NOVO, GETDATE(), CPFResponsavel, NomeResponsavel, 'CADASTRO DO PLI', 20, 'EM ELABORAÇÃO'
						)


						INSERT INTO #TabelaProduto
						SELECT PPR_ID, PLI_ID, PPR_CO_PRODUTO, PPR_CO_TP_PRODUTO, PPR_CO_MODELO_PRODUTO, PPR_DS_PRODUTO, 1
						FROM SCIEX_PLI_PRODUTO
						WHERE PLI_ID = idPliAtual

						WHILE EXISTS(SELECT * FROM #TabelaProduto WHERE SITUACAO = 1)
						BEGIN

									SELECT TOP 1 @PLI_ID = PLI_ID, @PPR_ID = PPR_ID FROM #TabelaProduto WHERE SITUACAO = 1

									INSERT INTO SCIEX_PLI_PRODUTO
									SELECT 	@PLI_ID_NOVO, PPR_CO_PRODUTO, PPR_CO_TP_PRODUTO, PPR_CO_MODELO_PRODUTO, PPR_DS_PRODUTO, NULL
									FROM #TabelaProduto 
									WHERE PPR_ID = @PPR_ID

									SELECT @PPR_ID_NOVO = @@IDENTITY;  	
	
									INSERT INTO #TabelaMercadoria
									SELECT  PME_ID, PLI_ID, MOE_ID, INC_ID, RTB_ID, FLE_ID, FAB_ID, FOR_ID, INF_ID,
											MOT_ID, MOP_ID, ALA_ID, NLD_ID, PPR_ID, CUT_ID, CCO_ID, RFB_ID_ENTRADA, RFB_ID_DESPACHO, 
											PAI_CO_MERCADORIA , PAI_DS_MERCADORIA, PAI_CO_ORIGEM_FABRICANTE, 
											PAI_DS_ORIGEM_FABRICANTE, PME_NU_PESO_LIQUIDO, PME_QT_UNID_MEDIDA_ESTATISTICA,
											PME_NU_COMUNICADO_COMPRA , PME_NU_ATO_DRAWBACK , PME_NU_AGENCIA_SECEX	,
											PME_VL_CRA, PME_TP_COBCAMBIAL, PME_NU_COBCAMBIAL_LIMITE_DIAS_PAGTO, PME_TP_ACORDO_TARIFARIO,
											PME_DS_INFORMACAO_COMPLEMENTAR, PME_TP_BEM_ENCOMENDA, PME_TP_MATERIAL_USADO,
											PME_NU_NCM_DESTAQUE, PME_CO_PRODUTO, PME_DS_PRODUTO, PME_CO_TP_PRODUTO,
											PME_CO_MODELO_PRODUTO, MER_CO_NCM_MERCADORIA, MER_DS_NCM_MERCADORIA	,
											PME_TP_FORNECEDOR , PME_VL_TOTAL_CONDICAO_VENDA, 
											PME_VL_TOTAL_CONDICAO_VENDA_REAL, PME_VL_TOTAL_CONDICAO_VENDA_DOLAR , 1
									FROM SCIEX_PLI_MERCADORIA
									WHERE PLI_ID = @PLI_ID AND PPR_ID = @PPR_ID
	
									WHILE EXISTS(SELECT * FROM #TabelaMercadoria WHERE SITUACAO = 1)
									BEGIN

										SELECT TOP 1 @PME_ID = PME_ID FROM #TabelaMercadoria WHERE SITUACAO = 1

										INSERT INTO SCIEX_PLI_MERCADORIA
										(PLI_ID, MOE_ID, INC_ID, RTB_ID, FLE_ID, FAB_ID, FOR_ID, INF_ID, MOT_ID, MOP_ID,
										 ALA_ID, NLD_ID, PPR_ID, CUT_ID, CCO_ID, RFB_ID_ENTRADA, RFB_ID_DESPACHO,
										 PAI_CO_MERCADORIA, PAI_DS_MERCADORIA, PAI_CO_ORIGEM_FABRICANTE,
										 PAI_DS_ORIGEM_FABRICANTE, PME_NU_PESO_LIQUIDO, PME_QT_UNID_MEDIDA_ESTATISTICA,
										 PME_NU_COMUNICADO_COMPRA, PME_NU_ATO_DRAWBACK, PME_NU_AGENCIA_SECEX,
										 PME_VL_CRA, PME_TP_COBCAMBIAL, PME_NU_COBCAMBIAL_LIMITE_DIAS_PAGTO,
										 PME_TP_ACORDO_TARIFARIO, PME_DS_INFORMACAO_COMPLEMENTAR, PME_TP_BEM_ENCOMENDA,
										 PME_TP_MATERIAL_USADO, PME_NU_NCM_DESTAQUE, PME_CO_PRODUTO, PME_DS_PRODUTO,
										 PME_CO_TP_PRODUTO, PME_CO_MODELO_PRODUTO, MER_CO_NCM_MERCADORIA,
										 MER_DS_NCM_MERCADORIA, PME_TP_FORNECEDOR, PME_VL_TOTAL_CONDICAO_VENDA,
										 PME_VL_TOTAL_CONDICAO_VENDA_REAL, PME_VL_TOTAL_CONDICAO_VENDA_DOLAR)
										SELECT  @PLI_ID_NOVO, MOE_ID, INC_ID, RTB_ID, FLE_ID, FAB_ID, FOR_ID, INF_ID, MOT_ID, MOP_ID, 
												ALA_ID,	NLD_ID, @PPR_ID_NOVO, CUT_ID, CCO_ID, RFB_ID_ENTRADA, RFB_ID_DESPACHO, PAI_CO_MERCADORIA, 
												PAI_DS_MERCADORIA, PAI_CO_ORIGEM_FABRICANTE, PAI_DS_ORIGEM_FABRICANTE, PME_NU_PESO_LIQUIDO,	
												PME_QT_UNID_MEDIDA_ESTATISTICA,	PME_NU_COMUNICADO_COMPRA, PME_NU_ATO_DRAWBACK, PME_NU_AGENCIA_SECEX, 
												PME_VL_CRA, PME_TP_COBCAMBIAL, PME_NU_COBCAMBIAL_LIMITE_DIAS_PAGTO,	PME_TP_ACORDO_TARIFARIO, 
												PME_DS_INFORMACAO_COMPLEMENTAR, PME_TP_BEM_ENCOMENDA, PME_TP_MATERIAL_USADO, PME_NU_NCM_DESTAQUE, 
												PME_CO_PRODUTO, PME_DS_PRODUTO, PME_CO_TP_PRODUTO,	PME_CO_MODELO_PRODUTO, MER_CO_NCM_MERCADORIA,
												MER_DS_NCM_MERCADORIA, PME_TP_FORNECEDOR, PME_VL_TOTAL_CONDICAO_VENDA, PME_VL_TOTAL_CONDICAO_VENDA_REAL,
												PME_VL_TOTAL_CONDICAO_VENDA_DOLAR
										FROM #TabelaMercadoria
										WHERE PME_ID = @PME_ID

										SELECT @PME_ID_NOVO = @@IDENTITY;
								
										INSERT INTO SCIEX_PLI_FORNECEDOR_FABRICANTE
										SELECT
											@PME_ID_NOVO,
											PFF_DS_FORNECEDOR,
											PFF_DS_LOGRADOURO_FORN,
											PFF_NU_FORN,
											PFF_DS_COMPLEMENTO_FORN,
											PFF_DS_CIDADE_FORN,
											PFF_DS_ESTADO_FORN,
											PFF_CO_PAIS_FORN,
											PFF_CO_AUSENCIA_FABRICANTE,
											PFF_DS_FABRICANTE,
											PFF_DS_LOGRADOURO_FAB,
											PFF_NU_FAB,
											PFF_DS_COMPLEMENTO_FAB,
											PFF_DS_CIDADE_FAB,
											PFF_DS_ESTADO_FAB,
											PFF_CO_PAIS_FAB,
											PFF_DS_PAIS_FAB,
											PFF_DS_PAIS_FORN
										FROM SCIEX_PLI_FORNECEDOR_FABRICANTE
										WHERE PME_ID = @PME_ID
																					
										INSERT INTO #TabelaDetalheMercadoria		
										SELECT  PDM_ID, PME_ID, UME_CO, UME_DS, UME_SG, DME_CO_DETALHE_MERCADORIA, PDM_DS_DETALHE,
										PDM_DS_COMPLEMENTO, PDM_DS_MAT_PRIMA_BASICA, PDM_DS_PART_NUMBER, PDM_DS_REF_FABRICANTE,
										PDM_QT_UNID_COMERCIALIZADA, PDM_VL_UNITARIO_COND_VENDA, PDM_VL_CONDICAO_VENDA, NULL,
										NULL, 1
										FROM SCIEX_PLI_DETALHE_MERCADORIA
										WHERE PME_ID = @PME_ID

										WHILE EXISTS(SELECT * FROM #TabelaDetalheMercadoria WHERE SITUACAO = 1)
										BEGIN
											SELECT TOP 1 @PDM_ID = PDM_ID FROM #TabelaDetalheMercadoria WHERE SITUACAO = 1
			
											INSERT INTO SCIEX_PLI_DETALHE_MERCADORIA
											(
												PME_ID, UME_CO, UME_DS, UME_SG, DME_CO_DETALHE_MERCADORIA, 
												PDM_DS_DETALHE ,PDM_DS_COMPLEMENTO, PDM_DS_MAT_PRIMA_BASICA, 
												PDM_DS_PART_NUMBER, PDM_DS_REF_FABRICANTE, PDM_QT_UNID_COMERCIALIZADA, 
												PDM_VL_UNITARIO_COND_VENDA, PDM_VL_CONDICAO_VENDA, PDM_VL_CONDICAO_VENDA_REAL,
												PDM_VL_CONDICAO_VENDA_DOLAR	
											)
											SELECT  @PME_ID_NOVO, UME_CO, UME_DS, UME_SG, DME_CO_DETALHE_MERCADORIA, PDM_DS_DETALHE,
													PDM_DS_COMPLEMENTO, PDM_DS_MAT_PRIMA_BASICA, PDM_DS_PART_NUMBER, PDM_DS_REF_FABRICANTE,
													PDM_QT_UNID_COMERCIALIZADA, PDM_VL_UNITARIO_COND_VENDA, PDM_VL_CONDICAO_VENDA, NULL,
													NULL
											FROM #TabelaDetalheMercadoria
											WHERE PDM_ID = @PDM_ID

											UPDATE #TabelaDetalheMercadoria 
											SET SITUACAO = 2
											WHERE PDM_ID = @PDM_ID;
										END

										DELETE FROM #TabelaDetalheMercadoria

										INSERT INTO #TabelaProcessoAnuente
										SELECT PPA_ID, PME_ID, OAN_ID, PPA_NU_PROCESSO, 1
										FROM SCIEX_PLI_PROCESSO_ANUENTE
										WHERE PME_ID = @PME_ID;

										WHILE EXISTS(SELECT * FROM #TabelaProcessoAnuente WHERE SITUACAO = 1)
										BEGIN
			
											SELECT TOP 1 @PPA_ID = PPA_ID FROM #TabelaProcessoAnuente WHERE SITUACAO = 1

											INSERT INTO SCIEX_PLI_PROCESSO_ANUENTE
											SELECT @PME_ID_NOVO, OAN_ID, PPA_NU_PROCESSO
											FROM #TabelaProcessoAnuente
											WHERE PPA_ID = @PPA_ID
			
											UPDATE #TabelaProcessoAnuente 
											SET SITUACAO = 2
											WHERE  PPA_ID = @PPA_ID 

										END

										DELETE FROM #TabelaProcessoAnuente

										UPDATE #TabelaMercadoria 
										SET SITUACAO = 2
										WHERE PME_ID = @PME_ID

									END

									UPDATE #TabelaProduto
									SET SITUACAO = 2
									WHERE PLI_ID = @PLI_ID AND  PPR_ID = @PPR_ID

									DELETE FROM #TabelaMercadoria
											
										
						END						

						
					COMMIT					

				END TRY

				BEGIN CATCH  

					IF @@TRANCOUNT > 0
						ROLLBACK
							
				END CATCH;

				SELECT @PLI_ID_NOVO
				DROP TABLE #TabelaProduto
				DROP TABLE #TabelaMercadoria
				DROP TABLE #TabelaProcessoAnuente
				DROP TABLE #TabelaDetalheMercadoria


			";

			SQL = SQL.Replace("idPliAtual", idPliAtual)
				.Replace("NumeroPLI", numeroPLI)
				.Replace("AnoPLI", anoPLI)
				.Replace("CPFResponsavel", cpfResponsavel)
				.Replace("NomeResponsavel", nomeResponsavel)
				.Replace("TipoOrigem", tipoOrigem);

			return SQL;
		}
		
		public PliVM RegrasValidar(long? IdPLI, int idPLIAplicacao, long? IdPliProduto, long? IdPliMercadoria, bool EntregarPli)
		{
			var pliEntity = _uowSciex.QueryStackSciex.Pli.Selecionar(o => o.IdPLI == IdPLI);
			var pliVM = AutoMapper.Mapper.Map<PliVM>(pliEntity);

			if (pliVM == null)
			{
				pliVM = new PliVM();
			}

			pliVM.IsPliValidado = true;

			var pliProduto = _uowSciex.QueryStackSciex.PliProduto.Listar(o =>
				o.IdPLI == IdPLI
				&& (
					IdPliProduto == null || o.IdPliProduto == IdPliProduto
				)
			);
			var pliMercadorias = _uowSciex.QueryStackSciex.PliMercadoria.Listar(o =>
				(
					IdPLI == null || o.IdPLI == IdPLI
				)
				&& (
					IdPliProduto == null || o.IdPliProduto == IdPliProduto
				)
				&& (
					IdPliMercadoria == null || o.IdPliMercadoria == IdPliMercadoria
				)
			);

			IEnumerable<int?> pliMercadoriaValidarFornecedor = new List<int?>();
			List<PliDetalheMercadoriaEntity> pliDetalheMercadoria = new List<PliDetalheMercadoriaEntity>();


			IEnumerable<List<PliMercadoriaEntity>> count = pliMercadorias.GroupBy(grp => new { grp.CodigoNCMMercadoria, grp.IdFornecedor })
				.Where(grp => grp.Count() > 1)
				.Select(grp => grp.ToList());


			if (count.Count() > 0)
			{
				// validação para quando a mercadoria tiver o mesmo fornecedor
				pliMercadoriaValidarFornecedor = _uowSciex.QueryStackSciex.PliMercadoria.Listar(o =>
					o.IdPLI == IdPLI && o.IdFornecedor != null
					&& 
					(
						IdPliProduto == null || IdPliProduto == 0 || o.IdPliProduto == IdPliProduto
					)
				)
				.GroupBy(o => o.IdFornecedor)
				.Where(grp => grp.Count() > 1)
				.Select(grp => grp.Key);
			}

			List<PliMercadoriaVM> ListaMercadoriasValidar = new List<PliMercadoriaVM>();

			if (idPLIAplicacao == 2)
			{
				if (pliMercadorias.Count > 0 && IdPLI == null && IdPliProduto == null)
				{
					PliProdutoEntity p = new PliProdutoEntity();
					p.PliMercadoria.Add(pliMercadorias.FirstOrDefault());
					pliProduto.Add(p);
				}
				
				foreach (var itemProduto in pliProduto)
				{
					if (!itemProduto.PliMercadoria.Any())
					{
						pliMercadorias = itemProduto.PliMercadoria.ToList();
						break;
					}

					foreach (var item in itemProduto.PliMercadoria)
					{
						pliVM.IsPliValidado = true;

						var mercadoria = AutoMapper.Mapper.Map<PliMercadoriaVM>(item);
						mercadoria.ValidarDadosMercadoria = new ValidarDadosMercadoria();

						if (mercadoria.PesoLiquido == null || mercadoria.PesoLiquido == 0)
						{
							mercadoria.ValidarDadosMercadoria.PesoLiquido = true;
							mercadoria.ValidarDadosMercadoria.TotalItens += 1;
							pliVM.IsPliValidado = false;
						}

						if (mercadoria.QuantidadeUnidadeMedidaEstatistica == null || mercadoria.QuantidadeUnidadeMedidaEstatistica == 0)
						{
							mercadoria.ValidarDadosMercadoria.QuantidadeMedidaEstatistica = true;
							mercadoria.ValidarDadosMercadoria.TotalItens += 1;
							pliVM.IsPliValidado = false;
						}

						if (mercadoria.IdMoeda == null)
						{
							mercadoria.ValidarDadosMercadoria.Moeda = true;
							mercadoria.ValidarDadosMercadoria.TotalItens += 1;
							pliVM.IsPliValidado = false;
						}

						if (mercadoria.IdIncoterms == null)
						{
							mercadoria.ValidarDadosMercadoria.Incorterms = true;
							mercadoria.ValidarDadosMercadoria.TotalItens += 1;
							pliVM.IsPliValidado = false;
						}

						if (mercadoria.CodigoPais == null)
						{
							mercadoria.ValidarDadosMercadoria.Pais = true;
							mercadoria.ValidarDadosMercadoria.TotalItens += 1;
							pliVM.IsPliValidado = false;
						}

						if (mercadoria.IdURFDespacho == null)
						{
							mercadoria.ValidarDadosMercadoria.URFDespacho = true;
							mercadoria.ValidarDadosMercadoria.TotalItens += 1;
							pliVM.IsPliValidado = false;
						}

						if (mercadoria.IdURFEntrada == null)
						{
							mercadoria.ValidarDadosMercadoria.URFEntrada = true;
							mercadoria.ValidarDadosMercadoria.TotalItens += 1;
							pliVM.IsPliValidado = false;
						}

						if (mercadoria.IdRegimeTributario == null)
						{
							mercadoria.ValidarDadosMercadoria.RegimeTributario = true;
							mercadoria.ValidarDadosMercadoria.TotalItens += 1;
							pliVM.IsPliValidado = false;
						}

						if (mercadoria.IdFundamentoLegal == null)
						{
							mercadoria.ValidarDadosMercadoria.FundamentoLegal = true;
							mercadoria.ValidarDadosMercadoria.TotalItens += 1;
							pliVM.IsPliValidado = false;
						}

						mercadoria.ValidarDetalhesMercadoria = new ValidarDetalhesMercadoria();

						pliDetalheMercadoria = _uowSciex.QueryStackSciex.PliDetalheMercadoria.Listar(o => o.IdPliMercadoria == mercadoria.IdPliMercadoria);

						if (pliDetalheMercadoria.Count == 0)
						{
							pliVM.IsPliValidado = false;
							mercadoria.ValidarDetalhesMercadoria.ItemMercadoria = true;
							mercadoria.ValidarDetalhesMercadoria.QuantidadeUnidadeComercializada = true;
							mercadoria.ValidarDetalhesMercadoria.UnidadeComercializada = true;
							mercadoria.ValidarDetalhesMercadoria.ValorUnitarioCondicaoVenda = true;
							mercadoria.ValidarDetalhesMercadoria.TotalItens = 4;
						}

						mercadoria.ValidarFornecedorFabricanteMercadoria = new ValidarFornecedorFabricanteMercadoria();

						var _pliFornecedorFabricante = _uowSciex.QueryStackSciex.PliFornecedorFabricante.Selecionar(o => o.IdPliMercadoria == mercadoria.IdPliMercadoria);

						if (mercadoria.TipoFornecedor == null)
						{
							mercadoria.ValidarFornecedorFabricanteMercadoria.TipoFornecedor = true;
							mercadoria.ValidarFornecedorFabricanteMercadoria.TotalItens = 1;
							pliVM.IsPliValidado = false;
						}

						if (mercadoria.TipoFornecedor == 0)
						{
							mercadoria.ValidarFornecedorFabricanteMercadoria.TipoFornecedor = true;
							mercadoria.ValidarFornecedorFabricanteMercadoria.TotalItens += 1;
							pliVM.IsPliValidado = false;
						}

						if (mercadoria.TipoFornecedor == 1 || mercadoria.TipoFornecedor == 2)
						{
							try
							{
								if (_pliFornecedorFabricante.DescricaoFornecedor.Trim().Length == 0)
								{
									mercadoria.ValidarFornecedorFabricanteMercadoria.Fornecedor = true;
									mercadoria.ValidarFornecedorFabricanteMercadoria.TotalItens += 1;
									pliVM.IsPliValidado = false;
								}
							}
							catch
							{
								mercadoria.ValidarFornecedorFabricanteMercadoria.Fornecedor = true;
								mercadoria.ValidarFornecedorFabricanteMercadoria.TotalItens += 1;
								pliVM.IsPliValidado = false;
							}

							if (mercadoria.TipoFornecedor == 2)
							{
								try
								{
									if (_pliFornecedorFabricante.DescricaoFabricante.Trim().Length == 0)
									{
										mercadoria.ValidarFornecedorFabricanteMercadoria.Fabricante = true;
										mercadoria.ValidarFornecedorFabricanteMercadoria.TotalItens += 1;
										pliVM.IsPliValidado = false;
									}
								}
								catch
								{
									mercadoria.ValidarFornecedorFabricanteMercadoria.Fabricante = true;
									mercadoria.ValidarFornecedorFabricanteMercadoria.TotalItens += 1;
									pliVM.IsPliValidado = false;
								}

							}
						}

						if (mercadoria.TipoFornecedor == 3)
						{
							if (mercadoria.CodigoPaisOrigemFabricante == null)
							{
								mercadoria.ValidarFornecedorFabricanteMercadoria.PaisOrigem = true;
								mercadoria.ValidarFornecedorFabricanteMercadoria.TotalItens += 1;
								pliVM.IsPliValidado = false;
							}

							if (mercadoria.TipoFornecedor == 0)
							{
								mercadoria.ValidarFornecedorFabricanteMercadoria.TipoFornecedor = true;
								mercadoria.ValidarFornecedorFabricanteMercadoria.TotalItens += 1;
								pliVM.IsPliValidado = false;
							}
						}

						mercadoria.ValidarNegociacaoMercadoria = new ValidarNegociacaoMercadoria();

						if (mercadoria.TipoCOBCambial == null)
						{
							mercadoria.ValidarNegociacaoMercadoria.TipoCobertura = true;
							mercadoria.ValidarNegociacaoMercadoria.TotalItens += 1;
							pliVM.IsPliValidado = false;
						}



						if (mercadoria.TipoCOBCambial == 1 || mercadoria.TipoCOBCambial == 2)
						{
							if (mercadoria.IdModalidadePagamento == null)
							{
								mercadoria.ValidarNegociacaoMercadoria.ModalidadePagamento = true;
								mercadoria.ValidarNegociacaoMercadoria.TotalItens += 1;
								pliVM.IsPliValidado = false;
							}

							if (mercadoria.TipoCOBCambial == 1)
							{
								if (mercadoria.NumeroCOBCambialLimiteDiasPagamento == null)
								{
									mercadoria.ValidarNegociacaoMercadoria.LimitePagamentoDias = true;
									mercadoria.ValidarNegociacaoMercadoria.TotalItens += 1;
									pliVM.IsPliValidado = false;
								}
							}
						}

						if (mercadoria.TipoCOBCambial == 4)
						{
							if (mercadoria.IdMotivo == null)
							{
								mercadoria.ValidarNegociacaoMercadoria.Motivo = true;
								mercadoria.ValidarNegociacaoMercadoria.TotalItens += 1;
								pliVM.IsPliValidado = false;
							}
						}

						if (mercadoria.TipoCOBCambial == 3)
						{
							if (mercadoria.IdInstituicaoFinanceira == null)
							{
								mercadoria.ValidarNegociacaoMercadoria.InstituicaoFinanceira = true;
								mercadoria.ValidarNegociacaoMercadoria.TotalItens += 1;
								pliVM.IsPliValidado = false;
							}
						}
						if (!pliVM.IsPliValidado || IdPliMercadoria != null)
						{
							ListaMercadoriasValidar.Add(mercadoria);
							if (EntregarPli)
							{
								break;
							}
						}
					}
				}
			}
			else
			{
				foreach (var item in pliMercadorias)
				{
					pliVM.IsPliValidado = true;

					var mercadoria = AutoMapper.Mapper.Map<PliMercadoriaComercializacaoVM>(item);
					mercadoria.ValidarDadosMercadoria = new ValidarDadosMercadoria();

					if (mercadoria.PesoLiquido == null || mercadoria.PesoLiquido == 0)
					{
						mercadoria.ValidarDadosMercadoria.PesoLiquido = true;
						mercadoria.ValidarDadosMercadoria.TotalItens += 1;
						pliVM.IsPliValidado = false;
					}

					if (mercadoria.QuantidadeUnidadeMedidaEstatistica == null || mercadoria.QuantidadeUnidadeMedidaEstatistica == 0)
					{
						mercadoria.ValidarDadosMercadoria.QuantidadeMedidaEstatistica = true;
						mercadoria.ValidarDadosMercadoria.TotalItens += 1;
						pliVM.IsPliValidado = false;
					}

					if (mercadoria.IdMoeda == null)
					{
						mercadoria.ValidarDadosMercadoria.Moeda = true;
						mercadoria.ValidarDadosMercadoria.TotalItens += 1;
						pliVM.IsPliValidado = false;
					}

					if (mercadoria.IdIncoterms == null)
					{
						mercadoria.ValidarDadosMercadoria.Incorterms = true;
						mercadoria.ValidarDadosMercadoria.TotalItens += 1;
						pliVM.IsPliValidado = false;
					}

					if (mercadoria.CodigoPais == null)
					{
						mercadoria.ValidarDadosMercadoria.Pais = true;
						mercadoria.ValidarDadosMercadoria.TotalItens += 1;
						pliVM.IsPliValidado = false;
					}

					if (mercadoria.IdURFDespacho == null)
					{
						mercadoria.ValidarDadosMercadoria.URFDespacho = true;
						mercadoria.ValidarDadosMercadoria.TotalItens += 1;
						pliVM.IsPliValidado = false;
					}

					if (mercadoria.IdURFEntrada == null)
					{
						mercadoria.ValidarDadosMercadoria.URFEntrada = true;
						mercadoria.ValidarDadosMercadoria.TotalItens += 1;
						pliVM.IsPliValidado = false;
					}

					if (mercadoria.IdRegimeTributario == null)
					{
						mercadoria.ValidarDadosMercadoria.RegimeTributario = true;
						mercadoria.ValidarDadosMercadoria.TotalItens += 1;
						pliVM.IsPliValidado = false;
					}

					if (mercadoria.IdFundamentoLegal == null)
					{
						mercadoria.ValidarDadosMercadoria.FundamentoLegal = true;
						mercadoria.ValidarDadosMercadoria.TotalItens += 1;
						pliVM.IsPliValidado = false;
					}

					mercadoria.ValidarDetalhesMercadoria = new ValidarDetalhesMercadoria();

					pliDetalheMercadoria = _uowSciex.QueryStackSciex.PliDetalheMercadoria.Listar(o => o.IdPliMercadoria == mercadoria.IdPliMercadoria);

					if (pliDetalheMercadoria.Count == 0)
					{
						pliVM.IsPliValidado = false;
						mercadoria.ValidarDetalhesMercadoria.ItemMercadoria = true;
						mercadoria.ValidarDetalhesMercadoria.QuantidadeUnidadeComercializada = true;
						mercadoria.ValidarDetalhesMercadoria.UnidadeComercializada = true;
						mercadoria.ValidarDetalhesMercadoria.ValorUnitarioCondicaoVenda = true;
						mercadoria.ValidarDetalhesMercadoria.TotalItens = 4;
					}
					var countDesc = pliDetalheMercadoria.Where(o => o.DescricaoDetalhe == null || o.DescricaoDetalhe.Trim() == "").ToList().Count;
					if (countDesc > 0)
					{
						pliVM.IsPliValidado = false;
						mercadoria.ValidarDetalhesMercadoria.ItemMercadoria = true;
						mercadoria.ValidarDetalhesMercadoria.TotalItens += 1;
					}
					var countUndComerc = pliDetalheMercadoria.Where(o => String.IsNullOrEmpty(o.DescricaoUnidadeMedida) ).ToList().Count;
					if (countUndComerc > 0)
					{
						pliVM.IsPliValidado = false;
						mercadoria.ValidarDetalhesMercadoria.UnidadeComercializada = true;
						mercadoria.ValidarDetalhesMercadoria.TotalItens += 1;
					}
					var countQtdComerc = pliDetalheMercadoria.Where(o => o.QuantidadeComercializada == null || o.QuantidadeComercializada == 0).ToList().Count;
					if (countQtdComerc > 0)
					{
						pliVM.IsPliValidado = false;
						mercadoria.ValidarDetalhesMercadoria.QuantidadeUnidadeComercializada = true;
						mercadoria.ValidarDetalhesMercadoria.TotalItens += 1;
					}
					var countVlrUntCondVenda = pliDetalheMercadoria.Where(o => o.ValorUnitarioCondicaoVenda == null || o.ValorUnitarioCondicaoVenda == 0).ToList().Count;
					if (countVlrUntCondVenda > 0)
					{
						pliVM.IsPliValidado = false;
						mercadoria.ValidarDetalhesMercadoria.ValorUnitarioCondicaoVenda = true;
						mercadoria.ValidarDetalhesMercadoria.TotalItens += 1;
					}


					mercadoria.ValidarFornecedorFabricanteMercadoria = new ValidarFornecedorFabricanteMercadoria();

					var _pliFornecedorFabricante = _uowSciex.QueryStackSciex.PliFornecedorFabricante.Selecionar(o => o.IdPliMercadoria == mercadoria.IdPliMercadoria);

					if (mercadoria.TipoFornecedor == null)
					{
						mercadoria.ValidarFornecedorFabricanteMercadoria.TipoFornecedor = true;
						mercadoria.ValidarFornecedorFabricanteMercadoria.TotalItens = 1;
						pliVM.IsPliValidado = false;
					}

					if (mercadoria.TipoFornecedor == 0)
					{
						mercadoria.ValidarFornecedorFabricanteMercadoria.TipoFornecedor = true;
						mercadoria.ValidarFornecedorFabricanteMercadoria.TotalItens += 1;
						pliVM.IsPliValidado = false;
					}

					if (mercadoria.TipoFornecedor == 1 || mercadoria.TipoFornecedor == 2)
					{
						try
						{
							if (_pliFornecedorFabricante.DescricaoFornecedor.Trim().Length == 0)
							{
								mercadoria.ValidarFornecedorFabricanteMercadoria.Fornecedor = true;
								mercadoria.ValidarFornecedorFabricanteMercadoria.TotalItens += 1;
								pliVM.IsPliValidado = false;
							}
						}
						catch
						{
							mercadoria.ValidarFornecedorFabricanteMercadoria.Fornecedor = true;
							mercadoria.ValidarFornecedorFabricanteMercadoria.TotalItens += 1;
							pliVM.IsPliValidado = false;
						}

						if (mercadoria.TipoFornecedor == 2)
						{
							try
							{
								if (_pliFornecedorFabricante.DescricaoFabricante.Trim().Length == 0)
								{
									mercadoria.ValidarFornecedorFabricanteMercadoria.Fabricante = true;
									mercadoria.ValidarFornecedorFabricanteMercadoria.TotalItens += 1;
									pliVM.IsPliValidado = false;
								}
							}
							catch
							{
								mercadoria.ValidarFornecedorFabricanteMercadoria.Fabricante = true;
								mercadoria.ValidarFornecedorFabricanteMercadoria.TotalItens += 1;
								pliVM.IsPliValidado = false;
							}

						}
					}

					if (mercadoria.TipoFornecedor == 3)
					{
						if (mercadoria.CodigoPaisOrigemFabricante == null)
						{
							mercadoria.ValidarFornecedorFabricanteMercadoria.PaisOrigem = true;
							mercadoria.ValidarFornecedorFabricanteMercadoria.TotalItens += 1;
							pliVM.IsPliValidado = false;
						}

						if (mercadoria.TipoFornecedor == 0)
						{
							mercadoria.ValidarFornecedorFabricanteMercadoria.TipoFornecedor = true;
							mercadoria.ValidarFornecedorFabricanteMercadoria.TotalItens += 1;
							pliVM.IsPliValidado = false;
						}
					}

					mercadoria.ValidarNegociacaoMercadoria = new ValidarNegociacaoMercadoria();

					if (mercadoria.TipoCOBCambial == null)
					{
						mercadoria.ValidarNegociacaoMercadoria.TipoCobertura = true;
						mercadoria.ValidarNegociacaoMercadoria.TotalItens += 1;
						pliVM.IsPliValidado = false;
					}

					if (mercadoria.TipoCOBCambial == 1 || mercadoria.TipoCOBCambial == 2)
					{
						if (mercadoria.IdModalidadePagamento == null)
						{
							mercadoria.ValidarNegociacaoMercadoria.ModalidadePagamento = true;
							mercadoria.ValidarNegociacaoMercadoria.TotalItens += 1;
							pliVM.IsPliValidado = false;
						}

						if (mercadoria.TipoCOBCambial == 1)
						{
							if (mercadoria.NumeroCOBCambialLimiteDiasPagamento == null)
							{
								mercadoria.ValidarNegociacaoMercadoria.LimitePagamentoDias = true;
								mercadoria.ValidarNegociacaoMercadoria.TotalItens += 1;
								pliVM.IsPliValidado = false;
							}
						}
					}

					if (mercadoria.TipoCOBCambial == 4)
					{
						if (mercadoria.IdMotivo == null)
						{
							mercadoria.ValidarNegociacaoMercadoria.Motivo = true;
							mercadoria.ValidarNegociacaoMercadoria.TotalItens += 1;
							pliVM.IsPliValidado = false;
						}
					}

					if (mercadoria.TipoCOBCambial == 3)
					{
						if (mercadoria.IdInstituicaoFinanceira == null)
						{
							mercadoria.ValidarNegociacaoMercadoria.InstituicaoFinanceira = true;
							mercadoria.ValidarNegociacaoMercadoria.TotalItens += 1;
							pliVM.IsPliValidado = false;
						}
					}

					if (!pliVM.IsPliValidado || IdPliMercadoria != null)
					{
						ListaMercadoriasValidar.Add(mercadoria);
						if (EntregarPli)
						{
							break;
						}
					}
				}
			}

			if(pliVM.TipoDocumento == 3 && (pliVM.Anexo == null || pliVM.Anexo.Length <= 0))
			{
				pliVM.IsPliValidado = false;
				pliVM.MensagemErro = "Erro na validação do Anexo. Deseja corrigir os dados?";
				pliVM.TipoErro = 1;
				return pliVM;
			}

			if (!string.IsNullOrEmpty(pliVM.NumeroLIReferencia))
			{
				var numeroLi = Convert.ToInt64(pliVM.NumeroLIReferencia);
				var verificaLI = _uowSciex.QueryStackSciex.Li.Selecionar(o => o.NumeroLi == numeroLi);

				if (verificaLI == null)
				{
					pliVM.MensagemErro = "LI de referência inexistente.";
					pliVM.IsPliValidado = false;
					pliVM.TipoErro = 2;
					return pliVM;
				}

				if (verificaLI.PliMercadoria.Pli.IdPLIAplicacao != pliVM.IdPLIAplicacao)
				{
					pliVM.MensagemErro = "A LI de referência informada não é da mesma Aplicação do novo PLI";
					pliVM.IsPliValidado = false;
					pliVM.TipoErro = 2;
					return pliVM;
				}

				var resp = _uowSciex.QueryStackSciex.VerificaLiIndeferidoCancelado(pliVM.NumeroLIReferencia);

				if (resp != null && resp.Count > 1)
				{
					pliVM.MensagemErro = "LI de referência não pode ser utilizada, pois está sendo referenciada em outro PLI.";
					pliVM.IsPliValidado = false;
					pliVM.TipoErro = 2;
					return pliVM;
				}

				var liParaImportador = _uowSciex.QueryStackSciex.VerificaLiDoImportador(numeroLi, pliEntity.Cnpj);

				if (liParaImportador.Count <= 0)
				{
					pliVM.MensagemErro = "LI de referência inválida para substituição. Para ser referência, a LI deve estar Deferida, sem DI e pertencer ao mesmo importador do novo PLI.";
					pliVM.IsPliValidado = false;
					pliVM.TipoErro = 2;
					return pliVM;
				}

				var li = _uowSciex.QueryStackSciex.Li.Selecionar(o => o.NumeroLi == numeroLi);
				var lis = _uowSciex.QueryStackSciex.LiSubstituida.Listar(o => o.IdLiSubstituida == li.IdPliMercadoria || o.IdLiSubstituta == li.IdPliMercadoria);
				if (lis.Count > 0)
				{
					var idLis = lis.FirstOrDefault().IdLiOrigem;
					var liOrigem = _uowSciex.QueryStackSciex.LiSubstituida.Listar(o => o.IdLiOrigem == idLis);

					if (liOrigem != null && liOrigem.LastOrDefault().NumeroLsu >= 3)
					{
						pliVM.MensagemErro = "LI de referência não pode ser utilizada, pois excedeu o limite máximo de 3 substituições.";
						pliVM.IsPliValidado = false;
						pliVM.TipoErro = 2;
						return pliVM;
					}
				}
			}

			if (pliProduto.Count == 0 && IdPliMercadoria == null && idPLIAplicacao == 2)
			{
				pliVM.MensagemErro = "Erro na validação. PLI não possui produto cadastrado";
				pliVM.IsPliValidado = false;
				pliVM.TipoErro = 2;
			}
			else if (pliMercadorias.Count == 0 && IdPliMercadoria == null && idPLIAplicacao == 2)
			{
				pliVM.MensagemErro = "Erro na validação. Existem produtos que não possuem mercadoria cadastrada";
				pliVM.IsPliValidado = false;
				pliVM.TipoErro = 2;
			}
			else if (pliMercadoriaValidarFornecedor.Count() > 0 && IdPliMercadoria == null)
			{
				pliVM.MensagemErro = "Erro na validação. Existem registros duplicados de fornecedor para a mesma mercadoria";
				pliVM.IsPliValidado = false;
				pliVM.TipoErro = 2;
			}
			else if (ListaMercadoriasValidar.Count > 0)
			{
				pliVM.IsPliValidado = false;
				pliVM.MensagemErro = "Erro na validação. Deseja corrigir os dados?";
				pliVM.TipoErro = 1;
				pliVM.ListaMercadorias = new List<PliMercadoriaVM>();
				pliVM.ListaMercadorias.AddRange(ListaMercadoriasValidar);

			}

			return pliVM;
		}

		public PliVM RegrasEntregar(PliVM filter)
		{
			var pliVM = new PliVM();
			var viewImportador = _IViewImportadorBll.Selecionar(_usuarioPssBll.ObterUsuarioLogado().usuCpfCnpjEmpresaOuLogado.Replace(".", "").Replace("-", "").Replace("/", ""));

			if (viewImportador == null || viewImportador.IdSituacaoInscricao != 1) // nao ativo
			{
				pliVM.MensagemErro = "Não foi possível realizar a entrega do PLI. Situação Cadastral do Importador não regularizada junto a SUFRAMA.";
				pliVM.IsPliValidado = false;
				return pliVM;
			}

			ViewSetorEntity setor1 = new ViewSetorEntity();
			//regra para 
			var usuCpfCnpjEmpresaOuLogado = _usuarioPssBll.ObterUsuarioLogado().usuCpfCnpjEmpresaOuLogado.Replace(".", "").Replace("-", "").Replace("/", ""); ;
			var empresa = _uowCadsuf.QueryStack.ViewSetorEmpresa.Listar(o => o.Cnpj == usuCpfCnpjEmpresaOuLogado);
			if (empresa != null)
			{
				ViewSetorEntity setor = new ViewSetorEntity();
				foreach (var item in empresa)
				{
					if (filter.IdPLIAplicacao != 1)
					{
						// RN 08 - Verifica se a empresa se enquada na categoria de industria
						setor = _uowCadsuf.QueryStack.ViewSetor.Selecionar(o => o.IdSetor == item.IdSetor && o.Codigo == 19);
						if (setor != null)
						{
							pliVM.DescricaoSetor = setor.Descricao;
							pliVM.CodigoSetor = setor.Codigo;
							setor1 = setor;
							break;
						}
					}
					else
					{
						setor = _uowCadsuf.QueryStack.ViewSetor.Selecionar(o => o.IdSetor == item.IdSetor && o.Codigo == 2);
						if (setor != null)
						{
							pliVM.DescricaoSetor = setor.Descricao;
							pliVM.CodigoSetor = setor.Codigo;
							setor1 = setor;
							break;
						}
					}
				}

				if (setor == null && filter.IdPLIAplicacao != 1)
				{
					pliVM.MensagemErro = "Aplicação do PLI inválida. Empresa não cadastrada no setor indústria";
					return pliVM;
				}
				else if(setor == null && filter.IdPLIAplicacao == 1)
				{
					pliVM.MensagemErro = "Aplicação do PLI inválida. Empresa não cadastrada no setor comércio";
					return pliVM;
				}
			}

			//verificar de onde pegar o idPliAplicacao para verificar junto ao validar
			pliVM = RegrasValidar(filter.IdPLI.Value, filter.IdPLIAplicacao, null, null, true);

			if (!pliVM.IsPliValidado)
			{
				pliVM.MensagemErro = "Não foi possível realizar a entrega do PLI. Realize a validação do PLI.";
				pliVM.IsPliValidado = false;
				return pliVM;
			}
			else
			{
				var pliEntity = _uowSciex.QueryStackSciex.Pli.Selecionar(x => x.IdPLI == filter.IdPLI.Value);
				pliEntity.StatusPli = (byte)EnumPliStatus.ENTREGUE;


				//inserir a data do envio do PLI
				//caso o pli seja entregue gerar histórico
				if (pliEntity.StatusPli == (byte)EnumPliStatus.ENTREGUE)
				{
					pliEntity.DataEnvioPli = DateTime.Now;
					pliEntity.DescricaoSetor = setor1.Descricao;
					pliEntity.CodigoSetor = setor1.Codigo;

					decimal valorTotalCondicaoCompra =
						_uowSciex.QueryStackSciex.PliMercadoria.Listar(o => o.IdPLI == pliEntity.IdPLI).Sum(o => o.ValorTotalCondicaoVenda != null ? o.ValorTotalCondicaoVenda.Value : 0);

					pliEntity.ValorTotalCondicaoVenda = valorTotalCondicaoCompra;
					_uowSciex.CommandStackSciex.Pli.Salvar(pliEntity);


					PliHistoricoEntity pliHistoricoEntidade = new PliHistoricoEntity();
					pliHistoricoEntidade.IdPLI = pliEntity.IdPLI;
					pliHistoricoEntidade.StatusPli = (byte)EnumPliStatus.ENTREGUE;
					pliHistoricoEntidade.DescricaoStatusPli = EnumPliStatus.ENTREGUE.ToString().Replace("_", "");
					pliHistoricoEntidade.DataEvento = DateTime.Now;
					pliHistoricoEntidade.Observacao = "PLI ENTREGUE";
					pliHistoricoEntidade.CPFCNPJ = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoCpfCnpj.Replace(".", "").Replace("-", "").Replace("/", ""); ;
					pliHistoricoEntidade.NomeResponsavel = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoNome.Replace(".", "").Replace("-", "").Replace("/", ""); ;

					_uowSciex.CommandStackSciex.PliHistorico.Salvar(pliHistoricoEntidade);
					_uowSciex.CommandStackSciex.Save();
				}

				_uowSciex.CommandStackSciex.Pli.Salvar(pliEntity);
				_uowSciex.CommandStackSciex.Save();

				//Chama o complementar PLI
				//_complementarPLIBll.ComplementarPLI(pliEntity.IdPLI.ToString());


				pliVM.IsPliValidado = true;
			}
			return pliVM;
		}

		// Script substituido pela procedure ==>> ST_SCIEX_PROCESSAR_PLI
		// Sprint2 - Melhorias no Serviço Processar PLI - Data: 03/03/2020
		#region Processar
//		public string SQLProcessarPLI()
//		{
//			string sql =
//				@"/**
//PROCESSAMENTO DE PLI
//DATA INICIO: 21/01/2019
//RESPONSAVEL: JANDERSON SILVA
//SITUAÇÃO: CLIENTE (ENTREGA 5)
//**/

//--DECLRAÇÃO DAS VARIÁVEIS
//DECLARE @PLI_PROCESSADO BIT                              --VARIAVEL DE CONTROLE PARA SABER SE O PLI FOI PROCESSADO COM SUCESSO
//DECLARE @PLI_MERCADORIA_PROCESSADA BIT                   --VARIAVEL DE CONTROLE PARA SABER SE A MERCADORIA FOI PROCESSDA COM SUCESSO
//DECLARE @IMPORTADOR_PROBLEMA BIT                         --VARIAVEL DE CONTROLE PARA SABER SE O IMPORTADOR ESTA COM PROBLEMAS
//DECLARE @RETORNO VARCHAR(MAX)-- GUARDA AS INFORMAÇÕES DE PROCESSAMENTO DO PLI

//DECLARE @PLI_ID BIGINT                                   --ID DO PLI DA TABELA SCIEX_PLI
//DECLARE @PLI_NU INT                                      --NUMERO DO PLI
//DECLARE @PLI_NU_ANO INT-- ANO DO PLI
//DECLARE @PLI_DH_ENVIO DATETIME                           --DATA DO ENVIO DO PLI
//DECLARE @PLI_PST_ID INT-- ID DO STATUS DO PLI
//DECLARE @PLI_PAP_ID INT                                  --ID DA APLICACAO DO PLI
//DECLARE @PST_COD_APLICACAO INT-- CODIGO APLICAÇÃO
//DECLARE @PLI_NU_CNPJ VARCHAR(14)-- CNPJ DO IMPORTADOR
//DECLARE @PLI_NO_CNPJ VARCHAR(60)                         --NOME DO IMPORTADOR
//DECLARE @PLI_SET_CO INT-- SETOR DO PLI
//DECLARE @PLI_TP_DOCUMENTO TINYINT                        --TIPO DE DOCUMENTO DO PLI
//DECLARE @PLI_NU_LI_REFERENCIA VARCHAR(10)-- NUMERO LI DE REFERENCIA
//DECLARE @PLI_CPF_SISCOMEX CHAR(11)-- CPF SISCOMEX LEGAL

//DECLARE @PLI_ID_REFERENCIA BIGINT                        --ID DA LI DE REFERENCIA
//DECLARE @LI_ORIGEM BIGINT-- ID DA LI ORIGINAL SE REFERENCIADA
//DECLARE @LI_ST BIGINT-- SITUACAO DA LI
//DECLARE @LI_REFERENCIAS_QTD INT                          --QUANTIDADE DE REFERENCIAS RETORNADAS
//DECLARE @DI_ID TINYINT                                   --ID DA DI

//DECLARE @PPR_ID BIGINT-- ID DO PRODUTO DA TABELA SCIEX_PLI_PRODUTO
//DECLARE @PPR_CO_PRODUTO SMALLINT-- CODIGO DO PRODUTO
//DECLARE @PPR_CO_TP_PRODUTO SMALLINT                      --CODIGO TIPO DE PRODUTO
//DECLARE @PPR_CO_MODELO_PRODUTO SMALLINT                  --CODIGO MODELO PRODUTO

//DECLARE @PME_ID BIGINT-- ID DA MERCADORIA
//DECLARE @PME_MER_CO_NCM_MERCADORIA VARCHAR(8)           --NCM DA MERCADORIA
//DECLARE @PME_RTB_ID INT-- ID REGIME TRIBUTÁRIO DA MERCADORIA
//DECLARE @PME_RTB_CO INT                                  --CODIGO REGIME TRIBUTÁRIO DA MERCADORIA
//DECLARE @PME_FLE_ID INT-- ID FUNDAMENTO LEGAL DA MERCADORIA
//DECLARE @PME_FLE_CO INT                                  --CODIGO FUNDAMENTO LEGAL DA MERCADORIA
//DECLARE @PME_CO_PRODUTO INT-- CODIGO DO PRODUTO DA MERCADORIA
//DECLARE @PME_QTD_UNID_MEDIDA_ESTATISTICA NUMERIC(14,5)   --QUANTIDADE DE UNIDADE ESTISTICA

//DECLARE @PDM_ID BIGINT                                   --ID DO DETALHE DA MERCADORIA
//DECLARE @PDM_CO_DETALHE_MERCADORIA INT-- CODIGO DETALHE MERCDORIA
//DECLARE @PDM_QTD_UNIDADE_COMERCIALIZADA NUMERIC(14,5)    --QUANTIDADE DA UNIDADE COMERCIALIZADA
//DECLARE @PDM_VL_CONDICAO_VENDA_DOLAR NUMERIC(18,7)       --VALOR DA CONDIÇÃO VENDA DOLAR

//DECLARE @ALI_NUMERO BIGINT-- NUMERO DA ALI
//DECLARE @ALI_NUMERO_ATUAL BIGINT                         --NUMERO DA ALI ATUAL
//DECLARE @ALI_STATUS INT                                  --VARIAVEL PARA VERIFICAR O STATUS DA ALI QUANDO FOR ATUALIZADA PARA DEFERIDA

//DECLARE @SITUACAO_EMPRESA INT                            --SITUAÇÃO DO IMPORTADOR(1 - ATIVO)
//DECLARE @IMP_INS_CO INT-- CODIGO DA INSCRIÇÃO CADASTRAL DO IMPORTADOR
//DECLARE @IMP_MUN_CO INT-- CODIGO DO MUNICIPIO DO IMPORTADOR
//DECLARE @IMP_SET_DS VARCHAR(100)                         --DESCRICAO DO SETOR DO IMPORTADOR
//DECLARE @IMP_UNI_CO INT-- CODIGO DA UNIDADE DO IMPORTADOR

//DECLARE @PLI_QTD_MERCADORIA INT                          --QUANTIDADE MERCADORIA DO PLI
//DECLARE @ALI_QTD_DEFERIDA INT                            --QUANTIDADE ALI_DEFERIDA
//DECLARE @ALI_QTD_INDEFERIDA INT                          --QUANTIDADE ALI_INDEFERIDA

//DECLARE @CONTROLE_SERVICO_COD INT                        --CODIGO DO CONTROLE SERVICO GERADO
//DECLARE @PLI_STATUS_PROCESSADO VARCHAR(50)-- STATUS DO PLI PROCESSADO(APROVADO, PARCIALMENTE APROVADO, REPROVADO)
//DECLARE @ERRO_MENSAGEM_PROCESSAMENTO VARCHAR(500)-- VARIAVEL PARA BUSCAR A DESCRICAO DO ERRO NA TABELA ERRO MENSAGEM

//--CONSTANTES
//DECLARE @REGISTRO_ATIVO INT = 1-- CODIGO DE REGISTRO ATIVO
//DECLARE @PST_CO TINYINT = 24-- CODIGO QUE INDICA O STATUS DO PLI DE AGUARDANDO PROCESSAMENTO
//DECLARE @LSE_ID INT = 6-- CODIGO DO SERVIÇCO QUE CONSTA NA TABELA SCIEX_LISTA_SERVICO
//DECLARE @APLI_INDUSTRIALIZACAO INT = 1-- INDUSTRIALIZAÇÃO
//DECLARE @ALI_COD_GERADA_COM_SUCESS0 INT = 1-- CODIGO STATUS ALI GERADA COM SUCESSO
//DECLARE @ALI_COD_INDEFERIDA_SUFRAMA INT = 4-- CODIGO STATUS ALI INDEFERIDA
//DECLARE @PLI_COD_PROCESSADO INT = 25-- CODIGO DO STATUS DO PLI PARA PROCESSADO
//DECLARE @PHI_DS_OBSERVACAO VARCHAR(50) = 'PLI PROCESSADO'-- DESCRIÇÃO DO HISTORICO DO PLI
//DECLARE @PHI_COD_PLI_STATUS INT = 25-- CODIGO DO STATUS DO PLI DE PLI PROCESSADO
//DECLARE @LAN_COD INT = 101-- CODIGO LANCAMENTO
//DECLARE @LAN_DESCRICAO VARCHAR(30) = 'CADASTRO DE ALI'-- DESCRICAO DO LANÇAMENTO
//DECLARE @DEBITO INT = 2-- DEBITO PARA A TABELA SCIEX_LANCAMENTO

//DECLARE @USU_CPFCNPJ VARCHAR(14) = '04407029000143'-- CPNJ DO ADMINISTRADOR DO SISTEMA
//DECLARE @USU_ADM VARCHAR(38) = 'ADMINISTRADOR DO SISTEMA - SUFRAMA'-- ADMINISTRADOR DO SISTEMA

//DECLARE @PLI_APROVADO INT = 1-- PLI APROVADO
//DECLARE @PLI_PARCIALMENTE_APROVADO INT = 2-- PLI PARCIALMENTE APROVADO
//DECLARE @PLI_REPROVADO INT = 3-- PLI REPROVADO

//DECLARE @NIVEL_ERRO_PLI INT = 1
//DECLARE @NIVEL_ERRO_MERC INT = 2
//DECLARE @NIVEL_ERRO_DETA INT = 3

//DECLARE @COD_ERRO_CPF_REPRESENTANTE INT = 103
//DECLARE @COD_ERRO_ANOPLI INT = 400
//DECLARE @COD_ERRO_SITUACAO_IMPORTADOR INT = 401
//DECLARE @COD_ERRO_NCM_EXCECAO INT = 402
//DECLARE @COD_ERRO_REGIME_TRIBUTARIO INT = 403
//DECLARE @COD_ERRO_PRODUTO INT = 404
//DECLARE @COD_ERRO_PRODUTO_TIPO INT = 405
//DECLARE @COD_ERRO_PRODUTO_MODELO INT = 406
//DECLARE @COD_ERRO_MERCADORIA INT = 407
//DECLARE @COD_ERRO_DETALHE_MERCADORIA INT = 408
//DECLARE @COD_ERRO_CRA INT = 409
//DECLARE @COD_ERRO_SETOR_MUNICIPIO INT = 410
//DECLARE @COD_ERRO_VALOR_MERCADORIA INT = 411

//DECLARE @ERRO_SITUACAO_IMPORTADOR VARCHAR(100) = 'SITUAÇÃO CADASTRAL DO IMPORTADOR {0} NÃO ESTÁ ATIVA'
//DECLARE @ERRO_NCM_EXCECAO VARCHAR(100) = 'A MERCADORIA {0} ESTÁ DEFINIDA COMO UM NCM DE EXCEÇÃO'
//DECLARE @ERRO_REGIME_TRIBUTARIO VARCHAR(200) = 'REGIME TRIBUTÁRIO {0}/FUNDAMENTO LEGAL {1} DA MERCADORIA {2} INVÁLIDO PARA O MUNICIPIO {3}'
//DECLARE @ERRO_PRODUTO VARCHAR(100) = 'PRODUTO {0} INVÁLIDO PARA O IMPORTADOR {1}'
//DECLARE @ERRO_PRODUTO_TIPO VARCHAR(150) = 'TIPO DO PRODUTO {0} INVÁLIDO PARA O PRODUTO {1} INFORMADO'
//DECLARE @ERRO_PRODUTO_MODELO VARCHAR(150) = 'MODELO DO PRODUTO {0} INVÁLIDO PARA O PRODUTO {1} INFORMADO'
//DECLARE @ERRO_MERCADORIA VARCHAR(100) = 'NCM {0} INVÁLIDA PARA O PRODUTO {1} INFORMADO'
//DECLARE @ERRO_DETALHE_MERCADORIA VARCHAR(150) = 'DETALHE {0} DA MERCADORIA {1} / PRODUTO {2} INVÁLIDO'
//DECLARE @ERRO_ANO_PLI VARCHAR(150) = 'ANO DO PLI {0} NÃO CORRESPONDE AO ANO CORRENTE {1}'

//DECLARE @VL_TOTAL_CONDICAO_VENDA NUMERIC(15, 2)-- VALOR TOTAL DA CONDICAO DE VENDA NA MERCADORIA
//DECLARE @VL_TOTAL_CONDICAO_VENDA_DETALHE NUMERIC(20, 12)-- VALOR TOTAL DA CONDICAO DE VENDA NA DETALHE MERCADORIA

//DECLARE @COD_CRII INT                           --CODIGO DO CRII
//DECLARE @VL_CRA NUMERIC(4, 2)--VALOR CRA
//DECLARE @SITUACAO_EMPRESA_PLI VARCHAR(40)--SITUACAO DO IMPORTADOR


//--INICIA O PROCESSAMENTO

///** 
//   CRIAÇÃO DA TABELA TEMPORÁRIA DE PLI
//   GUARDA A LISTA DE PLIS QUE TEM O STATUS DE AGUARDANDO PROCESSAMENTO PARA SEREM TRABALHADOS 
//**/
//CREATE TABLE #TTPLI
//(
//	PLI_ID BIGINT,
//	PLI_NU BIGINT,
//	PLI_NU_ANO INT,
//	PLI_NU_CNPJ VARCHAR(14),
//	SET_CO INT,
//	SET_DS VARCHAR(200),
//	PST_ID INT,
//	PAP_ID INT,
//	PLI_TP_DOCUMENTO TINYINT,
//	PLI_NU_LI_REFERENCIA VARCHAR(10),
//	IMP_DS_RAZAO_SOCIAL VARCHAR(100),
//	PLI_NU_CPF_REP_LEGAL_SISCOMEX CHAR(11),
//	SITUACAO INT
//)

///** 
//   CRIAÇÃO DA TABELA TEMPORÁRIA DE PLI_PRODUTO
//   GUARDA A LISTA DE PRODUTOS DOS PLIS QUE SERÃO VALIDADAS
//**/
//CREATE TABLE #TTPLI_PRODUTO(
//	PLI_ID BIGINT,
//	PPR_ID BIGINT,
//	PPR_CO_PRODUTO SMALLINT,
//	PPR_CO_TP_PRODUTO SMALLINT,
//	PPR_CO_MODELO_PRODUTO SMALLINT,
//	SITUACAO INT
//)

///** 
//   CRIAÇÃO DA TABELA TEMPORÁRIA DE PLI_MERCADORIA
//   GUARDA A LISTA DE MERCADORIA DOS PLIS QUE SERÃO VALIDADAS
//**/
//CREATE TABLE #TTPLI_MERCADORIA(
//	PME_ID BIGINT,
//	PME_CO_PRODUTO SMALLINT, 
//	MER_CO_NCM_MERCADORIA VARCHAR(8),
//	RTB_ID INT,
//	FLE_ID INT,
//	PME_QT_UNID_MEDIDA_ESTATISTICA NUMERIC(14,5),
//	PME_VL_TOTAL_CONDICAO_VENDA NUMERIC(15,2),
//	SITUACAO INT
//)

///** 
//   CRIAÇÃO DA TABELA TEMPORÁRIA DE PLI_DETALHE_MERCADORIA
//   GUARDA A LISTA DE DETALHE DA MERCADORIA QUE SERÃO VALIDADAS
//**/
//CREATE TABLE #TTPLI_DETALHE_MERCADORIA(
//	PDM_ID BIGINT,
//	PME_ID BIGINT,
//	DME_CO_DETALHE_MERCADORIA INT,
//	PDM_QT_UNID_COMERCIALIZADA NUMERIC(14, 5),
//	PDM_VL_CONDICAO_VENDA_DOLAR NUMERIC(18,7),
//	SITUACAO INT
//)

//BEGIN

//	SET @APLI_INDUSTRIALIZACAO = 1
//	SET @IMPORTADOR_PROBLEMA = 0
//	SET @PLI_QTD_MERCADORIA = 0
//	SET @ALI_QTD_DEFERIDA = 0
//	SET @ALI_QTD_INDEFERIDA = 0

//	--RN01 - PLI APTA PARA PROCESSAMENTO
//	--SELECIONA E INSERE TODOS OS PLIS COM STATUS DE AGUARDANDO PROCESSAMENTO
//	PRINT 'RN01'
//	INSERT INTO #TTPLI
//	SELECT
//		PLI_ID,
//		PLI_NU,
//		PLI_NU_ANO,
//		PLI_NU_CNPJ,
//		SET_CO,
//		SET_DS,
//		PLI_ST_PLI,
//		PAP_ID,
//		PLI_TP_DOCUMENTO,
//		PLI_NU_LI_REFERENCIA,
//		IMP_DS_RAZAO_SOCIAL,
//		PLI_NU_CPF_REP_LEGAL_SISCOMEX,
//		@REGISTRO_ATIVO
//	FROM SCIEX_PLI
//	WHERE PLI_ST_PLI = @PST_CO

//	PRINT 'LOOP PLI'

//	--RN01 - PLIS APTOS PARA PROFCESSAMENTO
//	--INICIA O LOOP NA LISTA DE PLIS RETORNADOS
//	WHILE EXISTS(SELECT* FROM #TTPLI WHERE SITUACAO = 1)
//	BEGIN

//		BEGIN TRY

//			BEGIN TRANSACTION; --INICIA A TRANSAÇÃO, CASO ACONTEÇA ALGUM ERRO, SEJA FEITO O ROLLBACK, SENAO COMMIT

//			SET @PLI_PROCESSADO = @REGISTRO_ATIVO

//			--SELECIONA UM PLI DA LISTA PARA SER TRABALHADO
//			SELECT TOP 1
//			   @PLI_ID = PLI_ID,
//			   @PLI_NU = PLI_NU,
//			   @PLI_NU_ANO = PLI_NU_ANO,
//			   @PLI_NU_CNPJ = PLI_NU_CNPJ,
//			   @PLI_SET_CO = SET_CO,
//			   @PLI_PST_ID = PST_ID,
//			   @PLI_PAP_ID = PAP_ID,
//			   @PLI_TP_DOCUMENTO = PLI_TP_DOCUMENTO,
//			   @PLI_NU_LI_REFERENCIA = PLI_NU_LI_REFERENCIA,
//			   @PLI_NO_CNPJ = IMP_DS_RAZAO_SOCIAL,
//			   @IMP_SET_DS = SET_DS,
//			   @PLI_CPF_SISCOMEX = PLI_NU_CPF_REP_LEGAL_SISCOMEX
//			FROM #TTPLI WHERE SITUACAO = @REGISTRO_ATIVO

//			--RN 14 - INICIO CONTROLE DE EXECUÇÃO DE SERVIÇO
//			INSERT INTO SCIEX_CONTROLE_EXEC_SERVICO
//			(CES_DH_EXECUCAO_INICIO, CES_ST_EXECUCAO, CES_NU_CPF_CNPJ_INTERESSADO, CES_NO_CPF_CNPJ_INTERESSADO, LSE_ID, CES_ME_OBJETO_ENVIO)
//			VALUES
//			(GETDATE(), 0, @PLI_NU_CNPJ, @PLI_NO_CNPJ, @LSE_ID, 'PLI(ID): ' + CAST(@PLI_ID as varchar))
//			--FIM RN 14 - INICIO CONTROLE DE EXECUÇÃO DE SERVIÇO

//			SET @CONTROLE_SERVICO_COD = @@IDENTITY

//			PRINT 'PLI ' + CAST(@PLI_ID AS VARCHAR)
//			PRINT @PLI_PST_ID

//			SELECT
//				@SITUACAO_EMPRESA_PLI = STI_DS,
//				@IMP_INS_CO = INS_CO
//			FROM VW_SCIEX_IMPORTADOR WHERE imp_nu_cnpj = @PLI_NU_CNPJ


//			--ERRO DE NUMERO DO CPF DO REPRESENTANTE LEGAL
//			IF(LEN(TRIM(@PLI_CPF_SISCOMEX)) = '' OR LEN(@PLI_CPF_SISCOMEX) < 11)
//			BEGIN

//				SET @IMPORTADOR_PROBLEMA = 1
//				SET @PLI_PROCESSADO = 0

//				SELECT
//					@ERRO_MENSAGEM_PROCESSAMENTO = EME_DS_ERRO
//				FROM SCIEX_ERRO_MENSAGEM WHERE EME_ID = @COD_ERRO_CPF_REPRESENTANTE

//				--RN 09 - REGISTRO DO ERRO
//				--INSERE TABELA SCIEX_ERRO_PROCESSAMENTO
//				INSERT INTO SCIEX_ERRO_PROCESSAMENTO
//				(EPR_DH_PROCESSAMENTO, PLI_ID, EME_ID, EPR_DS_MENSAGEM_ERRO, EPR_CO_NIVEL_ERRO)
//				VALUES
//				(GETDATE(), @PLI_ID, @COD_ERRO_CPF_REPRESENTANTE, @ERRO_MENSAGEM_PROCESSAMENTO, @NIVEL_ERRO_PLI)


//			END
//			--ELSE

//			IF(UPPER(@SITUACAO_EMPRESA_PLI) <> 'ATIVA')
//			BEGIN

//				--EMPRESA NÃO ATIVA, NÃO PROCESSAR PLI NEM GERAR ALI

//				SET @IMPORTADOR_PROBLEMA = 1
//				SET @PLI_PROCESSADO = 0

//				SELECT
//					@ERRO_MENSAGEM_PROCESSAMENTO = EME_DS_ERRO
//				FROM SCIEX_ERRO_MENSAGEM WHERE EME_ID = @COD_ERRO_SITUACAO_IMPORTADOR


//				SET @RETORNO = replace(@ERRO_MENSAGEM_PROCESSAMENTO, '[INS_CO]', CAST(@IMP_INS_CO AS VARCHAR))

//				--RN 09 - REGISTRO DO ERRO
//				--INSERE TABELA SCIEX_ERRO_PROCESSAMENTO
//				INSERT INTO SCIEX_ERRO_PROCESSAMENTO
//				(EPR_DH_PROCESSAMENTO, PLI_ID, EME_ID, EPR_DS_MENSAGEM_ERRO, EPR_CO_NIVEL_ERRO)
//				VALUES
//				(GETDATE(), @PLI_ID, @COD_ERRO_SITUACAO_IMPORTADOR, @RETORNO, @NIVEL_ERRO_PLI)

//			END
//			--ELSE

//			--RN 01 - ANO DO PLI
//			IF @PLI_NU_ANO<> YEAR(GETDATE())
//			BEGIN
//				PRINT 'RN01 - ANO PLI'

//				SET @IMPORTADOR_PROBLEMA = 1
//				SET @PLI_PROCESSADO = 0

//				SELECT
//					@ERRO_MENSAGEM_PROCESSAMENTO = EME_DS_ERRO
//				FROM SCIEX_ERRO_MENSAGEM WHERE EME_ID = @COD_ERRO_ANOPLI


//				SET @RETORNO = replace(@ERRO_MENSAGEM_PROCESSAMENTO, '[PLI_NU_ANO]', CAST(@PLI_NU_ANO AS VARCHAR))
//				SET @RETORNO = replace(@RETORNO, '[ANO_CORRENTE]', CAST(YEAR(GETDATE()) AS VARCHAR))

//				--RN 09 - REGISTRO DO ERRO
//				--INSERE TABELA SCIEX_ERRO_PROCESSAMENTO
//				INSERT INTO SCIEX_ERRO_PROCESSAMENTO
//				(EPR_DH_PROCESSAMENTO, PLI_ID, EME_ID, EPR_DS_MENSAGEM_ERRO, EPR_CO_NIVEL_ERRO)
//				VALUES
//				(GETDATE(), @PLI_ID, @COD_ERRO_ANOPLI, @RETORNO, @NIVEL_ERRO_PLI)

//			END-- FIM RN 01 - ANO DO PLI
//			--ELSE

//			-- RN18
//			IF(@PLI_TP_DOCUMENTO = 2)
//			BEGIN
//				--RN19
//				SELECT
//					@LI_ST = LI_ST,
//					@DI_ID = DI_ID

//				FROM
//				SCIEX_LI
//				WHERE SCIEX_LI.LI_NU = @PLI_NU_LI_REFERENCIA

//				IF(@LI_ST <> 1 OR @DI_ID <> NULL)
//				BEGIN
//					SET @IMPORTADOR_PROBLEMA = 1
//					SET @PLI_PROCESSADO = 0

//					SELECT
//						@ERRO_MENSAGEM_PROCESSAMENTO = EME_DS_ERRO
//					FROM SCIEX_ERRO_MENSAGEM WHERE EME_ID = 413

//					SET @RETORNO = replace(@ERRO_MENSAGEM_PROCESSAMENTO, '[PLI_NU_LI_REFERENCIA]', CAST(@PLI_NU_LI_REFERENCIA AS VARCHAR))
//					SET @RETORNO = replace(@RETORNO, '[PLI_NU_ANO/PLI_NU]', CAST(@PLI_NU_ANO AS VARCHAR) + '/' + CAST(@PLI_NU AS VARCHAR))

//					--RN 09 - REGISTRO DO ERRO
//					--INSERE TABELA SCIEX_ERRO_PROCESSAMENTO
//					INSERT INTO SCIEX_ERRO_PROCESSAMENTO
//					(EPR_DH_PROCESSAMENTO, PLI_ID, EME_ID, EPR_DS_MENSAGEM_ERRO, EPR_CO_NIVEL_ERRO)
//					VALUES
//					(GETDATE(), @PLI_ID, 413, @RETORNO, @NIVEL_ERRO_PLI)
//				END
//				--END RN19

//				--RN20
//				SELECT
//					@PLI_ID_REFERENCIA = SCIEX_PLI.PLI_ID
//				FROM
//				SCIEX_PLI
//				WHERE
//				SCIEX_PLI.PLI_NU_LI_REFERENCIA = @PLI_NU_LI_REFERENCIA
//				AND SCIEX_PLI.PLI_ID<> @PLI_ID


//				IF(@PLI_ID_REFERENCIA IS NOT NULL)
//				BEGIN
//					IF EXISTS
//					(
//							SELECT *
//							FROM SCIEX_PLI_MERCADORIA
//							INNER JOIN SCIEX_ALI on SCIEX_ALI.PME_ID = SCIEX_PLI_MERCADORIA.PME_ID
//							WHERE
//							SCIEX_PLI_MERCADORIA.PLI_ID = @PLI_ID_REFERENCIA AND
//							SCIEX_ALI.ALI_ST NOT IN(4, 5, 7, 8, 9)
//					)
//					BEGIN
//						SET @IMPORTADOR_PROBLEMA = 1
//						SET @PLI_PROCESSADO = 0

//						SELECT
//							@ERRO_MENSAGEM_PROCESSAMENTO = EME_DS_ERRO
//						FROM SCIEX_ERRO_MENSAGEM WHERE EME_ID = 412

//						SET @RETORNO = replace(@ERRO_MENSAGEM_PROCESSAMENTO, '[PLI_NU_LI_REFERENCIA]', CAST(@PLI_NU_LI_REFERENCIA AS VARCHAR))
//						SET @RETORNO = replace(@RETORNO, '[PLI_NU_ANO/PLI_NU]', CAST(@PLI_NU_ANO AS VARCHAR) + '/' + CAST(@PLI_NU AS VARCHAR))
//						--RN 09 - REGISTRO DO ERRO
//						--INSERE TABELA SCIEX_ERRO_PROCESSAMENTO
//						INSERT INTO SCIEX_ERRO_PROCESSAMENTO
//						(EPR_DH_PROCESSAMENTO, PLI_ID, EME_ID, EPR_DS_MENSAGEM_ERRO, EPR_CO_NIVEL_ERRO)
//						VALUES
//						(GETDATE(), @PLI_ID, 412, @RETORNO, @NIVEL_ERRO_PLI)
//					END
//				END
//				--END RN20

//				-- RN21
//				SELECT @LI_ORIGEM = SCIEX_LI_SUBSTITUIDA.pme_id_origem
//				FROM SCIEX_LI
//				INNER JOIN SCIEX_LI_SUBSTITUIDA ON SCIEX_LI.PME_ID = SCIEX_LI_SUBSTITUIDA.pme_id_substituta
//								or SCIEX_LI.PME_ID = SCIEX_LI_SUBSTITUIDA.pme_id_substituida
//				where SCIEX_LI.LI_NU = @PLI_NU_LI_REFERENCIA

//				IF(@LI_ORIGEM IS NOT NULL)
//				BEGIN
//					SELECT @LI_REFERENCIAS_QTD = MAX(SCIEX_LI_SUBSTITUIDA.lsu_nu)
//					FROM SCIEX_LI_SUBSTITUIDA
//					WHERE SCIEX_LI_SUBSTITUIDA.pme_id_origem = @LI_ORIGEM

//					IF(@LI_REFERENCIAS_QTD >= 3)
//					BEGIN
//						SET @IMPORTADOR_PROBLEMA = 1
//						SET @PLI_PROCESSADO = 0

//						SELECT
//							@ERRO_MENSAGEM_PROCESSAMENTO = EME_DS_ERRO
//						FROM SCIEX_ERRO_MENSAGEM WHERE EME_ID = 414

//						SET @RETORNO = replace(@ERRO_MENSAGEM_PROCESSAMENTO, '[PLI_NU_LI_REFERENCIA]', CAST(@PLI_NU_LI_REFERENCIA AS VARCHAR))
//						SET @RETORNO = replace(@RETORNO, '[PLI_NU_ANO/PLI_NU]', CAST(@PLI_NU_ANO AS VARCHAR) + '/' + CAST(@PLI_NU AS VARCHAR))

//						--RN 09 - REGISTRO DO ERRO
//						--INSERE TABELA SCIEX_ERRO_PROCESSAMENTO
//						INSERT INTO SCIEX_ERRO_PROCESSAMENTO
//						(EPR_DH_PROCESSAMENTO, PLI_ID, EME_ID, EPR_DS_MENSAGEM_ERRO, EPR_CO_NIVEL_ERRO)
//						VALUES
//						(GETDATE(), @PLI_ID, 414, @RETORNO, @NIVEL_ERRO_PLI)
//					END
//				END
//				--END RN21

//		   END
//			--END RN18

//		   --ELSE
//		   --BEGIN

//			   --GUARDA O CÓDIGO DE APLICAÇÃO DO PLI
//			   SELECT
//				   @PST_COD_APLICACAO = PAP_CO
//				FROM SCIEX_PLI_APLICACAO
//				WHERE PAP_ID = @PLI_PAP_ID

//				--RN02: CAPTURAR A SITUAÇÃO DO IMPORTADOR
//				SELECT
//					@SITUACAO_EMPRESA = STI_ID,
//					@IMP_INS_CO = INS_CO,
//					@IMP_MUN_CO = MUN_CO,
//					@IMP_UNI_CO = UND_CO
//				FROM VW_SCIEX_IMPORTADOR
//				WHERE imp_nu_cnpj = @PLI_NU_CNPJ

//				--RN04 SETOR X APLICACAO X MUNICIPIO
//				IF NOT EXISTS(
//					 SELECT 1
//					 FROM SCIEX_SETOR_APLICACAO
//					 WHERE
//					   PAP_ID = @PLI_PAP_ID AND
//					   SET_CO = @PLI_SET_CO AND
//					   @IMP_MUN_CO = MUN_CO AND
//					   SAP_ST = 1
//				  )
//				BEGIN

//						PRINT 'RN04'


//						SET @PLI_PROCESSADO = 0
//						SET @PLI_MERCADORIA_PROCESSADA = 0

//						SELECT
//							@ERRO_MENSAGEM_PROCESSAMENTO = EME_DS_ERRO
//						FROM SCIEX_ERRO_MENSAGEM WHERE EME_ID = @COD_ERRO_SETOR_MUNICIPIO

//						print @PLI_PAP_ID
//						print @IMP_SET_DS

//						SET @RETORNO = replace(@ERRO_MENSAGEM_PROCESSAMENTO, '[PAP_CO]', (SELECT PAP_CO FROM SCIEX_PLI_APLICACAO WHERE PAP_ID = @PLI_PAP_ID))
//						SET @RETORNO = replace(@RETORNO, '[SET_DS]', @IMP_SET_DS)
//						SET @RETORNO = replace(@RETORNO, '[MUN_CO]', @IMP_MUN_CO)

//						--RN 09 - REGISTRO DO ERRO
//						--INSERE TABELA SCIEX_ERRO_PROCESSAMENTO
//						INSERT INTO SCIEX_ERRO_PROCESSAMENTO
//						(EPR_DH_PROCESSAMENTO, PLI_ID, EME_ID, EPR_DS_MENSAGEM_ERRO, EPR_CO_NIVEL_ERRO)
//						VALUES
//						(GETDATE(), @PLI_ID, @COD_ERRO_SETOR_MUNICIPIO, @RETORNO, @NIVEL_ERRO_PLI)

//						PRINT @RETORNO

//				END

//				INSERT INTO #TTPLI_PRODUTO
//				SELECT
//					PLI_ID,
//					PPR_ID,
//					PPR_CO_PRODUTO,
//					PPR_CO_TP_PRODUTO,
//					PPR_CO_MODELO_PRODUTO,
//					@REGISTRO_ATIVO
//				FROM SCIEX_PLI_PRODUTO
//				WHERE PLI_ID = @PLI_ID

//				PRINT 'INICIO PRODUTO'
//				--INICIA O LOOP DOS PRODUTOS SELECIONADOS
//				WHILE EXISTS(SELECT* FROM #TTPLI_PRODUTO WHERE SITUACAO = @REGISTRO_ATIVO)
//				BEGIN

//					--SELECIONA UM PRODUTO DA LISTA PARA SER TRABALHADO
//					SELECT TOP 1
//						@PPR_ID = PPR_ID,
//						@PPR_CO_PRODUTO = PPR_CO_PRODUTO,
//						@PPR_CO_TP_PRODUTO = PPR_CO_TP_PRODUTO,
//						@PPR_CO_MODELO_PRODUTO = PPR_CO_MODELO_PRODUTO
//					FROM #TTPLI_PRODUTO WHERE SITUACAO = @REGISTRO_ATIVO
								
//					--INSERE TODAS AS MERCADORIAS DO PLI E PRODUTO
//					INSERT INTO #TTPLI_MERCADORIA
//					SELECT
//						PME_ID,
//						PME_CO_PRODUTO,
//						MER_CO_NCM_MERCADORIA,
//						RTB_ID,
//						FLE_ID,
//						PME_QT_UNID_MEDIDA_ESTATISTICA,
//						PME_VL_TOTAL_CONDICAO_VENDA,
//						@REGISTRO_ATIVO
//					FROM SCIEX_PLI_MERCADORIA
//					WHERE PLI_ID = @PLI_ID AND PPR_ID = @PPR_ID

//					PRINT 'INICIO MERCADORIA'
//					WHILE EXISTS(SELECT* FROM #TTPLI_MERCADORIA WHERE SITUACAO = @REGISTRO_ATIVO)
//					BEGIN

//						--CONTROLA SE A MERCADORIA TERÁ OU NÃO ALGUM PROBLEMA
//						SET @PLI_MERCADORIA_PROCESSADA = 1

//						--SELECIONA UMA MERCADORIA DA LISTA PARA SER TRABALHADA
//						SELECT TOP 1
//						  @PME_ID = PME_ID,
//						  @PME_MER_CO_NCM_MERCADORIA = MER_CO_NCM_MERCADORIA,
//						  @PME_RTB_ID = RTB_ID,
//						  @PME_FLE_ID = FLE_ID,
//						  @PME_CO_PRODUTO = PME_CO_PRODUTO,
//						  @PME_QTD_UNID_MEDIDA_ESTATISTICA = PME_QT_UNID_MEDIDA_ESTATISTICA
//						FROM #TTPLI_MERCADORIA WHERE SITUACAO = @REGISTRO_ATIVO

//						-- RN 03 - NCM DE EXCEÇÃO
//						IF EXISTS(SELECT
//									  NEX_ID
//								  FROM SCIEX_NCM_EXCECAO
//								  WHERE NEX_CO_NCM = @PME_MER_CO_NCM_MERCADORIA AND
//										NEX_CO_MUNICIPIO = @IMP_MUN_CO AND
//										NEX_CO_SETOR = @PLI_SET_CO AND
//										NEX_ST = @REGISTRO_ATIVO AND
//										CONVERT(DATE, NEX_DT_INICIO_VIGENCIA,103) <= CONVERT(DATE, GETDATE(), 103) )
//						BEGIN

//							PRINT 'RN03'


//							SET @PLI_PROCESSADO = 0
//							SET @PLI_MERCADORIA_PROCESSADA = 0

//							SELECT
//								@ERRO_MENSAGEM_PROCESSAMENTO = EME_DS_ERRO
//							FROM SCIEX_ERRO_MENSAGEM WHERE EME_ID = @COD_ERRO_NCM_EXCECAO

//							SET @RETORNO = replace(@ERRO_MENSAGEM_PROCESSAMENTO, '[MER_CO_NCM_MERCADORIA]', @PME_MER_CO_NCM_MERCADORIA)

//							--RN 09 - REGISTRO DO ERRO
//							--INSERE TABELA SCIEX_ERRO_PROCESSAMENTO
//							INSERT INTO SCIEX_ERRO_PROCESSAMENTO
//							(EPR_DH_PROCESSAMENTO, PLI_ID, EPR_ID_MERC_DETALHE, EME_ID, EPR_DS_MENSAGEM_ERRO, EPR_CO_NIVEL_ERRO)
//							VALUES
//							(GETDATE(), @PLI_ID, @PME_ID, @COD_ERRO_NCM_EXCECAO, @RETORNO, @NIVEL_ERRO_MERC)

//							PRINT @RETORNO

//						END
//						-- FIM RN 03 - NCM DE EXECEÇÃO

//						--RN 04 - REGIME TRIBUTÁRIO DA MERCADORIA
//					   IF NOT EXISTS(
//						   SELECT
//							   *
//						   FROM SCIEX_REGIME_TRIBUTARIO_MERCADORIA
//						   WHERE

//							   RTB_ID = @PME_RTB_ID AND
//							   FLE_ID = @PME_FLE_ID AND
//							   RTM_CO_MUNICIPIO = @IMP_MUN_CO AND
//							   RTM_ST = @REGISTRO_ATIVO AND
//							   CONVERT(DATE, RTM_DT_INICIO_VIGENCIA,103) <= CONVERT(DATE, GETDATE(), 103)
//						)
//						BEGIN

//							PRINT 'RN04'

//							SET @PLI_MERCADORIA_PROCESSADA = 0

//							--SELECIONAR O CODIGO DO REGIME TRIBUTARIO
//							SELECT
//								@PME_RTB_CO = RTB_CO
//							FROM SCIEX_REGIME_TRIBUTARIO WHERE RTB_ID = @PME_RTB_ID

//							--SELECIONAR O CODIGO DO FUNDAMENTO LEGAL
//							SELECT
//								@PME_FLE_CO = FLE_CO
//							FROM SCIEX_FUNDAMENTO_LEGAL WHERE FLE_ID = @PME_FLE_ID


//							SET @PLI_PROCESSADO = 0

//							SELECT
//								@ERRO_MENSAGEM_PROCESSAMENTO = EME_DS_ERRO
//							FROM SCIEX_ERRO_MENSAGEM WHERE EME_ID = @COD_ERRO_REGIME_TRIBUTARIO

//							SET @RETORNO = replace(@ERRO_MENSAGEM_PROCESSAMENTO, '[RTB_CO]', @PME_RTB_CO)
//							SET @RETORNO = replace(@RETORNO, '[FLE_CO]', @PME_FLE_CO)
//							SET @RETORNO = replace(@RETORNO, '[MER_CO_NCM_MERCADORIA]', @PME_MER_CO_NCM_MERCADORIA)
//							SET @RETORNO = replace(@RETORNO, '[MUN_CO]', @IMP_MUN_CO)

//							--RN 09 - REGISTRO DO ERRO
//							--INSERE TABELA SCIEX_ERRO_PROCESSAMENTO
//							INSERT INTO SCIEX_ERRO_PROCESSAMENTO
//							(EPR_DH_PROCESSAMENTO, PLI_ID, EPR_ID_MERC_DETALHE, EME_ID, EPR_DS_MENSAGEM_ERRO, EPR_CO_NIVEL_ERRO)
//							VALUES
//							(GETDATE(), @PLI_ID, @PME_ID, @COD_ERRO_REGIME_TRIBUTARIO, @RETORNO, @NIVEL_ERRO_MERC)

//							PRINT @RETORNO

//						END
//						-- FIM RN 04 - REGIME TRIBUTÁRIO DA MERCADORIA

//						IF @PST_COD_APLICACAO = @APLI_INDUSTRIALIZACAO--CASO PLI SEJA DE INDUSTRIALIZAÇÃO
//					   BEGIN

//							PRINT 'PLI INDUSTRIALIZAÇÃO'
//							-- RN 05 - VALIDAR PRODUTO
//							IF NOT EXISTS(
//								SELECT
//								   *
//								FROM VW_SCIEX_PRODUTO_EMPRESA
//								WHERE
//									PEM_NU_CNPJ = @PLI_NU_CNPJ AND
//									PEM_CO_PRODUTO = @PPR_CO_PRODUTO AND
//									PEM_CO_TP_PRODUTO = @PPR_CO_TP_PRODUTO AND
//									PEM_CO_MODELO_PRODUTO = @PPR_CO_MODELO_PRODUTO
//							)
//							BEGIN
//								PRINT 'RN05'

//								SET @PLI_MERCADORIA_PROCESSADA = 0
//								SET @PLI_PROCESSADO = 0

//								-- CASO NAO TENHA PRODUTO PARA O IMPORTADOR
//								IF NOT EXISTS(
//									SELECT
//									   *
//									FROM VW_SCIEX_PRODUTO_EMPRESA
//									WHERE
//										PEM_NU_CNPJ = @PLI_NU_CNPJ AND
//										PEM_CO_PRODUTO = @PPR_CO_PRODUTO
//								)
//								BEGIN

//									SET @PLI_PROCESSADO = 0
//									SET @PLI_MERCADORIA_PROCESSADA = 0

//									SELECT
//										@ERRO_MENSAGEM_PROCESSAMENTO = EME_DS_ERRO
//									FROM SCIEX_ERRO_MENSAGEM WHERE EME_ID = @COD_ERRO_PRODUTO

//									SET @RETORNO = replace(@ERRO_MENSAGEM_PROCESSAMENTO, '[PEM_CO_PRODUTO]', @PPR_CO_PRODUTO)
//									SET @RETORNO = replace(@RETORNO, '[PLI_NU_CNPJ]', @PLI_NU_CNPJ)

//									--RN 09 - REGISTRO DO ERRO
//									--INSERE TABELA SCIEX_ERRO_PROCESSAMENTO
//									INSERT INTO SCIEX_ERRO_PROCESSAMENTO
//									(EPR_DH_PROCESSAMENTO, PLI_ID, EPR_ID_MERC_DETALHE, EME_ID, EPR_DS_MENSAGEM_ERRO, EPR_CO_NIVEL_ERRO)
//									VALUES
//									(GETDATE(), @PLI_ID, @PME_ID, @COD_ERRO_PRODUTO, @RETORNO, @NIVEL_ERRO_MERC)

//									PRINT @RETORNO

//								END
//								ELSE --CASO NAO TENHA TIPO DE PRODUTO PARA O IMPORTADOR
//								IF NOT EXISTS(
//									SELECT
//									   *
//									FROM VW_SCIEX_PRODUTO_EMPRESA
//									WHERE
//										PEM_NU_CNPJ = @PLI_NU_CNPJ AND
//										PEM_CO_PRODUTO = @PPR_CO_PRODUTO AND
//										PEM_CO_TP_PRODUTO = @PPR_CO_TP_PRODUTO
//								)
//								BEGIN

//									SET @PLI_PROCESSADO = 0
//									SET @PLI_MERCADORIA_PROCESSADA = 0

//									SELECT
//										@ERRO_MENSAGEM_PROCESSAMENTO = EME_DS_ERRO
//									FROM SCIEX_ERRO_MENSAGEM WHERE EME_ID = @COD_ERRO_PRODUTO_TIPO

//									SET @RETORNO = replace(@ERRO_MENSAGEM_PROCESSAMENTO, '[PEM_CO_TP_PRODUTO]', @PPR_CO_TP_PRODUTO)
//									SET @RETORNO = replace(@RETORNO, '[PEM_CO_PRODUTO]', @PPR_CO_PRODUTO)

//									--RN 09 - REGISTRO DO ERRO
//									--INSERE TABELA SCIEX_ERRO_PROCESSAMENTO
//									INSERT INTO SCIEX_ERRO_PROCESSAMENTO
//									(EPR_DH_PROCESSAMENTO, PLI_ID, EPR_ID_MERC_DETALHE, EME_ID, EPR_DS_MENSAGEM_ERRO, EPR_CO_NIVEL_ERRO)
//									VALUES
//									(GETDATE(), @PLI_ID, @PME_ID, @COD_ERRO_PRODUTO_TIPO, @RETORNO, @NIVEL_ERRO_MERC)

//									PRINT @RETORNO

//								END
//								ELSE --CASO NAO TENHA MODELO PRODUTO PARA O IMPORTADOR
//								IF NOT EXISTS(
//									SELECT
//									   *
//									FROM VW_SCIEX_PRODUTO_EMPRESA
//									WHERE
//										PEM_NU_CNPJ = @PLI_NU_CNPJ AND
//										PEM_CO_PRODUTO = @PPR_CO_PRODUTO AND
//										PEM_CO_TP_PRODUTO = @PPR_CO_TP_PRODUTO AND
//										PEM_CO_MODELO_PRODUTO = @PPR_CO_MODELO_PRODUTO
//								)
//								BEGIN

//									SET @PLI_PROCESSADO = 0
//									SET @PLI_MERCADORIA_PROCESSADA = 0

//									SELECT
//										@ERRO_MENSAGEM_PROCESSAMENTO = EME_DS_ERRO
//									FROM SCIEX_ERRO_MENSAGEM WHERE EME_ID = @COD_ERRO_PRODUTO_MODELO

//									SET @RETORNO = replace(@ERRO_MENSAGEM_PROCESSAMENTO, '[PEM_CO_MODELO_PRODUTO]', @PPR_CO_MODELO_PRODUTO)
//									SET @RETORNO = replace(@RETORNO, '[PEM_CO_PRODUTO]', @PPR_CO_PRODUTO)
//									SET @RETORNO = replace(@RETORNO, '[PEM_CO_TP_PRODUTO]', @PPR_CO_TP_PRODUTO)

//									--RN 09 - REGISTRO DO ERRO
//									--INSERE TABELA SCIEX_ERRO_PROCESSAMENTO
//									INSERT INTO SCIEX_ERRO_PROCESSAMENTO
//									(EPR_DH_PROCESSAMENTO, PLI_ID, EPR_ID_MERC_DETALHE, EME_ID, EPR_DS_MENSAGEM_ERRO, EPR_CO_NIVEL_ERRO)
//									VALUES
//									(GETDATE(), @PLI_ID, @PME_ID, @COD_ERRO_PRODUTO_MODELO, @RETORNO, @NIVEL_ERRO_MERC)

//									PRINT @RETORNO

//								END


//							END
//							--ELSE
//							--BEGIN

//							---- RN VALIDAÇÃO DO VALOR DO CRA:
//							----SE O CÓDIGO DO CRII FOR IGUAL A 2, VALIDAR SE O VALOR DO CRA É IGUAL A 88
//						 --  SELECT
//						 --     @COD_CRII = PEM_CO_CRII
//						 --  FROM VW_SCIEX_PRODUTO_EMPRESA
//							--WHERE
//							--      PEM_NU_CNPJ = @PLI_NU_CNPJ AND
//							--      PEM_CO_PRODUTO = @PPR_CO_PRODUTO AND
//							--      PEM_CO_TP_PRODUTO = @PPR_CO_TP_PRODUTO AND
//							--      PEM_CO_MODELO_PRODUTO = @PPR_CO_MODELO_PRODUTO

//							--  IF @COD_CRII = 2
//							--  BEGIN

//							--      SELECT
//							--          @VL_CRA = PME_VL_CRA
//							--      FROM SCIEX_PLI_MERCADORIA WHERE PME_ID = @PME_ID

//							--      IF(@VL_CRA <> 88.00)
//							--      BEGIN

//							--          SET @PLI_PROCESSADO = 0
//							--          SET @PLI_MERCADORIA_PROCESSADA = 0

//							--          SELECT
//							--              @ERRO_MENSAGEM_PROCESSAMENTO = EME_DS_ERRO
//							--          FROM SCIEX_ERRO_MENSAGEM WHERE EME_ID = @COD_ERRO_CRA

//							--          SET @RETORNO = replace(@ERRO_MENSAGEM_PROCESSAMENTO, '[PEM_CO_MODELO_PRODUTO]', @PPR_CO_MODELO_PRODUTO)
//							--          SET @RETORNO = replace(@RETORNO, '[PEM_CO_PRODUTO]', @PPR_CO_PRODUTO)
//							--          SET @RETORNO = replace(@RETORNO, '[PEM_CO_TP_PRODUTO]', @PPR_CO_TP_PRODUTO)

//							----RN 09 - REGISTRO DO ERRO
//							----INSERE TABELA SCIEX_ERRO_PROCESSAMENTO
//							--INSERT INTO SCIEX_ERRO_PROCESSAMENTO
//							--(EPR_DH_PROCESSAMENTO, PLI_ID, EPR_ID_MERC_DETALHE, EME_ID, EPR_DS_MENSAGEM_ERRO, EPR_CO_NIVEL_ERRO)
//							--          VALUES
//							--(GETDATE(), @PLI_ID, @PME_ID, @COD_ERRO_CRA, @RETORNO, @NIVEL_ERRO_MERC)

//							--          PRINT @RETORNO

//							--END

//							--  END


//							--END
//							-- FIM RN 05 - VALIDAR PRODUTO


//							-- RN 06 - VALIDAR MERCADORIA
//							IF NOT EXISTS(
//								SELECT
//									*
//								FROM VW_SCIEX_MERCADORIA
//								WHERE
//									MER_CO_PRODUTO = @PME_CO_PRODUTO AND
//									MER_CO_NCM_MERCADORIA = @PME_MER_CO_NCM_MERCADORIA AND
//									MER_ST_MERCADORIA = @REGISTRO_ATIVO
//							)
//							BEGIN
//								PRINT 'RN06'
//								SET @PLI_PROCESSADO = 0
//								SET @PLI_MERCADORIA_PROCESSADA = 0

//								SELECT
//									@ERRO_MENSAGEM_PROCESSAMENTO = EME_DS_ERRO
//								FROM SCIEX_ERRO_MENSAGEM WHERE EME_ID = @COD_ERRO_MERCADORIA

//								SET @RETORNO = replace(@ERRO_MENSAGEM_PROCESSAMENTO, '[MER_CO_NCM_MERCADORIA]', @PME_MER_CO_NCM_MERCADORIA)
//								SET @RETORNO = replace(@RETORNO, '[PEM_CO_PRODUTO]', @PPR_CO_PRODUTO)

//								--RN 09 - REGISTRO DO ERRO
//								--INSERE TABELA SCIEX_ERRO_PROCESSAMENTO
//								INSERT INTO SCIEX_ERRO_PROCESSAMENTO
//								(EPR_DH_PROCESSAMENTO, PLI_ID, EPR_ID_MERC_DETALHE, EME_ID, EPR_DS_MENSAGEM_ERRO, EPR_CO_NIVEL_ERRO)
//								VALUES
//								(GETDATE(), @PLI_ID, @PME_ID, @COD_ERRO_MERCADORIA, @RETORNO, @NIVEL_ERRO_MERC)

//								PRINT @RETORNO

//							END
//							-- FIM RN 06 - VALIDAR MERCADORIA

//							-- RN 07 - VALIDAR DETALHE MERCADORIA
//							INSERT INTO #TTPLI_DETALHE_MERCADORIA
//							SELECT
//								PDM_ID,
//								PME_ID,
//								DME_CO_DETALHE_MERCADORIA,
//								PDM_QT_UNID_COMERCIALIZADA,
//								PDM_VL_CONDICAO_VENDA_DOLAR,
//								@REGISTRO_ATIVO
//							FROM SCIEX_PLI_DETALHE_MERCADORIA
//							WHERE PME_ID = @PME_ID

//							PRINT 'INICIO DETALHE MERCADORIA'
//							WHILE EXISTS(SELECT* FROM #TTPLI_DETALHE_MERCADORIA WHERE SITUACAO = @REGISTRO_ATIVO)
//							BEGIN


//								--SELECIONA UM ITEM DA TABELA #TTPLI_DETALHE_MERCADORIA PARA SER TRABALHADO
//								SELECT TOP 1
//									@PDM_ID = PDM_ID, 
//									@PDM_CO_DETALHE_MERCADORIA = DME_CO_DETALHE_MERCADORIA,
//									@PDM_QTD_UNIDADE_COMERCIALIZADA = PDM_QT_UNID_COMERCIALIZADA,
//									@PDM_VL_CONDICAO_VENDA_DOLAR = PDM_VL_CONDICAO_VENDA_DOLAR
//								FROM #TTPLI_DETALHE_MERCADORIA WHERE SITUACAO = @REGISTRO_ATIVO

//								IF NOT EXISTS(
//									SELECT
//										*
//									FROM VW_SCIEX_DETALHE_MERCADORIA
//									WHERE
//										DME_CO_PRODUTO = @PME_CO_PRODUTO AND
//										DME_CO_NCM_MERCADORIA = @PME_MER_CO_NCM_MERCADORIA AND
//										DME_CO_DETALHE_MERCADORIA = @PDM_CO_DETALHE_MERCADORIA AND
//										DME_ST_DETALHE_MERCADORIA = @REGISTRO_ATIVO
//								)
//								BEGIN

//									SET @PLI_PROCESSADO = 0
//									SET @PLI_MERCADORIA_PROCESSADA = 0

//									SELECT
//										@ERRO_MENSAGEM_PROCESSAMENTO = EME_DS_ERRO
//									FROM SCIEX_ERRO_MENSAGEM WHERE EME_ID = @COD_ERRO_DETALHE_MERCADORIA

//									SET @RETORNO = replace(@ERRO_MENSAGEM_PROCESSAMENTO, '[DME_CO_DETALHE_MERCADORIA]', @PDM_CO_DETALHE_MERCADORIA)
//									SET @RETORNO = replace(@RETORNO, '[MER_CO_NCM_MERCADORIA]', @PME_MER_CO_NCM_MERCADORIA)
//									SET @RETORNO = replace(@RETORNO, '[PEM_CO_PRODUTO]', @PME_CO_PRODUTO)

//									--RN 09 - REGISTRO DO ERRO
//									--INSERE TABELA SCIEX_ERRO_PROCESSAMENTO
//									INSERT INTO SCIEX_ERRO_PROCESSAMENTO
//									(EPR_DH_PROCESSAMENTO, PLI_ID, EPR_ID_MERC_DETALHE, EME_ID, EPR_DS_MENSAGEM_ERRO, EPR_CO_NIVEL_ERRO)
//									VALUES
//									(GETDATE(), @PLI_ID, @PDM_ID, @COD_ERRO_DETALHE_MERCADORIA, @RETORNO, @NIVEL_ERRO_DETA)

//									PRINT @RETORNO

//								END

//								--RN 08 - ATUALIZAR COTA / CRÉDITO
//								/**
//									TIPO DA ATUALIZAÇÃO                         - 2-DÉBITO(1-CRÉDITO,2-DÉBITO)
//									NUMERO DO PLI                               - @PLI_NU                             - SCIEX_PLI
//									DATA DO ENVIO DO PLI                        - @PLI_DT_ENVIO                       - SCIEX_PLI
//									INSCRICAO CADASTRAL                         - @PLI_INS_CO                         - SCIEX_PLI
//									CODIGO DO PRODURO                           - @PME_CO_PRODUTO                     - SCIEX_PLI_MERCADORIA
//									CODIGO DO TIPO DO PRODUTO                   - @PME_CO_TP_PRODUTO                  - SCIEX_PLI_MERCADORIA
//									CODIGO DO MODELO DO PRODUTO                 - @PME_CO_MODELO_PRODUTO              - SCIEX_PLI_MERCADORIA
//									CODIGO MERCADORIA                           - @PME_MER_CO_NCM_MERCADORIA          - SCIEX_PLI_MERCADORIA
//									CODIGO DO DETALHE DA MERCADORIA             - @PDM_CO_DETALHE_MERCADORIA          - SCIEX_PLI_DETALHE_MERCADORIA
//									QUANTIDADE DE UNIDADE DE MEDIDA ESTATISTICA - @PME_QTD_UNID_MEDIDA_ESTATISTICA    - SCIEX_PLI_MERCADORIA
//									QUANTIDADE DA UNIDADE COMERCIALIZADA        - @PDM_QTD_UNIDADE_COMERCIALIZADA     - SCIEX_PLI_DETALHE_MERCADORIA
//									VALOR DA CONDIÇÃO DE VENDA EM DOLAR         - @PDM_VL_CONDICAO_VENDA_DOLAR        - SCIEX_PLI_DETALHE_MERCADORIA
//								**/

//								IF(@PLI_PROCESSADO = 1)
//								BEGIN

//									PRINT 'ATUALIZANDO COTA/CRÉDITO'

//								END

//								UPDATE #TTPLI_DETALHE_MERCADORIA
//								SET SITUACAO = 2
//								WHERE PDM_ID = @PDM_ID

//							END
//							-- FIM RN 07 - VALIDAR DETALHE MERCADORIA

//							DELETE FROM #TTPLI_DETALHE_MERCADORIA

//							--VALOR TOTAL DAS MERCADORIAS
//							--O SISTEMA DEVE COMPARAR O VALOR TOTAL DA CONDIÇÃO DE VENDA DA MERCADORIA COM O SOMATÓRIO DO
//							--VALOR DA CONDIÇÃO DE VENDA DE SEUS DETALHES. CASO TENHA UMA DIFERENTE DE ±0,05, 
//							--NÃO SERÁ APROVADO O PROCESSAMENTO.
//							SELECT
//								@VL_TOTAL_CONDICAO_VENDA_DETALHE = SUM(ISNULL(PDM_VL_CONDICAO_VENDA, 0))
//							FROM SCIEX_PLI_DETALHE_MERCADORIA
//							WHERE PME_ID = @PME_ID

//							SELECT
//								@VL_TOTAL_CONDICAO_VENDA = SUM(ISNULL(PME_VL_TOTAL_CONDICAO_VENDA, 0))
//							FROM SCIEX_PLI_MERCADORIA
//							WHERE PME_ID = @PME_ID


//							PRINT(ISNULL(@VL_TOTAL_CONDICAO_VENDA, 0) - @VL_TOTAL_CONDICAO_VENDA_DETALHE)

//							PRINT ISNULL(@VL_TOTAL_CONDICAO_VENDA,0)
//							PRINT ISNULL(@VL_TOTAL_CONDICAO_VENDA_DETALHE,0)

//							IF(((ISNULL(@VL_TOTAL_CONDICAO_VENDA, 0) - @VL_TOTAL_CONDICAO_VENDA_DETALHE) > 0.05)  or
//								 ((ISNULL(@VL_TOTAL_CONDICAO_VENDA, 0) - @VL_TOTAL_CONDICAO_VENDA_DETALHE) < -0.05))
//							BEGIN

//								SET @PLI_PROCESSADO = 0
//								SET @PLI_MERCADORIA_PROCESSADA = 0

//								SELECT
//									@ERRO_MENSAGEM_PROCESSAMENTO = EME_DS_ERRO
//								FROM SCIEX_ERRO_MENSAGEM WHERE EME_ID = @COD_ERRO_VALOR_MERCADORIA

//								SET @RETORNO = replace(@ERRO_MENSAGEM_PROCESSAMENTO, '[PME_VL_TOTAL_CONDICAO_VENDA]', @VL_TOTAL_CONDICAO_VENDA)


//								--RN 09 - REGISTRO DO ERRO
//								--INSERE TABELA SCIEX_ERRO_PROCESSAMENTO
//								INSERT INTO SCIEX_ERRO_PROCESSAMENTO
//								(EPR_DH_PROCESSAMENTO, PLI_ID, EPR_ID_MERC_DETALHE, EME_ID, EPR_DS_MENSAGEM_ERRO, EPR_CO_NIVEL_ERRO)
//								VALUES
//								(GETDATE(), @PLI_ID, @PME_ID, @COD_ERRO_MERCADORIA, @RETORNO, @NIVEL_ERRO_MERC)

//								PRINT @RETORNO

//							END

//						END


//						--RN 10 - GERAÇÃO DA ALI
//						IF @IMPORTADOR_PROBLEMA = 1 OR @PLI_PROCESSADO = 0--SE TIVER PROBLEMA NO IMPORTADOR TODAS AS ALI SERÃO GERADAS COMO INDEFERIDAS
//						BEGIN

//							SET @ALI_QTD_INDEFERIDA = @ALI_QTD_INDEFERIDA + 1

//							IF NOT EXISTS(SELECT * FROM SCIEX_ALI WHERE PME_ID = @PME_ID)
//							BEGIN

//								IF NOT EXISTS(SELECT MAX(ALI_NU) FROM SCIEX_ALI)
//								BEGIN--CASO NAO EXISTA AINDA NUMERO DA ALI NA BASE
//								   SELECT @ALI_NUMERO =
//									   SUBSTRING(CAST(YEAR(GETDATE()) AS VARCHAR), 3, 2) +
//									   CAST((CAST(PCO_VL_PARAMETRO AS BIGINT) + 1) AS VARCHAR)
//									FROM SCIEX_PARAMETRO_CONFIGURACAO
//									WHERE PCO_DS_PARAMETRO = 'VALOR INICIAL DA ALI'
//								END
//								BEGIN
//									--PEGA A ULTIMA ALI GERADA
//									SELECT @ALI_NUMERO_ATUAL = MAX(ALI_NU) FROM SCIEX_ALI

//									PRINT SUBSTRING(CAST(@ALI_NUMERO_ATUAL AS VARCHAR) , 1,2 )

//									--VERIFICA SE A ULTIMA ALI É DO MESMO ANO ATUAL
//									IF(SUBSTRING(CAST(YEAR(GETDATE()) AS VARCHAR), 3, 2) = SUBSTRING(CAST(@ALI_NUMERO_ATUAL AS VARCHAR), 1, 2))
//									BEGIN
//										SET @ALI_NUMERO = CAST((SELECT MAX(ALI_NU) + 1 FROM SCIEX_ALI) AS BIGINT )
//									END
//									ELSE --SE FOR NOVO ANO
//								   BEGIN
//										SELECT @ALI_NUMERO =
//											SUBSTRING(CAST(YEAR(GETDATE()) AS VARCHAR), 3, 2) +
//											CAST((CAST(PCO_VL_PARAMETRO AS BIGINT) + 1) AS VARCHAR)
//										FROM SCIEX_PARAMETRO_CONFIGURACAO
//										WHERE PCO_DS_PARAMETRO = 'VALOR INICIAL DA ALI'
//									END

//								END

//								INSERT INTO SCIEX_ALI
//								(PME_ID, ALI_NU, ALI_ST, ALI_TP, ALI_DH_CADASTRO, ALI_DH_PROCESSAMENTO_SUFRAMA)
//								VALUES
//								(@PME_ID, @ALI_NUMERO, @ALI_COD_INDEFERIDA_SUFRAMA, @PLI_TP_DOCUMENTO, GETDATE(), GETDATE())

//							END
//							ELSE
//							BEGIN

//								UPDATE SCIEX_ALI
//								SET ALI_ST = @ALI_COD_INDEFERIDA_SUFRAMA,
//									ALI_DH_PROCESSAMENTO_SUFRAMA = GETDATE()
//								WHERE PME_ID = @PME_ID

//							END

//						END
//						ELSE-- CASO NÃO TENHA ACONTECIDO PROBLEMA NA RN02
//					   BEGIN

//							IF @PLI_MERCADORIA_PROCESSADA = 1--CONTROLA SE A MERCADORIA TEVE ALGUM PROBLEMA DURANTE A VALIDÇÃO
//							BEGIN

//								SET @ALI_QTD_DEFERIDA = @ALI_QTD_DEFERIDA + 1

//								IF NOT EXISTS(SELECT * FROM SCIEX_ALI WHERE PME_ID = @PME_ID)
//								BEGIN

//									IF NOT EXISTS(SELECT MAX(ALI_NU) FROM SCIEX_ALI)
//									BEGIN--CASO NAO EXISTA AINDA NUMERO DA ALI NA BASE
//									   SELECT @ALI_NUMERO =
//										   SUBSTRING(CAST(YEAR(GETDATE()) AS VARCHAR), 3, 2) +
//										   CAST((CAST(PCO_VL_PARAMETRO AS BIGINT) + 1) AS VARCHAR)
//										FROM SCIEX_PARAMETRO_CONFIGURACAO
//										WHERE PCO_DS_PARAMETRO = 'VALOR INICIAL DA ALI'
//									END
//									BEGIN
//										--PEGA A ULTIMA ALI GERADA
//										SELECT @ALI_NUMERO_ATUAL = MAX(ALI_NU) FROM SCIEX_ALI

//										PRINT SUBSTRING(CAST(@ALI_NUMERO_ATUAL AS VARCHAR) , 1,2 )

//										--VERIFICA SE A ULTIMA ALI É DO MESMO ANO ATUAL
//										IF(SUBSTRING(CAST(YEAR(GETDATE()) AS VARCHAR), 3, 2) = SUBSTRING(CAST(@ALI_NUMERO_ATUAL AS VARCHAR), 1, 2))
//										BEGIN
//											SET @ALI_NUMERO = CAST((SELECT MAX(ALI_NU) + 1 FROM SCIEX_ALI) AS BIGINT )
//										END
//										ELSE --SE FOR NOVO ANO
//									   BEGIN
//											SELECT @ALI_NUMERO =
//												SUBSTRING(CAST(YEAR(GETDATE()) AS VARCHAR), 3, 2) +
//												CAST((CAST(PCO_VL_PARAMETRO AS BIGINT) + 1) AS VARCHAR)
//											FROM SCIEX_PARAMETRO_CONFIGURACAO
//											WHERE PCO_DS_PARAMETRO = 'VALOR INICIAL DA ALI'
//										END

//									END

//									INSERT INTO SCIEX_ALI
//									(PME_ID, ALI_NU, ALI_ST, ALI_TP, ALI_DH_CADASTRO, ALI_DH_PROCESSAMENTO_SUFRAMA)
//									VALUES
//									(@PME_ID, @ALI_NUMERO, @ALI_COD_GERADA_COM_SUCESS0, @PLI_TP_DOCUMENTO, GETDATE(), GETDATE())

//									-- RN13 - LANÇAMENTO
//									INSERT INTO SCIEX_LANCAMENTO
//									(CLA_ID, LAN_DS_OBSERVACAO, LAN_CO_UNIDADE_CADASTRADORA, LAN_DH_CADASTRO, LAN_NU_CPFCNPJ_RESPONSAVEL, PME_ID, PLI_ID)
//									VALUES
//									(@LAN_COD, @LAN_DESCRICAO, @IMP_UNI_CO, GETDATE(), @USU_CPFCNPJ, @PME_ID, @PLI_ID)

//								END
//								ELSE
//								BEGIN

//									--BUSCA O STATUS ATUAL DA ALI ANTES DE ATUALIZAR
//									SELECT
//										@ALI_STATUS = ALI_ST
//									FROM SCIEX_ALI
//									WHERE PME_ID = @PME_ID

//									--CASO A ALI ANTERIOR ESTIVER COM O STATUS DE INDEFERIDA E AGORA PASSA A SER DEFERIDA
//									IF(@ALI_STATUS = @ALI_COD_INDEFERIDA_SUFRAMA)
//									BEGIN

//										-- RN13 - LANÇAMENTO
//										INSERT INTO SCIEX_LANCAMENTO
//										(CLA_ID, LAN_DS_OBSERVACAO, LAN_CO_UNIDADE_CADASTRADORA, LAN_DH_CADASTRO, LAN_NU_CPFCNPJ_RESPONSAVEL, PME_ID, PLI_ID)
//										VALUES
//										(@LAN_COD, @LAN_DESCRICAO, @IMP_UNI_CO, GETDATE(), @USU_CPFCNPJ, @PME_ID, @PLI_ID)

//										UPDATE SCIEX_ALI
//										SET ALI_ST = @ALI_COD_GERADA_COM_SUCESS0, ALI_DH_PROCESSAMENTO_SUFRAMA = GETDATE()
//										WHERE PME_ID = @PME_ID

//									END

//								END

//							END
//							ELSE --PROBLEMAS NA VALIDAÇÃO DA MERCADORIA
//							BEGIN

//								SET @ALI_QTD_INDEFERIDA = @ALI_QTD_INDEFERIDA + 1

//								IF NOT EXISTS(SELECT * FROM SCIEX_ALI WHERE PME_ID = @PME_ID)
//								BEGIN

//									IF NOT EXISTS(SELECT MAX(ALI_NU) FROM SCIEX_ALI)
//									BEGIN--CASO NAO EXISTA AINDA NUMERO DA ALI NA BASE
//									   SELECT @ALI_NUMERO =
//										   SUBSTRING(CAST(YEAR(GETDATE()) AS VARCHAR), 3, 2) +
//										   CAST((CAST(PCO_VL_PARAMETRO AS BIGINT) + 1) AS VARCHAR)
//										FROM SCIEX_PARAMETRO_CONFIGURACAO
//										WHERE PCO_DS_PARAMETRO = 'VALOR INICIAL DA ALI'
//									END
//									BEGIN
//										--PEGA A ULTIMA ALI GERADA
//										SELECT @ALI_NUMERO_ATUAL = MAX(ALI_NU) FROM SCIEX_ALI

//										PRINT SUBSTRING(CAST(@ALI_NUMERO_ATUAL AS VARCHAR) , 1,2 )

//										--VERIFICA SE A ULTIMA ALI É DO MESMO ANO ATUAL
//										IF(SUBSTRING(CAST(YEAR(GETDATE()) AS VARCHAR), 3, 2) = SUBSTRING(CAST(@ALI_NUMERO_ATUAL AS VARCHAR), 1, 2))
//										BEGIN
//											SET @ALI_NUMERO = CAST((SELECT MAX(ALI_NU) + 1 FROM SCIEX_ALI) AS BIGINT )
//										END
//										ELSE --SE FOR NOVO ANO GERA O ANO ATUAL MAIS 40000001
//										BEGIN
//											SELECT @ALI_NUMERO =
//												SUBSTRING(CAST(YEAR(GETDATE()) AS VARCHAR), 3, 2) +
//												CAST((CAST(PCO_VL_PARAMETRO AS BIGINT) + 1) AS VARCHAR)
//											FROM SCIEX_PARAMETRO_CONFIGURACAO
//											WHERE PCO_DS_PARAMETRO = 'VALOR INICIAL DA ALI'
//										END

//									END

//									INSERT INTO SCIEX_ALI
//									(PME_ID, ALI_NU, ALI_ST, ALI_TP, ALI_DH_CADASTRO, ALI_DH_PROCESSAMENTO_SUFRAMA)
//									VALUES
//									(@PME_ID, @ALI_NUMERO, @ALI_COD_INDEFERIDA_SUFRAMA, @PLI_TP_DOCUMENTO, GETDATE(), GETDATE())

//								END
//								ELSE
//								BEGIN

//									UPDATE SCIEX_ALI
//									SET ALI_ST = @ALI_COD_INDEFERIDA_SUFRAMA, ALI_DH_PROCESSAMENTO_SUFRAMA = GETDATE()
//									WHERE PME_ID = @PME_ID

//								END

//							END

//						END
//						-- FIM RN 10 - GERAÇÃO DA LI


//						UPDATE #TTPLI_MERCADORIA
//						SET SITUACAO = 2
//						WHERE PME_ID = @PME_ID

//					END

//					DELETE FROM #TTPLI_MERCADORIA	-- LIMPA A TABELA TEMPORARIO DE MERCADORIA	
											
//					UPDATE #TTPLI_PRODUTO
//					SET SITUACAO = 2
//					WHERE PPR_ID = @PPR_ID

//				END
//				--FIM O LOOP DOS PRODUTOS SELECIONADOS

//				DELETE FROM #TTPLI_PRODUTO -- LIMPA A TABELA TEMPORARIO DE PLI PRODUTOS			
//				--REGRA DE NEGOCIO: VALIDAR PRODUTO


//				--RN 11 - STATUS DO PLI
//				--GUARDA A QUANTIDADE DE MERCADORIA DO PLI
//				SELECT
//					@PLI_QTD_MERCADORIA = COUNT(*)
//				FROM SCIEX_PLI_MERCADORIA
//				WHERE PLI_ID = @PLI_ID


//				IF @PLI_QTD_MERCADORIA = @ALI_QTD_DEFERIDA--SE TODAS AS MERCADORIAS FORAM DEFERIDAS
//				BEGIN

//					UPDATE SCIEX_PLI
//					SET PLI_ST_PLI = @PLI_COD_PROCESSADO,
//						PLI_ST_PROCESSAMENTO = @PLI_APROVADO
//					WHERE PLI_ID = @PLI_ID

//					SET @PLI_STATUS_PROCESSADO = ' APROVADO'

//					PRINT 'PLI APROVADO'
//				END

//				IF @PLI_QTD_MERCADORIA = @ALI_QTD_INDEFERIDA--SE TODAS AS MERCADORIAS FORAM INDEFERIDAS
//				BEGIN

//					UPDATE SCIEX_PLI
//					SET PLI_ST_PLI = @PLI_COD_PROCESSADO,
//						PLI_ST_PROCESSAMENTO = @PLI_REPROVADO
//					WHERE PLI_ID = @PLI_ID

//					SET @PLI_STATUS_PROCESSADO = ' REPROVADO'

//					PRINT 'PLI REPROVADO'

//				END

//				--SE ALGUMAS AS MERCADORIAS FORAM DEFERIDAS E OUTRAS INDEFERIDAS
//				IF @PLI_QTD_MERCADORIA<> @ALI_QTD_DEFERIDA AND @PLI_QTD_MERCADORIA<> @ALI_QTD_INDEFERIDA
//			   BEGIN


//				   UPDATE SCIEX_PLI

//				   SET PLI_ST_PLI = @PLI_COD_PROCESSADO,
//					   PLI_ST_PROCESSAMENTO = @PLI_PARCIALMENTE_APROVADO

//				   WHERE PLI_ID = @PLI_ID


//				   SET @PLI_STATUS_PROCESSADO = ' PARCIALMENTE APROVADO'


//				   PRINT 'PLI PARCIALMENTE APROVADO'

//				END

//				--INSERIR O HISTORICO DO PLI
//				INSERT INTO SCIEX_PLI_HISTORICO
//				(
//				 PLI_ID, PHI_DH_EVENTO, PHI_DS_OBSERVACAO, PHI_NU_CPFCNPJ_RESPONSAVEL,
//				 PHI_NO_RESPONSAVEL, PLI_ST_PLI, PLI_ST_PLI_DESCRICAO)
//				VALUES
//				(@PLI_ID, GETDATE(), @PHI_DS_OBSERVACAO + @PLI_STATUS_PROCESSADO, @USU_CPFCNPJ,
//				 @USU_ADM, @PLI_COD_PROCESSADO, @PHI_DS_OBSERVACAO)
//				 -- FIM RN 11 - STATUS DO PLI

//			   --RN 14 - INICIO CONTROLE DE EXECUÇÃO DE SERVIÇO
//				UPDATE SCIEX_CONTROLE_EXEC_SERVICO
//				SET
//					CES_DH_EXECUCAO_FIM = GETDATE(),
//					CES_ST_EXECUCAO = 1,
//					CES_ME_OBJETO_RETORNO = 'PLI ' + CAST(@PLI_ID AS VARCHAR) + ' PROCESSADO'
//				WHERE CES_ID = @CONTROLE_SERVICO_COD
//				-- FIM RN 14 - INICIO CONTROLE DE EXECUÇÃO DE SERVIÇO

//			--END

//			--ATUALIZA O PLI TRABALHADO PARA NÃO SER UTILIZADO MAIS NA LISTA
//			UPDATE #TTPLI
//			SET SITUACAO = 2
//			WHERE PLI_ID = @PLI_ID

//			SET @PLI_QTD_MERCADORIA = 0
//			SET @ALI_QTD_DEFERIDA = 0
//			SET @ALI_QTD_INDEFERIDA = 0

//			COMMIT-- CONFIRMA TODAS AS TRANAÇÕES


//		END TRY   --CASO ERRO, NÃO EFETIVAR AS TRANSAÇÕES
//		BEGIN CATCH

//			IF @@TRANCOUNT > 0--SE EXISTIREM TRANSAÇÕES, DESFAZE - LAS
//			BEGIN
//				ROLLBACK

//					SELECT
//						 ERROR_NUMBER() AS ErrorNumber
//						, ERROR_SEVERITY() AS ErrorSeverity
//						 , ERROR_STATE() AS ErrorState
//						  , ERROR_PROCEDURE() AS ErrorProcedure
//						   , ERROR_LINE() AS ErrorLine
//							, ERROR_MESSAGE() AS ErrorMessage;

//			BEGIN TRANSACTION

//					UPDATE SCIEX_CONTROLE_EXEC_SERVICO
//					SET
//						CES_DH_EXECUCAO_FIM = GETDATE(),
//						CES_ST_EXECUCAO = 2,
//						CES_ME_OBJETO_RETORNO = ERROR_MESSAGE()
//					WHERE CES_ID = @CONTROLE_SERVICO_COD

//					UPDATE #TTPLI
//						SET SITUACAO = 2
//					WHERE PLI_ID = @PLI_ID

//					COMMIT

//			END

//		END CATCH

//	END

//	DELETE FROM #TTPLI -- LIMPA A TABELA TEMPORÁRIO DE PLI

//END

//DROP TABLE #TTPLI_DETALHE_MERCADORIA	-- DESTROI A TABELA TEMPORARIA DA MEMORIA
//DROP TABLE #TTPLI_MERCADORIA			-- DESTROI A TABELA TEMPORARIA DA MEMORIA
//DROP TABLE #TTPLI_PRODUTO				-- DESTROI A TABELA TEMPORARIA DA MEMORIA
//DROP TABLE #TTPLI						-- DESTROI A TABELA TEMPORARIA DA MEMORIA

//--FIM DO PROCESSAMENTO
//					 ";

//			return sql;
//		}
		//
		#endregion

		public string ProcessarPLI()
		{
			return _uowSciex.CommandStackSciex.Salvar("EXEC dbo.ST_SCIEX_PROCESSAR_PLI", "");
		}

		public bool CopiarPliParaCancelamentoLi(List<long> ListaPliMercadoriasSelecionados)
		{
			try
			{
				long id = ListaPliMercadoriasSelecionados[0];
				var pliMercadoria = _uowSciex.QueryStackSciex.PliMercadoria.Selecionar(o => o.IdPliMercadoria == id);
				PliVM pliAtual = Selecionar(pliMercadoria.IdPLI);
				var idPLi = pliAtual.IdPLI;

				pliAtual.Ano = 0;
				pliAtual.NumeroPli = 0;
				pliAtual.IdPLI = null;
				pliAtual.NomeResponsavelRegistro = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoNome.Replace(".", "").Replace("-", "").Replace("/", ""); ; //_IUsuarioLogado.Usuario.NomeUsuario;
				pliAtual.NumeroResponsavelRegistro = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoCpfCnpj.Replace(".", "").Replace("-", "").Replace("/", ""); ;
				pliAtual.DataCadastro = DateTime.Now;

				//Continua o mesmo status atual do pli			

				//Modificar o status tipo documento para 3 - CANCELAMENTO
				pliAtual.TipoDocumento = (byte)EnumPliTipoDocumento.CANCELAMENTO;

				//gerar nova sequencia do pli junto com o ano atual
				var sequence = this.GerarSequence(pliAtual.Cnpj, DateTime.Now.Year);
				pliAtual.Ano = DateTime.Now.Year;
				pliAtual.NumeroPli = sequence;
				pliAtual.TipoOrigem = Convert.ToByte(EnumPliTipoOrigem.PliWeb);

				// gerar codigos pli mercadorias
				string codigosPliMercadorias = "";
				foreach (var item in ListaPliMercadoriasSelecionados)
				{
					codigosPliMercadorias = item.ToString() + ",";
				}

				codigosPliMercadorias += "0";

				_idPLiRetorno = _uowSciex.CommandStackSciex.Salvar(MontarCopiarPliParaCancelamentoAliSql(idPLi.ToString(),
					"'" + pliAtual.NumeroResponsavelRegistro + "'", "'" + pliAtual.NomeResponsavelRegistro + "'",
					pliAtual.NumeroPli.ToString(), pliAtual.Ano.ToString(), pliAtual.TipoOrigem.ToString(), codigosPliMercadorias));

				return true;
			}
			catch (Exception ex)
			{ return false; }
		}

		private string MontarCopiarPliParaCancelamentoAliSql(string idPliAtual, string cpfResponsavel,
			string nomeResponsavel, string numeroPLI, string anoPLI, string tipoOrigem, string codigosPliMercadorias)
		{

			string SQL =
			@"				
				DECLARE @PLI_ID bigint;
				DECLARE @PPR_ID bigint;
				DECLARE @PME_ID bigint;
				DECLARE @PDM_ID bigint;
				DECLARE @PPA_ID int;

				DECLARE @PLI_ID_NOVO bigint;
				DECLARE @PPR_ID_NOVO bigint;
				DECLARE @PME_ID_NOVO bigint;											

				CREATE TABLE #TabelaProduto
				(
					PPR_ID	bigint,	PLI_ID	bigint,	PPR_CO_PRODUTO	smallint, PPR_CO_TP_PRODUTO	smallint, 
					PPR_CO_MODELO_PRODUTO smallint, PPR_DS_PRODUTO varchar(400), SITUACAO int
				)

				
				CREATE TABLE #TabelaMercadoria
				(
					PME_ID	bigint, PLI_ID	bigint, MOE_ID	int, INC_ID	int, RTB_ID	int, FLE_ID	int, FAB_ID	int, FOR_ID	int, INF_ID	int,
					MOT_ID	int, MOP_ID	int, ALA_ID	int, NLD_ID	int, PPR_ID	bigint, CUT_ID int, CCO_ID int, RFB_ID_ENTRADA	int, RFB_ID_DESPACHO int, 
					PAI_CO_MERCADORIA varchar(3), PAI_DS_MERCADORIA	varchar(50), PAI_CO_ORIGEM_FABRICANTE varchar(3), 
					PAI_DS_ORIGEM_FABRICANTE varchar(50), PME_NU_PESO_LIQUIDO numeric(15,5), PME_QT_UNID_MEDIDA_ESTATISTICA numeric(14,5),
					PME_NU_COMUNICADO_COMPRA varchar(13), PME_NU_ATO_DRAWBACK varchar(13), PME_NU_AGENCIA_SECEX	varchar(5),
					PME_VL_CRA	numeric(4,2), PME_TP_COBCAMBIAL	int, PME_NU_COBCAMBIAL_LIMITE_DIAS_PAGTO int, PME_TP_ACORDO_TARIFARIO tinyint,
					PME_DS_INFORMACAO_COMPLEMENTAR	varchar(4048), PME_TP_BEM_ENCOMENDA	tinyint, PME_TP_MATERIAL_USADO	tinyint,
					PME_NU_NCM_DESTAQUE	char(3), PME_CO_PRODUTO	smallint, PME_DS_PRODUTO varchar(500), PME_CO_TP_PRODUTO smallint,
					PME_CO_MODELO_PRODUTO	smallint, MER_CO_NCM_MERCADORIA	char(8), MER_DS_NCM_MERCADORIA	varchar(120),
					PME_TP_FORNECEDOR smallint, PME_VL_TOTAL_CONDICAO_VENDA	numeric(15,2), PME_VL_TOTAL_CONDICAO_VENDA_REAL numeric(15,2),
					PME_VL_TOTAL_CONDICAO_VENDA_DOLAR numeric(15,2), SITUACAO int
				)

				CREATE TABLE #TabelaDetalheMercadoria
				(
				PDM_ID bigint, PME_ID bigint, UME_CO int, UME_DS varchar(40), UME_SG varchar(5), DME_CO_DETALHE_MERCADORIA int, 
				PDM_DS_DETALHE varchar(254),PDM_DS_COMPLEMENTO	varchar(3783), PDM_DS_MAT_PRIMA_BASICA varchar(20), 
				PDM_DS_PART_NUMBER varchar(20), PDM_DS_REF_FABRICANTE varchar(20), PDM_QT_UNID_COMERCIALIZADA numeric(14,5), 
				PDM_VL_UNITARIO_COND_VENDA numeric(18,7), PDM_VL_CONDICAO_VENDA numeric(20,12), PDM_VL_CONDICAO_VENDA_REAL numeric(20,12),
				PDM_VL_CONDICAO_VENDA_DOLAR	numeric(20,12), SITUACAO int
				)

				CREATE TABLE #TabelaProcessoAnuente
				(
					PPA_ID	int, PME_ID	bigint, OAN_ID int, PPA_NU_PROCESSO varchar(20), SITUACAO int
				)
				

				BEGIN TRY   -- INICIA TRATAMENTO DE ERRO

					BEGIN TRANSACTION; 

						INSERT INTO SCIEX_PLI						
						SELECT  PAP_ID, NumeroPLI, AnoPLI, PLI_NU_CNPJ, INS_CO, SET_CO, SET_DS, PLI_TP_DOCUMENTO,
							PLI_ST_ANALISE_VISUAL, PLI_ST_DISTRIBUICAO, NULL, NULL, NULL, NULL, NULL, NULL, 
							PLI_NU_LI_REFERENCIA, PLI_NU_DI_REFERENCIA, PLI_NU_PEXPAM, PLI_NU_ANO_PEXPAM, 
							PLI_NU_LOTE_PEXPAM, NULL, TipoOrigem, CPFResponsavel, NomeResponsavel,
							GETDATE(), NULL, PLI_NU_CPF_REP_LEGAL_SISCOMEX, PLI_CO_CNAE,
							NULL, NULL, NULL, NULL, IMP_DS_RAZAO_SOCIAL, 20, NULL
						FROM SCIEX_PLI
						WHERE PLI_ID = idPliAtual
				
						SELECT @PLI_ID_NOVO = @@IDENTITY; 

						INSERT INTO SCIEX_PLI_HISTORICO
						(PLI_ID, PHI_DH_EVENTO, PHI_NU_CPFCNPJ_RESPONSAVEL, PHI_NO_RESPONSAVEL, PHI_DS_OBSERVACAO, PLI_ST_PLI, PLI_ST_PLI_DESCRICAO)
						VALUES
						(
							@PLI_ID_NOVO, GETDATE(), CPFResponsavel, NomeResponsavel, 'CADASTRO DO PLI', 20, 'EM ELABORAÇÃO'
						)


						INSERT INTO #TabelaProduto
						SELECT PPR_ID, PLI_ID, PPR_CO_PRODUTO, PPR_CO_TP_PRODUTO, PPR_CO_MODELO_PRODUTO, PPR_DS_PRODUTO, 1
						FROM SCIEX_PLI_PRODUTO
						WHERE PLI_ID = idPliAtual

						WHILE EXISTS(SELECT * FROM #TabelaProduto WHERE SITUACAO = 1)
						BEGIN

									SELECT TOP 1 @PLI_ID = PLI_ID, @PPR_ID = PPR_ID FROM #TabelaProduto WHERE SITUACAO = 1

									INSERT INTO SCIEX_PLI_PRODUTO
									SELECT 	@PLI_ID_NOVO, PPR_CO_PRODUTO, PPR_CO_TP_PRODUTO, PPR_CO_MODELO_PRODUTO, PPR_DS_PRODUTO, NULL
									FROM #TabelaProduto 
									WHERE PPR_ID = @PPR_ID

									SELECT @PPR_ID_NOVO = @@IDENTITY;  	
	
									INSERT INTO #TabelaMercadoria
									SELECT  PME_ID, PLI_ID, MOE_ID, INC_ID, RTB_ID, FLE_ID, FAB_ID, FOR_ID, INF_ID,
											MOT_ID, MOP_ID, ALA_ID, NLD_ID, PPR_ID, CUT_ID, CCO_ID, RFB_ID_ENTRADA, RFB_ID_DESPACHO, 
											PAI_CO_MERCADORIA , PAI_DS_MERCADORIA, PAI_CO_ORIGEM_FABRICANTE, 
											PAI_DS_ORIGEM_FABRICANTE, PME_NU_PESO_LIQUIDO, PME_QT_UNID_MEDIDA_ESTATISTICA,
											PME_NU_COMUNICADO_COMPRA , PME_NU_ATO_DRAWBACK , PME_NU_AGENCIA_SECEX	,
											PME_VL_CRA, PME_TP_COBCAMBIAL, PME_NU_COBCAMBIAL_LIMITE_DIAS_PAGTO, PME_TP_ACORDO_TARIFARIO,
											PME_DS_INFORMACAO_COMPLEMENTAR, PME_TP_BEM_ENCOMENDA, PME_TP_MATERIAL_USADO,
											PME_NU_NCM_DESTAQUE, PME_CO_PRODUTO, PME_DS_PRODUTO, PME_CO_TP_PRODUTO,
											PME_CO_MODELO_PRODUTO, MER_CO_NCM_MERCADORIA, MER_DS_NCM_MERCADORIA	,
											PME_TP_FORNECEDOR , NULL, NULL, NULL , 1
									FROM SCIEX_PLI_MERCADORIA
									WHERE PME_ID IN (codigosPliMercadorias)							
	
									WHILE EXISTS(SELECT * FROM #TabelaMercadoria WHERE SITUACAO = 1)
									BEGIN

										SELECT TOP 1 @PME_ID = PME_ID FROM #TabelaMercadoria WHERE SITUACAO = 1

										INSERT INTO SCIEX_PLI_MERCADORIA
										(PLI_ID, MOE_ID, INC_ID, RTB_ID, FLE_ID, FAB_ID, FOR_ID, INF_ID, MOT_ID, MOP_ID,
										 ALA_ID, NLD_ID, PPR_ID, CUT_ID, CCO_ID, RFB_ID_ENTRADA, RFB_ID_DESPACHO,
										 PAI_CO_MERCADORIA, PAI_DS_MERCADORIA, PAI_CO_ORIGEM_FABRICANTE,
										 PAI_DS_ORIGEM_FABRICANTE, PME_NU_PESO_LIQUIDO, PME_QT_UNID_MEDIDA_ESTATISTICA,
										 PME_NU_COMUNICADO_COMPRA, PME_NU_ATO_DRAWBACK, PME_NU_AGENCIA_SECEX,
										 PME_VL_CRA, PME_TP_COBCAMBIAL, PME_NU_COBCAMBIAL_LIMITE_DIAS_PAGTO,
										 PME_TP_ACORDO_TARIFARIO, PME_DS_INFORMACAO_COMPLEMENTAR, PME_TP_BEM_ENCOMENDA,
										 PME_TP_MATERIAL_USADO, PME_NU_NCM_DESTAQUE, PME_CO_PRODUTO, PME_DS_PRODUTO,
										 PME_CO_TP_PRODUTO, PME_CO_MODELO_PRODUTO, MER_CO_NCM_MERCADORIA,
										 MER_DS_NCM_MERCADORIA, PME_TP_FORNECEDOR, PME_VL_TOTAL_CONDICAO_VENDA,
										 PME_VL_TOTAL_CONDICAO_VENDA_REAL, PME_VL_TOTAL_CONDICAO_VENDA_DOLAR)
										SELECT  @PLI_ID_NOVO, MOE_ID, INC_ID, RTB_ID, FLE_ID, FAB_ID, FOR_ID, INF_ID, MOT_ID, MOP_ID, 
												ALA_ID,	NLD_ID, @PPR_ID_NOVO, CUT_ID, CCO_ID, RFB_ID_ENTRADA, RFB_ID_DESPACHO, PAI_CO_MERCADORIA, 
												PAI_DS_MERCADORIA, PAI_CO_ORIGEM_FABRICANTE, PAI_DS_ORIGEM_FABRICANTE, PME_NU_PESO_LIQUIDO,	
												PME_QT_UNID_MEDIDA_ESTATISTICA,	PME_NU_COMUNICADO_COMPRA, PME_NU_ATO_DRAWBACK, PME_NU_AGENCIA_SECEX, 
												PME_VL_CRA, PME_TP_COBCAMBIAL, PME_NU_COBCAMBIAL_LIMITE_DIAS_PAGTO,	PME_TP_ACORDO_TARIFARIO, 
												PME_DS_INFORMACAO_COMPLEMENTAR, PME_TP_BEM_ENCOMENDA, PME_TP_MATERIAL_USADO, PME_NU_NCM_DESTAQUE, 
												PME_CO_PRODUTO, PME_DS_PRODUTO, PME_CO_TP_PRODUTO,	PME_CO_MODELO_PRODUTO, MER_CO_NCM_MERCADORIA,
												MER_DS_NCM_MERCADORIA, PME_TP_FORNECEDOR, PME_VL_TOTAL_CONDICAO_VENDA, PME_VL_TOTAL_CONDICAO_VENDA_REAL,
												PME_VL_TOTAL_CONDICAO_VENDA_DOLAR
										FROM #TabelaMercadoria
										WHERE PME_ID = @PME_ID										

										SELECT @PME_ID_NOVO = @@IDENTITY;

										INSERT INTO #TabelaDetalheMercadoria		
										SELECT  PDM_ID, PME_ID, UME_CO, UME_DS, UME_SG, DME_CO_DETALHE_MERCADORIA, PDM_DS_DETALHE,
										PDM_DS_COMPLEMENTO, PDM_DS_MAT_PRIMA_BASICA, PDM_DS_PART_NUMBER, PDM_DS_REF_FABRICANTE,
										PDM_QT_UNID_COMERCIALIZADA, PDM_VL_UNITARIO_COND_VENDA, PDM_VL_CONDICAO_VENDA, NULL,
										NULL, 1
										FROM SCIEX_PLI_DETALHE_MERCADORIA
										WHERE PME_ID = @PME_ID

										INSERT INTO SCIEX_ALI
										SELECT	@PME_ID_NOVO, AAE_ID, ALI_NU, 6, 3, GETDATE(), NULL, ALI_DH_PROCESSAMENTO_SUFRAMA,
												ALI_DH_RESPOSTA_SISCOMEX,AAE_DS_NOME_ARQUIVO
										FROM SCIEX_ALI
										WHERE PME_ID = @PME_ID

										INSERT INTO SCIEX_LI
										SELECT	@PME_ID_NOVO, DI_ID, LAR_ID, LI_NU, 3, 2, LI_DH_CADASTRO, NULL, LI_NU_LI_PROTOCOLADA,
												LI_DT_GERACAO, LI_NU_DIAGNOSTICO_ERRO, LI_DS_MENSAGEM
										FROM SCIEX_LI
										WHERE PME_ID = @PME_ID

										WHILE EXISTS(SELECT * FROM #TabelaDetalheMercadoria WHERE SITUACAO = 1)
										BEGIN
											SELECT TOP 1 @PDM_ID = PDM_ID FROM #TabelaDetalheMercadoria WHERE SITUACAO = 1
			
											INSERT INTO SCIEX_PLI_DETALHE_MERCADORIA
											(
												PME_ID, UME_CO, UME_DS, UME_SG, DME_CO_DETALHE_MERCADORIA, 
												PDM_DS_DETALHE ,PDM_DS_COMPLEMENTO, PDM_DS_MAT_PRIMA_BASICA, 
												PDM_DS_PART_NUMBER, PDM_DS_REF_FABRICANTE, PDM_QT_UNID_COMERCIALIZADA, 
												PDM_VL_UNITARIO_COND_VENDA, PDM_VL_CONDICAO_VENDA, PDM_VL_CONDICAO_VENDA_REAL,
												PDM_VL_CONDICAO_VENDA_DOLAR	
											)
											SELECT  @PME_ID_NOVO, UME_CO, UME_DS, UME_SG, DME_CO_DETALHE_MERCADORIA, PDM_DS_DETALHE,
													PDM_DS_COMPLEMENTO, PDM_DS_MAT_PRIMA_BASICA, PDM_DS_PART_NUMBER, PDM_DS_REF_FABRICANTE,
													PDM_QT_UNID_COMERCIALIZADA, PDM_VL_UNITARIO_COND_VENDA, PDM_VL_CONDICAO_VENDA, NULL,
													NULL
											FROM #TabelaDetalheMercadoria
											WHERE PDM_ID = @PDM_ID

											UPDATE #TabelaDetalheMercadoria 
											SET SITUACAO = 2
											WHERE PDM_ID = @PDM_ID;
										END

										DELETE FROM #TabelaDetalheMercadoria

										INSERT INTO #TabelaProcessoAnuente
										SELECT PPA_ID, PME_ID, OAN_ID, PPA_NU_PROCESSO, 1
										FROM SCIEX_PLI_PROCESSO_ANUENTE
										WHERE PME_ID = @PME_ID;

										WHILE EXISTS(SELECT * FROM #TabelaProcessoAnuente WHERE SITUACAO = 1)
										BEGIN
			
											SELECT TOP 1 @PPA_ID = PPA_ID FROM #TabelaProcessoAnuente WHERE SITUACAO = 1

											INSERT INTO SCIEX_PLI_PROCESSO_ANUENTE
											SELECT @PME_ID_NOVO, OAN_ID, PPA_NU_PROCESSO
											FROM #TabelaProcessoAnuente
											WHERE PPA_ID = @PPA_ID
			
											UPDATE #TabelaProcessoAnuente 
											SET SITUACAO = 2
											WHERE  PPA_ID = @PPA_ID 

										END

										DELETE FROM #TabelaProcessoAnuente

										UPDATE #TabelaMercadoria 
										SET SITUACAO = 2
										WHERE PME_ID = @PME_ID

									END

									UPDATE #TabelaProduto
									SET SITUACAO = 2
									WHERE PLI_ID = @PLI_ID AND  PPR_ID = @PPR_ID

									DELETE FROM #TabelaMercadoria
											
										
						END						

						
					COMMIT					

				END TRY

				BEGIN CATCH  

					IF @@TRANCOUNT > 0
						ROLLBACK
							
				END CATCH;

				SELECT @PLI_ID_NOVO
				DROP TABLE #TabelaProduto
				DROP TABLE #TabelaMercadoria
				DROP TABLE #TabelaProcessoAnuente
				DROP TABLE #TabelaDetalheMercadoria


			";

			SQL = SQL.Replace("idPliAtual", idPliAtual)
				.Replace("NumeroPLI", numeroPLI)
				.Replace("AnoPLI", anoPLI)
				.Replace("CPFResponsavel", cpfResponsavel)
				.Replace("NomeResponsavel", nomeResponsavel)
				.Replace("TipoOrigem", tipoOrigem)
				.Replace("codigosPliMercadorias", codigosPliMercadorias);

			return SQL;
		}

	}
}