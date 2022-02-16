using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Data.SqlClient;
using System.Net.Http;
using System.Configuration;
using Suframa.Sciex.BusinessLogic.Pss;
using System.Threading.Tasks;

namespace Suframa.Sciex.BusinessLogic
{
	public class IncluirDetalheInsumoBll : IIncluirDetalheInsumoBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;

		public IncluirDetalheInsumoBll(IUnitOfWorkSciex uowSciex)
		{
			_uowSciex = uowSciex;
		}



	}
}
