using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.DataAccess;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
    public class UFBll : IUFBll
    {
        private readonly IUnitOfWork _uow;

        public UFBll(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IEnumerable<UFDto> Listar()
        {
            var lista = _uow
                        .QueryStack
                        .UF
                        .Listar();

            return AutoMapper.Mapper.Map<IEnumerable<UFDto>>(lista);
        }
    }
}