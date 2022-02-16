using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Drop down de classe atividade</summary>
	public class ModalidadePagamentoDropDownController : ApiController
	{
		private readonly IModalidadePagamentoBll _modalidadePagamentoBll;

		/// <summary>Classe Atividade injetar regras de negócio</summary>
		/// <param name="modalidadePagamento"></param>
		public ModalidadePagamentoDropDownController(IModalidadePagamentoBll modalidadePagamentoBll)
		{
			_modalidadePagamentoBll = modalidadePagamentoBll;
		}

		/// <summary>Obter ID e Descrição da SubClasse Atividade filtrada</summary>
		/// <returns></returns>
		public IEnumerable<object> Get([FromUri]ModalidadePagamentoVM modalidadePagamentoVM)
		{
			return _modalidadePagamentoBll.ListarChave(modalidadePagamentoVM);
		}
	}
}