using System;

namespace Suframa.Sciex.DataAccess.RestService.ApiDto
{
    public class SituacaoUsuarioLegadoApiDto
    {
        public long dataHora { get; set; }
        public string dataHoraFormatado { get; set; }
        public string inscricao { get; set; }
        public int status { get; set; }
    }
}