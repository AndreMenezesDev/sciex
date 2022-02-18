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

namespace Suframa.Sciex.BusinessLogic
{
	public class PEProdutoBll : IPEProdutoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUnitOfWork _uowCadsuf;
		private readonly IUsuarioPssBll _usuarioPssBll;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;

		public PEProdutoBll(
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
			var pe = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.SelecionarGrafo(o => new PEProdutoVM()
			{
				IdPEProduto = o.IdPEProduto,
				IdPlanoExportacao = o.IdPlanoExportacao,
				CodigoProdutoExportacao = o.CodigoProdutoExportacao,
				CodigoProdutoSuframa = o.CodigoProdutoSuframa,
				CodigoNCM = o.CodigoNCM,
				CodigoTipoProduto = o.CodigoTipoProduto,
				DescricaoModelo = o.DescricaoModelo,
				Qtd = o.Qtd,
				ValorDolar = o.ValorDolar,
				ValorFluxoCaixa = o.ValorFluxoCaixa,
				CodigoUnidade = o.CodigoUnidade
			}
			,
			o=> o.IdPEProduto == idPEProduto);

			var codProdSuf = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao.Listar(o => o.CodigoProduto == pe.CodigoProdutoSuframa).FirstOrDefault();
			pe.DescCodigoProdutoSuframa = codProdSuf.CodigoProduto.ToString("D4") + " | " + codProdSuf.DescricaoProduto;
			var codTipoProdSuf = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao.Listar(o => o.CodigoProduto == pe.CodigoProdutoSuframa && o.CodigoTipoProduto == pe.CodigoTipoProduto).FirstOrDefault();
			pe.DescCodigoTipoProduto = codTipoProdSuf.CodigoTipoProduto.ToString("D3") + " | " + codTipoProdSuf.DescricaoTipoProduto;
			var undMed = _uowSciex.QueryStackSciex.ViewUnidadeMedida.Selecionar(o => o.CodigoUnidadeMedida == pe.CodigoUnidade);
			pe.DescCodigoUnidade = undMed.CodigoUnidadeMedida.ToString("D3") + " | " + undMed.Descricao;

			pe.QtdFormatado = pe.Qtd != 0 ? pe.Qtd.ToString("N5") : "0";
			pe.ValorDolarFormatado = pe.ValorDolar != 0 ? pe.ValorDolar.ToString("N") : "0";

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

		public void Deletar(int id)
		{
			var leProduto = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Selecionar(s => s.IdPEProduto == id);

			if (leProduto != null)
			{
				var pePaises = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.Listar(o => o.IdPEProduto == leProduto.IdPEProduto);

				foreach (var item in pePaises)
				{
					_uowSciex.CommandStackSciex.PlanoExportacaoProdutoPais.Apagar(item.IdPEProdutoPais);
					_uowSciex.CommandStackSciex.Save();
				}

				_uowSciex.CommandStackSciex.PlanoExportacaoProduto.Apagar(leProduto.IdPEProduto);
				_uowSciex.CommandStackSciex.Save();
			}
		}
	}
}