using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class MotivoController : ApiController
	{
		private readonly IMotivoBll _motivoBll;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="motivoBll"></param>
		public MotivoController(IMotivoBll motivoBll)
		{
			_motivoBll = motivoBll;
		}

		/// <summary>Seleciona a Motivo pelo ID</summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public MotivoVM Get(int id)
		{
			return _motivoBll.Selecionar(id);
		}

		/// <summary>Seleciona uma lista de Motivo</summary>
		/// <param name="motivoVM"></param>
		/// <returns></returns>
		public IEnumerable<MotivoVM> Get([FromUri]MotivoVM motivoVM)
		{
			return _motivoBll.Listar(motivoVM);
		}

		/// <summary>Salva a Motivo</summary>
		/// <param name="motivoVM"></param>
		/// <returns></returns>
		public MotivoVM Put([FromBody]MotivoVM motivoVM)
		{
			_motivoBll.Salvar(motivoVM);
			return motivoVM;
		}

		/// <summary>Deletar Motivo pelo ID</summary>
		/// <param name="id">ID Motivo</param>
		public void Delete(int id)
		{
			_motivoBll.Deletar(id);
		}
	}
}