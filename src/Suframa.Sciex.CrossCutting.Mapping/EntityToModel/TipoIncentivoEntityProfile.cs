using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.Entities
{
    public class TipoIncentivoEntityProfile : Profile
    {
        public TipoIncentivoEntityProfile()
        {
            CreateMap<TipoIncentivoEntity, TipoIncentivoVM>();

            CreateMap<TipoIncentivoEntity, InscricaoCadastralTipoIncentivoVM>();
        }
    }
}