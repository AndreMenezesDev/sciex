using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Gerar de Arquivo da ALI</summary>
	public class GerarArquivoAliNormalController : ApiController
	{
		private readonly IAliBll _aliBll;

		/// <summary>Construtor</summary>
		/// <param name="aliBll"></param>
		public GerarArquivoAliNormalController(IAliBll aliBll)
		{
			_aliBll = aliBll;

		}

		/// <summary>Gerar arquivo</summary>
		/// <param name="hash">Codigo para manter segurança de acesso</param>
		/// <returns></returns>		
		[AllowAnonymous]
		[HttpGet]
		public JsonResult<string> GerarArquivo(string hash)
		{
			try
			{
				if (!string.IsNullOrEmpty(hash) && hash.ToUpper().Trim() == "A71E3FE602EA53827A3AB490709C94F4E13F84E90F6D07100F38052D22A99DF8F63E8BE20E2E753AC40F03C3B73B194555A79E8E717888525E5288298F53B7F1")
				{
					try
					{
						_aliBll.GerarAquivoEnvio();
						return Json("Sucesso");
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