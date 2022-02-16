using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.Dto
{
    public class UsuarioSaaDto : BaseDto
    {
        
		public string id { get; set; }

		public string usuNm { get; set; }

		public string usuCpf { get; set; }

		public int status { get; set; }

	}
}