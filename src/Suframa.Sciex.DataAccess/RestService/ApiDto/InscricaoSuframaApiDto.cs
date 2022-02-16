using System;

namespace Suframa.Sciex.DataAccess.RestService.ApiDto
{
    public class InscricaoSuframaApiDto
    {
        public string cnpj { get; set; }
        public int inscsuf { get; set; }
        public long localidade { get; set; }
        public string nome { get; set; }
        public string setor { get; set; }
        public int situacao { get; set; }
        public string validade { get; set; }
    }
}