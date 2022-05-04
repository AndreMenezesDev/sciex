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
using System.Web.UI;
using Suframa.Sciex.CrossCutting.Mensagens;
using Suframa.Sciex.CrossCutting.Compressor;
using System.IO;
using System.Linq.Expressions;

namespace Suframa.Sciex.BusinessLogic
{
	public class ProcessoInsumoBll : IProcessoInsumoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUnitOfWork _uowCadsuf;
		private readonly IUsuarioPssBll _usuarioPssBll;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;

		public ProcessoInsumoBll(
			IUnitOfWorkSciex uowSciex, 
			IUnitOfWork uowCadsuf,
			IUsuarioPssBll usuarioPssBll, 
			IUsuarioInformacoesBll usuarioInformacoesBll
			)
		{
			_uowSciex = uowSciex;
			_uowCadsuf = uowCadsuf;
			_usuarioPssBll = usuarioPssBll;
			_usuarioInformacoesBll = usuarioInformacoesBll;
		}

		public PagedItems<PRCInsumoVM> ListarProcessoInsumosNacionalOuImportadoPorIdProcessoProduto(ListarProcessoInsumosNacionalImportadosVM vm)
		{

			if (vm == null || vm.IdProcessoProduto == 0)
				return null;

			string sort = null;

			PagedItems<PRCInsumoVM> listaProcessoInsumosNacionaisOuImportados;
			if (vm.isQuadroNacional)
			{
				listaProcessoInsumosNacionaisOuImportados = _uowSciex.QueryStackSciex.PRCInsumo.ListarPaginadoGrafo(q => new PRCInsumoVM()
				{
					IdInsumo = q.IdInsumo,
					IdPrcProduto = q.IdPrcProduto,
					CodigoInsumo = q.CodigoInsumo,
					CodigoUnidade = q.CodigoUnidade,
					TipoInsumo = q.TipoInsumo,
					CodigoNCM = q.CodigoNCM,
					DescricaoInsumo = q.DescricaoInsumo,
					ValorPercentualPerda = q.ValorPercentualPerda,
					CodigoDetalhe = q.CodigoDetalhe,
					DescricaoPartNumber = q.DescricaoPartNumber,
					DescricaoEspecificacaoTecnica = q.DescricaoEspecificacaoTecnica,
					ValorCoeficienteTecnico = q.ValorCoeficienteTecnico,
					ValorDolarAprovado = q.ValorDolarAprovado,
					QuantidadeAprovado = q.QuantidadeAprovado,
					ValorNacionalAprovado = q.ValorNacionalAprovado,
					ValorDolarFOBAprovado = q.ValorDolarFOBAprovado,
					ValorDolarCFRAprovado = q.ValorDolarCFRAprovado,
					ValorFreteAprovado = q.ValorFreteAprovado,
					ValorDolarComp = q.ValorDolarComp,
					QuantidadeComp = q.QuantidadeComp,
					ValorDolarSaldo = q.ValorDolarSaldo,
					QuantidadeSaldo = q.QuantidadeSaldo,
					DescricaoTipoInsumo = q.TipoInsumo == "N" ? "Nacional" : q.TipoInsumo == "R" ? "Regional" : "Padrão",
					StatusInsumo = q.StatusInsumo,
					StatusInsumoNovo = q.StatusInsumoNovo,
					Produto = new PRCProdutoVM()
					{
						IdProduto = q.PrcProduto.IdProduto,
						IdProcesso = q.PrcProduto.IdProcesso,
						CodigoProdutoExportacao = q.PrcProduto.CodigoProdutoExportacao,
						CodigoProdutoSuframa = q.PrcProduto.CodigoProdutoSuframa,
						CodigoNCM = q.PrcProduto.CodigoNCM,
						TipoProduto = q.PrcProduto.TipoProduto,
						DescricaoModelo = q.PrcProduto.DescricaoModelo,
						QuantidadeAprovado = q.PrcProduto.QuantidadeAprovado,
						CodigoUnidade = q.PrcProduto.CodigoUnidade,
						ValorDolarAprovado = q.PrcProduto.ValorDolarAprovado,
						ValorFluxoCaixa = q.PrcProduto.ValorFluxoCaixa
					}
					
				}
				,
				q=>
				q.IdPrcProduto == vm.IdProcessoProduto
				&&
				(q.TipoInsumo.Equals("N") || q.TipoInsumo.Equals("R"))
				&&
				(
					vm.CodigoInsumo == null || q.CodigoInsumo == vm.CodigoInsumo
					||
					vm.CodigoNCM == null || q.CodigoNCM.Equals(vm.CodigoNCM.ToString())
				)
				,
				vm
				);
			}
			else
			{
				

				listaProcessoInsumosNacionaisOuImportados = _uowSciex.QueryStackSciex.PRCInsumo.ListarPaginadoGrafo(q => new PRCInsumoVM()
				{
					IdInsumo = q.IdInsumo,
					IdPrcProduto = q.IdPrcProduto,
					CodigoInsumo = q.CodigoInsumo,
					CodigoUnidade = q.CodigoUnidade,
					TipoInsumo = q.TipoInsumo,
					CodigoNCM = q.CodigoNCM,
					DescricaoInsumo = q.DescricaoInsumo,
					ValorPercentualPerda = q.ValorPercentualPerda,
					CodigoDetalhe = q.CodigoDetalhe,
					DescricaoPartNumber = q.DescricaoPartNumber,
					DescricaoEspecificacaoTecnica = q.DescricaoEspecificacaoTecnica,
					ValorCoeficienteTecnico = q.ValorCoeficienteTecnico,
					ValorDolarAprovado = q.ValorDolarAprovado,
					QuantidadeAprovado = q.QuantidadeAprovado,
					QuantidadeAdicional = q.QuantidadeAdicional,
					ValorNacionalAprovado = q.ValorNacionalAprovado,
					ValorDolarFOBAprovado = q.ValorDolarFOBAprovado,
					ValorDolarCFRAprovado = q.ValorDolarCFRAprovado,
					ValorFreteAprovado = q.ValorFreteAprovado,
					ValorAdicionalFrete = q.ValorAdicionalFrete,
					ValorDolarComp = q.ValorDolarComp,
					ValorAdicional = q.ValorAdicional,
					QuantidadeComp = q.QuantidadeComp,
					ValorDolarSaldo = q.ValorDolarSaldo,
					QuantidadeSaldo = q.QuantidadeSaldo,
					DescricaoTipoInsumo = q.TipoInsumo == "N" ? "Nacional" : q.TipoInsumo == "R" ? "Regional" : "Padrão",
					StatusInsumo = q.StatusInsumo,
					StatusInsumoNovo = q.StatusInsumoNovo,
					Produto = new PRCProdutoVM()
					{
						IdProduto = q.PrcProduto.IdProduto,
						IdProcesso = q.PrcProduto.IdProcesso,
						CodigoProdutoExportacao = q.PrcProduto.CodigoProdutoExportacao,
						CodigoProdutoSuframa = q.PrcProduto.CodigoProdutoSuframa,
						CodigoNCM = q.PrcProduto.CodigoNCM,
						TipoProduto = q.PrcProduto.TipoProduto,
						DescricaoModelo = q.PrcProduto.DescricaoModelo,
						QuantidadeAprovado = q.PrcProduto.QuantidadeAprovado,
						CodigoUnidade = q.PrcProduto.CodigoUnidade,
						ValorDolarAprovado = q.PrcProduto.ValorDolarAprovado,
						ValorFluxoCaixa = q.PrcProduto.ValorFluxoCaixa					
					}					
				},
				q=>
				
				q.IdPrcProduto == vm.IdProcessoProduto
				&&
				(q.TipoInsumo.Equals("P") || q.TipoInsumo.Equals("E"))
				&& 
				(q.StatusInsumoNovo == 1 || q.StatusInsumoNovo == 0 || q.StatusInsumoNovo == null)
				&&
				(q.StatusInsumo != 2 || q.StatusInsumoNovo == 1)
				&&
				(vm.CodigoInsumo == null || q.CodigoInsumo == vm.CodigoInsumo)
				&&
				(vm.CodigoNCM == null || q.CodigoNCM.Equals(vm.CodigoNCM.ToString()))
				,
				vm
				);
			}

			foreach (var insumo in listaProcessoInsumosNacionaisOuImportados.Items)
			{
				var undMed = _uowSciex.QueryStackSciex.ViewUnidadeMedida.Selecionar(o => o.CodigoUnidadeMedida == insumo.CodigoUnidade);
				insumo.DescCodigoUnidade = undMed != null ? undMed.CodigoUnidadeMedida.ToString("D3") + " | " + undMed.Descricao : "-";

				var qtdProduto = insumo?.Produto?.QuantidadeAprovado ?? 0;
				var coefTec = insumo?.ValorCoeficienteTecnico ?? 0;
				var percPerda = insumo?.ValorPercentualPerda ?? 0;

				insumo.QtdMaxInsumo = qtdProduto * coefTec;

				insumo.QtdMaxInsumo = insumo.QtdMaxInsumo + (insumo.QtdMaxInsumo * (percPerda / 100));

				insumo.IsNovoRegistro = (insumo.StatusInsumoNovo == 1) ? "SIM" : "NÃO";
			}

			return listaProcessoInsumosNacionaisOuImportados;
		}

