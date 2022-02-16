using System;
using System.Collections.Generic;
using System.Linq;

namespace Suframa.Sciex.DataAccess.RestService.ApiDto
{
    public class DefaultLegadoResApiDto
    {
        public string dataHora { get; set; }
        public IEnumerable<string> execucaoResultado { get; set; } = Enumerable.Empty<string>();
        public string inscricao { get; set; }
        public string requestBody { get; set; }
        public string status { get; set; }
    }
}