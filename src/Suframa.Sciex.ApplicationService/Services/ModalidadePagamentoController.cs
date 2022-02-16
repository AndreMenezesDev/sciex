using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.Services
{
	/// <summary>Service Envia Recurso</summary>
	public class ModalidadePagamentoController : ApiController
	{
		private readonly IModalidadePagamentoBll _modalidadePagamentoBll;

		/// <summary>Construtor para injetar as dependências</summary>
		/// <param name="modalidadePagamentoBll"></param>
		public ModalidadePagamentoController(IModalidadePagamentoBll modalidadePagamentoBll)
		{
			_modalidadePagamentoBll = modalidadePagamentoBll;
		}

		/// <summary>Seleciona a ModalidadePagamento pelo ID</summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public ModalidadePagamentoVM Get(int id)
		{
			return _modalidadePagamentoBll.Selecionar(id);
		}

		/// <summary>Seleciona uma lista de ModalidadePagamento</summary>
		/// <param name="modalidadePagamentoVM"></param>
		/// <returns></returns>
		public IEnumerable<ModalidadePagamentoVM> Get([FromUri]ModalidadePagamentoVM modalidadePagamentoVM)
		{
			return _modalidadePagamentoBll.Listar(modalidadePagamentoVM);
		}

		/// <summary>Salva a ModalidadePagamento</summary>
		/// <param name="modalidadePagamentoVM"></param>
		/// <returns></returns>
		public ModalidadePagamentoVM Put([FromBody]ModalidadePagamentoVM modalidadePagamentoVM)
		{
			_modalidadePagamentoBll.Salvar(modalidadePagamentoVM);
			return modalidadePagamentoVM;
		}

		/// <summary>Deletar ModalidadePagamento pelo ID</summary>
		/// <param name="id">ID ModalidadePagamento</param>
		public void Delete(int id)
		{
			_modalidadePagamentoBll.Deletar(id);
		}
	}
}