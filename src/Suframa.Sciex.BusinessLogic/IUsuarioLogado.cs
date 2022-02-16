using Suframa.Sciex.CrossCutting.DataTransferObject;

namespace Suframa.Sciex.BusinessLogic
{
	public interface IUsuarioLogado
	{
		TokenDto Usuario { get; set; }

		void Load();
	}
}