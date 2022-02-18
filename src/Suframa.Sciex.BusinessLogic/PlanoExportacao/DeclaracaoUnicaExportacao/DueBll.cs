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
using System.Runtime.InteropServices.WindowsRuntime;

namespace Suframa.Sciex.BusinessLogic
{
	public class DueBll : IDueBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUnitOfWork _uowCadsuf;
		private readonly IUsuarioPssBll _usuarioPssBll;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;


		private long _idPLiRetorno;

		public DueBll(
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
		public PagedItems<PlanoExportacaoDUEComplementoVM> ListarDUEPaginadoParaAnalise(PEProdutoVM pagedFilter)
		{
			if (pagedFilter == null || pagedFilter.IdPEProduto == 0) { return new PagedItems<PlanoExportacaoDUEComplementoVM>(); }

			var listaProdutoPaisEntity = _uowSciex.QueryStackSciex.PlanoExportacaoProdutoPais.ListarGrafo(o => new PEProdutoPaisVM()
			{
				IdPEProduto = o.IdPEProduto,
				IdPEProdutoPais = o.IdPEProdutoPais,
				CodigoPais = o.CodigoPais
			},
			o =>
			(
				o.IdPEProduto == pagedFilter.IdPEProduto
			)
			)
			.ToList();
			var idPEProduto = listaProdutoPaisEntity.Select(q => q.IdPEProduto).FirstOrDefault();
			var listaIdProdutoPais = listaProdutoPaisEntity.Select(q => (int?)q.IdPEProdutoPais).ToList();

			string sort = null;
			if (!string.IsNullOrEmpty(pagedFilter.Sort) && pagedFilter.Sort.Equals("DescricaoPais"))
			{
				sort = "DescricaoPais";
				pagedFilter.Sort = null;
			}

			var listaDUE = _uowSciex.QueryStackSciex.PlanoExportacaoDue.ListarPaginadoGrafo(q => new PlanoExportacaoDUEComplementoVM()
			{
				IdPEProduto = idPEProduto,
				IdPEProdutoPais = q.IdPEProdutoPais,
				IdDue = q.IdDue,
				CodigoPais = q.CodigoPais,
				Numero = q.Numero,
				DataAverbacao = q.DataAverbacao,
				Quantidade = q.Quantidade,
				ValorDolar = q.ValorDolar,
				SituacaoAnalise = q.SituacaoAnalise,
				DescricaoJustificativa = q.DescricaoJustificativa
			},
			q => listaIdProdutoPais.Contains(q.IdPEProdutoPais)
			, pagedFilter);

			foreach (var item in listaDUE.Items)
			{
				string codigoPais = item.CodigoPais.ToString("D3");
				var pais = _uowSciex.QueryStackSciex.ViewPais.Selecionar(o => o.CodigoPais == codigoPais);

				item.DescricaoPais = pais.Descricao;
				item.DataAverbacaoFormatada = item.DataAverbacao != DateTime.MinValue ? item.DataAverbacao.ToShortDateString() : "-";
				item.DescricaoSituacaoAnalise = ObterDescricaoSituacaoAnalise(item.SituacaoAnalise);
			}

			if (!string.IsNullOrWhiteSpace(sort))
			{
				switch (sort)
				{
					case "DescricaoPais":
						if (pagedFilter.Reverse)
						{
							listaDUE.Items = listaDUE.Items.OrderBy(q => q.DescricaoPais).ThenBy(q => q.DescricaoPais).ToList();
						}
						else
						{
							listaDUE.Items = listaDUE.Items.OrderByDescending(q => q.DescricaoPais).ThenByDescending(q => q.DescricaoPais).ToList();
						}
						break;
				}
			}

			return listaDUE;
		}

		private string ObterDescricaoSituacaoAnalise(int? situacaoAnalise)
		{
			switch (situacaoAnalise)
			{
				case null: return "-";
				case 1: return "Aprovado";
				case 2: return "Reprovado";
				case 3: return "Alterado";
				case 4: return "Corrigido";
				case 5: return "Inativo";
				case 6: return "Novo";
				default: return "-";

			}
		}

		public ResultadoMensagemProcessamentoVM AprovarTodasDeclaracoesUnicasExportacao(PlanoExportacaoDUEComplementoVM vm)
		{
			if (vm == null || vm.IdPEProduto == 0)
			{
				return new ResultadoMensagemProcessamentoVM()
				{
					Resultado = false,
					Mensagem = "Id do produto inválido"
				};
			}

			var result = new ResultadoMensagemProcessamentoVM()
			{
				Resultado = true
			};


			IList<PlanoExportacaoDUEVM> listaDues = BuscarListaRegistrosNulos(vm.IdPEProduto);

			AprovarRegistrosNulos(listaDues, result);


			return result;
		}

		private IList<PlanoExportacaoDUEVM> BuscarListaRegistrosNulos(int? IdPEProduto)
		{
			if (IdPEProduto == 0)
				return new List<PlanoExportacaoDUEVM>();

			IList<PlanoExportacaoDUEVM> listaDue;
			List<int?> statusAnalise = new List<int?>()
			{
				null
				//,(int)EnumSituacaoAnalisePlanoExportacao.APROVADO
				//,(int)EnumSituacaoAnalisePlanoExportacao.REPROVADO
				//,(int)EnumSituacaoAnalisePlanoExportacao.CORRIGIDO
				//,(int)EnumSituacaoAnalisePlanoExportacao.ALTERADO
			};

			listaDue = _uowSciex.QueryStackSciex.PlanoExportacaoDue.Listar<PlanoExportacaoDUEVM>(
				q => q.PEProdutoPais.IdPEProduto == IdPEProduto
					&&
					statusAnalise.Contains(q.SituacaoAnalise)
				);

			return listaDue;
		}

		private void AprovarRegistrosNulos(IList<PlanoExportacaoDUEVM> listaDues, ResultadoMensagemProcessamentoVM result)
		{

			try
			{

				foreach (var due in listaDues)
				{
					var regDUE = _uowSciex.QueryStackSciex.PlanoExportacaoDue.Selecionar(q => q.IdDue == due.IdDue);

					regDUE.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.APROVADO;
					_uowSciex.CommandStackSciex.PlanoExportacaoDue.Salvar(regDUE);
				}

				_uowSciex.CommandStackSciex.Save();
			}
			catch (Exception e)
			{
				result.Resultado = false;
				result.Mensagem = $"Falha ao aprovar registros: (Mensagem: {e.Message} / StackTrace: {e.StackTrace})";
			}
		}

		public ResultadoMensagemProcessamentoVM AprovarOuReprovarDeclaracaoUnicaExportacao(AnalisePlanoExportacaoDUEVM vm)
		{
			if (vm.IdDue == 0) return new ResultadoMensagemProcessamentoVM() { Resultado = false, Mensagem = "ID Due inválido" };

			var result = new ResultadoMensagemProcessamentoVM() { Resultado = true};

			var regDUE = _uowSciex.QueryStackSciex.PlanoExportacaoDue.Selecionar(q => q.IdDue == vm.IdDue);

			if (regDUE.SituacaoAnalise == (int)EnumSituacaoAnaliseDUE.CORRIGIDO)
			{
				regDUE.SituacaoAnalise = vm.AcaoIsAprovar ? (int)EnumSituacaoAnaliseDUE.APROVADO : (int)EnumSituacaoAnaliseDUE.REPROVADO;

				_uowSciex.CommandStackSciex.PlanoExportacaoDue.Salvar(regDUE);

				int statusAlterado = 3;
				var regDUEAlterado = _uowSciex.QueryStackSciex.PlanoExportacaoDue.Selecionar(q => q.IdPEProdutoPais == regDUE.IdPEProdutoPais
																								&&
																								q.SituacaoAnalise == statusAlterado);

				regDUEAlterado.SituacaoAnalise = (int)EnumSituacaoAnaliseDUE.INATIVO;
				_uowSciex.CommandStackSciex.PlanoExportacaoDue.Salvar(regDUEAlterado);
			}
			else
			{
				if (vm.AcaoIsAprovar)
				{
					regDUE.SituacaoAnalise = (int)EnumSituacaoAnaliseDUE.APROVADO;
				}
				else
				{
					regDUE.SituacaoAnalise = (int)EnumSituacaoAnaliseDUE.REPROVADO;
					regDUE.DescricaoJustificativa = vm.DescricaoJustificativa;
				}

				_uowSciex.CommandStackSciex.PlanoExportacaoDue.Salvar(regDUE);
				
			}

			_uowSciex.CommandStackSciex.Save();

			return result;
		}

	}

}