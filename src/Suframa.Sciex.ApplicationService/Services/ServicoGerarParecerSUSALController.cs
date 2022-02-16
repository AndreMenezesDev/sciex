using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service paridade cambial</summary>
	public class ServicoGerarParecerSUSCAController : ApiController
	{
		private readonly IServicoGerarParecerSuspensaoBll _bll;

		/// <summary>Paridade Cambial injetar regras de negócio</summary>
		/// <param name="paridadeCambialBll"></param>
		public ServicoGerarParecerSUSCAController(IServicoGerarParecerSuspensaoBll bll)
		{
			_bll = bll;

		}

		/// <summary>Geracao Parecer Suspensao Alterado</summary>
		/// <param name="hash">hash para execucao</param>
		/// <param name="idprocesso">ID do Processo</param>
		/// <param name="idSolicitacao">Status do Processo</param>
		/// <returns>Numero Processo</returns>		
		/// <returns>Ano Processo</returns>		
		/// <summary>Geracao Parecer Suspensao Cancelado</summary>
		/// <param name="hash">hash para execucao</param>
		/// <param name="idprocesso">ID do Processo</param>
		/// <returns>Numero Processo</returns>		
		/// <returns>Ano Processo</returns>	
		[AllowAnonymous]
		[HttpGet]
		public string GerarParecerSuspensao([FromUri] GerarParecerSuspensaoVM view)
		{
			try
			{
				if (!string.IsNullOrEmpty(view.Hash) && view.Hash.ToUpper().Trim() == "A71E3FE602EA53827A3AB490709C94F4E13F84E90F6D07100F38052D22A99DF8F63E8BE20E2E753AC40F03C3B73B194555A79E8E717888525E5288298F53B7F1")
				{

					if (view.IdSolicitacaoAlteracao != 0 && view.IdProcesso != 0)
					{
						var _result = _bll.GerarParecerSuspensaoAlterado(view);

						if (_result != null)
						{
							return $"ID Processo: {_result.NumeroControle:D3} - Parecer: {_result.AnoControle}";
						}
						else
						{
							return "nulo";
						}
					}
					else if (view.IdProcesso != 0)
					{
						var result = _bll.GerarParecerSuspensaoCancelado(view);

						if (result != null)
						{
							return $"ID Processo: {result.IdProcesso} - Valor Dola Aprovado: {result.NumeroControle:D3} - Qtd Aprovado: {result.AnoControle}";
						}
						else
						{
							return "nulo";
						}
					}
					else
					{
						return "Parâmetros inválidos";
					}
					
				}
				else
				{
					return "Você não tem autorização para executar este serviço";
				}
			}
			catch (System.Exception ex)
			{
				return $"Erro ao executar o serviço : {ex.Message}";

			}

		}
	}
}