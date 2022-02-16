using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;

namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class RequerimentoDocumentoEntityProfile : Profile
	{
		public RequerimentoDocumentoEntityProfile()
		{
			CreateMap<RequerimentoDocumentoEntity, DocumentoComprobatorioVM>()
				.ForMember(dest => dest.NomeArquivo, opt => opt.MapFrom(src => src.Arquivo.Nome))
				.ForMember(dest => dest.DescricaoTipoDocumento, opt => opt.MapFrom(src => src.TipoDocumento.Descricao))
				.ForMember(dest => dest.IsStatusInfoComplementar, opt => opt.MapFrom(src => src.TipoDocumento.StatusInformacaoComplementar))
				.ForMember(dest => dest.HoraExpedicao, opt => opt.MapFrom(src => src.DataExpedicao.HasValue && src.IdTipoDocumento != 7 ? src.DataExpedicao.Value.ToString("HH:mm:ss") : null));
		}
	}
}