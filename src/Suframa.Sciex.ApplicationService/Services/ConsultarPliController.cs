using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class ConsultarPliController : ApiController
	{
		private readonly IConsultarPliBll _ConsultarPliBll;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="ConsultarPliBll"></param>
		public ConsultarPliController(IConsultarPliBll ConsultarPliBll)
		{
			_ConsultarPliBll = ConsultarPliBll;
		}
		
		/// <summary>Seleciona a ConsultarPli pelo ID</summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public PliVM Get(int id)
		{
			return _ConsultarPliBll.Selecionar(id);
		}
	

		/// <summary>Salva a ConsultarPli</summary>
		/// <param name="ConsultarPliVM"></param>
		/// <returns></returns>
		public PliVM Put([FromBody]PliVM PliVM)
		{
			_ConsultarPliBll.Salvar(PliVM);
			return PliVM;
		}


	}
}