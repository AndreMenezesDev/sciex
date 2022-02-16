﻿using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Grid
{
	public class NcmHistoricoGridController : ApiController
	{
		private readonly IAuditoriaBll _auditoriaBll;

		/// <summary>ALADI injetar regras de negócio</summary>
		/// <param name="ncmAmazoniaOcidentalBll"></param>
		public NcmHistoricoGridController(IAuditoriaBll auditoriaBll)
		{
			_auditoriaBll = auditoriaBll;
		}

		/// <summary>Obter dados para o grid de ncmAmazoniaOcidental paginado e filtrado</summary>
		/// <param name="pagedFilter">filtro de ncmAmazoniaOcidental</param>
		/// <returns></returns>
		
		
		public PagedItems<AuditoriaVM> Get([FromUri]NcmVM ncmVM)
		{
			return _auditoriaBll.ListarPorIdNcm(ncmVM);
		}
	}
}