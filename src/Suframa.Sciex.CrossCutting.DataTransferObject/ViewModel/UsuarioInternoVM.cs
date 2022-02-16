namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class UsuarioInternoVM
	{
		public string Cpf { get; set; }

		public string Cnpj { get; set; }

		public int? IdUnidadeCadastradora { get; set; }

		public int? IdUsuarioInterno { get; set; }

		public bool IsCoordenadorDescentralizada { get; set; }

		public bool IsCoordenadorGeralCOCAD { get; set; }

		public bool IsCoordenadorOutrasAreas { get; set; }

		public bool IsSuperintendenteAdjunto { get; set; }

		public bool IsTecnico { get; set; }

		public bool? IsUnidadeCadastradoraManaus { get; set; }

		public string Nome { get; set; }

		public string Setor { get; set; }

		public int? Status { get; set; }
	}
}