using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.Dto
{
    public class CredenciadoConsultorWrapper
    {
        public CredenciadoConsultorDto credenciadoConsultor { get; set; }
        public EmpresaInscricaoApi empresaInscricao { get; set; } //"EmpresaInscricao"
                                                                  //"CredenciadoConsultorDto"
    }
}