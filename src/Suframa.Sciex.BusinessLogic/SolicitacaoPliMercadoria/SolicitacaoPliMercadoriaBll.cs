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
	public class SolicitacaoPliMercadoriaBll : ISolicitacaoPliMercadoriaBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUsuarioLogado _IUsuarioLogado;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;
		private readonly IViewImportadorBll _IViewImportadorBll;

		private string CNPJ;

		public SolicitacaoPliMercadoriaBll(
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
		
		public IEnumerable<SolicitacaoPliMercadoriaVM> Listar(SolicitacaoPliMercadoriaVM solicitacaoPliMercadoriaVM)
		{
			var solicitacaoplimercadoria = _uowSciex.QueryStackSciex.SolicitacaoPliMercadoria.Listar<SolicitacaoPliMercadoriaVM>();
			return AutoMapper.Mapper.Map<IEnumerable<SolicitacaoPliMercadoriaVM>>(solicitacaoplimercadoria);
		}

		public SolicitacaoPliMercadoriaVM Salvar(SolicitacaoPliMercadoriaVM solicitacaoPliMercadoriaVM)
		{
			var solicitacaoPliMercadoriaEntity = AutoMapper.Mapper.Map<SolicitacaoPliMercadoriaEntity>(solicitacaoPliMercadoriaVM);
			_uowSciex.CommandStackSciex.SolicitacaoPliMercadoria.Salvar(solicitacaoPliMercadoriaEntity);
			_uowSciex.CommandStackSciex.Save();

			solicitacaoPliMercadoriaVM = AutoMapper.Mapper.Map<SolicitacaoPliMercadoriaVM>(solicitacaoPliMercadoriaEntity);
			return solicitacaoPliMercadoriaVM;
		}
	}





}