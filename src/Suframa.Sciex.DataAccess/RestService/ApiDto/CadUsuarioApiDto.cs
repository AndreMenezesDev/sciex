using System;

namespace Suframa.Sciex.DataAccess.RestService.ApiDto
{
    public class CadUsuarioApiDto
    {
        public string id { get; set; }

        public string perfil { get; set; }

        public string usuCpf { get; set; }

        public string usuEmail { get; set; }

        public string usuNm { get; set; }

		public int status { get; set; }

		public CadUsuarioApiDto()
        {
        }

        //public TipoNaturezauSUApiDto tipoNaturezaUsuario { get; set; }
    }
}