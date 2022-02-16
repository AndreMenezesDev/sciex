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
using System.IO;
using System.Text;

namespace Suframa.Sciex.BusinessLogic
{
	public class SolicitacaoFornecedorFabricanteBll : ISolicitacaoFornecedorFabricanteBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUsuarioLogado _IUsuarioLogado;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;
		private readonly IViewImportadorBll _IViewImportadorBll;

		private string CNPJ;

		public SolicitacaoFornecedorFabricanteBll(
			IUnitOfWorkSciex uowSciex,
			IUsuarioLogado usuarioLogado,
			IViewImportadorBll viewImportadorBll,
			IUsuarioInformacoesBll usuarioInformacoesBll)
		{
			_uowSciex = uowSciex;
			_IUsuarioLogado = usuarioLogado;
			_IViewImportadorBll = viewImportadorBll;
			_usuarioInformacoesBll = usuarioInformacoesBll;
			this.CNPJ = _usuarioInformacoesBll.ObterCNPJ();

		}
		
		public IEnumerable<SolicitacaoFornecedorFabricanteVM> Listar(SolicitacaoFornecedorFabricanteVM solicitacaoFornecedorFabricanteVM)
		{
			var solicitacaopliff = _uowSciex.QueryStackSciex.SolicitacaoFornecedorFabricante.Listar<SolicitacaoFornecedorFabricanteVM>();
			return AutoMapper.Mapper.Map<IEnumerable<SolicitacaoFornecedorFabricanteVM>>(solicitacaopliff);
		}

		public SolicitacaoFornecedorFabricanteVM Salvar(SolicitacaoFornecedorFabricanteVM solicitacaoFornecedorFabricanteVM)
		{
			var solicitacaoPliFornecedorFabricanteEntity = AutoMapper.Mapper.Map<SolicitacaoPliMercadoriaEntity>(solicitacaoFornecedorFabricanteVM);
			_uowSciex.CommandStackSciex.SolicitacaoPliMercadoria.Salvar(solicitacaoPliFornecedorFabricanteEntity);
			_uowSciex.CommandStackSciex.Save();

			solicitacaoFornecedorFabricanteVM = AutoMapper.Mapper.Map<SolicitacaoFornecedorFabricanteVM>(solicitacaoPliFornecedorFabricanteEntity);
			return solicitacaoFornecedorFabricanteVM;
		}
	}





}