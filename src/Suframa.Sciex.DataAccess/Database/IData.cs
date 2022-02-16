using System;

namespace Suframa.Sciex.DataAccess.Database
{
    public interface IData
    {
        DateTime DataAlteracao { get; set; }
        DateTime DataInclusao { get; set; }
    }
}