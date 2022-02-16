using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using System.Collections.Generic;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel
{
	public class DadosCadastroVM
	{
		public AtividadesPessoaJuridicaVM AtividadesPessoaJuridica { get; set; }
		public IEnumerable<CampoSistemaVM> CamposSistema { get; set; }
		public DadosSolicitanteVM DadosSolicitante { get; set; }
		public DocumentosComprobatoriosVM DocumentosComprobatorios { get; set; }
		public IdentificacaoPessoaFisicaVM IdentificacaoPessoaFisica { get; set; }
		public IdentificacaoPessoaJuridicaVM IdentificacaoPessoaJuridica { get; set; }
		public int? IdProtocolo { get; set; }
		public int? IdRequerimento { get; set; }
		public EnumTipoRequerimento? IdTipoRequerimento { get; set; }
		public bool IsCredenciamento { get; set; }
		public bool IsInscricaoCadastral { get; set; }
		public bool IsInscricaoCadastralPessoaJuridica { get; set; }
		public PedidoCorrecaoVM[] PedidoCorrecao { get; set; }
		public IEnumerable<PendenciaCadastralVM> PendenciasCadastrais { get; set; }
		public ProtocoloVM Protocolo { get; set; }
		public IEnumerable<QuadroPessoalVM> QuadroPessoal { get; set; }
		public QuadrosAdministradoresVM QuadrosAdministradores { get; set; }
		public QuadrosSocietariosVM QuadrosSocietarios { get; set; }
		public IEnumerable<ResumoVM> Resumo { get; set; }
		public EnumTipoDadoCadastro TipoDadoCadastro { get; set; }
	}
}