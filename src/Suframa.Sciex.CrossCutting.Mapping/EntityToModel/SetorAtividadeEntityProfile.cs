using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping
{
    public class SetorAtividadeEntityProfile : Profile
    {
        public SetorAtividadeEntityProfile()
        {
            CreateMap<SetorAtividadeEntity, SetorAtividadeDto>()
               .ForMember(dest => dest.IdSetor, opt => opt.MapFrom(src => src.IdSetor))
               .ForMember(dest => dest.IdSetorAtividade, opt => opt.MapFrom(src => src.IdSetorAtividade))
               .ForMember(dest => dest.IdSubClasseAtividade, opt => opt.MapFrom(src => src.IdSubclasseAtividade));

            CreateMap<SetorAtividadeEntity, SetorAtividadeVM>()
               .ForMember(dest => dest.CodigoSecao, opt => opt.MapFrom(src => src.SubclasseAtividade.ClasseAtividade.GrupoAtividade.DivisaoAtividade.SecaoAtividade.Codigo))
               .ForMember(dest => dest.CodigoDivisao, opt => opt.MapFrom(src => src.SubclasseAtividade.ClasseAtividade.GrupoAtividade.DivisaoAtividade.Codigo))
               .ForMember(dest => dest.CodigoGrupo, opt => opt.MapFrom(src => src.SubclasseAtividade.ClasseAtividade.GrupoAtividade.Codigo))
               .ForMember(dest => dest.CodigoClasse, opt => opt.MapFrom(src => src.SubclasseAtividade.ClasseAtividade.Codigo))
               .ForMember(dest => dest.CodigoSubClasse, opt => opt.MapFrom(src => src.SubclasseAtividade.Codigo))
               .ForMember(dest => dest.DescricaoClasse, opt => opt.MapFrom(src => src.SubclasseAtividade.ClasseAtividade.Descricao))
               .ForMember(dest => dest.IdSetorAtividade, opt => opt.MapFrom(src => src.IdSetorAtividade))
               .ForMember(dest => dest.IdSetorEmpresarial, opt => opt.MapFrom(src => src.IdSetor))
               .ForMember(dest => dest.IdSubClasse, opt => opt.MapFrom(src => src.IdSubclasseAtividade));
        }
    }
}