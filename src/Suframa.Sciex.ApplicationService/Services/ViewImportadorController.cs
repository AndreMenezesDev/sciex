using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.Compressor;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service View Importador</summary>
	public class ViewImportadorController : ApiController
	{
		private readonly IViewImportadorBll _viewImportadorBll;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="viewImportadorBll"></param>
		public ViewImportadorController(IViewImportadorBll viewImportadorBll)
		{
			_viewImportadorBll = viewImportadorBll;
		}

		/// <summary>Seleciona o Importador pela INSCRICAO</summary>
		/// <param name="inscricao"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpGet]
		public ViewImportadorVM Get(string inscricao)
		{
			return _viewImportadorBll.SelecionarInscricao(Convert.ToInt32(inscricao));
		}
		
	}
}