using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
    public interface ICampoSistemaBll
    {
        IEnumerable<CampoSistemaDto> Listar();
    }
}