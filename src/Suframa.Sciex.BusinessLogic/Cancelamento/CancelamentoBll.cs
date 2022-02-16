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
	public class CancelamentoBll : ICancelamentoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUnitOfWork _uowCadsuf;
		

		public CancelamentoBll(IUnitOfWorkSciex uowSciex, IUnitOfWork uowCadsuf)
		{
			_uowSciex = uowSciex;
			_uowCadsuf = uowCadsuf;
			

	}

		public DateTime GetDateTimeNowUtc()
		{
			var manausTime = TimeZoneInfo.ConvertTime(DateTime.Now,
				 TimeZoneInfo.FindSystemTimeZoneById("SA Western Standard Time"));

			return manausTime;

		}
		public PRCStatusVM CancelarProcesso(PRCStatusVM filtro)
		{
		
			var prcStatus = new PRCStatusVM();


			var tipoInsumo = new List<string>()
			{ 
				"P",
				"E"
			};
			try
			{
				var reg = _uowSciex.QueryStackSciex.PRCInsumo.Listar(q => tipoInsumo.Contains(q.TipoInsumo)).Sum(x => x.ValorDolarComp);
				var paridadeCambial = _uowSciex.QueryStackSciex.ConsultarExistenciaParidadePorData(DateTime.Today);
				if (reg > 0)
				{
					prcStatus.StatusProcessamento = 1;
					return prcStatus;
				}
				else
				{
					if(paridadeCambial != null) 
					{
						var regProcesso = _uowSciex.QueryStackSciex.Processo.Selecionar(x => x.IdProcesso == filtro.IdProcesso);

						if (regProcesso != null)
						{
							regProcesso.TipoStatus = "CA";
						}
						else
						{
							prcStatus.StatusProcessamento = 1;
							return prcStatus;
						}



						PRCStatusEntity prcStatusEntity = new PRCStatusEntity();

						prcStatusEntity.Data = GetDateTimeNowUtc();
						prcStatusEntity.IdProcesso = filtro.IdProcesso;
						prcStatusEntity.Tipo = "CA";
						prcStatusEntity.DescricaoObservacao = filtro.DescricaoObservacao;

						//Salvando na Entity PRCStatus
						_uowSciex.CommandStackSciex.PRCStatus.Salvar(prcStatusEntity);
						_uowSciex.CommandStackSciex.Save();

						//Salvando na Entity Processo
						_uowSciex.CommandStackSciex.Processo.Salvar(regProcesso);
						_uowSciex.CommandStackSciex.Save();



						prcStatus.StatusProcessamento = 0;


						try
						{
							_uowSciex.QueryStackSciex.IniciarStoreProcedureParecerSuspensaoCancelado(regProcesso.IdProcesso);
							NotificarEmailBll email = new NotificarEmailBll();
							email.EnviarEmail(tipo: regProcesso.TipoStatus, 
											nomeEmpresa: regProcesso.RazaoSocial, 
											numeroAnoProcesso: ((int)(regProcesso.NumeroProcesso)).ToString("D4") + "/" + regProcesso.AnoProcesso.ToString(),
											dataValidade: regProcesso.DataValidade.ToString(), 
											uowSciex: _uowSciex) ;

						}
						catch (Exception e)
						{
							prcStatus.StatusProcessamento = 2;
							return prcStatus;
						}


						return prcStatus;
					}

					else
					{
						prcStatus.StatusProcessamento = 3;
						return prcStatus;
					}


				}


			}
			catch(Exception err){

				string erro;
				prcStatus.StatusProcessamento = 2;
				return prcStatus;
			}
		}
		
		


	}
}
