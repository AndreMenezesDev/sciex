using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Linq;

namespace Suframa.Sciex.CrossCutting.Mapping
{
    public class SubclasseAtividadeEntityProfile : Profile
    {
        public SubclasseAtividadeEntityProfile()
        {
            CreateMap<SubclasseAtividadeEntity, SubClasseAtividadeDto>()
                .ForMember(dest => dest.IdSubClasseAtividade, opt => opt.MapFrom(src => src.IdSubclasseAtividade))
                .ForMember(dest => dest.IdClasseAtividade, opt => opt.MapFrom(src => src.IdClasseAtividade))
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao))
                .ForMember(dest => dest.DataAlteracao, opt => opt.MapFrom(src => src.DataAlteracao))
                .ForMember(dest => dest.DataInclusao, opt => opt.MapFrom(src => src.DataInclusao))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.CodigoGrupoAtividade, opt => opt.MapFrom(src => src.ClasseAtividade.GrupoAtividade.Codigo))
                .ForMember(dest => dest.CodigoDivisaoAtividade, opt => opt.MapFrom(src => src.ClasseAtividade.GrupoAtividade.DivisaoAtividade.Codigo))
                .ForMember(dest => dest.CodigoClasseAtividade, opt => opt.MapFrom(src => src.ClasseAtividade.Codigo))
                .ForMember(dest => dest.DescricaoSetorEmpresarial, opt => opt.MapFrom(src =>
                    src.SetorAtividade
                            .Where(f => f.IdSubclasseAtividade == src.IdSubclasseAtividade)
                            .Select(s => s.Setor.Descricao)
                            .ToArray()
                            ));

            CreateMap<SubclasseAtividadeEntity, ManterAtividadeEconomicaVM>()
                .ForMember(dest => dest.IdSubClasseAtividade, opt => opt.MapFrom(src => src.IdSubclasseAtividade))
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.IdClasseAtividade, opt => opt.MapFrom(src => src.ClasseAtividade.IdClasseAtividade))
                .ForMember(dest => dest.IdGrupoAtividade, opt => opt.MapFrom(src => src.ClasseAtividade.GrupoAtividade.IdGrupoAtividade))
                .ForMember(dest => dest.IdDivisaoAtividade, opt => opt.MapFrom(src => src.ClasseAtividade.GrupoAtividade.DivisaoAtividade.IdDivisaoAtividade))
                .ForMember(dest => dest.IdSecaoAtividade, opt => opt.MapFrom(src => src.ClasseAtividade.GrupoAtividade.DivisaoAtividade.SecaoAtividade.IdSecaoAtividade))
                .ForMember(dest => dest.CodigoGrupoAtividade, opt => opt.MapFrom(src => src.ClasseAtividade.GrupoAtividade.Codigo))
                .ForMember(dest => dest.CodigoDivisaoAtividade, opt => opt.MapFrom(src => src.ClasseAtividade.GrupoAtividade.DivisaoAtividade.Codigo))
                .ForMember(dest => dest.CodigoClasseAtividade, opt => opt.MapFrom(src => src.ClasseAtividade.Codigo));
        }
    }
}