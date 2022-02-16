using FluentValidation;
using NLog;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using Suframa.Sciex.DataAccess.RestService;
using Suframa.Sciex.DataAccess.RestService.ApiDto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Suframa.Sciex.BusinessLogic
{
	public class RepresentacaoBll : IRepresentacaoBll
	{				
		private readonly IUnitOfWorkSciex _uow;

		public RepresentacaoBll(IUnitOfWorkSciex uow)
		{
			_uow = uow;			
		}

		public IEnumerable<RepresentacaoVM> Listar(RepresentacaoVM representacaoVM)
		{
			/*var representacao = _uow.QueryStackSciex.Representacao.Listar<RepresentacaoVM>()
				.Where(o => o.CPF == representacaoVM.CPF);
			return AutoMapper.Mapper.Map<IEnumerable<RepresentacaoVM>>(representacao);*/

			return null;
		}
	}
}