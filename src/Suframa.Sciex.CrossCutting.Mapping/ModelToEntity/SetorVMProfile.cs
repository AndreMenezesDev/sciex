using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping.ViewModels
{
	public class SetorVMProfile : Profile
	{
		public SetorVMProfile()
		{
			CreateMap<ManterSetorEmpresarialVM, SetorEntity>()
			   .ForMember(dest => dest.IdSetor, opt => opt.MapFrom(src => src.IdSetor))
			   .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo))
			   .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao))
			   .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.Tipo))
			   .ForMember(dest => dest.DescricaoObservacao, opt => opt.MapFrom(src => src.Observacao))
			   .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
			   .ForMember(dest => dest.SetorAtividade, opt => opt.Ignore());
		}
	}
}