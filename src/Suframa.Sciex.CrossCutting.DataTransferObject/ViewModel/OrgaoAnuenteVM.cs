using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using System;
using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
    public class OrgaoAnuenteVM
	{
		public int? IdOrgaoAnuente { get; set; }
		public string OrgaoSigla { get; set; }
		public string Descricao { get; set; }
		public string Cnpj { get; set; }
		
		// complemento de classe
		public int? Id { get; set; }
	}
}