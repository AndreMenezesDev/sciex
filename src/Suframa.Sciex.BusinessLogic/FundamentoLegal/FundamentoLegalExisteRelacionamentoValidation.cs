﻿using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;

namespace Suframa.Sciex.BusinessLogic
{
	public class FundamentoLegalExisteRelacionamentoValidation : FundamentoLegalValidation<FundamentoLegalDto>
	{
		public FundamentoLegalExisteRelacionamentoValidation()
		{
			ValidarFundamentoLegalExisteRelacionamento();
		}
	}
}