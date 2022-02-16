using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.Dto
{
    public class MenuDto
    {
        public string Grupo { get; set; }
        public int? IdPapel { get; set; }
        public int? IdTipoUsuario { get; set; }
        public string Menu { get; set; }
        public int? MenuGrupoId { get; set; }
        public int? MenuId { get; set; }
        public string Rota { get; set; }
    }
}