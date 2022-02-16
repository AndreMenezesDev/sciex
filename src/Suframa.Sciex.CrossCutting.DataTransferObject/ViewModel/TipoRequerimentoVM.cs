namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class TipoRequerimentoVM
	{
		public string Descricao { get; set; }
		public int? IdServico { get; set; }
		public int IdTipoRequerimento { get; set; }
		public int? IdTipoUsuario { get; set; }
		public bool StatusExigeAnalise { get; set; }
		public bool StatusTipoCobranca { get; set; }
	}
}