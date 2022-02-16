using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class NaturezaJuridicaEntityProfile : Profile
	{
		public NaturezaJuridicaEntityProfile()
		{
			CreateMap<NaturezaJuridicaEntity, NaturezaJuridicaDto>()
				.ForMember(dest => dest.IdNaturezaJuridica, opt => opt.MapFrom(src => src.IdNaturezaJuridica))
				.ForMember(dest => dest.IdNaturezaGrupo, opt => opt.MapFrom(src => src.IdNaturezaGrupo))
				.ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo))
				.ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao))
				.ForMember(dest => dest.DataAlteracao, opt => opt.MapFrom(src => src.DataAlteracao))
				.ForMember(dest => dest.DataInclusao, opt => opt.MapFrom(src => src.DataInclusao))
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
				.ForMember(dest => dest.StatusQuadroSocial, opt => opt.MapFrom(src => src.StatusQuadroSocial))
				.ForMember(dest => dest.NaturezaGrupo, opt => opt.MapFrom(src => Mapper.Map<NaturezaGrupoEntity, NaturezaGrupoDto>(src.NaturezaGrupo)));

			CreateMap<NaturezaJuridicaEntity, ManterNaturezaJuridicaVM>()
				.ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo))
				.ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao))
				.ForMember(dest => dest.IdNaturezaGrupo, opt => opt.MapFrom(src => src.IdNaturezaGrupo))
				.ForMember(dest => dest.IdNaturezaJuridica, opt => opt.MapFrom(src => src.IdNaturezaJuridica))
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
				.ForMember(dest => dest.StatusQuadroSocial, opt => opt.MapFrom(src => src.StatusQuadroSocial));

			CreateMap<NaturezaJuridicaEntity, NaturezaJuridicaVM>()
				.ForMember(dest => dest.IdNaturezaJuridica, opt => opt.MapFrom(src => src.IdNaturezaJuridica))
				.ForMember(dest => dest.IdNaturezaGrupo, opt => opt.MapFrom(src => src.IdNaturezaGrupo))
				.ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao));
		}
	}
}