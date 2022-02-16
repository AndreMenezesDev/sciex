using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System.Linq;

namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class PessoaJuridicaAtividadeEntityProfile : Profile
	{
		public PessoaJuridicaAtividadeEntityProfile()
		{
			CreateMap<PessoaJuridicaAtividadeEntity, AtividadePessoaJuridicaVM>()
				.ForMember(dest => dest.DescricaoSetorEmpresarial, opt => opt.MapFrom(src => src.SubClasseAtividade.SetorAtividade.Where(f => f.IdSubclasseAtividade == src.IdSubclasseAtividade)
																																											.Select(s => s.Setor.Descricao)
																																											.ToArray()))
				.ForMember(dest => dest.TipoAtividade, opt => opt.MapFrom(src => src.SubClasseAtividade.Descricao))
				.ForMember(dest => dest.CodigoSetor, opt => opt.MapFrom(src => src.SubClasseAtividade.SetorAtividade.OrderBy(x => x.IdSetorAtividade).FirstOrDefault().Setor.Codigo))
				.ForMember(dest => dest.CodigoSubclasse, opt => opt.MapFrom(src => src.SubClasseAtividade.Codigo))
				.ForMember(dest => dest.CodigoSubClasseAtividade, opt => opt.MapFrom(src => src.SubClasseAtividade.Codigo));

			CreateMap<PessoaJuridicaAtividadeEntity, InscricaoCadastralAtividadeVM>()
				.ForMember(dest => dest.Codigo, opt => opt.MapFrom(src =>
					string.Format("{0}.{1}-{2}-{3}",
						(src.SubClasseAtividade != null && src.SubClasseAtividade.ClasseAtividade != null && src.SubClasseAtividade.ClasseAtividade.GrupoAtividade != null && src.SubClasseAtividade.ClasseAtividade.GrupoAtividade.DivisaoAtividade != null) ? src.SubClasseAtividade.ClasseAtividade.GrupoAtividade.DivisaoAtividade.Codigo.ToString().PadLeft(2, '0') : null,
						(src.SubClasseAtividade != null && src.SubClasseAtividade.ClasseAtividade != null && src.SubClasseAtividade.ClasseAtividade.GrupoAtividade != null) ? src.SubClasseAtividade.ClasseAtividade.GrupoAtividade.Codigo.ToString().PadLeft(2, '0') : null,
						(src.SubClasseAtividade != null && src.SubClasseAtividade.ClasseAtividade != null) ? src.SubClasseAtividade.ClasseAtividade.Codigo.ToString() : null,
						(src.SubClasseAtividade != null) ? src.SubClasseAtividade.Codigo.ToString().PadLeft(2, '0') : null
					)
				))
				.ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => (src.SubClasseAtividade != null) ? src.SubClasseAtividade.Descricao : null))
				.ForMember(dest => dest.IsExercida, opt => opt.MapFrom(src => (src.SubClasseAtividade != null) ? src.SubClasseAtividade.Status : false));
		}
	}
}