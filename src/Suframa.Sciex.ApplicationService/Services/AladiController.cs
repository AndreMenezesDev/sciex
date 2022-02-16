using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class AladiController : ApiController
	{
		private readonly IAladiBll _aladiBll;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="aladiBll"></param>
		public AladiController(IAladiBll aladiBll)
		{
			_aladiBll = aladiBll;
		}
		
		/// <summary>Seleciona a Aladi pelo ID</summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public AladiVM Get(int id)
		{
			return _aladiBll.Selecionar(id);
		}

		/// <summary>Seleciona uma lista de Aladi</summary>
		/// <param name="aladiVM"></param>
		/// <returns></returns>
		public IEnumerable<AladiVM> Get([FromUri]AladiVM aladiVM)
		{
			return _aladiBll.Listar(aladiVM);
		}

		/// <summary>Salva a Aladi</summary>
		/// <param name="aladiVM"></param>
		/// <returns></returns>
		public AladiVM Put([FromBody]AladiVM aladiVM)
		{
			_aladiBll.Salvar(aladiVM);
			return aladiVM;
		}

		/// <summary>Deletar Aladi pelo ID</summary>
		/// <param name="id">ID Aladi</param>
		public void Delete(int id)
		{
			_aladiBll.Deletar(id);
		}

		/// <summary></summary>
		/// <param name="valor"></param>
		/// <returns></returns>
		public string Get(string valor)
		{
			int contadorPonto = 0;
			int contadorVirgula = 0;

			string valorFormatado = "";
			string valorformatar = "";

			for (int i = 0; i < valor.Length; i++)
			{
				if (valor[i] == '.')
				{
					contadorPonto = contadorPonto + 1;
				}

				if (valor[i] == ',')
				{
					contadorVirgula = contadorVirgula + 1;
				}
			}

			if (contadorPonto == 1 && contadorVirgula == 0)
			{
				valorformatar = valor.Replace(".", ",");
			}

			if (contadorPonto >= 1 && contadorVirgula == 1)
			{
				valorformatar = valor.Replace(".", "");
			}

			if (contadorPonto == 0 && contadorVirgula == 1)
			{
				valorformatar = valor;
			}
						
			string[] dados = valorformatar.Split(',');
			dados[1] = dados[1].PadRight(5,'0').Substring(0, 5);

			valorFormatado = Convert.ToDecimal(dados[0] + "," + dados[1]).ToString("N5");

			return valorFormatado;
		}

	}
}