		public string ValidaSeUsuarioAlterouInsumos(ListarProcessoInsumosNacionalImportadosVM vm)
		{
			var naoENovoInsumo = 0;
			var emAlteracao = 2;
			var existsInsumoDuplicado = _uowSciex.QueryStackSciex.PRCInsumo.Listar(o => (o.StatusInsumo == naoENovoInsumo || o.StatusInsumo == emAlteracao) 
																					  && o.IdPrcProduto == vm.IdProcessoProduto);
			return existsInsumoDuplicado.Count > 0 ? "SIM" : "NAO";
		}


		public PRCProdutoVM BuscarInformacoesAdicionaisProduto(PRCProdutoVM vm)
		{
			if (vm == null )
				return null;	
			var vmRetorno = new PRCProdutoVM();

			var codigoUnidadeMedida = _uowSciex.QueryStackSciex.ViewUnidadeMedida.Selecionar(o => o.CodigoUnidadeMedida == vm.CodigoUnidade);
			if (codigoUnidadeMedida != null)
			{
				vmRetorno.DescricaoUnidadeMedida = codigoUnidadeMedida.Descricao;
			}

			return vmRetorno;

		}

		public PagedItems<PRCDetalheInsumoVM> ListarProcessoInsumosNacionalOuImportadoPorIdInsumo(ListarProcessoInsumosNacionalImportadosVM vm)
		{

			if (vm == null || vm.IdProcessoInsumo == 0)
				return null;

			string filtroPosterior = null;

			if (vm.Sort != null)
			{
				if (vm.Sort.Equals("ValorTotal") || vm.Sort.Equals("CodigoPais") || vm.Sort.Equals("CodigoDescricaoMoeda"))
				{
					filtroPosterior = vm.Sort;
					vm.Sort = null;
				}
			}

			PagedItems<PRCDetalheInsumoVM> listaCodigoInsumosIncluidos;
			if (vm.isQuadroNacional)
			{

				listaCodigoInsumosIncluidos = _uowSciex.QueryStackSciex.PRCDetalheInsumo.ListarPaginadoGrafo(q => new PRCDetalheInsumoVM()
				{
					IdDetalheInsumo = q.IdDetalheInsumo,
					IdPrcInsumo = q.IdPrcInsumo,
					IdMoeda = q.IdMoeda,
					NumeroSequencial = q.NumeroSequencial,
					CodigoPais = q.CodigoPais,
					Quantidade = q.Quantidade,
					ValorUnitario = q.ValorUnitario,
					ValorFrete = q.ValorFrete,
					ValorDolar = q.ValorDolar,
					ValorUnitarioCFR = q.ValorUnitarioCFR,
					ValorDolarCFR = q.ValorDolarCFR,
					ValorDolarFOB = q.ValorDolarFOB,
					PrcInsumo = new PRCInsumoVM()
					{
						IdInsumo = q.PrcInsumo.IdInsumo,
						IdPrcProduto = q.PrcInsumo.IdPrcProduto,
						CodigoInsumo = q.PrcInsumo.CodigoInsumo,
						CodigoUnidade = q.PrcInsumo.CodigoUnidade,
						TipoInsumo = q.PrcInsumo.TipoInsumo,
						CodigoNCM = q.PrcInsumo.CodigoNCM,
						DescricaoInsumo = q.PrcInsumo.DescricaoInsumo,
						ValorPercentualPerda = q.PrcInsumo.ValorPercentualPerda,
						CodigoDetalhe = q.PrcInsumo.CodigoDetalhe,
						DescricaoPartNumber = q.PrcInsumo.DescricaoPartNumber,
						DescricaoEspecificacaoTecnica = q.PrcInsumo.DescricaoEspecificacaoTecnica,
						ValorCoeficienteTecnico = q.PrcInsumo.ValorCoeficienteTecnico,
						ValorDolarAprovado = q.PrcInsumo.ValorDolarAprovado,
						QuantidadeAprovado = q.PrcInsumo.QuantidadeAprovado,
						ValorNacionalAprovado = q.PrcInsumo.ValorNacionalAprovado,
						ValorDolarFOBAprovado = q.PrcInsumo.ValorDolarFOBAprovado,
						ValorDolarCFRAprovado = q.PrcInsumo.ValorDolarCFRAprovado,
						ValorFreteAprovado = q.PrcInsumo.ValorFreteAprovado,
						ValorDolarComp = q.PrcInsumo.ValorDolarComp,
						QuantidadeComp = q.PrcInsumo.QuantidadeComp,
						ValorDolarSaldo = q.PrcInsumo.ValorDolarSaldo,
						QuantidadeSaldo = q.PrcInsumo.QuantidadeSaldo,
						DescricaoTipoInsumo = q.PrcInsumo.TipoInsumo == "N" ? "Nacional" : q.PrcInsumo.TipoInsumo == "R" ? "Regional" : "Padrão" ,
						Produto = new PRCProdutoVM()
						{
							IdProduto = q.PrcInsumo.PrcProduto.IdProduto,
							IdProcesso = q.PrcInsumo.PrcProduto.IdProcesso,
							CodigoProdutoExportacao = q.PrcInsumo.PrcProduto.CodigoProdutoExportacao,
							CodigoProdutoSuframa = q.PrcInsumo.PrcProduto.CodigoProdutoSuframa,
							CodigoNCM = q.PrcInsumo.PrcProduto.CodigoNCM,
							TipoProduto = q.PrcInsumo.PrcProduto.TipoProduto,
							DescricaoModelo = q.PrcInsumo.PrcProduto.DescricaoModelo,
							QuantidadeAprovado = q.PrcInsumo.PrcProduto.QuantidadeAprovado,
							CodigoUnidade = q.PrcInsumo.PrcProduto.CodigoUnidade,
							ValorDolarAprovado = q.PrcInsumo.PrcProduto.ValorDolarAprovado,
							ValorFluxoCaixa = q.PrcInsumo.PrcProduto.ValorFluxoCaixa
						}
					}
				}
				,
				q =>
				q.IdPrcInsumo == vm.IdProcessoInsumo
				&&
				(q.PrcInsumo.TipoInsumo.Equals("N") || q.PrcInsumo.TipoInsumo.Equals("R"))
				,
				vm
				);

				foreach (var item in listaCodigoInsumosIncluidos.Items)
				{
					var valorUnitario = item.ValorUnitario != null ? item.ValorUnitario : 0;
					item.ValorTotal = item.Quantidade * valorUnitario;
				}

			}
			else
			{

				listaCodigoInsumosIncluidos = _uowSciex.QueryStackSciex.PRCDetalheInsumo.ListarPaginadoGrafo(q => new PRCDetalheInsumoVM()
				{
					IdDetalheInsumo = q.IdDetalheInsumo,
					IdPrcInsumo = q.IdPrcInsumo,
					IdMoeda = q.IdMoeda,
					NumeroSequencial = q.NumeroSequencial,
					CodigoPais = q.CodigoPais,
					Quantidade = q.Quantidade,
					ValorUnitario = q.ValorUnitario,
					ValorFrete = q.ValorFrete,
					ValorDolar = q.ValorDolar,
					ValorUnitarioCFR = q.ValorUnitarioCFR,
					ValorDolarCFR = q.ValorDolarCFR,
					ValorDolarFOB = q.ValorDolarFOB,
					PrcInsumo = new PRCInsumoVM()
					{
						IdInsumo = q.PrcInsumo.IdInsumo,
						IdPrcProduto = q.PrcInsumo.IdPrcProduto,
						CodigoInsumo = q.PrcInsumo.CodigoInsumo,
						CodigoUnidade = q.PrcInsumo.CodigoUnidade,
						TipoInsumo = q.PrcInsumo.TipoInsumo,
						CodigoNCM = q.PrcInsumo.CodigoNCM,
						DescricaoInsumo = q.PrcInsumo.DescricaoInsumo,
						ValorPercentualPerda = q.PrcInsumo.ValorPercentualPerda,
						CodigoDetalhe = q.PrcInsumo.CodigoDetalhe,
						DescricaoPartNumber = q.PrcInsumo.DescricaoPartNumber,
						DescricaoEspecificacaoTecnica = q.PrcInsumo.DescricaoEspecificacaoTecnica,
						ValorCoeficienteTecnico = q.PrcInsumo.ValorCoeficienteTecnico,
						ValorDolarAprovado = q.PrcInsumo.ValorDolarAprovado,
						QuantidadeAprovado = q.PrcInsumo.QuantidadeAprovado,
						ValorNacionalAprovado = q.PrcInsumo.ValorNacionalAprovado,
						ValorDolarFOBAprovado = q.PrcInsumo.ValorDolarFOBAprovado,
						ValorDolarCFRAprovado = q.PrcInsumo.ValorDolarCFRAprovado,
						ValorFreteAprovado = q.PrcInsumo.ValorFreteAprovado,
						ValorDolarComp = q.PrcInsumo.ValorDolarComp,
						QuantidadeComp = q.PrcInsumo.QuantidadeComp,
						ValorDolarSaldo = q.PrcInsumo.ValorDolarSaldo,
						QuantidadeSaldo = q.PrcInsumo.QuantidadeSaldo,
						DescricaoTipoInsumo = q.PrcInsumo.TipoInsumo == "N" ? "Nacional" : q.PrcInsumo.TipoInsumo == "R" ? "Regional" : "Padrão",
						Produto = new PRCProdutoVM()
						{
							IdProduto = q.PrcInsumo.PrcProduto.IdProduto,
							IdProcesso = q.PrcInsumo.PrcProduto.IdProcesso,
							CodigoProdutoExportacao = q.PrcInsumo.PrcProduto.CodigoProdutoExportacao,
							CodigoProdutoSuframa = q.PrcInsumo.PrcProduto.CodigoProdutoSuframa,
							CodigoNCM = q.PrcInsumo.PrcProduto.CodigoNCM,
							TipoProduto = q.PrcInsumo.PrcProduto.TipoProduto,
							DescricaoModelo = q.PrcInsumo.PrcProduto.DescricaoModelo,
							QuantidadeAprovado = q.PrcInsumo.PrcProduto.QuantidadeAprovado,
							CodigoUnidade = q.PrcInsumo.PrcProduto.CodigoUnidade,
							ValorDolarAprovado = q.PrcInsumo.PrcProduto.ValorDolarAprovado,
							ValorFluxoCaixa = q.PrcInsumo.PrcProduto.ValorFluxoCaixa
						}
					}
				}
				,
				q =>
				q.IdPrcInsumo == vm.IdProcessoInsumo
				&&
				(q.PrcInsumo.TipoInsumo.Equals("P") || q.PrcInsumo.TipoInsumo.Equals("E"))
				,
				vm
				);

				
			}

			foreach (var item in listaCodigoInsumosIncluidos.Items)
			{
				string codigoPais = Convert.ToInt32(item.CodigoPais).ToString("D3");
				if(codigoPais != "000")
				{
					var pais = _uowSciex.QueryStackSciex.ViewPais.Selecionar(o => o.CodigoPais == codigoPais);

					item.DescricaoPais = pais.Descricao;
				}


				var regMoeda = _uowSciex.QueryStackSciex.Moeda.Selecionar(o => o.IdMoeda == item.IdMoeda);

				item.CodigoDescricaoMoeda = regMoeda.CodigoMoeda + " | " + regMoeda.Descricao;
			}

			if (!string.IsNullOrEmpty(filtroPosterior))
			{
				if (filtroPosterior.Equals("ValorTotal"))
				{
					if (!vm.Reverse)
					{
						listaCodigoInsumosIncluidos.Items = listaCodigoInsumosIncluidos.Items.OrderBy(q => q.ValorTotal).ToList();
					}
					else
					{
						listaCodigoInsumosIncluidos.Items = listaCodigoInsumosIncluidos.Items.OrderByDescending(q => q.ValorTotal).ToList();
					} 
				}
				else if (filtroPosterior.Equals("CodigoPais"))
				{
					if (!vm.Reverse)
					{
						listaCodigoInsumosIncluidos.Items = listaCodigoInsumosIncluidos.Items.OrderBy(q => q.CodigoPais).ToList();
					}
					else
					{
						listaCodigoInsumosIncluidos.Items = listaCodigoInsumosIncluidos.Items.OrderByDescending(q => q.CodigoPais).ToList();
					}
				}
				else if (filtroPosterior.Equals("CodigoDescricaoMoeda"))
				{
					if (!vm.Reverse)
					{
						listaCodigoInsumosIncluidos.Items = listaCodigoInsumosIncluidos.Items.OrderBy(q => q.CodigoDescricaoMoeda).ToList();
					}
					else
					{
						listaCodigoInsumosIncluidos.Items = listaCodigoInsumosIncluidos.Items.OrderByDescending(q => q.CodigoDescricaoMoeda).ToList();
					}
				}
			}

			return listaCodigoInsumosIncluidos;
		}

