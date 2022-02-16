using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;

namespace Suframa.Sciex.CrossCutting.DataTransferObject
{
	public class HistoricoNcmDto : BaseDto
	{
		public PagedItems<AuditoriaVM> ListaHistorico { get; set; }
		public string NomeEmpresa { get; set; }
		public string CodigoNcm { get; set; }
	}
}