using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping
{
    public class AtividadeEconomicaEntityProfile : Profile
    {
        public AtividadeEconomicaEntityProfile()
        {
            CreateMap<SubclasseAtividadeEntity, AtividadeEconomicaVM>()
            .ForMember(dest => dest.DescricaoSecao, opt => opt.MapFrom(src => src.ClasseAtividade.GrupoAtividade.DivisaoAtividade.SecaoAtividade.Descricao))
            .ForMember(dest => dest.DescricaoDivisao, opt => opt.MapFrom(src => src.ClasseAtividade.GrupoAtividade.DivisaoAtividade.Descricao))
            .ForMember(dest => dest.DescricaoGrupo, opt => opt.MapFrom(src => src.ClasseAtividade.GrupoAtividade.Descricao))
            .ForMember(dest => dest.DescricaoClasse, opt => opt.MapFrom(src => src.ClasseAtividade.Descricao))
            .ForMember(dest => dest.DescricaoSubClasse, opt => opt.MapFrom(src => src.Descricao))
            .ForMember(dest => dest.CodigoSecao, opt => opt.MapFrom(src => src.ClasseAtividade.GrupoAtividade.DivisaoAtividade.SecaoAtividade.Codigo))
            .ForMember(dest => dest.CodigoDivisao, opt => opt.MapFrom(src => src.ClasseAtividade.GrupoAtividade.DivisaoAtividade.Codigo))
            .ForMember(dest => dest.CodigoGrupo, opt => opt.MapFrom(src => src.ClasseAtividade.GrupoAtividade.Codigo))
            .ForMember(dest => dest.CodigoClasse, opt => opt.MapFrom(src => src.ClasseAtividade.Codigo))
            .ForMember(dest => dest.CodigoSubClasse, opt => opt.MapFrom(src => src.Codigo))
            .ForMember(dest => dest.IdSecao, opt => opt.MapFrom(src => src.ClasseAtividade.GrupoAtividade.DivisaoAtividade.SecaoAtividade.IdSecaoAtividade))
            .ForMember(dest => dest.IdDivisao, opt => opt.MapFrom(src => src.ClasseAtividade.GrupoAtividade.DivisaoAtividade.IdDivisaoAtividade))
            .ForMember(dest => dest.IdGrupo, opt => opt.MapFrom(src => src.ClasseAtividade.GrupoAtividade.IdGrupoAtividade))
            .ForMember(dest => dest.IdClasse, opt => opt.MapFrom(src => src.ClasseAtividade.IdClasseAtividade))
            .ForMember(dest => dest.IdSubClasse, opt => opt.MapFrom(src => src.IdSubclasseAtividade));
        }
    }
}