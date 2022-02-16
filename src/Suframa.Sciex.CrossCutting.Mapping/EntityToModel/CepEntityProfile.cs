using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping
{
    public class CepEntityProfile : Profile
    {
        public CepEntityProfile()
        {
            CreateMap<CepEntity, CepVM>();

            CreateMap<CepEntity, ManterEnderecoVM>()
                .ForMember(dest => dest.IdCep, opt => opt.MapFrom(src => src.IdCep))
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo.ToString().CepUnformat()))
                .ForMember(dest => dest.IdMunicipio, opt => opt.MapFrom(src => src.IdMunicipio))
                .ForMember(dest => dest.Endereco, opt => opt.MapFrom(src => src.Endereco))
                .ForMember(dest => dest.Logradouro, opt => opt.MapFrom(src => src.Logradouro))
                .ForMember(dest => dest.Bairro, opt => opt.MapFrom(src => src.Bairro))
                .ForMember(dest => dest.UF, opt => opt.MapFrom(src => src.Municipio.SiglaUF));

            CreateMap<CepEntity, CepDto>()
                .ForMember(dest => dest.IdCep, opt => opt.MapFrom(src => src.IdCep))
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.Codigo.ToString().CepUnformat()))
                .ForMember(dest => dest.IdMunicipio, opt => opt.MapFrom(src => src.IdMunicipio))
                .ForMember(dest => dest.Endereco, opt => opt.MapFrom(src => src.Endereco))
                .ForMember(dest => dest.Logradouro, opt => opt.MapFrom(src => src.Logradouro))
                .ForMember(dest => dest.Bairro, opt => opt.MapFrom(src => src.Bairro))
                .ForMember(dest => dest.DataInclusao, opt => opt.MapFrom(src => src.DataInclusao))
                .ForMember(dest => dest.DataAlteracao, opt => opt.MapFrom(src => src.DataAlteracao))
                .ForMember(dest => dest.Municipio, opt => opt.MapFrom(src => Mapper.Map<MunicipioEntity, MunicipioDto>(src.Municipio)));
        }
    }
}