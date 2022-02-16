using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;
using Suframa.Sciex.DataAccess;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
    public class CampoSistemaBll : ICampoSistemaBll
    {
        private readonly IUnitOfWork _uow;

        public CampoSistemaBll(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IEnumerable<CampoSistemaDto> Listar()
        {
            var camposSistema = _uow.QueryStack.CampoSistema.Listar();

            return AutoMapper.Mapper.Map<IEnumerable<CampoSistemaDto>>(camposSistema);
        }
    }
}