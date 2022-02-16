namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class SetorArmazenamentoVM : PagedOptions
	{
		
		public int? Id { get; set; }

		public string Descricao { get; set; }

		public byte Status { get; set; }

		public int Codigo { get; set; }

		public byte[] RowVersion { get; set; }

		public int IdRecintoAlfandega { get; set; }

		#region Codigo Recinto Alfandega
		public int CodigoRecintoAlfandega { get; set; }
		#endregion

	}
}
