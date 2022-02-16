using Suframa.Sciex.BusinessLogic;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Suframa.Sciex.ApplicationService.DropDown
{
	/// <summary>Drop down de classe atividade</summary>
	public class LiStatusDropDownController : ApiController
	{
		/// <summary>Classe Atividade injetar regras de negócio dropdown tipo pli</summary>
		/// <param name="LiStatus"></param>
		public LiStatusDropDownController()
		{
		}

		/// <summary>Obter ID e Descrição da SubClasse Atividade filtrada</summary>
		/// <returns></returns>
		public IEnumerable<object> Get([FromUri]LiStatusVM pliTipoVM)
		{
			List<LiStatusVM> listLiTipo = new List<LiStatusVM>();
			listLiTipo.Add(new LiStatusVM() {Id = 0, Text = "Todos" });
			listLiTipo.Add(new LiStatusVM() { Id = 1, Text = "Deferida" });
			listLiTipo.Add(new LiStatusVM() { Id = 2, Text = "Cancelada" });
			listLiTipo.Add(new LiStatusVM() { Id = 3, Text = "Solicitada para Cancelamento" });
			listLiTipo.Add(new LiStatusVM() { Id = 4, Text = "Enviada para Cancelamento SISCOMEX" });

			return listLiTipo.ToList();
		}
	}
}