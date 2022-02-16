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
	public class SolicitacaoPliProcessoAnuenteBll : ISolicitacaoPliProcessoAnuenteBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUsuarioLogado _IUsuarioLogado;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;
		private readonly IViewImportadorBll _IViewImportadorBll;

		private string CNPJ;

		public SolicitacaoPliProcessoAnuenteBll(
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
		
		public IEnumerable<SolicitacaoPliProcessoAnuenteVM> Listar(SolicitacaoPliProcessoAnuenteVM solicitacaoPliProcessoAnuenteVM)
		{
			var solicitacaoplimercadoria = _uowSciex.QueryStackSciex.SolicitacaoPliProcessoAnuente.Listar<SolicitacaoPliProcessoAnuenteVM>();
			return AutoMapper.Mapper.Map<IEnumerable<SolicitacaoPliProcessoAnuenteVM>>(solicitacaoplimercadoria);
		}

		public SolicitacaoPliProcessoAnuenteVM Salvar(SolicitacaoPliProcessoAnuenteVM solicitacaoPliProcessoAnuenteVM)
		{
			var solicitacaoPliMercadoriaEntity = AutoMapper.Mapper.Map<SolicitacaoPliProcessoAnuenteEntity>(solicitacaoPliProcessoAnuenteVM);
			_uowSciex.CommandStackSciex.SolicitacaoPliProcessoAnuente.Salvar(solicitacaoPliMercadoriaEntity);
			_uowSciex.CommandStackSciex.Save();

			solicitacaoPliProcessoAnuenteVM = AutoMapper.Mapper.Map<SolicitacaoPliProcessoAnuenteVM>(solicitacaoPliMercadoriaEntity);
			return solicitacaoPliProcessoAnuenteVM;
		}
	}





}