using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using Suframa.Sciex.DataAccess.Database;
using System.Linq;
using System;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using Suframa.Sciex.CrossCutting.Extension;
using Suframa.Sciex.CrossCutting.SuperStructs;
using Suframa.Sciex.CrossCutting.Resources;
using HandlebarsDotNet;
using Newtonsoft.Json;
using Suframa.Sciex.CrossCutting.Json;
using Suframa.Sciex.BusinessLogic.Pss;

namespace Suframa.Sciex.BusinessLogic
{
	public class NotificarEmailBll : INotificarEmailBll
	{
		private  IUnitOfWorkSciex _uowSciex;
		private readonly IUsuarioLogado _usuarioLogado;
		private readonly IUsuarioPssBll _usuarioPssBll;

		public NotificarEmailBll()
		{

		}

		private int ConfigurarControleExecutacaoServico(ControleExecucaoServicoEntity controleExecucaoServicoEntity, bool inicio)
		{

			if(inicio){
				
				_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(controleExecucaoServicoEntity);

				_uowSciex.CommandStackSciex.Save();

				_uowSciex.CommandStackSciex.DetachEntries();
				return controleExecucaoServicoEntity.IdControleExecucaoServico;
			}
			else{
				var regControle =_uowSciex.QueryStackSciex.ControleExecucaoServico.Selecionar(o => o.IdControleExecucaoServico == controleExecucaoServicoEntity.IdControleExecucaoServico);
				var manausTime = TimeZoneInfo.ConvertTime(DateTime.Now,
				 TimeZoneInfo.FindSystemTimeZoneById("SA Western Standard Time"));
				regControle.DataHoraExecucaoFim = manausTime;
				regControle.StatusExecucao = controleExecucaoServicoEntity.StatusExecucao;
				regControle.MemoObjetoRetorno = controleExecucaoServicoEntity.MemoObjetoRetorno;

				_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(controleExecucaoServicoEntity);
				_uowSciex.CommandStackSciex.Save();

				return 0;
			}
		}



		public string EnviarEmail(string tipo, string nomeEmpresa, string numeroAnoProcesso, string dataValidade, IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;

			var controleExecucaoServicoEntity = new ControleExecucaoServicoEntity();

			var manausTime = TimeZoneInfo.ConvertTime(DateTime.Now,
			TimeZoneInfo.FindSystemTimeZoneById("SA Western Standard Time"));
			controleExecucaoServicoEntity.DataHoraExecucaoInicio = manausTime;
			controleExecucaoServicoEntity.IdListaServico = 27;
			controleExecucaoServicoEntity.StatusExecucao = 0;
			controleExecucaoServicoEntity.NomeCPFCNPJInteressado = "teste";
			controleExecucaoServicoEntity.NumeroCPFCNPJInteressado = "133";

			controleExecucaoServicoEntity.IdControleExecucaoServico = ConfigurarControleExecutacaoServico(controleExecucaoServicoEntity, true);

			try
			{

				int idLista = 27;
				var regConfig = _uowSciex.QueryStackSciex.ParametroConfiguracao.Listar(x => x.IdListaServico == idLista).FirstOrDefault();
				string titulo = null;
				if (tipo == "CA")
				{
					titulo = "Cancelamento de Processo de Exportação";
					var body = "A empresa " + nomeEmpresa + " CANCELOU o processo de exportação " + numeroAnoProcesso + ".";
					try
					{
						Email.Enviar(body, titulo, regConfig.Valor);
					}
					catch (Exception ex)
					{
						controleExecucaoServicoEntity.StatusExecucao = 2;
						controleExecucaoServicoEntity.MemoObjetoRetorno = "Erro ao enviar email";
						controleExecucaoServicoEntity.IdControleExecucaoServico = ConfigurarControleExecutacaoServico(controleExecucaoServicoEntity, false);
					}
				}
				if (tipo == "PR")
				{
					titulo = "Prorrogação de Processo de Exportação";
					var body = "A empresa " + nomeEmpresa + " PRORROGOU o processo de exportação " + numeroAnoProcesso + " . Data de Validade " + dataValidade + ".";
					try
					{
						Email.Enviar(body, titulo, regConfig.Valor);
					}
					catch (Exception ex)
					{
						controleExecucaoServicoEntity.StatusExecucao = 2;
						controleExecucaoServicoEntity.MemoObjetoRetorno = "Erro ao enviar email";
						controleExecucaoServicoEntity.DataHoraExecucaoFim = manausTime;
						controleExecucaoServicoEntity.IdControleExecucaoServico = ConfigurarControleExecutacaoServico(controleExecucaoServicoEntity, false);
					}
				}
				controleExecucaoServicoEntity.StatusExecucao = 1;
				controleExecucaoServicoEntity.MemoObjetoRetorno = "Email enviado";
				controleExecucaoServicoEntity.DataHoraExecucaoFim = manausTime;
				controleExecucaoServicoEntity.IdControleExecucaoServico = ConfigurarControleExecutacaoServico(controleExecucaoServicoEntity, false);
			}
			catch (Exception ex)
			{
				controleExecucaoServicoEntity.StatusExecucao = 2;
				controleExecucaoServicoEntity.MemoObjetoRetorno = "Erro ao enviar email";
				controleExecucaoServicoEntity.DataHoraExecucaoFim = manausTime;
				controleExecucaoServicoEntity.IdControleExecucaoServico = ConfigurarControleExecutacaoServico(controleExecucaoServicoEntity, false);
			}
			return null;
		}

	}
}