		public bool Deletar(int id)
		{
			if (id == 0)
				return false;

			try
			{
				var regInsumo = _uowSciex.QueryStackSciex.PEInsumo.Selecionar(s => s.IdPEInsumo == id);

				if (regInsumo != null)
				{
					var regDetalhesInsumo = _uowSciex.QueryStackSciex.PEDetalheInsumo.Listar(o => o.IdPEInsumo == regInsumo.IdPEInsumo);

					if (regDetalhesInsumo.Count > 0)
					{
						foreach (var item in regDetalhesInsumo)
						{
							_uowSciex.CommandStackSciex.PEDetalheInsumo.Apagar(item.IdPEDetalheInsumo);
							_uowSciex.CommandStackSciex.Save();
						} 
					}

					_uowSciex.CommandStackSciex.PEInsumo.Apagar(regInsumo.IdPEInsumo);
					_uowSciex.CommandStackSciex.Save();
				}

				return true;
			}
			catch (Exception e)
			{
				return false;
			}

		}

		public bool AdicionarInsumoAoProduto(LEInsumoVM vm)
		{
			if (vm == null || vm.IdPEProduto == 0)
				return false;

			try
			{
				foreach (var dadosInsumo in vm.ListaInsumosSelecionados)
				{

					var novoInsumo = new PEInsumoEntity()
					{
						CodigoInsumo = dadosInsumo.CodigoInsumo,
						CodigoUnidade = dadosInsumo.CodigoUnidadeMedida,
						TipoInsumo = dadosInsumo.TipoInsumo,
						CodigoNcm = dadosInsumo.CodigoNCM,
						CodigoDetalhe = dadosInsumo.CodigoDetalhe,
						DescricaoInsumo = dadosInsumo.DescricaoInsumo,
						DescricaoEspecificacaoTecnica = dadosInsumo.DescricaoEspecTecnica,
						IdPEProduto = vm.IdPEProduto,
						ValorCoeficienteTecnico = dadosInsumo.ValorCoeficienteTecnico
					};

					_uowSciex.CommandStackSciex.PEInsumo.Salvar(novoInsumo);
				}

				_uowSciex.CommandStackSciex.Save();

				return true;
			}
			catch (Exception e)
			{
				return false;
			}
		}
		
		public PagedItems<PEInsumoVM> ListarInsumosNacionalImportadosPorIdProduto(ListarInsumosNacionalImportadosVM vm)
		{
			if (vm == null || vm.IdPEProduto == 0)
				return null;

			PagedItems<PEInsumoVM> listaInsumos;

			if (vm.isQuadroNacional)
			{
				listaInsumos = _uowSciex.QueryStackSciex.PEInsumo.ListarPaginadoGrafo(w => 

				new PEInsumoVM()
				{
					IdPEInsumo = w.IdPEInsumo,
					IdPEProduto = w.IdPEProduto,
					CodigoDetalhe = w.CodigoDetalhe,
					CodigoInsumo = w.CodigoInsumo,
					CodigoNcm = w.CodigoNcm,
					CodigoUnidade = w.CodigoUnidade,
					DescricaoEspecificacaoTecnica = w.DescricaoEspecificacaoTecnica,
					DescricaoInsumo = w.DescricaoInsumo,
					DescricaoPartNumber = w.DescricaoPartNumber,
					TipoInsumo = w.TipoInsumo,
					ValorCoeficienteTecnico = w.ValorCoeficienteTecnico,
					ValorDolar = w.ValorDolar,
					ValorPercentualPerda = w.ValorPercentualPerda,
					ListaPEDetalheInsumo = w.ListaPEDetalheInsumo.Select(q=> new PEDetalheInsumoVM()
					{
						IdPEInsumo = q.IdPEInsumo,
						IdMoeda = q.IdMoeda,
						IdPEDetalheInsumo = q.IdPEDetalheInsumo,
						CodigoPais = q.CodigoPais,
						NumeroSequencial = q.NumeroSequencial,
						Quantidade = q.Quantidade,
						ValorDolar = q.ValorDolar,
						ValorDolarCRF = q.ValorDolarCRF,
						ValorDolarFOB = q.ValorDolarFOB,
						ValorFrete = q.ValorFrete,
						ValorUnitario = q.ValorUnitario
					}).ToList()

				}, 
				q=> q.IdPEProduto == vm.IdPEProduto && (q.TipoInsumo.Equals("N") || q.TipoInsumo.Equals("R"))
				,vm);

				

			}
			else
			{
				listaInsumos = _uowSciex.QueryStackSciex.PEInsumo.ListarPaginadoGrafo(w =>

				new PEInsumoVM()
				{
					IdPEInsumo = w.IdPEInsumo,
					IdPEProduto = w.IdPEProduto,
					CodigoDetalhe = w.CodigoDetalhe,
					CodigoInsumo = w.CodigoInsumo,
					CodigoNcm = w.CodigoNcm,
					CodigoUnidade = w.CodigoUnidade,
					DescricaoEspecificacaoTecnica = w.DescricaoEspecificacaoTecnica,
					DescricaoInsumo = w.DescricaoInsumo,
					DescricaoPartNumber = w.DescricaoPartNumber,
					TipoInsumo = w.TipoInsumo,
					ValorCoeficienteTecnico = w.ValorCoeficienteTecnico,
					ValorDolar = w.ValorDolar,
					ValorPercentualPerda = w.ValorPercentualPerda,
					ListaPEDetalheInsumo = w.ListaPEDetalheInsumo.Select(q => new PEDetalheInsumoVM()
					{
						IdPEInsumo = q.IdPEInsumo,
						IdMoeda = q.IdMoeda,
						IdPEDetalheInsumo = q.IdPEDetalheInsumo,
						CodigoPais = q.CodigoPais,
						NumeroSequencial = q.NumeroSequencial,
						Quantidade = q.Quantidade,
						ValorDolar = q.ValorDolar,
						ValorDolarCRF = q.ValorDolarCRF,
						ValorDolarFOB = q.ValorDolarFOB,
						ValorFrete = q.ValorFrete,
						ValorUnitario = q.ValorUnitario
					}).ToList()
				},
				q => q.IdPEProduto == vm.IdPEProduto && (q.TipoInsumo.Equals("P") || q.TipoInsumo.Equals("E")), vm);

				
			}

			foreach (var regInsumo in listaInsumos.Items)
			{
				regInsumo.QtdSomatorioDetalheInsumo = regInsumo.ListaPEDetalheInsumo?.Sum(q => q.Quantidade) ?? 0;
				regInsumo.QtdSomatorioDetalheInsumoFormatada = regInsumo.QtdSomatorioDetalheInsumo != null ? Convert.ToDecimal(regInsumo.QtdSomatorioDetalheInsumo).ToString("N5") : "0" ;

				regInsumo.ValorInsumo = regInsumo.ListaPEDetalheInsumo?.Sum(q => q.Quantidade * q.ValorUnitario) ?? 0;
				regInsumo.ValorInsumoFormatada = regInsumo.ValorInsumo != null ? Convert.ToDecimal(regInsumo.ValorInsumo).ToString("N5") : "0";

			}

			return listaInsumos;

		}
		
