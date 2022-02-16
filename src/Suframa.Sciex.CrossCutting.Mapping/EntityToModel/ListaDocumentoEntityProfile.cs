using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.Entities
{
    public class ListaDocumentoEntityProfile : Profile
    {
        public ListaDocumentoEntityProfile()
        {
            CreateMap<ListaDocumentoEntity, TipoDocumentoVM>()
               .ForMember(dest => dest.IdTipoDocumento, opt => opt.MapFrom(src => src.IdTipoDocumento))
               .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.TipoDocumento.Descricao))
               .ForMember(dest => dest.Link, opt => opt.MapFrom(src => src.TipoDocumento.Link))
               .ForMember(dest => dest.TipoOrigem, opt => opt.MapFrom(src => (int)src.TipoDocumento.TipoOrigem))
               .ForMember(dest => dest.StatusInformacaoComplementar, opt => opt.MapFrom(src => src.TipoDocumento.StatusInformacaoComplementar));
        }
    }
}