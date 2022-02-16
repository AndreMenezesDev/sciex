using Suframa.Sciex.BusinessLogic.Pss;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.Ftp;
using Suframa.Sciex.CrossCutting.Texto;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Suframa.Sciex.BusinessLogic
{
	public class ConsultarProcessoImportacaoBll : IConsultarProcessoExportacaoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUnitOfWork _uowCadsuf;
		private readonly IUsuarioPssBll _usuarioPssBll;

		public ConsultarProcessoImportacaoBll(IUnitOfWorkSciex uowSciex, IUnitOfWork uowCadsuf, IUsuarioPssBll usuarioPssBll)
		{
			_uowSciex = uowSciex;
			_uowCadsuf = uowCadsuf;
			_usuarioPssBll = usuarioPssBll;


		}

		public PRCSolicProrrogacaoVM ListarRegistroAlteracao(int IdProcesso)
		{
			var busca = _uowSciex.QueryStackSciex.ProcessoSolicProrrogacao.Selecionar(x => x.IdProcesso == IdProcesso && x.Status==1); 
			var buscaVm = new PRCSolicProrrogacaoVM();

			buscaVm.Justificativa = busca.Justificativa;  

			return buscaVm != null ? buscaVm : null;

		}

		public PRCSolicProrrogacaoVM ReprovarProrrogacao(PRCSolicProrrogacaoVM filtro)
		{
			var prcSolicProrrogacaoVM = new PRCSolicProrrogacaoVM();
			var prcSolicProrrogacaoEntity = _uowSciex.QueryStackSciex.ProcessoSolicProrrogacao.Selecionar(x=> x.IdProcesso == filtro.IdProcesso&& x.Status == 1);
			
			try
			{	
				prcSolicProrrogacaoEntity.Status = 3;
				prcSolicProrrogacaoEntity.IdProcesso = filtro.IdProcesso;
				prcSolicProrrogacaoEntity.JustificativaReprovado = filtro.JustificativaReprovado;
				_uowSciex.CommandStackSciex.ProcessoSolicitacaoProrrogacao.Salvar(prcSolicProrrogacaoEntity);
				_uowSciex.CommandStackSciex.Save();
				prcSolicProrrogacaoVM.Reprovacao = true;

				return prcSolicProrrogacaoVM;
			}
			catch(Exception err)
			{
				prcSolicProrrogacaoVM.Reprovacao = false;
				return prcSolicProrrogacaoVM;
			}
		
		}
		



		public PRCStatusVM AprovarProrogacao(PRCStatusVM  filtro)
		{	
			try { 

				var prcStatusVM = new PRCStatusVM();
				var prcStatus = new PRCStatusEntity();
				var prcProcessoStatus = new PRCSolicProrrogacaoVM();
				var prcSolicProrrogacao = _uowSciex.QueryStackSciex.ProcessoSolicProrrogacao.Selecionar(x=> x.IdProcesso == filtro.IdProcesso && x.Status == 1);
				var prcProcesso = _uowSciex.QueryStackSciex.Processo.Selecionar(x => x.IdProcesso == filtro.IdProcesso); 
				var usuarioLogado = _usuarioPssBll.PossuiUsuarioLogado();
				if (usuarioLogado != null)
				{
					prcStatus.CpfResponsavel = usuarioLogado.usuarioLogadoCpfCnpj.Replace(".","");
					prcStatus.CpfResponsavel = prcStatus.CpfResponsavel.Replace("-", "");
					prcStatus.NomeResponsavel = usuarioLogado.usuarioLogadoNome; 
					
				}
				else
				{
					prcStatusVM.Aprovacao = 1;
					return prcStatusVM;

				}

				prcStatus.Tipo = "PE";
				prcStatus.Data = DateTime.Now;
				prcStatus.IdProcesso = filtro.IdProcesso;
				prcStatus.DescricaoObservacao = filtro.DescricaoObservacao;

				DateTime date = new DateTime();
				date = (DateTime)prcProcesso.DataValidade;
				date = date.AddDays(180);
				prcProcesso.DataValidade = date;
				prcStatus.DataValidade = date;

				prcSolicProrrogacao.Status = 2;

				_uowSciex.CommandStackSciex.PRCStatus.Salvar(prcStatus);
				_uowSciex.CommandStackSciex.Save();
				_uowSciex.CommandStackSciex.Processo.Salvar(prcProcesso);
				_uowSciex.CommandStackSciex.Save();
				_uowSciex.CommandStackSciex.ProcessoSolicitacaoProrrogacao.Salvar(prcSolicProrrogacao);
				_uowSciex.CommandStackSciex.Save();

				prcStatusVM.Aprovacao = 0;
				return prcStatusVM;

			}
			catch(Exception err) { 
			
				var prcStatusVM = new PRCStatusVM();
				prcStatusVM.Aprovacao = 1;
				return prcStatusVM;

			}
		}
	
	
	
	}

}
