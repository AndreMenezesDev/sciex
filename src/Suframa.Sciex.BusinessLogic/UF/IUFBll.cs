using Suframa.Sciex.CrossCutting.DataTransferObject;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
    public interface IUFBll
    {
        IEnumerable<UFDto> Listar();
    }
}