using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Linq;

namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class ControleImportacaoEntityProfile : Profile
	{
		public ControleImportacaoEntityProfile()
		{
			CreateMap<ControleImportacaoEntity, ControleImportacaoVM>()
				.ForMember(dest => dest.CodigodaConta, opt => opt.MapFrom(src => src.CodigoConta.Codigo))
				.ForMember(dest => dest.DescricaoCodigoConta, opt => opt.MapFrom(src => src.CodigoConta.Codigo.ToString().Length == 1 ? "0" + src.CodigoConta.Codigo.ToString() + " - " + src.CodigoConta.Descricao.ToUpper() : src.CodigoConta.Codigo.ToString() + " - " + src.CodigoConta.Descricao.ToUpper()))
				.ForMember(dest => dest.DescricaoCodigoUtilizacao, opt => opt.MapFrom(src => src.CodigoUtilizacao.Codigo.ToString().Length == 1 ? "0" + src.CodigoUtilizacao.Codigo.ToString() + " - " + src.CodigoUtilizacao.Descricao.ToUpper() : src.CodigoUtilizacao.Codigo.ToString() + " - " + src.CodigoUtilizacao.Descricao.ToUpper()))
				.ForMember(dest => dest.DescricaoPliAplicacao, opt => opt.MapFrom(src => src.PliAplicacao.Descricao.ToUpper()))
				.ForMember(dest => dest.DescricaoSetor, opt => opt.MapFrom(src => src.DescricaoSetor.ToUpper()))
				.ForMember(dest => dest.DescricaoPliAplicacao, opt => opt.MapFrom(src => src.PliAplicacao.Descricao.ToUpper()));
		}
	}
}