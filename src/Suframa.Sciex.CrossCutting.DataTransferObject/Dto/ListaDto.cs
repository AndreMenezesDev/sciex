using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.Dto
{
	public class ListaDto<T>
	{
		public IList<T> items { get; set; }
	}
}