		public PagedItems<PlanoExportacaoVM> ListarPaginado(ConsultarPlanoExportacaoVM pagedFilter)
		{
			var dataInicio = pagedFilter.DataInicio == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataInicio.Value.Year, pagedFilter.DataInicio.Value.Month, pagedFilter.DataInicio.Value.Day);
			var dataFim = pagedFilter.DataFim == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataFim.Value.Year, pagedFilter.DataFim.Value.Month, pagedFilter.DataFim.Value.Day, 23, 59, 59);
			//var usuCpfCnpjEmpresaOuLogado = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoCpfCnpj;
			var usuCpfCnpjEmpresaOuLogado = "04817052000106";

			if (pagedFilter == null) { return new PagedItems<PlanoExportacaoVM>(); }

			var retornoConsulta = new PagedItems<PlanoExportacaoVM>();

			long digitoPlano = 0;
			int anoPlano = 0;

			if (!string.IsNullOrEmpty(pagedFilter.NumeroPlano))
			{
				digitoPlano = Convert.ToInt64(pagedFilter.NumeroPlano.Substring(0, 5));
				anoPlano = Convert.ToInt32(pagedFilter.NumeroPlano.Substring(6,4));
			}

			
			retornoConsulta = _uowSciex.QueryStackSciex.PlanoExportacao.ListarPaginado<PlanoExportacaoVM>(o =>
			(
				(digitoPlano == 0 || o.NumeroPlano == digitoPlano && o.AnoPlano == anoPlano)
				&&
				(pagedFilter.StatusPlano == null || o.Situacao == pagedFilter.StatusPlano)
				&&
				(
					(pagedFilter.DataInicio == null) || (dataInicio <= o.DataCadastro && o.DataCadastro <= dataFim)
				)
				&&
				(
					(o.Cnpj == usuCpfCnpjEmpresaOuLogado)
				)
			),
			pagedFilter);

			foreach (var registro in retornoConsulta.Items)
			{
				registro.DataCadastroFormatada = registro.DataCadastro == DateTime.MinValue ? DateTime.MinValue.ToShortDateString() : ((DateTime)registro.DataCadastro).ToShortDateString();
				registro.NumeroAnoPlanoFormatado = registro.NumeroPlano + "/" + registro.AnoPlano;
				registro.SituacaoString = registro.Situacao == 1 ? "EM ELABORAÇÃO"
												: registro.Situacao == 2 ? "ENTREGUE"
												: registro.Situacao == 3 ? "EM ANÁLISE"
												: "-";

				registro.TipoExportacaoString = registro.TipoExportacao == "AP" ? "APROVAÇÃO"
													: registro.TipoExportacao == "CO" ? "COMPROVAÇÃO"
													: "-";

				registro.TipoModalidadeString = registro.TipoModalidade == "S" ? "SUSPENSÃO"
													: "-";
			}

			return retornoConsulta;

		}

		public PEProdutoVM Selecionar(int idPEProduto)
		{
			var pe = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Selecionar<PEProdutoVM>(o => o.IdPEProduto == idPEProduto);
			return pe;
		}

		public PEProdutoVM Salvar(PEProdutoVM vm)
		{
			if (vm == null) { return null; }

			var peProduto = new PEProdutoEntity();
			if (vm.IdPEProduto > 0)
			{
				peProduto = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Selecionar(x => x.IdPEProduto == vm.IdPEProduto);

				peProduto = AutoMapper.Mapper.Map(vm, peProduto);
				vm.MensagemErro = "";
			}
			else
			{
				var objLEProduto = _uowSciex.QueryStackSciex.LEProduto.Selecionar(o => o.IdLe == vm.IdLEProduto);

				peProduto.CodigoProdutoExportacao = objLEProduto.CodigoProduto;
				peProduto.CodigoProdutoSuframa = objLEProduto.CodigoProdutoSuframa;
				peProduto.CodigoNCM = objLEProduto.CodigoNCM;
				peProduto.CodigoTipoProduto = objLEProduto.CodigoTipoProduto;
				peProduto.DescricaoModelo = objLEProduto.DescricaoModelo;
				peProduto.CodigoUnidade = objLEProduto.CodigoUnidadeMedida;
				peProduto.IdPlanoExportacao = vm.IdPlanoExportacao;

				var peProdutoValida = _uowSciex.QueryStackSciex.PlanoExportacaoProduto
					.Listar(o => o.IdPlanoExportacao == peProduto.IdPlanoExportacao && o.CodigoProdutoExportacao == objLEProduto.CodigoProduto);

				vm.MensagemErro = "";
				if (peProdutoValida.Count != 0)
				{
					vm.MensagemErro = "O produto selecionado já foi cadastrado.";
					return vm;
				}
				_uowSciex.CommandStackSciex.PlanoExportacaoProduto.Salvar(peProduto, true);
				_uowSciex.CommandStackSciex.Save();
			}

			return vm;
		}

		public bool AtualizarInsumo(PEInsumoVM vm)
		{
			if (vm == null || vm.IdPEInsumo == 0) 
				return false;

			var regInsumo = _uowSciex.QueryStackSciex.PEInsumo.Selecionar(q => q.IdPEInsumo == vm.IdPEInsumo);

			regInsumo.DescricaoPartNumber = vm.DescricaoPartNumber;
			regInsumo.ValorPercentualPerda = vm.ValorPercentualPerda;

			_uowSciex.CommandStackSciex.PEInsumo.Salvar(regInsumo);
			_uowSciex.CommandStackSciex.Save();

			return true;
		}

		public string FormatarQtdMax(PEInsumoVM vm)
		{
			var qtdMaxima = vm.QtdProduto.Value * vm.ValorCoeficienteTecnico.Value;

			decimal retorno = qtdMaxima + (qtdMaxima * (vm.ValorPercentualPerda.Value / 100));
			return retorno.ToString("N5");
		}

		public PRCInsumoVM SelecionarPrcInsumo(int idPrcInsumo)
		{
			if (idPrcInsumo == 0)
				return new PRCInsumoVM();

			var regInsumo = _uowSciex.QueryStackSciex.PRCInsumo.SelecionarGrafo(w => new PRCInsumoVM()
			{
				IdInsumo = w.IdInsumo,
				IdPrcProduto = w.IdPrcProduto,
				CodigoInsumo = w.CodigoInsumo,
				CodigoUnidade = w.CodigoUnidade,
				TipoInsumo = w.TipoInsumo,
				CodigoNCM = w.CodigoNCM,
				DescricaoInsumo = w.DescricaoInsumo,
				ValorPercentualPerda = w.ValorPercentualPerda != null ? w.ValorPercentualPerda : 0,
				CodigoDetalhe = w.CodigoDetalhe,
				DescricaoPartNumber = w.DescricaoPartNumber,
				DescricaoEspecificacaoTecnica = w.DescricaoEspecificacaoTecnica,
				ValorCoeficienteTecnico = w.ValorCoeficienteTecnico,
				ValorDolarAprovado = w.ValorDolarAprovado,
				QuantidadeAprovado = w.QuantidadeAprovado,
				ValorNacionalAprovado = w.ValorNacionalAprovado,
				ValorDolarFOBAprovado = w.ValorDolarFOBAprovado,
				ValorDolarCFRAprovado = w.ValorDolarCFRAprovado,
				ValorFreteAprovado = w.ValorFreteAprovado,
				ValorDolarComp = w.ValorDolarComp,
				QuantidadeComp = w.QuantidadeComp,
				ValorDolarSaldo = w.ValorDolarSaldo,
				QuantidadeSaldo = w.QuantidadeSaldo,
				DescricaoTipoInsumo = w.TipoInsumo == "N" ? "Nacional" : w.TipoInsumo == "R" ? "Regional" : "Padrão",
				Produto = new PRCProdutoVM()
				{
					IdProduto = w.PrcProduto.IdProduto,
					IdProcesso = w.PrcProduto.IdProcesso,
					CodigoProdutoExportacao = w.PrcProduto.CodigoProdutoExportacao,
					CodigoProdutoSuframa = w.PrcProduto.CodigoProdutoSuframa,
					CodigoNCM = w.PrcProduto.CodigoNCM,
					TipoProduto = w.PrcProduto.TipoProduto,
					DescricaoModelo = w.PrcProduto.DescricaoModelo,
					QuantidadeAprovado = w.PrcProduto.QuantidadeAprovado,
					CodigoUnidade = w.PrcProduto.CodigoUnidade,
					ValorDolarAprovado = w.PrcProduto.ValorDolarAprovado,
					ValorFluxoCaixa = w.PrcProduto.ValorFluxoCaixa
				},
				ListaDetalheInsumos = w.ListaDetalheInsumos.Select(q => new PRCDetalheInsumoVM()
				{
					IdPrcInsumo = q.IdPrcInsumo,
					IdMoeda = q.IdMoeda,
					IdDetalheInsumo = q.IdDetalheInsumo,
					CodigoPais = q.CodigoPais,
					NumeroSequencial = q.NumeroSequencial,
					Quantidade = q.Quantidade,
					ValorDolar = q.ValorDolar,
					ValorDolarCFR = q.ValorDolarCFR,
					ValorDolarFOB = q.ValorDolarFOB,
					ValorFrete = q.ValorFrete,
					ValorUnitario = q.ValorUnitario
				}
				).ToList()

			},
			q => q.IdInsumo == idPrcInsumo);

			var undMed = _uowSciex.QueryStackSciex.ViewUnidadeMedida.Selecionar(o => o.CodigoUnidadeMedida == regInsumo.CodigoUnidade);
			regInsumo.DescCodigoUnidade = undMed != null ? undMed.CodigoUnidadeMedida.ToString("D3") + " | " + undMed.Descricao : "-";

			var qtdProduto = regInsumo?.Produto?.QuantidadeAprovado ?? 0;
			var coefTec = regInsumo?.ValorCoeficienteTecnico ?? 0;
			var percPerda = regInsumo?.ValorPercentualPerda ?? 0;

			regInsumo.QtdMaxInsumo = qtdProduto * coefTec;
			regInsumo.PercentualPerda = percPerda;
			regInsumo.QtdMaxInsumo = regInsumo.QtdMaxInsumo + (regInsumo.QtdMaxInsumo * (percPerda / 100));

			var _lisDetalhesInsumosEntity = _uowSciex.QueryStackSciex.PRCDetalheInsumo.Listar(o => o.IdPrcInsumo == regInsumo.IdInsumo);
			if (_lisDetalhesInsumosEntity.Count > 0)
				regInsumo.ValoresTotais = _lisDetalhesInsumosEntity.Sum(o => o.Quantidade);
					   					   
			return regInsumo;
		}

		public DadosProcessoDetalhesInsumosVM ListarDetalhePorIdProcessoInsumo(ListarProcessoInsumosNacionalImportadosVM vm)
		{
			if (vm == null || vm.IdProcessoInsumo == 0)
				return null;

			string filtroPosterior = null;

			if (vm.Sort != null)
			{
				if (vm.Sort.Equals("ValorTotal") || vm.Sort.Equals("CodigoPais") || vm.Sort.Equals("CodigoDescricaoMoeda"))
				{
					filtroPosterior = vm.Sort;
					vm.Sort = null;
				}
			}
			DadosProcessoDetalhesInsumosVM dados = new DadosProcessoDetalhesInsumosVM();
			PagedItems<PRCDetalheInsumoVM> listaProcessoDetalheInsumo;
			if (vm.isQuadroNacional)
			{

				listaProcessoDetalheInsumo = _uowSciex.QueryStackSciex.PRCDetalheInsumo.ListarPaginadoGrafo(q => new PRCDetalheInsumoVM()
				{
					IdDetalheInsumo = q.IdDetalheInsumo,
					IdPrcInsumo = q.IdPrcInsumo,
					IdMoeda = q.IdMoeda,
					NumeroSequencial = q.NumeroSequencial,
					CodigoPais = q.CodigoPais,
					Quantidade = q.Quantidade,
					ValorUnitario = q.ValorUnitario,
					ValorFrete = q.ValorFrete,
					ValorDolar = q.ValorDolar,
					ValorUnitarioCFR = q.ValorUnitarioCFR,
					ValorDolarCFR = q.ValorDolarCFR,
					ValorDolarFOB = q.ValorDolarFOB,
					PrcInsumo = new PRCInsumoVM()
					{
						IdInsumo = q.PrcInsumo.IdInsumo,
						StatusInsumo = q.PrcInsumo.StatusInsumo,
						StatusInsumoNovo = q.PrcInsumo.StatusInsumoNovo,
						IdPrcProduto = q.PrcInsumo.IdPrcProduto,
						CodigoInsumo = q.PrcInsumo.CodigoInsumo,
						CodigoUnidade = q.PrcInsumo.CodigoUnidade,
						TipoInsumo = q.PrcInsumo.TipoInsumo,
						CodigoNCM = q.PrcInsumo.CodigoNCM,
						DescricaoInsumo = q.PrcInsumo.DescricaoInsumo,
						ValorPercentualPerda = q.PrcInsumo.ValorPercentualPerda,
						CodigoDetalhe = q.PrcInsumo.CodigoDetalhe,
						DescricaoPartNumber = q.PrcInsumo.DescricaoPartNumber,
						DescricaoEspecificacaoTecnica = q.PrcInsumo.DescricaoEspecificacaoTecnica,
						ValorCoeficienteTecnico = q.PrcInsumo.ValorCoeficienteTecnico,
						ValorDolarAprovado = q.PrcInsumo.ValorDolarAprovado,
						QuantidadeAprovado = q.PrcInsumo.QuantidadeAprovado,
						ValorNacionalAprovado = q.PrcInsumo.ValorNacionalAprovado,
						ValorDolarFOBAprovado = q.PrcInsumo.ValorDolarFOBAprovado,
						ValorDolarCFRAprovado = q.PrcInsumo.ValorDolarCFRAprovado,
						ValorFreteAprovado = q.PrcInsumo.ValorFreteAprovado,
						ValorDolarComp = q.PrcInsumo.ValorDolarComp,
						QuantidadeComp = q.PrcInsumo.QuantidadeComp,
						ValorDolarSaldo = q.PrcInsumo.ValorDolarSaldo,
						QuantidadeSaldo = q.PrcInsumo.QuantidadeSaldo,
						DescricaoTipoInsumo = q.PrcInsumo.TipoInsumo == "N" ? "Nacional" : q.PrcInsumo.TipoInsumo == "R" ? "Regional" : "Padrão",
						Produto = new PRCProdutoVM()
						{
							IdProduto = q.PrcInsumo.PrcProduto.IdProduto,
							IdProcesso = q.PrcInsumo.PrcProduto.IdProcesso,
							CodigoProdutoExportacao = q.PrcInsumo.PrcProduto.CodigoProdutoExportacao,
							CodigoProdutoSuframa = q.PrcInsumo.PrcProduto.CodigoProdutoSuframa,
							CodigoNCM = q.PrcInsumo.PrcProduto.CodigoNCM,
							TipoProduto = q.PrcInsumo.PrcProduto.TipoProduto,
							DescricaoModelo = q.PrcInsumo.PrcProduto.DescricaoModelo,
							QuantidadeAprovado = q.PrcInsumo.PrcProduto.QuantidadeAprovado,
							CodigoUnidade = q.PrcInsumo.PrcProduto.CodigoUnidade,
							ValorDolarAprovado = q.PrcInsumo.PrcProduto.ValorDolarAprovado,
							ValorFluxoCaixa = q.PrcInsumo.PrcProduto.ValorFluxoCaixa
						}
					}
				}
				,
				q =>
				q.IdPrcInsumo == vm.IdProcessoInsumo
				&&
				(q.PrcInsumo.TipoInsumo.Equals("N") || q.PrcInsumo.TipoInsumo.Equals("R"))
				,
				vm
				);

				foreach (var item in listaProcessoDetalheInsumo.Items)
				{
					var valorUnitario = item.ValorUnitario != null ? item.ValorUnitario : 0;
					item.ValorTotal = item.Quantidade * valorUnitario;
				}

			}
			else
			{

				listaProcessoDetalheInsumo = _uowSciex.QueryStackSciex.PRCDetalheInsumo.ListarPaginadoGrafo(q => new PRCDetalheInsumoVM()
				{
					IdDetalheInsumo = q.IdDetalheInsumo,
					IdPrcInsumo = q.IdPrcInsumo,
					IdMoeda = q.IdMoeda,
					NumeroSequencial = q.NumeroSequencial,
					CodigoPais = q.CodigoPais,
					Quantidade = q.Quantidade,
					ValorUnitario = q.ValorUnitario,
					ValorFrete = q.ValorFrete,
					ValorDolar = q.ValorDolar,
					ValorUnitarioCFR = q.ValorUnitarioCFR,
					ValorDolarCFR = q.ValorDolarCFR,
					ValorDolarFOB = q.ValorDolarFOB,
					PrcInsumo = new PRCInsumoVM()
					{
						IdInsumo = q.PrcInsumo.IdInsumo,
						IdPrcProduto = q.PrcInsumo.IdPrcProduto,
						CodigoInsumo = q.PrcInsumo.CodigoInsumo,
						CodigoUnidade = q.PrcInsumo.CodigoUnidade,
						TipoInsumo = q.PrcInsumo.TipoInsumo,
						CodigoNCM = q.PrcInsumo.CodigoNCM,
						DescricaoInsumo = q.PrcInsumo.DescricaoInsumo,
						ValorPercentualPerda = q.PrcInsumo.ValorPercentualPerda,
						CodigoDetalhe = q.PrcInsumo.CodigoDetalhe,
						DescricaoPartNumber = q.PrcInsumo.DescricaoPartNumber,
						DescricaoEspecificacaoTecnica = q.PrcInsumo.DescricaoEspecificacaoTecnica,
						ValorCoeficienteTecnico = q.PrcInsumo.ValorCoeficienteTecnico,
						ValorDolarAprovado = q.PrcInsumo.ValorDolarAprovado,
						QuantidadeAprovado = q.PrcInsumo.QuantidadeAprovado,
						ValorNacionalAprovado = q.PrcInsumo.ValorNacionalAprovado,
						ValorDolarFOBAprovado = q.PrcInsumo.ValorDolarFOBAprovado,
						ValorDolarCFRAprovado = q.PrcInsumo.ValorDolarCFRAprovado,
						ValorFreteAprovado = q.PrcInsumo.ValorFreteAprovado,
						ValorDolarComp = q.PrcInsumo.ValorDolarComp,
						QuantidadeComp = q.PrcInsumo.QuantidadeComp,
						ValorDolarSaldo = q.PrcInsumo.ValorDolarSaldo,
						QuantidadeSaldo = q.PrcInsumo.QuantidadeSaldo,
						DescricaoTipoInsumo = q.PrcInsumo.TipoInsumo == "N" ? "Nacional" : q.PrcInsumo.TipoInsumo == "R" ? "Regional" : "Padrão",
						Produto = new PRCProdutoVM()
						{
							IdProduto = q.PrcInsumo.PrcProduto.IdProduto,
							IdProcesso = q.PrcInsumo.PrcProduto.IdProcesso,
							CodigoProdutoExportacao = q.PrcInsumo.PrcProduto.CodigoProdutoExportacao,
							CodigoProdutoSuframa = q.PrcInsumo.PrcProduto.CodigoProdutoSuframa,
							CodigoNCM = q.PrcInsumo.PrcProduto.CodigoNCM,
							TipoProduto = q.PrcInsumo.PrcProduto.TipoProduto,
							DescricaoModelo = q.PrcInsumo.PrcProduto.DescricaoModelo,
							QuantidadeAprovado = q.PrcInsumo.PrcProduto.QuantidadeAprovado,
							CodigoUnidade = q.PrcInsumo.PrcProduto.CodigoUnidade,
							ValorDolarAprovado = q.PrcInsumo.PrcProduto.ValorDolarAprovado,
							ValorFluxoCaixa = q.PrcInsumo.PrcProduto.ValorFluxoCaixa
						}
					}
				}
				,
				q =>
				q.IdPrcInsumo == vm.IdProcessoInsumo
				&&
				(q.PrcInsumo.TipoInsumo.Equals("P") || q.PrcInsumo.TipoInsumo.Equals("E"))
				,
				vm
				);

			}

			foreach (var itemInsumo in listaProcessoDetalheInsumo.Items)
			{
				string codigoPais = Convert.ToInt32(itemInsumo.CodigoPais).ToString("D3");

				if (codigoPais != "000")
				{
					var pais = _uowSciex.QueryStackSciex.ViewPais.Selecionar(o => o.CodigoPais == codigoPais);						
					itemInsumo.DescricaoPais = pais.Descricao;
				}

				#region Validação se Existe Solicitação de Alteracao para o Processo.
				var emElaboracao = 1;
				var solicitaAlteracaoEntity = _uowSciex.QueryStackSciex.PRCSolicitacaoAlteracao.Selecionar(o => o.IdProcesso == itemInsumo.PrcInsumo.Produto.IdProcesso 
																												&& o.Status == emElaboracao);
				itemInsumo.ExisteSolicitacaoDeAlteracao = (solicitaAlteracaoEntity == null) ? false : true;
				#endregion

				#region Validação se Existe Solicitação de Alteracao de Insumo em Andamento para este insumo.
				int entregue = 2;
				int emAnalise = 3;
				var listaStatusValidacao = new List<int?>(){ entregue, emAnalise };
				var listaSolicitacoes = _uowSciex.QueryStackSciex.PRCSolicitacaoAlteracao.
																Listar(o => o.IdProcesso == itemInsumo.PrcInsumo.Produto.IdProcesso
																												&& listaStatusValidacao.Contains(o.Status));
				var listaIdInsumosJaSolicitados = new List<int>();
				foreach (var solicitacao in listaSolicitacoes)
				{
					foreach (var detalheSolic in solicitacao.ListaSolicDetalhe)
					{
						listaIdInsumosJaSolicitados.Add(detalheSolic.IdInsumo);
					}
				}

				int emAlteracao = 2;
				var regInsumoJaSolicitado = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(o => o.CodigoInsumo == itemInsumo.PrcInsumo.CodigoInsumo
																						 && o.IdPrcProduto == itemInsumo.PrcInsumo.IdPrcProduto
																						 && o.StatusInsumo == emAlteracao);

				itemInsumo.ExisteInsumoJaSolicitadoAlteracao = regInsumoJaSolicitado != null && listaIdInsumosJaSolicitados.Contains(regInsumoJaSolicitado.IdInsumo) ? true : false;
				#endregion

				#region VERIFICA EXISTENCIA DENTRO DE PRC_SOLIC_DETALHE
								
				var _existeInsumoCopia = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar<PRCInsumoTableColunsVM>(o => o.CodigoInsumo == itemInsumo.PrcInsumo.CodigoInsumo
																												  && o.IdPrcProduto == itemInsumo.PrcInsumo.IdPrcProduto
																												  && o.StatusInsumo == 2); //Em Alteração
				if(_existeInsumoCopia != null)
				{
					var listaSolicDetalhe = _uowSciex.QueryStackSciex.PRCSolicDetalhe.Listar(o => o.IdInsumo == _existeInsumoCopia.IdInsumo);

					if(listaSolicDetalhe.Count > 0)
					{
						foreach(var _itemSolicDetalhe in listaSolicDetalhe)
						{
							switch (_itemSolicDetalhe.IdTipoSolicitacao)
							{
								case (int) EnumTipoAlteracaoInsumo.PAIS :
										itemInsumo.FlagExisteAlteracaoPais = true;
										itemInsumo.IdSolicDetalhePais = _itemSolicDetalhe.Id;
									break;
								case (int) EnumTipoAlteracaoInsumo.MOEDA:
										itemInsumo.FlagExisteAlteracaoMoeda = true;
										itemInsumo.IdSolicDetalheMoeda = _itemSolicDetalhe.Id;
									break;
								case (int) EnumTipoAlteracaoInsumo.QUANTIDADE_COEF_TECNICO:
										itemInsumo.FlagExisteAlteracaoQuantidade = true;
										itemInsumo.IdSolicDetalheQuantidade = _itemSolicDetalhe.Id;
									break;
								case (int) EnumTipoAlteracaoInsumo.VALOR_UNITARIO:
										itemInsumo.FlagExisteAlteracaoValorUnitario = true;
										itemInsumo.IdSolicDetalheVlrUnitario = _itemSolicDetalhe.Id;
									break;
								case (int) EnumTipoAlteracaoInsumo.VALOR_FRETE:
										itemInsumo.FlagExisteAlteracaoValorFrete = true;
										itemInsumo.IdSolicDetalheVlrFrete = _itemSolicDetalhe.Id;
									break;
							}							
						}
					}
				}
				
				#endregion
				
				var regMoeda = _uowSciex.QueryStackSciex.Moeda.Selecionar(o => o.IdMoeda == itemInsumo.IdMoeda);

				itemInsumo.CodigoDescricaoMoeda = regMoeda.CodigoMoeda + " | " + regMoeda.Descricao;
			}

			if (!string.IsNullOrEmpty(filtroPosterior))
			{
				if (filtroPosterior.Equals("ValorTotal"))
				{
					if (!vm.Reverse)
					{
						listaProcessoDetalheInsumo.Items = listaProcessoDetalheInsumo.Items.OrderBy(q => q.ValorTotal).ToList();
					}
					else
					{
						listaProcessoDetalheInsumo.Items = listaProcessoDetalheInsumo.Items.OrderByDescending(q => q.ValorTotal).ToList();
					}
				}
				else if (filtroPosterior.Equals("CodigoPais"))
				{
					if (!vm.Reverse)
					{
						listaProcessoDetalheInsumo.Items = listaProcessoDetalheInsumo.Items.OrderBy(q => q.CodigoPais).ToList();
					}
					else
					{
						listaProcessoDetalheInsumo.Items = listaProcessoDetalheInsumo.Items.OrderByDescending(q => q.CodigoPais).ToList();
					}
				}
				else if (filtroPosterior.Equals("CodigoDescricaoMoeda"))
				{
					if (!vm.Reverse)
					{
						listaProcessoDetalheInsumo.Items = listaProcessoDetalheInsumo.Items.OrderBy(q => q.CodigoDescricaoMoeda).ToList();
					}
					else
					{
						listaProcessoDetalheInsumo.Items = listaProcessoDetalheInsumo.Items.OrderByDescending(q => q.CodigoDescricaoMoeda).ToList();
					}
				}
			}

			dados.listaProcessoDetalhesInsumos = listaProcessoDetalheInsumo;

			return dados;
		}
			   
		public List<PRCInsumoVM> BuscarValoresAtuais(BuscarValoresInsumoVM parametros)
		{
			List<PRCInsumoVM> listaValores = new List<PRCInsumoVM>();

			var insumoStatusUm = _uowSciex.QueryStackSciex.PRCInsumo.SelecionarGrafo(w => new PRCInsumoVM()
			{
				IdInsumo = w.IdInsumo,
				StatusInsumo = w.StatusInsumo,
				IdPrcProduto = w.IdPrcProduto,
				CodigoInsumo = w.CodigoInsumo,
				CodigoUnidade = w.CodigoUnidade,
				TipoInsumo = w.TipoInsumo,
				CodigoNCM = w.CodigoNCM,
				DescricaoInsumo = w.DescricaoInsumo,
				ValorPercentualPerda = w.ValorPercentualPerda != null ? w.ValorPercentualPerda : 0,
				CodigoDetalhe = w.CodigoDetalhe,
				DescricaoPartNumber = w.DescricaoPartNumber,
				DescricaoEspecificacaoTecnica = w.DescricaoEspecificacaoTecnica,
				ValorCoeficienteTecnico = w.ValorCoeficienteTecnico,
				ValorDolarAprovado = w.ValorDolarAprovado,
				QuantidadeAprovado = w.QuantidadeAprovado,
				ValorNacionalAprovado = w.ValorNacionalAprovado,
				ValorDolarFOBAprovado = w.ValorDolarFOBAprovado,
				ValorDolarCFRAprovado = w.ValorDolarCFRAprovado,
				ValorFreteAprovado = w.ValorFreteAprovado,
				ValorDolarComp = w.ValorDolarComp,
				QuantidadeComp = w.QuantidadeComp,
				ValorDolarSaldo = w.ValorDolarSaldo,
				QuantidadeSaldo = w.QuantidadeSaldo,
				DescricaoTipoInsumo = w.TipoInsumo == "N" ? "Nacional" : w.TipoInsumo == "R" ? "Regional" : "Padrão",
				Produto = new PRCProdutoVM()
				{
					IdProduto = w.PrcProduto.IdProduto,
					IdProcesso = w.PrcProduto.IdProcesso,
					CodigoProdutoExportacao = w.PrcProduto.CodigoProdutoExportacao,
					CodigoProdutoSuframa = w.PrcProduto.CodigoProdutoSuframa,
					CodigoNCM = w.PrcProduto.CodigoNCM,
					TipoProduto = w.PrcProduto.TipoProduto,
					DescricaoModelo = w.PrcProduto.DescricaoModelo,
					QuantidadeAprovado = w.PrcProduto.QuantidadeAprovado,
					CodigoUnidade = w.PrcProduto.CodigoUnidade,
					ValorDolarAprovado = w.PrcProduto.ValorDolarAprovado,
					ValorFluxoCaixa = w.PrcProduto.ValorFluxoCaixa
				},
				ListaDetalheInsumos = w.ListaDetalheInsumos.Select(q => new PRCDetalheInsumoVM()
				{
					IdPrcInsumo = q.IdPrcInsumo,
					IdMoeda = q.IdMoeda,
					IdDetalheInsumo = q.IdDetalheInsumo,
					CodigoPais = q.CodigoPais,
					NumeroSequencial = q.NumeroSequencial,
					Quantidade = q.Quantidade,
					ValorDolar = q.ValorDolar,
					ValorDolarCFR = q.ValorDolarCFR,
					ValorDolarFOB = q.ValorDolarFOB,
					ValorFrete = q.ValorFrete,
					ValorUnitario = q.ValorUnitario,
					Moeda = new MoedaVM()
					{
						CodigoMoeda = q.Moeda.CodigoMoeda,
						Descricao = q.Moeda.Descricao,
						IdMoeda = q.Moeda.IdMoeda
					}
				}
				).ToList(),
				ListaSolicDetalhe = w.PRCSolicDetalhe.Select(q => new PRCSolicDetalheVM()
				{
					Id = q.Id,
					Status = q.Status,
					DescricaoDe = q.DescricaoDe,
					DescricaoPara = q.DescricaoPara,
					IdInsumo = q.IdInsumo,
					IdDetalheInsumo = q.IdDetalheInsumo,
					IdSolicitacaoAlteracao = q.IdSolicitacaoAlteracao,
					IdTipoSolicitacao = q.IdTipoSolicitacao,
					DescricaoTipoAlteracao = q.TipoSolicAlteracao.Descricao
				}).ToList(),

			},
			q => q.CodigoInsumo == parametros.codigoInsumo && q.StatusInsumo == 1 && q.IdPrcProduto == parametros.idProduto);

			listaValores.Add(insumoStatusUm);

			var insumoStatusDois = _uowSciex.QueryStackSciex.PRCInsumo.SelecionarGrafo(w => new PRCInsumoVM()
			{
				IdInsumo = w.IdInsumo,
				IdPrcProduto = w.IdPrcProduto,
				StatusInsumo = w.StatusInsumo,
				CodigoInsumo = w.CodigoInsumo,
				CodigoUnidade = w.CodigoUnidade,
				TipoInsumo = w.TipoInsumo,
				CodigoNCM = w.CodigoNCM,
				DescricaoInsumo = w.DescricaoInsumo,
				ValorPercentualPerda = w.ValorPercentualPerda != null ? w.ValorPercentualPerda : 0,
				CodigoDetalhe = w.CodigoDetalhe,
				DescricaoPartNumber = w.DescricaoPartNumber,
				DescricaoEspecificacaoTecnica = w.DescricaoEspecificacaoTecnica,
				ValorCoeficienteTecnico = w.ValorCoeficienteTecnico,
				ValorDolarAprovado = w.ValorDolarAprovado,
				QuantidadeAprovado = w.QuantidadeAprovado,
				ValorNacionalAprovado = w.ValorNacionalAprovado,
				ValorDolarFOBAprovado = w.ValorDolarFOBAprovado,
				ValorDolarCFRAprovado = w.ValorDolarCFRAprovado,
				ValorFreteAprovado = w.ValorFreteAprovado,
				ValorDolarComp = w.ValorDolarComp,
				QuantidadeComp = w.QuantidadeComp,
				ValorDolarSaldo = w.ValorDolarSaldo,
				QuantidadeSaldo = w.QuantidadeSaldo,
				DescricaoTipoInsumo = w.TipoInsumo == "N" ? "Nacional" : w.TipoInsumo == "R" ? "Regional" : "Padrão",
				Produto = new PRCProdutoVM()
				{
					IdProduto = w.PrcProduto.IdProduto,
					IdProcesso = w.PrcProduto.IdProcesso,
					CodigoProdutoExportacao = w.PrcProduto.CodigoProdutoExportacao,
					CodigoProdutoSuframa = w.PrcProduto.CodigoProdutoSuframa,
					CodigoNCM = w.PrcProduto.CodigoNCM,
					TipoProduto = w.PrcProduto.TipoProduto,
					DescricaoModelo = w.PrcProduto.DescricaoModelo,
					QuantidadeAprovado = w.PrcProduto.QuantidadeAprovado,
					CodigoUnidade = w.PrcProduto.CodigoUnidade,
					ValorDolarAprovado = w.PrcProduto.ValorDolarAprovado,
					ValorFluxoCaixa = w.PrcProduto.ValorFluxoCaixa
				},
				ListaDetalheInsumos = w.ListaDetalheInsumos.Select(q => new PRCDetalheInsumoVM()
				{
					IdPrcInsumo = q.IdPrcInsumo,
					IdMoeda = q.IdMoeda,
					IdDetalheInsumo = q.IdDetalheInsumo,
					CodigoPais = q.CodigoPais,
					NumeroSequencial = q.NumeroSequencial,
					Quantidade = q.Quantidade,
					ValorDolar = q.ValorDolar,
					ValorDolarCFR = q.ValorDolarCFR,
					ValorDolarFOB = q.ValorDolarFOB,
					ValorFrete = q.ValorFrete,
					ValorUnitario = q.ValorUnitario,
					Moeda = new MoedaVM()
					{
						CodigoMoeda = q.Moeda.CodigoMoeda,
						Descricao = q.Moeda.Descricao,
						IdMoeda = q.Moeda.IdMoeda
					}
				}
				).ToList(),
				ListaSolicDetalhe = w.PRCSolicDetalhe.Select(q => new PRCSolicDetalheVM()
				{
					Id = q.Id,
					Status = q.Status,
					DescricaoDe = q.DescricaoDe,
					DescricaoPara = q.DescricaoPara,
					IdInsumo = q.IdInsumo,
					IdDetalheInsumo = q.IdDetalheInsumo,
					IdSolicitacaoAlteracao = q.IdSolicitacaoAlteracao,
					IdTipoSolicitacao = q.IdTipoSolicitacao,
					DescricaoTipoAlteracao = q.TipoSolicAlteracao.Descricao
				}).ToList()

			},
			q => q.CodigoInsumo == parametros.codigoInsumo && q.StatusInsumo == 2 && q.IdPrcProduto == parametros.idProduto);

			listaValores.Add(insumoStatusDois);
			
			return listaValores;

		}

		public SolicitacoesAlteracaoVM CalculaParidade(MoedaVM objeto) {
			return new SolicitacoesAlteracaoVM()
			{
				ValorParidade = CalcularParidadeBll.CalcularFatorConversao(objeto.CodigoMoeda, _uowSciex)
			};
		}

		enum EnumStatusSalvarDetalhe
		{
			ERRO = 1,
			PAIS_JA_CADASTRADO = 2,
			SUCESSO = 3,
			ACIMA_LIMITE = 4
		}

		public bool SalvarNovoDetalheAdicional(SalvarDetalhePRCInsumoVM vm)
		{
			if (vm == null || vm.IdPRCInsumo == 0) { return false; }
			bool IsSucesso = true;

			var prcInsumoEntity = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(q => q.IdInsumo == vm.IdPRCInsumo);

			prcInsumoEntity.DescricaoPartNumber = vm.DescricaoPartNumber;
			prcInsumoEntity.ValorPercentualPerda = vm.ValorPercentualPerda;
			_uowSciex.CommandStackSciex.PRCInsumo.Salvar(prcInsumoEntity);
			_uowSciex.CommandStackSciex.Save();

			var _listPRCDetalheInsumo = _uowSciex.QueryStackSciex.PRCDetalheInsumo.Listar(o => o.IdPrcInsumo == vm.IdPRCInsumo);

			try
			{
				if (_listPRCDetalheInsumo.Count > 0)
				{
					prcInsumoEntity.ValorAdicional = _listPRCDetalheInsumo.Sum(o => (o.ValorDolarFOB == null) ? 0 : o.ValorDolarFOB);
					prcInsumoEntity.ValorAdicionalFrete = _listPRCDetalheInsumo.Sum(o => (o.ValorFrete == null) ? 0 : o.ValorFrete);
					prcInsumoEntity.QuantidadeAdicional = _listPRCDetalheInsumo.Sum(o => o.Quantidade);
					prcInsumoEntity.ValorDolarSaldo = _listPRCDetalheInsumo.Sum(o => o.ValorDolar == null ? 0 : o.ValorDolar);
					prcInsumoEntity.QuantidadeSaldo = _listPRCDetalheInsumo.Sum(o => o.Quantidade);
					prcInsumoEntity.ValorDolarUnitario = _listPRCDetalheInsumo.Sum(o => (o.ValorDolarFOB == null)
																							? 0
																							: o.ValorDolarFOB) / _listPRCDetalheInsumo.Sum(o => o.Quantidade);

					prcInsumoEntity.ValorDolarUnitarioCrf = _listPRCDetalheInsumo.Sum(o => (o.ValorDolarCFR == null)
																							? 0
																							: o.ValorDolarCFR) / _listPRCDetalheInsumo.Sum(o => o.Quantidade);

					_uowSciex.CommandStackSciex.PRCInsumo.Salvar(prcInsumoEntity);
					_uowSciex.CommandStackSciex.Save();
				}

				return IsSucesso;
			}
			catch (Exception e)
			{
				return !IsSucesso;
			}

		}

		enum EnumTipoAlteracaoInsumo
		{
			INCLUSAO_INSUMO = 1,
			TRANSFERENCIA_SALDO_INSUMO = 2,
			PAIS = 3,
			MOEDA = 4,
			QUANTIDADE_COEF_TECNICO = 5,
			VALOR_UNITARIO = 6,
			VALOR_FRETE = 7
		}


		public IEnumerable<object> ListarChave(CodigoDescricaoInsumoDropDownVM view)
		{

			if (string.IsNullOrEmpty(view.Descricao) && view.Id == 0)
			{
				return new List<object>();
			}

			view.Descricao = view.Descricao != null ? view.Descricao.TrimStart('0') : null;

			var ncm = _uowSciex.QueryStackSciex.PRCInsumo
				.Listar()
				.Where(o =>
						(	string.IsNullOrEmpty(view.Descricao) 
							|| 
							o.DescricaoInsumo.ToLower().Contains(view.Descricao.ToLower()) 
							|| o.CodigoInsumo.ToString().Contains(view.Descricao.ToString())
						)
						&&
						(view.Id == 0 
							|| 
							o.CodigoInsumo == view.Id
						)
						&&
						(o.IdPrcProduto == view.IdProcessoProduto)
						&&
						(o.TipoInsumo.Equals("P") || o.TipoInsumo.Equals("E"))
						&&
						(o.StatusInsumoNovo == 1 || o.StatusInsumoNovo == 0 || o.StatusInsumoNovo == null)
						&&
						(o.StatusInsumo != 2 || o.StatusInsumoNovo == 1)
				
					   )
				.Select(
					s => new
					{
						id = s.CodigoInsumo,
						text = s.CodigoInsumo + " | " + s.DescricaoInsumo
					})
				.Distinct().OrderBy(q=> q.id);

			return ncm;
		}

		public List<PRCHistoricoInsumoVM> SelecionarRelatorio(PRCHistoricoInsumoVM parametros)
		{

			List<PRCHistoricoInsumoVM> Listahistorico = null;

			if (parametros.IsInsumo)
			{
				var regInsumo = _uowSciex.QueryStackSciex.PRCInsumo.Selecionar(o => o.IdInsumo == parametros.Id);

				 Listahistorico = _uowSciex.QueryStackSciex.PRCHistoricoInsumo.ListarGrafo(o => new PRCHistoricoInsumoVM()
				{

					IdPRCHistoricoInsumo = o.IdPRCHistoricoInsumo,
					CodigoInsumo = o.CodigoInsumo,
					IdProduto = o.IdProduto,
					IdSolicitacaoAlteracao = o.IdSolicitacaoAlteracao,
					DataHistorico = o.DataHistorico,
					NomeResponsavel = o.NomeResponsavel,
					DescricaoInsumo = o.DescricaoInsumo,
					DescricaoEmpresa = o.DescricaoEmpresa,
					DescricaoProcesso = o.DescricaoProcesso,
					DescricaoSolicitacao = o.DescricaoSolicitacao,
					DescricaoProduto = o.DescricaoProduto

				}, o => o.CodigoInsumo == regInsumo.CodigoInsumo.ToString()).ToList();
			}
			else
			{

				 Listahistorico = _uowSciex.QueryStackSciex.PRCHistoricoInsumo.ListarGrafo(o => new PRCHistoricoInsumoVM()
				{

					IdPRCHistoricoInsumo = o.IdPRCHistoricoInsumo,
					CodigoInsumo = o.CodigoInsumo,
					IdProduto = o.IdProduto,
					IdSolicitacaoAlteracao = o.IdSolicitacaoAlteracao,
					DataHistorico = o.DataHistorico,
					NomeResponsavel = o.NomeResponsavel,
					DescricaoInsumo = o.DescricaoInsumo,
					DescricaoEmpresa = o.DescricaoEmpresa,
					DescricaoProcesso = o.DescricaoProcesso,
					DescricaoSolicitacao = o.DescricaoSolicitacao,
					DescricaoProduto = o.DescricaoProduto
				}, o => o.IdProduto == parametros.Id).ToList();
			}

			var listaContador = Listahistorico.Count();

			if (listaContador > 0)
			{
				foreach (var regHistorico in Listahistorico)
				{
					regHistorico.DataHistoricoFormatada = regHistorico.DataHistorico != null ? Convert.ToDateTime(regHistorico.DataHistorico).ToShortDateString() : "-"; 

					regHistorico.ListaAlteracao = _uowSciex.QueryStackSciex.PRCDetalheHistoricoInsumo.ListarGrafo( o=> new PRCHistoricoDetalheInsumoVM()
					{

						IdPRCHistoricoInsumo = o.IdPRCHistoricoInsumo,
						IdDetalheHistoricoInsumo = o.IdDetalheHistoricoInsumo,
						DescricaoEvento = o.DescricaoEvento,
						DescricaoDetalhe = o.DescricaoDetalhe,
						TipoEvento = o.TipoEvento,
						DescricaoEventoDetalhe = o.DescricaoEvento + " " + (o.DescricaoDetalhe == null ? " -- " : o.DescricaoDetalhe)

					}, o => o.IdPRCHistoricoInsumo == regHistorico.IdPRCHistoricoInsumo 
					  && 
					  o.TipoEvento == "ALTERACAO"
					).ToList();

					regHistorico.ListaValorAfetado = _uowSciex.QueryStackSciex.PRCDetalheHistoricoInsumo.ListarGrafo(o => new PRCHistoricoDetalheInsumoVM()
					{

						IdPRCHistoricoInsumo = o.IdPRCHistoricoInsumo,
						IdDetalheHistoricoInsumo = o.IdDetalheHistoricoInsumo,
						DescricaoEvento = o.DescricaoEvento,
						DescricaoDetalhe = o.DescricaoDetalhe,
						TipoEvento = o.TipoEvento,
						DescricaoEventoDetalhe = o.DescricaoEvento + " " + (o.DescricaoDetalhe == null ? " -- " : o.DescricaoDetalhe)

					}, o => o.IdPRCHistoricoInsumo == regHistorico.IdPRCHistoricoInsumo
					  &&
					  o.TipoEvento == "VALOR AFETADO"
					).ToList();

					regHistorico.ListaAcrescimo = _uowSciex.QueryStackSciex.PRCDetalheHistoricoInsumo.ListarGrafo(o => new PRCHistoricoDetalheInsumoVM()
					{

						IdPRCHistoricoInsumo = o.IdPRCHistoricoInsumo,
						IdDetalheHistoricoInsumo = o.IdDetalheHistoricoInsumo,
						DescricaoEvento = o.DescricaoEvento,
						DescricaoDetalhe = o.DescricaoDetalhe,
						TipoEvento = o.TipoEvento,
						DescricaoEventoDetalhe = o.DescricaoEvento + " " + (o.DescricaoDetalhe == null ? " -- " : o.DescricaoDetalhe)

					}, o => o.IdPRCHistoricoInsumo == regHistorico.IdPRCHistoricoInsumo
					  &&
					  o.TipoEvento == "ACRESCIMO"
					).ToList();

					regHistorico.ListaDescrescimo = _uowSciex.QueryStackSciex.PRCDetalheHistoricoInsumo.ListarGrafo(o => new PRCHistoricoDetalheInsumoVM()
					{

						IdPRCHistoricoInsumo = o.IdPRCHistoricoInsumo,
						IdDetalheHistoricoInsumo = o.IdDetalheHistoricoInsumo,
						DescricaoEvento = o.DescricaoEvento,
						DescricaoDetalhe = o.DescricaoDetalhe,
						TipoEvento = o.TipoEvento,
						DescricaoEventoDetalhe = o.DescricaoEvento + " " + (o.DescricaoDetalhe == null ? " -- " : o.DescricaoDetalhe)

					}, o => o.IdPRCHistoricoInsumo == regHistorico.IdPRCHistoricoInsumo
					  &&
					  o.TipoEvento == "DECRESCIMO"
					).ToList();

				}

			}

			if(Listahistorico != null)
			{
				Listahistorico[0].DescricaoProcesso = Listahistorico.FirstOrDefault().DescricaoProcesso.PadLeft(9, '0');
			}

			return Listahistorico;
		}

	}
}