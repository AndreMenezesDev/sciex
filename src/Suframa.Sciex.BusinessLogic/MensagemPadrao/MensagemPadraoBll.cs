using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using System.Collections.Generic;

namespace Suframa.Sciex.BusinessLogic
{
    public class MensagemPadraoBll : IMensagemPadraoBll
    {
        private readonly IUnitOfWork _uow;

        public MensagemPadraoBll(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IEnumerable<MensagemPadraoVM> Listar(MensagemPadraoVM mensagemPadraoVM)
        {
            var lista = _uow.QueryStack.MensagemPadrao.Listar<MensagemPadraoVM>(x =>
               x.Status == (int)EnumStatus.Ativo &&
               (!mensagemPadraoVM.TipoGrupo.HasValue || x.TipoGrupo == (int)mensagemPadraoVM.TipoGrupo)
           );
            return AutoMapper.Mapper.Map<IEnumerable<MensagemPadraoVM>>(lista);
        }
    }
}