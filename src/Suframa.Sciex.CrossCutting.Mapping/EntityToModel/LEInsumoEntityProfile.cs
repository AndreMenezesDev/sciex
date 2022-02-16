using AutoMapper;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Linq;


namespace Suframa.Sciex.CrossCutting.Mapping
{
	public class LEInsumoEntityProfile : Profile
	{
		public LEInsumoEntityProfile()
		{
			CreateMap<LEInsumoEntity, LEInsumoVM>()
				.ForMember(dest => dest.CodigoProdutoSuframa, opt => opt.MapFrom(src => src.LEProduto.CodigoProdutoSuframa));
				//.ForMember(dest => dest.IdPliMercadoria, opt => opt.MapFrom(src => src.PLIMercadoria.FirstOrDefault().IdPliMercadoria))
				//.ForMember(dest => dest.NumeroALIReferencia, opt => opt.MapFrom(src => src.PLIMercadoria.FirstOrDefault().Ali == null ? " " : src.PLIMercadoria.FirstOrDefault().Ali.NumeroAli.ToString()))
				//.ForMember(dest => dest.DescricaoAplicacao, opt => opt.MapFrom(src => src.PliAplicacao.Descricao))
				//.ForMember(dest => dest.DescricaoTipoDocumento, opt => opt.MapFrom(src => (src.TipoDocumento == 1) ? EnumPliTipoDocumento.NORMAL.ToString() : (src.TipoDocumento == 2) ? EnumPliTipoDocumento.SUBSTITUTIVO.ToString() : (src.TipoDocumento == 3) ? "RETIFICADORA" : ""))
				//.ForMember(dest => dest.CodigoPLIStatus, opt => opt.MapFrom(src => (int)src.StatusPli))
				//.ForMember(dest => dest.DescricaoStatus, opt => opt.MapFrom(src => ((EnumPliStatus)src.StatusPli).ToString().Replace("_", " ")))
				//.ForMember(dest => dest.CodigoPliAplicao, opt => opt.MapFrom(src => src.PliAplicacao.Codigo))
				//.ForMember(dest => dest.IsMercadorias, opt => opt.MapFrom(src => src.PLIMercadoria.Any() && src.StatusPli == (byte)EnumPliStatus.EM_ELABORAÇÃO))
				//.ForMember(dest => dest.DescricaoSetor, opt => opt.MapFrom(src => src.DescricaoSetor))
				//.ForMember(dest => dest.CodigoSetor, opt => opt.MapFrom(src => src.CodigoSetor))
				//.ForMember(dest => dest.NumCPFRepLegalSISCO, opt => opt.MapFrom(src => src.NumCPFRepLegalSISCO.CnpjCpfFormat()))
				//.ForMember(dest => dest.Cnpj, opt => opt.MapFrom(src => src.Cnpj.CnpjCpfFormat()))
				//.ForMember(dest => dest.QuantidadeMercadorias, opt => opt.MapFrom(src => src.PLIMercadoria.Count))
				//.ForMember(dest => dest.ValorTotalDolarMercadorias, opt => opt.MapFrom(src => src.PLIMercadoria.Any() ? String.Format("{0:n2}", src.PLIMercadoria.Sum(o => o.ValorTotalCondicaoVendaDolar)) : "0,00"))
				//.ForMember(dest => dest.ValorTotalRealMercadorias, opt => opt.MapFrom(src => src.PLIMercadoria.Any() ? String.Format("{0:n2}", src.PLIMercadoria.Sum(o => o.ValorTotalCondicaoVendaReal)) : "0,00"))
				//.ForMember(dest => dest.DataPliFormatada, opt => opt.MapFrom(src => src.DataCadastro.ToShortDateString()))
				//.ForMember(dest => dest.DataEnvioPliFormatada, opt => opt.MapFrom(src => src.DataEnvioPli == null ? "" : src.DataEnvioPli.Value.ToShortDateString()))
				//.ForMember(dest => dest.StatusTaxa, opt => opt.MapFrom(src => src.TaxaPli.StatusTaxa))
				//.ForMember(dest => dest.QuantidadeErroProcessamento, opt => opt.MapFrom(src => src.ErroProcessamento.Count))
				//.ForMember(dest => dest.StatusALI, opt => opt.MapFrom(src => src.PLIMercadoria.Count() > 1 ? src.PLIMercadoria.Max(o => o.Ali.Status) : src.PLIMercadoria.FirstOrDefault().Ali.Status))
				//.ForMember(dest => dest.DataProcessamento, opt => opt.MapFrom(src => src.PLIMercadoria.FirstOrDefault().Ali == null ? null : src.PLIMercadoria.FirstOrDefault().Ali.DataCadastro.ToShortDateString()))
				//.ForMember(dest => dest.AnaliseVisualStatus, opt => opt.MapFrom(src => src.PliAnaliseVisual == null ? null : src.PliAnaliseVisual.StatusAnalise))
				//.ForMember(dest => dest.AnaliseVisualStatusFormatado, opt => opt.MapFrom(src => src.PliAnaliseVisual == null ? null : src.PliAnaliseVisual.StatusAnalise == 2 ? "EM ANÁLISE VISUAL" : src.PliAnaliseVisual.StatusAnalise == 9 ? "ANÁLISE VISUAL PENDENTE" : " - "))
				//.ForMember(dest => dest.DescricaoMotivo, opt => opt.MapFrom(src => src.PliAnaliseVisual == null ? null : src.PliAnaliseVisual.DescricaoMotivo))
				//.ForMember(dest => dest.DescricaoResposta, opt => opt.MapFrom(src => src.PliAnaliseVisual == null ? null : src.PliAnaliseVisual.DescricaoResposta))
				//.ForMember(dest => dest.AnalistaDesignado, opt => opt.MapFrom(src => src.PliAnaliseVisual == null ? null : String.IsNullOrEmpty(src.PliAnaliseVisual.NomeAnalista) ? " - " : src.PliAnaliseVisual.NomeAnalista))
				//.ForMember(dest => dest.NomeAnexo, opt => opt.MapFrom(src => src.NomeAnexo))
				//.ForMember(dest => dest.Anexo, opt => opt.MapFrom(src => src.Anexo == null ? null : src.Anexo));
			//.ForMember(dest => dest.IdUtilizadaDI, opt => opt.MapFrom(src => src.PLIMercadoria.FirstOrDefault().Li == null ? "Não" : "Sim"))
			//.ForMember(dest => dest.NumeroDI, opt => opt.MapFrom(src => src.PLIMercadoria.FirstOrDefault().Li.NumeroLi));
		}
	}
}
