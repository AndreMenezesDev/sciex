using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject
{
    public class PagedItems<T>
    {
        public IList<T> Items { get; set; }
        public long? Total { get; set; }
    }
}