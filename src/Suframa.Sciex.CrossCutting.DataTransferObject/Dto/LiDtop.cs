using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.Dto
{
	public class LiDto : BaseDto
	{
		public long IdPli { get; set; }

		public long IdLiRef { get; set; }

		public string NumeroLiReferencia { get; set; }
	}
}