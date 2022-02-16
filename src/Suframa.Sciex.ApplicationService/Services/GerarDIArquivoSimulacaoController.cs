﻿using Suframa.Sciex.BusinessLogic;
using System;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Ler Arquivo de LI</summary>
	public class GerarDIArquivoSimulacaoController : ApiController
	{
		private readonly IDiBll _diBll;

		/// <summary>Construtor</summary>
		/// <param name="liBll"></param>
		public GerarDIArquivoSimulacaoController(IDiBll diBll)
		{
			_diBll = diBll;

		}

		/// <summary>Ler arquivo DI</summary>
		/// <param name="hash">Codigo para manter segurança de acesso</param>
		/// <returns></returns>		
		[AllowAnonymous]
		[HttpGet]
		public System.Web.Http.Results.JsonResult<string> GerarArquivoDISimulacao(string hash)
		{
			try
			{
				if (!string.IsNullOrEmpty(hash) && hash.ToUpper().Trim() == "A71E3FE602EA53827A3AB490709C94F4E13F84E90F6D07100F38052D22A99DF8F63E8BE20E2E753AC40F03C3B73B194555A79E8E717888525E5288298F53B7F1")
				{
					try
					{
						string msg = _diBll.GerarArquivoSimulacaoDI();
						return Json(msg);
					}
					catch (Exception ex)
					{
						return Json("Problema na geração do arquivo: " + ex.Message.ToString());
					}

				}
				else
				{
					return Json("Você não tem autorização para executar este serviço");
				}
			}
			catch (System.Exception ex)
			{
				return Json("Erro ao executar o serviço : " + ex.Message.ToString());
			}

		}
	}
}