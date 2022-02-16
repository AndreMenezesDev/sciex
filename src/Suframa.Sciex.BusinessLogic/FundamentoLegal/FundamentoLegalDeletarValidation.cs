﻿using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;

namespace Suframa.Sciex.BusinessLogic
{
	public class FundamentoLegalDeletarValidation : FundamentoLegalValidation<FundamentoLegalDto>
	{
		public FundamentoLegalDeletarValidation()
		{
			ValidarFundamentoLegalExcluir();
		}
	}
}