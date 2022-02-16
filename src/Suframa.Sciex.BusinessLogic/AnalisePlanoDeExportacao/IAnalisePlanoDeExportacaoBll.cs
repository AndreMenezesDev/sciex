using System;
using FluentValidation;
using NLog;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.BusinessLogic.Pss;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;

namespace Suframa.Sciex.BusinessLogic 
{ 
	public interface IAnalisePlanoDeExportacaoBll
	{
		PagedItems<PlanoExportacaoVM> ListarPaginado(PlanoExportacaoVM pagedFilter);
		AnalisarPlanoExportacaoVM SalvarAnalise(AnalisarPlanoExportacaoVM pagedFilter);
		AnalisarPlanoExportacaoVM IndeferirPlano(AnalisarPlanoExportacaoVM pagedFilter);
	}
}
