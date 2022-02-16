using RestSharp;
using RestSharp.Authenticators;
using Suframa.Sciex.CrossCutting.Configuration;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.Json;
using Suframa.Sciex.DataAccess.RestService.ApiDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization.Json;

namespace Suframa.Sciex.DataAccess.RestService
{
	public class ArredacacaoApi : IArredacacaoApi
	{
		private RestClient client = null;

		public ArredacacaoApi()
		{
			var url = CrossCutting.Web.UriHelper.Slashfy(PrivateSettings.URL_ARRECADACAO);
			client = new RestClient(url)
			{
				Authenticator = new HttpBasicAuthenticator(PrivateSettings.USER_ARRECADACAO, PrivateSettings.PASS_ARRECADACAO)
			};
		}

		public SolicitarGeracaoDebitoVM RegistrarDebito(SolicitarGeracaoDebitoVM dto)
		{
			try
			{
				var request = new RestRequest("debito", Method.POST);

				request.AddJsonBody(dto);

				IRestResponse<List<DebitoGerado>> response = client.Execute<List<DebitoGerado>>(request);
				var res = response.Data;
				dto.requestBody = response.Content;

				// Executar DE/PARA STATUS
				/*
                TXS_ST	TXS_DESCRICAO	Quando acontece?
                0	Aguardando envio do débito SAC	Criação da tupla
                1	Gerado aguardando pagamento	Resposta SAC_Gerar_Debito = 1, 2

                1 = Débito gerado com sucesso
                2 – Débito existente (não é erro, o SAC devolve o número do débito que já havia sido gerado para o quarteto  “CNPJ, SERVIÇO, N° PROTOCOLO, ANO PROTOCOLO”)
                2	Erro de Processamento SAC	Resposta SAC_Gerar_Debito = 0
                3	Erro de Comunicação	Resposta SAC_Gerar_Debito = 3

                Ou

                O CADSUF não conseguiu comunicar com SAC_Gerar_Debito
                4	Débito pago	Resposta SAC_Comunicar_Liquidação

                O SAC informa que o protocolo foi pago.
                5	Debito expirado	Resposta SAC_Comunicar_Expiração

                O SAC informa que o prazo para pagamento expirou e o protocolo não deve ser atendido.
                */
				if (!string.IsNullOrEmpty(response.ErrorMessage) && !(response.Content.Contains("Débito Existente")))
				{
					throw new TimeoutException("Falha na comunicação API SAC");
				}

				if (res == null || res.Count == 0)
				{
					dto.MensagemErro = "Sac retornou objeto NULL";
					dto.StatusDePara = CrossCutting.DataTransferObject.Enum.EnumStatusTaxaServico.ErroComunicacao;
					return dto;
				}

				if (response.ResponseStatus == ResponseStatus.Error)
				{

					dto.MensagemErro = "Sac retornou objeto erro";
					dto.StatusDePara = CrossCutting.DataTransferObject.Enum.EnumStatusTaxaServico.ErroComunicacao;
					return dto;

				}

				//se nao existir, na lista do sac, registro com valor da capa diferente de 0 
				// entao retorne algum registro com tipoDebitoRetorno == 2 (debito existente)
				//senao retorne o registro com valor da capa != 0
				DebitoGerado regDebitoCapa = res.Find(x => x.ValorCapa != 0) == null  
											? res.Find(q => q.TipoDebitoRetorno == 2) 
											: res.Find(x => x.ValorCapa != 0); 

				if (regDebitoCapa?.TipoDebitoRetorno == 0)
				{
					dto.StatusDePara = CrossCutting.DataTransferObject.Enum.EnumStatusTaxaServico.ErroProcessamentoSac;
				}
				else if (regDebitoCapa?.TipoDebitoRetorno == 1 || regDebitoCapa?.TipoDebitoRetorno == 2)
				{
					//se 
					if (res.Find(x => x.ValorCapa != 0) == null 
						&& 
						res.Find(q => q.TipoDebitoRetorno == 2) != null)
					{
						dto.Debito = res;
						foreach (var debito in dto.Debito)
						{
							if (debito.TipoDebitoRetorno == 2)
							{
								debito.ValorCapa = dto.ValorTcifPli;
								debito.ValorItens = dto.ValorTcifItens;
							}
						}
					}
					else
					{
						dto.Debito = res;
					}
					dto.StatusDePara = CrossCutting.DataTransferObject.Enum.EnumStatusTaxaServico.DebitoGeradoComSucesso;

				}
				else if (regDebitoCapa?.TipoDebitoRetorno == 3)
				{
					dto.StatusDePara = CrossCutting.DataTransferObject.Enum.EnumStatusTaxaServico.ErroComunicacao;
				}
				else
				{
					dto.StatusDePara = CrossCutting.DataTransferObject.Enum.EnumStatusTaxaServico.ErroProcessamentoSac;
					throw new ValidationException("Erro processamento Sac");

				}

				return dto;
			}
			catch (Exception ex)
			{
				if(ex.Message == "Erro processamento Sac")
				{
					dto.MensagemErro = ex.Message;
					throw;
				}
				else
				{
					dto.MensagemErro = ex.Message;
					dto.StatusDePara = CrossCutting.DataTransferObject.Enum.EnumStatusTaxaServico.ErroComunicacao;
				}
				
				return dto;
			}
		}
	}
}