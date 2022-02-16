using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.Validation;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class FuncoesController : ApiController
	{
		
		/// <summary>Construtor para injetar as dependências</summary>
		public FuncoesController()
		{
			
		}

		/// <summary></summary>
		/// <param name="valor">o valor que será formatado</param>
		/// <param name="mascara">quantidade após a virgula (5 ou 7)</param>
		/// <param name="tamanhoMascara">tamanho do valor aceito antes da virgula</param>
		/// <returns>valor formatado conforme a mascara passada</returns>
		public string Get(string valor, int mascara, int tamanhoMascara)
		{
			Funcoes objFuncoes = new Funcoes();			
			return objFuncoes.FormatarMascaraValor(valor, mascara, tamanhoMascara);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="valor1"></param>
		/// <param name="valor2"></param>
		/// <returns></returns>
		public ValoresVM Get(string valor1, string valor2)
		{
			Funcoes objFuncoes = new Funcoes();
			return objFuncoes.Valor(valor1, valor2);
		}

	}
}