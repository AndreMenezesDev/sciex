using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class NaladiController : ApiController
	{
		private readonly INaladiBll _naladiBll;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="naladiBll"></param>
		public NaladiController(INaladiBll naladiBll)
		{
			_naladiBll = naladiBll;
		}

		/// <summary>Seleciona a naladi pelo ID</summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public NaladiVM Get(int id)
		{
			return _naladiBll.Selecionar(id);
		}

		/// <summary>Seleciona uma lista de naladi</summary>
		/// <param name="naladiVM"></param>
		/// <returns></returns>
		public IEnumerable<NaladiVM> Get([FromUri]NaladiVM naladiVM)
		{
			return _naladiBll.Listar(naladiVM);
		}

		/// <summary>Salva a naladi</summary>
		/// <param name="naladiVM"></param>
		/// <returns></returns>
		public NaladiVM Put([FromBody]NaladiVM naladiVM)
		{
			_naladiBll.Salvar(naladiVM);
			return naladiVM;
		}

		/// <summary>Deletar naladi pelo ID</summary>
		/// <param name="id">ID naladi</param>
		public void Delete(int id)
		{
			_naladiBll.Deletar(id);
		}
	}
}