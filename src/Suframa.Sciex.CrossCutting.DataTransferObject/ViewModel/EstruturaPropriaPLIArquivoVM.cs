using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class EstruturaPropriaPLIArquivoVM : PagedOptions
	{
		public long IdEstruturaPropria { get; set; }
		public byte[] Arquivo { get; set; }

		//complementar
		public string NomeArquivo { get; set; }
		public bool TecnologiaAssistida { get; set; }
		public string VersaoEstrutura { get; set; }
        public string LocalPastaEstruturaArquivo { get; set; }

		//Complemento de classe
		public string NomeAnexo { get; set; }
		public byte[] Anexo { get; set; }
	}
}
