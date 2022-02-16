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
	public class SolicitacaoPliDetalheMercadoriaBll : ISolicitacaoPliDetalheMercadoriaBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUsuarioLogado _IUsuarioLogado;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;
		private readonly IViewImportadorBll _IViewImportadorBll;

		private string CNPJ;

		public SolicitacaoPliDetalheMercadoriaBll(
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
		
		public IEnumerable<SolicitacaoPliDetalheMercadoriaVM> Listar(SolicitacaoPliDetalheMercadoriaVM solicitacaoPliDetalheMercadoriaVM)
		{
			var solicitacaoplidetalhemercadoria = _uowSciex.QueryStackSciex.SolicitacaoPliDetalheMercadoria.Listar<SolicitacaoPliDetalheMercadoriaVM>();
			return AutoMapper.Mapper.Map<IEnumerable<SolicitacaoPliDetalheMercadoriaVM>>(solicitacaoplidetalhemercadoria);
		}

		public SolicitacaoPliDetalheMercadoriaVM Salvar(SolicitacaoPliDetalheMercadoriaVM solicitacaoPliDetalheMercadoriaVM)
		{
			var solicitacaoPliDetalheMercadoriaEntity = AutoMapper.Mapper.Map<SolicitacaoPliDetalheMercadoriaEntity>(solicitacaoPliDetalheMercadoriaVM);
			_uowSciex.CommandStackSciex.SolicitacaoPliDetalheMercadoria.Salvar(solicitacaoPliDetalheMercadoriaEntity);
			_uowSciex.CommandStackSciex.Save();

			solicitacaoPliDetalheMercadoriaVM = AutoMapper.Mapper.Map<SolicitacaoPliDetalheMercadoriaVM>(solicitacaoPliDetalheMercadoriaEntity);
			return solicitacaoPliDetalheMercadoriaVM;
		}
	}





}