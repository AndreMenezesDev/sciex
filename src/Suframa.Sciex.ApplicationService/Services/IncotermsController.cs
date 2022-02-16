using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class IncotermsController : ApiController
	{
		private readonly IIncotermsBll _incotermsBll;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="incotermsBll"></param>
		public IncotermsController(IIncotermsBll incotermsBll)
		{
			_incotermsBll = incotermsBll;
		}

		/// <summary>Seleciona a Incoterms pelo ID</summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public IncotermsVM Get(int id)
		{
			return _incotermsBll.Selecionar(id);
		}

		/// <summary>Seleciona uma lista de Incoterms</summary>
		/// <param name="incotermsVM"></param>
		/// <returns></returns>
		public IEnumerable<IncotermsVM> Get([FromUri]IncotermsVM incotermsVM)
		{
			return _incotermsBll.Listar(incotermsVM);
		}

		/// <summary>Salva a Incoterms</summary>
		/// <param name="incotermsVM"></param>
		/// <returns></returns>
		public IncotermsVM Put([FromBody]IncotermsVM incotermsVM)
		{
			_incotermsBll.Salvar(incotermsVM);
			return incotermsVM;
		}

		/// <summary>Deletar Incoterms pelo ID</summary>
		/// <param name="id">ID Incoterms</param>
		public void Delete(int id)
		{
			_incotermsBll.Deletar(id);
		}
	}
}