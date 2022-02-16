using Suframa.Sciex.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.BusinessLogic.DeferirPlanoExportacao;
using Suframa.Sciex.DataAccess.Database.Entities;
using Suframa.Sciex.BusinessLogic.Pss;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;

namespace Suframa.Sciex.BusinessLogic
{
	public static class  CalcularParidadeBll
	{
		public static decimal CalcularFatorConversao(int? codMoeda, IUnitOfWorkSciex _uowSciex)
		{
			int codigoDolar = 220;
			if (codMoeda != codigoDolar)
			{
				decimal fatorConvEmDolar = 0;
				decimal? fatorMoedaEstrangeira = 0;
				decimal? fatorMoedaDolar = 0;
				var dataHoje = DateTime.Now.Date;

				fatorMoedaEstrangeira = _uowSciex.QueryStackSciex.ParidadeValor.Selecionar
										(q => q.Moeda.CodigoMoeda == codMoeda && q.ParidadeCambial.DataParidade == dataHoje)?.Valor;
								
				fatorMoedaDolar = _uowSciex.QueryStackSciex.ParidadeValor.Selecionar
									(q => q.Moeda.CodigoMoeda == codigoDolar && q.ParidadeCambial.DataParidade == dataHoje)?.Valor;

				if(fatorMoedaEstrangeira == null || fatorMoedaDolar == null)
					return Decimal.MinValue; //CASO NAO EXISTA REGISTRO NA SCIEX_PARIDADE_VALOR PARA ESSA MOEDA NA DATA ATUAL.

				return fatorConvEmDolar = (decimal)fatorMoedaEstrangeira / (decimal)fatorMoedaDolar; 
			}
			else
			{
				return 1;
			}
		}
	}
}
