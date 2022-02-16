using System;
using System.Collections.Generic;
using System.Linq;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.BusinessLogic.Pss;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.BusinessLogic
{
	public class MinhaSolicitacaoAlteracaoDetalheBll : IMinhaSolicitacaoAlteracaoDetalheBll
	{
		private readonly int emAlteracao = 2, ativo = 1;
		private readonly IMinhaSolicitacaoAlteracaoBll _minhaSolicitacaoAlteracaoBll;
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUsuarioPssBll _usuarioLogado;
		private readonly IQuantidadeCoeficienteBll _quantidadeCoeficienteBll;
		private readonly ICalcularMoedaBll _calcularMoedaBll;
		private readonly IValorUnitarioBll _valorUnitarioBll;
		private readonly IValorFreteBll _valorFreteBll;

		public MinhaSolicitacaoAlteracaoDetalheBll(IMinhaSolicitacaoAlteracaoBll minhaSolicitacaoAlteracaoBll,
												   IUnitOfWorkSciex uowSciex,
												   IUsuarioPssBll usuarioLogado,
												   IQuantidadeCoeficienteBll quantidadeCoeficienteBll,
												   IValorUnitarioBll valorUnitarioBll,
												   IValorFreteBll valorFreteBll,
												   ICalcularMoedaBll calcularMoedaBll
												   )
		{
			_minhaSolicitacaoAlteracaoBll = minhaSolicitacaoAlteracaoBll;
			_uowSciex = uowSciex;
			_usuarioLogado = usuarioLogado;
			_quantidadeCoeficienteBll = quantidadeCoeficienteBll;
			_valorUnitarioBll = valorUnitarioBll;
			_valorFreteBll = valorFreteBll;
			_calcularMoedaBll = calcularMoedaBll;
		}

		public PagedItems<PRCSolicDetalheVM> ListarPaginado (PRCSolicDetalheVM objeto)
		{
			#region Create Flag To Sortable Validations 
			string sort = null;
			if (!string.IsNullOrEmpty(objeto.Sort) && objeto.Sort.Equals("CodigoDetalheInsumo"))
			{
				sort = "CodigoDetalheInsumo";
				objeto.Sort = null;
			}
			if (!string.IsNullOrEmpty(objeto.Sort) && objeto.Sort.Equals("DescricaoStatus"))
			{
				sort = "DescricaoStatus";
				objeto.Sort = null;
			}
			if (!string.IsNullOrEmpty(objeto.Sort) && objeto.Sort.Equals("DescricaoInsumo"))
			{
				sort = "DescricaoInsumo";
				objeto.Sort = null;
			}
			if (!string.IsNullOrEmpty(objeto.Sort) && objeto.Sort.Equals("DescricaoTipoAlteracao"))
			{
				sort = "DescricaoTipoAlteracao";
				objeto.Sort = null;
			}
			#endregion

			#region Send Query to The Database
			var pagedItems = _uowSciex.QueryStackSciex.PRCSolicDetalhe.ListarPaginadoGrafo(o =>
			new PRCSolicDetalheVM()
			{
				CodigoInsumo = (int)o.PrcInsumo.CodigoInsumo,
				CodigoDetalheInsumo = (int)o.PrcInsumo.CodigoDetalhe,
				DescricaoInsumo = o.PrcInsumo.DescricaoInsumo,
				SolicitacaoAlteracao = new PRCSolicitacaoAlteracaoVM()
				{
					Status = o.Status
				},				
				DescricaoDe = o.DescricaoDe,
				DescricaoPara = o.DescricaoPara,
				IdSolicitacaoAlteracao = o.IdSolicitacaoAlteracao,
				Id = o.Id,
				IdTipoSolicitacao = o.IdTipoSolicitacao,
				Status = o.Status
			},
			o => o.IdSolicitacaoAlteracao == objeto.IdSolicitacaoAlteracao, objeto);
			#endregion

			if (pagedItems.Total > 0)
			{
				#region GET Decricação Status and Descrição Tipo Alteração.
				foreach (var item in pagedItems.Items)
				{
					item.DescricaoStatus = (item.SolicitacaoAlteracao.Status > 0) ?
						_minhaSolicitacaoAlteracaoBll.getDescricaoStatus(item.Status) : 
							"--";
					item.DescricaoTipoAlteracao =
						_uowSciex.QueryStackSciex.TipoSolicAlteracao.Selecionar(o => o.Id == item.IdTipoSolicitacao).Descricao;
				}
				#endregion

				#region Ordering Pagination List.
				if (!string.IsNullOrWhiteSpace(sort))
				{
					switch (sort)
					{
						case "CodigoDetalheInsumo":
							if (objeto.Reverse)
							{
								pagedItems.Items = pagedItems.Items.OrderBy(q => q.CodigoDetalheInsumo).ThenBy(q => q.CodigoDetalheInsumo).ToList();
							}
							else
							{
								pagedItems.Items = pagedItems.Items.OrderByDescending(q => q.CodigoDetalheInsumo).ThenByDescending(q => q.CodigoDetalheInsumo).ToList();
							}
							break;

						case "DescricaoStatus":
							if (objeto.Reverse)
							{
								pagedItems.Items = pagedItems.Items.OrderBy(q => q.DescricaoStatus).ThenBy(q => q.DescricaoStatus).ToList();
							}
							else
							{
								pagedItems.Items = pagedItems.Items.OrderByDescending(q => q.DescricaoStatus).ThenByDescending(q => q.DescricaoStatus).ToList();
							}
							break;

						case "DescricaoInsumo":
							if (objeto.Reverse)
							{
								pagedItems.Items = pagedItems.Items.OrderBy(q => q.DescricaoInsumo).ThenBy(q => q.DescricaoInsumo).ToList();
							}
							else
							{
								pagedItems.Items = pagedItems.Items.OrderByDescending(q => q.DescricaoInsumo).ThenByDescending(q => q.DescricaoInsumo).ToList();
							}
							break;

						case "DescricaoTipoAlteracao":
							if (objeto.Reverse)
							{
								pagedItems.Items = pagedItems.Items.OrderBy(q => q.DescricaoTipoAlteracao).ThenBy(q => q.DescricaoTipoAlteracao).ToList();
							}
							else
							{
								pagedItems.Items = pagedItems.Items.OrderByDescending(q => q.DescricaoTipoAlteracao).ThenByDescending(q => q.DescricaoTipoAlteracao).ToList();
							}
							break;
					}
				}
				#endregion
			}

			return pagedItems;
		}
		
		public DetalhesMinhaSolicitacaoAlteracaoVM BuscarInfoDetalhes(int idSolicAlteracao)
		{
			var retorno = new DetalhesMinhaSolicitacaoAlteracaoVM();
			var cnpjUsuarioLogado = _usuarioLogado.ObterUsuarioLogado();
			string cnpj = null;

			if (cnpjUsuarioLogado.Perfis.Contains(EnumPerfil.Importador) || cnpjUsuarioLogado.Perfis.Contains(EnumPerfil.Preposto))
			{
				cnpj = cnpjUsuarioLogado.usuCpfCnpjEmpresaOuLogado.CnpjCpfUnformat(); 
			}
			var solicitacaoAlteracaoEntity = _uowSciex.QueryStackSciex.PRCSolicitacaoAlteracao.Selecionar(o => o.Id == idSolicAlteracao);
			var processoEntity = _uowSciex.QueryStackSciex.Processo.Selecionar(o => o.IdProcesso == solicitacaoAlteracaoEntity.IdProcesso);
			var produtoEntity = _uowSciex.QueryStackSciex.PRCProduto.Selecionar(o => o.IdProcesso == solicitacaoAlteracaoEntity.IdProcesso);
			var pRJProdutoEmpresaExportacaoEntity = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao.Listar(o => o.CodigoProduto == produtoEntity.CodigoProdutoSuframa && 
																														   (string.IsNullOrEmpty(cnpj) || o.Cnpj == cnpj)
																														   &&																													   
																														   o.CodigoTipoProduto == produtoEntity.TipoProduto
																												 ).LastOrDefault();
			var UnidadeMedida = _uowSciex.QueryStackSciex.ViewUnidadeMedida.Selecionar(o => o.IdUnidadeMedida == pRJProdutoEmpresaExportacaoEntity.IdUnidadeMedida);

			retorno.NumeroSolicitacao = solicitacaoAlteracaoEntity.NumeroSolicitacao != null ? Convert.ToInt32(solicitacaoAlteracaoEntity.NumeroSolicitacao).ToString("D4") + "/" + Convert.ToInt32(solicitacaoAlteracaoEntity.AnoSolicitacao).ToString("D4") : null ;
			retorno.NumeroProcesso = processoEntity.NumeroProcesso?.ToString("D4") + "/" + processoEntity.AnoProcesso?.ToString("D4");
			retorno.Modalidade = (processoEntity.TipoModalidade == "S") ? "Suspensão" : "Isenção";
			retorno.DescricaoStatus = getStatusProcesso(processoEntity.TipoStatus);
			retorno.NumeroProduto = produtoEntity.CodigoProdutoExportacao;
			retorno.DescricaoProduto = produtoEntity.CodigoProdutoSuframa + " | " + produtoEntity.DescricaoModelo;
			retorno.DescricaoTipo = produtoEntity.TipoProduto?.ToString("D3") + " | " + pRJProdutoEmpresaExportacaoEntity.DescricaoTipoProduto;
			retorno.Modelo = produtoEntity.DescricaoModelo;
			retorno.Ncm = produtoEntity.CodigoNCM;
			retorno.Unidade = produtoEntity.CodigoUnidade + " | " + UnidadeMedida.Descricao;
			retorno.QtdTotal = (produtoEntity.QuantidadeAprovado != null) ? Convert.ToDecimal(produtoEntity.QuantidadeAprovado).ToString("N5") : "0";
			retorno.ValorTotal = (produtoEntity.ValorDolarAprovado != null) ? Convert.ToDecimal(produtoEntity.ValorDolarAprovado).ToString("N5") : "0";

			return retorno;
		}

		public int ApagarDetalheSolicitacaoAlteracao(int id)
		{
			try
			{ 			
				var _solicDetalhe = _uowSciex.QueryStackSciex.PRCSolicDetalhe.Selecionar(s => s.Id == id);

				var _insumoCopia = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(o => o.IdInsumo == _solicDetalhe.IdInsumo);

				var _qtdRegistroDetalhes = _uowSciex.QueryStackSciex.PRCSolicDetalhe.Listar(o => o.IdInsumo == _insumoCopia.IdInsumo).Count;
				
				var _prcDetalheInsumo = _uowSciex.QueryStackSciex.PRCDetalheInsumo.Selecionar(o => o.IdDetalheInsumo == _solicDetalhe.IdDetalheInsumo);

				if (_qtdRegistroDetalhes == 1)
				{
					#region RN03 - Se para o insumo houver somente uma(1) alteração, o insumo de Cópia e seus filhos devem ser excluídos.

					_uowSciex.CommandStackSciex.PRCSolicDetalhe.Apagar(id);

					var _insumoToDelete = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(o => o.CodigoInsumo == _insumoCopia.CodigoInsumo
																						   && o.IdPrcProduto == _insumoCopia.IdPrcProduto
																						   && o.StatusInsumo == emAlteracao);

					var _listDetalheInsumos = _uowSciex.QueryStackSciex.PRCDetalheInsumo.Listar(o => o.IdPrcInsumo == _insumoToDelete.IdInsumo);
								
					_uowSciex.CommandStackSciex.PRCSolicDetalhe.Apagar(id);

					if (_listDetalheInsumos.Count > 0)
					{
						foreach (var item in _listDetalheInsumos)
						{
							_uowSciex.CommandStackSciex.PRCDetalheInsumo.Apagar(item.IdDetalheInsumo);
						}
					}

					_uowSciex.CommandStackSciex.PRCInsumo.Apagar(_insumoCopia.IdInsumo);
					_uowSciex.CommandStackSciex.Save();

					return (int)EnumStatusRetornoRequisicao.SUCESSO;

					#endregion
				}
				else
				{
					var codigoMoeda = _uowSciex.QueryStackSciex.Moeda.Selecionar(o => o.IdMoeda == _prcDetalheInsumo.IdMoeda).CodigoMoeda;

					decimal valorParidadeCambial = CalcularParidadeBll.CalcularFatorConversao(codigoMoeda, _uowSciex);

					var Paridade = valorParidadeCambial;

					if (Paridade == Decimal.MinValue)
						return (int)EnumStatusRetornoRequisicao.PARIDADE_CAMBIAL_NAO_CADASTRADA;


					if (_solicDetalhe.IdTipoSolicitacao == (int)EnumTipoAlteracaoInsumo.INCLUSAO_INSUMO || _solicDetalhe.IdTipoSolicitacao == (int)EnumTipoAlteracaoInsumo.TRANSFERENCIA_SALDO_INSUMO)
					{
						int? codigoInsumoConvertido = (int?)Convert.ToInt32(_solicDetalhe.DescricaoPara);

						var _insumoToDelete = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(o => o.CodigoInsumo == codigoInsumoConvertido
																							   && o.IdPrcProduto == _insumoCopia.IdPrcProduto
																							   && o.StatusInsumo == emAlteracao);

						var _listDetalheInsumos = _uowSciex.QueryStackSciex.PRCDetalheInsumo.Listar(o => o.IdPrcInsumo == _insumoToDelete.IdInsumo);

						_uowSciex.CommandStackSciex.DetachEntries();
						_uowSciex.CommandStackSciex.PRCSolicDetalhe.Apagar(id);

						if (_listDetalheInsumos.Count > 0)
						{
							foreach (var item in _listDetalheInsumos)
							{
								_uowSciex.CommandStackSciex.PRCDetalheInsumo.Apagar(item.IdDetalheInsumo);
							}
						}

						_uowSciex.CommandStackSciex.PRCInsumo.Apagar(_insumoCopia.IdInsumo);
						_uowSciex.CommandStackSciex.Save();

						return (int)EnumStatusRetornoRequisicao.SUCESSO;
					}
					else if (_solicDetalhe.IdTipoSolicitacao == (int)EnumTipoAlteracaoInsumo.MOEDA)
					{
						var PRCInsumoEntity = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(o => o.IdInsumo == _insumoCopia.IdInsumo);

						#region SCIEX_PRC_DETALHE_INSUMO

						var _calculoDetalheInsumoEntity = _calcularMoedaBll.CalcularMoedaPRCDetalheInsumo(_prcDetalheInsumo);

						_prcDetalheInsumo.Quantidade = _calculoDetalheInsumoEntity.Quantidade;
						_prcDetalheInsumo.ValorDolar = _calculoDetalheInsumoEntity.ValorDolar;
						_prcDetalheInsumo.ValorDolarCFR = _calculoDetalheInsumoEntity.ValorDolarCFR;
						_prcDetalheInsumo.ValorDolarFOB = _calculoDetalheInsumoEntity.ValorDolarFOB;

						#endregion

						#region SCIEX_PRC_INSUMOS

						var _calculoPrcInsumoEntity = _calcularMoedaBll.CalcularMoedaPRCInsumo(PRCInsumoEntity);

						PRCInsumoEntity.QuantidadeSaldo = _calculoPrcInsumoEntity.QuantidadeSaldo;
						PRCInsumoEntity.ValorDolarSaldo = _calculoPrcInsumoEntity.ValorDolarSaldo;
						PRCInsumoEntity.ValorAdicional = _calculoPrcInsumoEntity.ValorAdicional;

						_uowSciex.CommandStackSciex.DetachEntries();
						_uowSciex.CommandStackSciex.PRCSolicDetalhe.Apagar(id);

						var updatePrcInsumo = new PRCInsumoEntity()
						{
							CodigoDetalhe = PRCInsumoEntity.CodigoDetalhe,
							CodigoInsumo = PRCInsumoEntity.CodigoInsumo,
							CodigoNCM = PRCInsumoEntity.CodigoNCM,
							CodigoUnidade = PRCInsumoEntity.CodigoUnidade,
							DescricaoEspecificacaoTecnica = PRCInsumoEntity.DescricaoEspecificacaoTecnica,
							DescricaoInsumo = PRCInsumoEntity.DescricaoInsumo,
							DescricaoPartNumber = PRCInsumoEntity.DescricaoPartNumber,
							IdInsumo = PRCInsumoEntity.IdInsumo,
							IdPrcProduto = PRCInsumoEntity.IdPrcProduto,
							QuantidadeAdicional = PRCInsumoEntity.QuantidadeAdicional,
							QuantidadeAprovado = PRCInsumoEntity.QuantidadeAprovado,
							QuantidadeComp = PRCInsumoEntity.QuantidadeComp,
							StatusInsumo = PRCInsumoEntity.StatusInsumo,
							StatusInsumoNovo = PRCInsumoEntity.StatusInsumoNovo,
							TipoInsumo = PRCInsumoEntity.TipoInsumo,
							ValorAdicionalFrete = PRCInsumoEntity.ValorAdicionalFrete,
							ValorCoeficienteTecnico = PRCInsumoEntity.ValorCoeficienteTecnico,
							ValorDolarCFRAprovado = PRCInsumoEntity.ValorDolarCFRAprovado,
							ValorDolarAprovado = PRCInsumoEntity.ValorDolarAprovado,
							ValorDolarComp = PRCInsumoEntity.ValorDolarComp,
							ValorDolarFOBAprovado = PRCInsumoEntity.ValorDolarFOBAprovado,
							ValorDolarUnitario = PRCInsumoEntity.ValorDolarUnitario,
							ValorDolarUnitarioCrf = PRCInsumoEntity.ValorDolarUnitarioCrf,
							ValorFreteAprovado = PRCInsumoEntity.ValorFreteAprovado,
							ValorNacionalAprovado = PRCInsumoEntity.ValorNacionalAprovado,
							ValorPercentualPerda = PRCInsumoEntity.ValorPercentualPerda,
							QuantidadeSaldo = PRCInsumoEntity.QuantidadeSaldo,
							ValorDolarSaldo = PRCInsumoEntity.ValorDolarSaldo,
							ValorAdicional = PRCInsumoEntity.ValorAdicional
						};

						_uowSciex.CommandStackSciex.PRCInsumo.Salvar(updatePrcInsumo);
						#endregion

						var updateDetalheInsumo = new PRCDetalheInsumoEntity()
						{
							CodigoPais = _prcDetalheInsumo.CodigoPais,
							IdDetalheInsumo = _prcDetalheInsumo.IdDetalheInsumo,
							IdMoeda = _prcDetalheInsumo.IdMoeda,
							IdPrcInsumo = _prcDetalheInsumo.IdPrcInsumo,
							NumeroSequencial = _prcDetalheInsumo.NumeroSequencial,
							Quantidade = _prcDetalheInsumo.Quantidade,
							ValorDolar = _prcDetalheInsumo.ValorDolar,
							ValorDolarCFR = _prcDetalheInsumo.ValorDolarCFR,
							ValorDolarFOB = _prcDetalheInsumo.ValorDolarFOB,
							ValorFrete = _prcDetalheInsumo.ValorFrete,
							ValorUnitario = _prcDetalheInsumo.ValorUnitario,
							ValorUnitarioCFR = _prcDetalheInsumo.ValorUnitarioCFR
						};

						_uowSciex.CommandStackSciex.PRCDetalheInsumo.Salvar(updateDetalheInsumo);
						_uowSciex.CommandStackSciex.Save();

						return (int)EnumStatusRetornoRequisicao.SUCESSO;
					}
					else if (_solicDetalhe.IdTipoSolicitacao == (int)EnumTipoAlteracaoInsumo.PAIS)
					{
						var PaisDE = _uowSciex.QueryStackSciex.ViewPais.Selecionar(o => o.Descricao == _solicDetalhe.DescricaoDe);
						_prcDetalheInsumo.CodigoPais = Convert.ToInt32(PaisDE.CodigoPais);

						_uowSciex.CommandStackSciex.DetachEntries();
						_uowSciex.CommandStackSciex.PRCSolicDetalhe.Apagar(id);
						_uowSciex.CommandStackSciex.PRCDetalheInsumo.Salvar(_prcDetalheInsumo);
						_uowSciex.CommandStackSciex.Save();

						return (int)EnumStatusRetornoRequisicao.SUCESSO;
					}
					else if (_solicDetalhe.IdTipoSolicitacao == (int)EnumTipoAlteracaoInsumo.QUANTIDADE_COEF_TECNICO)
					{

						var objeto = new SolicitacoesAlteracaoVM();
						decimal? _descricaoDe = Convert.ToDecimal(_solicDetalhe.DescricaoDe);
						objeto.QuantidadeCoefTecnicoPara = new QuantidadeCoefTecnicoVM();
						objeto.QuantidadeCoefTecnicoPara.ValorDe = _descricaoDe;

						var PRCInsumoEntity = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(o => o.IdInsumo == _insumoCopia.IdInsumo);

						#region SCIEX_PRC_DETALHE_INSUMO

						var _calculoDetalheInsumoEntity = _quantidadeCoeficienteBll.CalcDetalheInsumoQtdCoefTecnico(false, objeto, PRCInsumoEntity, _prcDetalheInsumo);

						if (_calculoDetalheInsumoEntity == null)
							return (int)EnumStatusRetornoRequisicao.PARIDADE_CAMBIAL_NAO_CADASTRADA;

						_prcDetalheInsumo.Quantidade = _calculoDetalheInsumoEntity.Quantidade;
						_prcDetalheInsumo.ValorDolar = _calculoDetalheInsumoEntity.ValorDolar;
						_prcDetalheInsumo.ValorDolarCFR = _calculoDetalheInsumoEntity.ValorDolarCFR;
						_prcDetalheInsumo.ValorDolarFOB = _calculoDetalheInsumoEntity.ValorDolarFOB;

						#endregion
						
						#region SCIEX_PRC_INSUMOS

						var _calculoPrcInsumoEntity = _quantidadeCoeficienteBll.CalcularQtdCoefTecnico(false, objeto, PRCInsumoEntity);

						if (_calculoPrcInsumoEntity == null)
							return (int)EnumStatusRetornoRequisicao.PARIDADE_CAMBIAL_NAO_CADASTRADA;

						PRCInsumoEntity.QuantidadeAdicional = _calculoPrcInsumoEntity.QuantidadeAdicional;
						PRCInsumoEntity.ValorAdicional = _calculoPrcInsumoEntity.ValorAdicional;
						PRCInsumoEntity.ValorDolarSaldo = _calculoPrcInsumoEntity.ValorDolarSaldo;
						PRCInsumoEntity.QuantidadeSaldo = _calculoPrcInsumoEntity.QuantidadeSaldo;

						var _insumoOriginal = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar<PRCInsumoTableColunsVM>(o => o.CodigoInsumo == PRCInsumoEntity.CodigoInsumo
																													   && o.IdPrcProduto == PRCInsumoEntity.IdPrcProduto
																													   && o.StatusInsumo == ativo);    //ativo	
						PRCInsumoEntity.ValorCoeficienteTecnico = _insumoOriginal.ValorCoeficienteTecnico;

						#endregion

						_uowSciex.CommandStackSciex.DetachEntries();

						_uowSciex.CommandStackSciex.PRCSolicDetalhe.Apagar(id);

						var updatePrcInsumo = new PRCInsumoEntity()
						{
							CodigoDetalhe = PRCInsumoEntity.CodigoDetalhe,
							CodigoInsumo = PRCInsumoEntity.CodigoInsumo,
							CodigoNCM = PRCInsumoEntity.CodigoNCM,
							CodigoUnidade = PRCInsumoEntity.CodigoUnidade,
							DescricaoEspecificacaoTecnica = PRCInsumoEntity.DescricaoEspecificacaoTecnica,
							DescricaoInsumo = PRCInsumoEntity.DescricaoInsumo,
							DescricaoPartNumber = PRCInsumoEntity.DescricaoPartNumber,
							IdInsumo = PRCInsumoEntity.IdInsumo,
							IdPrcProduto = PRCInsumoEntity.IdPrcProduto,
							QuantidadeAdicional = PRCInsumoEntity.QuantidadeAdicional,
							QuantidadeAprovado = PRCInsumoEntity.QuantidadeAprovado,
							QuantidadeComp = PRCInsumoEntity.QuantidadeComp,
							QuantidadeSaldo = PRCInsumoEntity.QuantidadeSaldo,
							StatusInsumo = PRCInsumoEntity.StatusInsumo,
							StatusInsumoNovo = PRCInsumoEntity.StatusInsumoNovo,
							TipoInsumo = PRCInsumoEntity.TipoInsumo,
							ValorAdicional = PRCInsumoEntity.ValorAdicional,
							ValorAdicionalFrete = PRCInsumoEntity.ValorAdicionalFrete,
							ValorCoeficienteTecnico = PRCInsumoEntity.ValorCoeficienteTecnico,
							ValorDolarCFRAprovado = PRCInsumoEntity.ValorDolarCFRAprovado,
							ValorDolarAprovado = PRCInsumoEntity.ValorDolarAprovado,
							ValorDolarComp = PRCInsumoEntity.ValorDolarComp,
							ValorDolarFOBAprovado = PRCInsumoEntity.ValorDolarFOBAprovado,
							ValorDolarSaldo = PRCInsumoEntity.ValorDolarSaldo,
							ValorDolarUnitario = PRCInsumoEntity.ValorDolarUnitario,
							ValorDolarUnitarioCrf = PRCInsumoEntity.ValorDolarUnitarioCrf,
							ValorFreteAprovado = PRCInsumoEntity.ValorFreteAprovado,
							ValorNacionalAprovado = PRCInsumoEntity.ValorNacionalAprovado,
							ValorPercentualPerda = PRCInsumoEntity.ValorPercentualPerda
						};

						_uowSciex.CommandStackSciex.PRCInsumo.Salvar(updatePrcInsumo);

						var updateDetalheInsumo = new PRCDetalheInsumoEntity()
						{
							CodigoPais = _prcDetalheInsumo.CodigoPais,
							IdDetalheInsumo = _prcDetalheInsumo.IdDetalheInsumo,
							IdMoeda = _prcDetalheInsumo.IdMoeda,
							IdPrcInsumo = _prcDetalheInsumo.IdPrcInsumo,
							NumeroSequencial = _prcDetalheInsumo.NumeroSequencial,
							Quantidade = _prcDetalheInsumo.Quantidade,
							ValorDolar = _prcDetalheInsumo.ValorDolar,
							ValorDolarCFR = _prcDetalheInsumo.ValorDolarCFR,
							ValorDolarFOB = _prcDetalheInsumo.ValorDolarFOB,
							ValorFrete = _prcDetalheInsumo.ValorFrete,
							ValorUnitario = _prcDetalheInsumo.ValorUnitario,
							ValorUnitarioCFR = _prcDetalheInsumo.ValorUnitarioCFR
						};

						_uowSciex.CommandStackSciex.PRCDetalheInsumo.Salvar(updateDetalheInsumo);
						_uowSciex.CommandStackSciex.Save();

						return (int)EnumStatusRetornoRequisicao.SUCESSO;
					}
					else if (_solicDetalhe.IdTipoSolicitacao == (int)EnumTipoAlteracaoInsumo.VALOR_UNITARIO)
					{
						var objeto = new SolicitacoesAlteracaoVM();
						decimal? _descricaoDe = Convert.ToDecimal(_solicDetalhe.DescricaoDe);
						objeto.ValorUnitarioAlteracaoDe = new ValorUnitarioVM();
						objeto.ValorUnitarioAlteracaoDe.ValorDe = _descricaoDe;

						var PRCInsumoEntity = _insumoCopia;

						#region SCIEX_PRC_DETALHE_INSUMO

						var _calculoDetalheInsumoEntity = _valorUnitarioBll.CalcularValoresDetalhe(false, objeto, PRCInsumoEntity, _prcDetalheInsumo);

						_prcDetalheInsumo.ValorUnitario = _calculoDetalheInsumoEntity.ValorUnitario;
						_prcDetalheInsumo.ValorDolar = _calculoDetalheInsumoEntity.ValorDolar;
						_prcDetalheInsumo.ValorDolarCFR = _calculoDetalheInsumoEntity.ValorDolarCFR;
						_prcDetalheInsumo.ValorDolarFOB = _calculoDetalheInsumoEntity.ValorDolarFOB;

						#endregion


						#region SCIEX_PRC_INSUMOS

						_valorUnitarioBll.CalcularValoresInsumo(false, objeto, ref PRCInsumoEntity);

						#endregion

						_uowSciex.CommandStackSciex.DetachEntries();

						_uowSciex.CommandStackSciex.PRCSolicDetalhe.Apagar(id);

						var updatePrcInsumo = new PRCInsumoEntity()
						{
							CodigoDetalhe = PRCInsumoEntity.CodigoDetalhe,
							CodigoInsumo = PRCInsumoEntity.CodigoInsumo,
							CodigoNCM = PRCInsumoEntity.CodigoNCM,
							CodigoUnidade = PRCInsumoEntity.CodigoUnidade,
							DescricaoEspecificacaoTecnica = PRCInsumoEntity.DescricaoEspecificacaoTecnica,
							DescricaoInsumo = PRCInsumoEntity.DescricaoInsumo,
							DescricaoPartNumber = PRCInsumoEntity.DescricaoPartNumber,
							IdInsumo = PRCInsumoEntity.IdInsumo,
							IdPrcProduto = PRCInsumoEntity.IdPrcProduto,
							QuantidadeAdicional = PRCInsumoEntity.QuantidadeAdicional,
							QuantidadeAprovado = PRCInsumoEntity.QuantidadeAprovado,
							QuantidadeComp = PRCInsumoEntity.QuantidadeComp,
							QuantidadeSaldo = PRCInsumoEntity.QuantidadeSaldo,
							StatusInsumo = PRCInsumoEntity.StatusInsumo,
							StatusInsumoNovo = PRCInsumoEntity.StatusInsumoNovo,
							TipoInsumo = PRCInsumoEntity.TipoInsumo,
							ValorAdicional = PRCInsumoEntity.ValorAdicional,
							ValorAdicionalFrete = PRCInsumoEntity.ValorAdicionalFrete,
							ValorCoeficienteTecnico = PRCInsumoEntity.ValorCoeficienteTecnico,
							ValorDolarCFRAprovado = PRCInsumoEntity.ValorDolarCFRAprovado,
							ValorDolarAprovado = PRCInsumoEntity.ValorDolarAprovado,
							ValorDolarComp = PRCInsumoEntity.ValorDolarComp,
							ValorDolarFOBAprovado = PRCInsumoEntity.ValorDolarFOBAprovado,
							ValorDolarSaldo = PRCInsumoEntity.ValorDolarSaldo,
							ValorDolarUnitario = PRCInsumoEntity.ValorDolarUnitario,
							ValorDolarUnitarioCrf = PRCInsumoEntity.ValorDolarUnitarioCrf,
							ValorFreteAprovado = PRCInsumoEntity.ValorFreteAprovado,
							ValorNacionalAprovado = PRCInsumoEntity.ValorNacionalAprovado,
							ValorPercentualPerda = PRCInsumoEntity.ValorPercentualPerda
						};

						_uowSciex.CommandStackSciex.PRCInsumo.Salvar(updatePrcInsumo);

						var updateDetalheInsumo = new PRCDetalheInsumoEntity()
						{
							CodigoPais = _prcDetalheInsumo.CodigoPais,
							IdDetalheInsumo = _prcDetalheInsumo.IdDetalheInsumo,
							IdMoeda = _prcDetalheInsumo.IdMoeda,
							IdPrcInsumo = _prcDetalheInsumo.IdPrcInsumo,
							NumeroSequencial = _prcDetalheInsumo.NumeroSequencial,
							Quantidade = _prcDetalheInsumo.Quantidade,
							ValorDolar = _prcDetalheInsumo.ValorDolar,
							ValorDolarCFR = _prcDetalheInsumo.ValorDolarCFR,
							ValorDolarFOB = _prcDetalheInsumo.ValorDolarFOB,
							ValorFrete = _prcDetalheInsumo.ValorFrete,
							ValorUnitario = _prcDetalheInsumo.ValorUnitario,
							ValorUnitarioCFR = _prcDetalheInsumo.ValorUnitarioCFR
						};

						_uowSciex.CommandStackSciex.PRCDetalheInsumo.Salvar(updateDetalheInsumo);
						_uowSciex.CommandStackSciex.Save();

						return (int)EnumStatusRetornoRequisicao.SUCESSO;
					}
					else if (_solicDetalhe.IdTipoSolicitacao == (int)EnumTipoAlteracaoInsumo.VALOR_FRETE)
					{
						var objeto = new SolicitacoesAlteracaoVM();
						decimal? _descricaoDe = Convert.ToDecimal(_solicDetalhe.DescricaoDe);
						objeto.ValorFreteAlteracaoDe = new ValorFreteVM();
						objeto.ValorFreteAlteracaoDe.ValorDe = _descricaoDe;

						var PRCInsumoEntity = _insumoCopia;

						objeto.PRCInsumoDE = new PRCInsumoTableColunsVM();
						objeto.PRCInsumoDE.ValorFreteAprovado = PRCInsumoEntity.ValorFreteAprovado;
						objeto.PRCInsumoDE.ValorAdicionalFrete = PRCInsumoEntity.ValorAdicionalFrete;

						#region SCIEX_PRC_DETALHE_INSUMO

						var _calculoDetalheInsumoEntity = _valorFreteBll.CalcularValoresDetalhe(false, objeto, PRCInsumoEntity, _prcDetalheInsumo);

						_prcDetalheInsumo.ValorFrete = _calculoDetalheInsumoEntity.ValorFrete;
						_prcDetalheInsumo.ValorDolar = _calculoDetalheInsumoEntity.ValorDolar;
						_prcDetalheInsumo.ValorDolarCFR = _calculoDetalheInsumoEntity.ValorDolarCFR;
						_prcDetalheInsumo.ValorUnitarioCFR = _calculoDetalheInsumoEntity.ValorUnitarioCFR;

						#endregion


						#region SCIEX_PRC_INSUMOS

						_valorFreteBll.CalcularValoresInsumo(false, objeto, ref PRCInsumoEntity);

						#endregion

						_uowSciex.CommandStackSciex.DetachEntries();

						_uowSciex.CommandStackSciex.PRCSolicDetalhe.Apagar(id);

						var updatePrcInsumo = new PRCInsumoEntity()
						{
							CodigoDetalhe = PRCInsumoEntity.CodigoDetalhe,
							CodigoInsumo = PRCInsumoEntity.CodigoInsumo,
							CodigoNCM = PRCInsumoEntity.CodigoNCM,
							CodigoUnidade = PRCInsumoEntity.CodigoUnidade,
							DescricaoEspecificacaoTecnica = PRCInsumoEntity.DescricaoEspecificacaoTecnica,
							DescricaoInsumo = PRCInsumoEntity.DescricaoInsumo,
							DescricaoPartNumber = PRCInsumoEntity.DescricaoPartNumber,
							IdInsumo = PRCInsumoEntity.IdInsumo,
							IdPrcProduto = PRCInsumoEntity.IdPrcProduto,
							QuantidadeAdicional = PRCInsumoEntity.QuantidadeAdicional,
							QuantidadeAprovado = PRCInsumoEntity.QuantidadeAprovado,
							QuantidadeComp = PRCInsumoEntity.QuantidadeComp,
							QuantidadeSaldo = PRCInsumoEntity.QuantidadeSaldo,
							StatusInsumo = PRCInsumoEntity.StatusInsumo,
							StatusInsumoNovo = PRCInsumoEntity.StatusInsumoNovo,
							TipoInsumo = PRCInsumoEntity.TipoInsumo,
							ValorAdicional = PRCInsumoEntity.ValorAdicional,
							ValorAdicionalFrete = PRCInsumoEntity.ValorAdicionalFrete,
							ValorCoeficienteTecnico = PRCInsumoEntity.ValorCoeficienteTecnico,
							ValorDolarCFRAprovado = PRCInsumoEntity.ValorDolarCFRAprovado,
							ValorDolarAprovado = PRCInsumoEntity.ValorDolarAprovado,
							ValorDolarComp = PRCInsumoEntity.ValorDolarComp,
							ValorDolarFOBAprovado = PRCInsumoEntity.ValorDolarFOBAprovado,
							ValorDolarSaldo = PRCInsumoEntity.ValorDolarSaldo,
							ValorDolarUnitario = PRCInsumoEntity.ValorDolarUnitario,
							ValorDolarUnitarioCrf = PRCInsumoEntity.ValorDolarUnitarioCrf,
							ValorFreteAprovado = PRCInsumoEntity.ValorFreteAprovado,
							ValorNacionalAprovado = PRCInsumoEntity.ValorNacionalAprovado,
							ValorPercentualPerda = PRCInsumoEntity.ValorPercentualPerda
						};

						_uowSciex.CommandStackSciex.PRCInsumo.Salvar(updatePrcInsumo);

						var updateDetalheInsumo = new PRCDetalheInsumoEntity()
						{
							CodigoPais = _prcDetalheInsumo.CodigoPais,
							IdDetalheInsumo = _prcDetalheInsumo.IdDetalheInsumo,
							IdMoeda = _prcDetalheInsumo.IdMoeda,
							IdPrcInsumo = _prcDetalheInsumo.IdPrcInsumo,
							NumeroSequencial = _prcDetalheInsumo.NumeroSequencial,
							Quantidade = _prcDetalheInsumo.Quantidade,
							ValorDolar = _prcDetalheInsumo.ValorDolar,
							ValorDolarCFR = _prcDetalheInsumo.ValorDolarCFR,
							ValorDolarFOB = _prcDetalheInsumo.ValorDolarFOB,
							ValorFrete = _prcDetalheInsumo.ValorFrete,
							ValorUnitario = _prcDetalheInsumo.ValorUnitario,
							ValorUnitarioCFR = _prcDetalheInsumo.ValorUnitarioCFR
						};

						_uowSciex.CommandStackSciex.PRCDetalheInsumo.Salvar(updateDetalheInsumo);
						_uowSciex.CommandStackSciex.Save();

						return(int)EnumStatusRetornoRequisicao.SUCESSO;
					}
				}
				
				return (int)EnumStatusRetornoRequisicao.SUCESSO;
			}
			catch (Exception e)
			{
				return (int)EnumStatusRetornoRequisicao.ERRO;
			}
			
		}
			   
		private void RecalcularInsumo(int idInsumo)
		{
			var _lisPEDetalheInsumo = _uowSciex.QueryStackSciex.PEDetalheInsumo.Listar(q => q.IdPEInsumo == idInsumo);
			var _listPRCDetalheInsumo = _uowSciex.QueryStackSciex.PRCDetalheInsumo.Listar(o => o.IdPrcInsumo == idInsumo);
			var prcInsumoEntity = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(q => q.IdInsumo == idInsumo);

			if (_lisPEDetalheInsumo.Count > 0)
			{
				prcInsumoEntity.ValorAdicional = _lisPEDetalheInsumo.Sum(o => (o.ValorDolarFOB == null) ? 0 : o.ValorDolarFOB);
				prcInsumoEntity.ValorAdicionalFrete = _lisPEDetalheInsumo.Sum(o => (o.ValorFrete == null) ? 0 : o.ValorFrete);
				prcInsumoEntity.QuantidadeAdicional = _listPRCDetalheInsumo.Sum(o => o.Quantidade);
				prcInsumoEntity.ValorDolarSaldo = _listPRCDetalheInsumo.Sum(o => o.ValorDolar == null ? 0 : o.ValorDolar);
				prcInsumoEntity.QuantidadeSaldo = _listPRCDetalheInsumo.Sum(o => o.Quantidade);
				prcInsumoEntity.ValorDolarUnitario = _lisPEDetalheInsumo.Sum(o => (o.ValorDolarFOB == null) ? 0 : o.ValorDolarFOB) / _listPRCDetalheInsumo.Sum(o => o.Quantidade);
				prcInsumoEntity.ValorDolarUnitarioCrf = _lisPEDetalheInsumo.Sum(o => (o.ValorDolarCRF == null) ? 0 : o.ValorDolarCRF) / _listPRCDetalheInsumo.Sum(o => o.Quantidade);

				_uowSciex.CommandStackSciex.PRCInsumo.Salvar(prcInsumoEntity);
				_uowSciex.CommandStackSciex.Save();
			}
		}

		private string getStatusProcesso(string tipoStatus)
		{
			return (tipoStatus == "AP") ? "Aprovado" :
				   (tipoStatus == "AL") ? "Alterado" :
				   (tipoStatus == "PR") ? "Prorrogado" :
				   (tipoStatus == "PE") ? "Prorrogado em caráter especial" :
				   (tipoStatus == "CO") ? "Comprovado" :
				   (tipoStatus == "AU") ? "Autorizado" :
				   (tipoStatus == "CA") ? "Cancelado" :	"--";
		}
		
		enum EnumTipoAlteracaoInsumo
		{
			INCLUSAO_INSUMO = 1,
			TRANSFERENCIA_SALDO_INSUMO = 2,
			PAIS = 3,
			MOEDA = 4,
			QUANTIDADE_COEF_TECNICO = 5,
			VALOR_UNITARIO = 6,
			VALOR_FRETE =7
		}

		enum EnumStatusRetornoRequisicao
		{
			ERRO = 0,
			SUCESSO = 1,
			PARIDADE_CAMBIAL_NAO_CADASTRADA = 2,
			NAO_EXISTE_SOLCITACAO_ALTERACAO_CADASTRADA = 3
		}


	}
}
