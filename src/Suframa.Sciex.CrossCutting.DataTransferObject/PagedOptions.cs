using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject
{
    /// <summary>
    /// Utilize a propriedade Sort e Reverse ou a coleção SortManny O uso das duas simultaneamente
    /// não é permitido
    /// </summary>
    public class PagedOptions
    {
        public int? Page { get; set; } = 1;
        public bool Reverse { get; set; }
        public int? Size { get; set; } = 10;
        public string Sort { get; set; }
        public IEnumerable<SortOptions> SortManny { get; set; }
    }
}