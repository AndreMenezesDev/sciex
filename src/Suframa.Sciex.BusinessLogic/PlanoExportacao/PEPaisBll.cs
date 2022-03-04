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
	public class PEPaisBll : IPEPaisBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUnitOfWork _uowCadsuf;
		private readonly IUsuarioPssBll _usuarioPssBll;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;

		public PEPaisBll(
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

		public PagedItems<PEProdutoPaisVM> ListarPaginado(PEProdutoVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<PEProdutoPaisVM>(); }

			var retornoConsulta = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.ListarPaginado<PEProdutoPaisVM>(o =>
			(
				o.IdPEProduto == pagedFilter.IdPEProduto
				&&
				(o.ValorDolar != 0
				&&
				o.Quantidade != 0)
			),
			pagedFilter);

			foreach (var item in retornoConsulta.Items)
			{				
				string codigoPais = item.CodigoPais.ToString("D3");
				var pais = _uowSciex.QueryStackSciex.ViewPais.Selecionar(o => o.CodigoPais == codigoPais);

				item.DescricaoPais = pais.Descricao;
			}

			return retornoConsulta;
		}
		
		public PagedItems<PEProdutoPaisVM> ListarPaginadoCorrecao(PEProdutoVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<PEProdutoPaisVM>(); }

			var retornoConsulta = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.ListarPaginado<PEProdutoPaisVM>(o =>
			(
				o.IdPEProduto == pagedFilter.IdPEProduto
				&&
				(
				o.ValorDolar > 0 
					&&
				o.Quantidade > 0
				)
			),
			pagedFilter);

			foreach (var item in retornoConsulta.Items)
			{				
				string codigoPais = item.CodigoPais.ToString("D3");
				var pais = _uowSciex.QueryStackSciex.ViewPais.Selecionar(o => o.CodigoPais == codigoPais);

				item.DescricaoPais = pais.Descricao;
			}

			return retornoConsulta;
		}

		public PEProdutoPaisVM Selecionar(int id)
		{
			var pe = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.Selecionar<PEProdutoPaisVM>(o => o.IdPEProdutoPais == id);
			return pe;
		}

		public PEProdutoPaisVM Salvar(PEProdutoPaisVM vm)
		{
			if (vm == null) { return null; }

			var prodPais = new PEProdutoPaisEntity();
			if (vm.IdPEProdutoPais > 0)
			{
				var prod = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Selecionar(o => o.IdPEProduto == vm.IdPEProduto);
				prodPais = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.Selecionar(x => x.IdPEProdutoPais == vm.IdPEProdutoPais);

				prod.ValorDolar = prod.ValorDolar - prodPais.ValorDolar;
				prod.Qtd = prod.Qtd - prodPais.Quantidade;

				prodPais = AutoMapper.Mapper.Map(vm, prodPais);
				vm.Mensagem = "";

				
				prod.ValorDolar = prod.ValorDolar + prodPais.ValorDolar;
				prod.Qtd = prod.Qtd + prodPais.Quantidade;
				_uowSciex.CommandStackSciex.PlanoExportacaoProduto.Salvar(prod);
			}
			else
			{
				var paisValidar = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais
					.Listar(o => o.IdPEProduto == vm.IdPEProduto && o.CodigoPais == vm.CodigoPais);

				vm.Mensagem = "";
				if (paisValidar.Count != 0)
				{
					vm.Mensagem = "O pais selecionado já foi cadastrado.";
					return vm;
				}

				prodPais.CodigoPais = vm.CodigoPais;
				prodPais.IdPEProduto = vm.IdPEProduto;
				prodPais.Quantidade = vm.Quantidade;
				prodPais.ValorDolar = vm.ValorDolar;

				var prod = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Selecionar(o => o.IdPEProduto == vm.IdPEProduto);
				prod.ValorDolar = vm.ValorDolar + prod.ValorDolar;
				prod.Qtd = vm.Quantidade + prod.Qtd;
				_uowSciex.CommandStackSciex.PlanoExportacaoProduto.Salvar(prod);
				_uowSciex.CommandStackSciex.Save();
			}

			_uowSciex.CommandStackSciex.PlanoExportacaoProdutoPais.Salvar(prodPais);
			_uowSciex.CommandStackSciex.Save();
			return vm;
		}

		public void Deletar(int id)
		{
			var pais = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.Selecionar(s => s.IdPEProdutoPais == id);

			if (pais != null)
			{
				if(pais.IdPEProduto != null)
				{
					var prod = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Selecionar(o => o.IdPEProduto == pais.IdPEProduto);
					prod.ValorDolar = prod.ValorDolar - pais.ValorDolar;
					prod.Qtd = prod.Qtd - pais.Quantidade;
					_uowSciex.CommandStackSciex.PlanoExportacaoProduto.Salvar(prod);
					_uowSciex.CommandStackSciex.Save();
				}
				_uowSciex.CommandStackSciex.PlanoExportacaoProdutoPais.Apagar(pais.IdPEProdutoPais);
			}
			_uowSciex.CommandStackSciex.Save();
		}

	}
}