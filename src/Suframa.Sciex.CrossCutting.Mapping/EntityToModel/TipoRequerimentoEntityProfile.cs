using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.Entities
{
    public class TipoRequerimentoEntityProfile : Profile
    {
        public TipoRequerimentoEntityProfile()
        {
            CreateMap<TipoRequerimentoEntity, TipoRequerimentoVM>();
        }
    }
}