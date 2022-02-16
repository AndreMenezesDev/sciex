using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suframa.Sciex.DataAccess.Database.Entities
{
	[Table("SCIEX_ESTRUTURA_PROPRIA")]
	public class EstruturaPropriaPliEntity : BaseEntity
	{
		public virtual EstruturaPropriaPliArquivoEntity EstruturaPropriaPliArquivo { get; set; }
		public virtual EstruturaPropriaLEEntity EstruturaPropriaLE { get; set; }
		public virtual ICollection<SolicitacaoPliEntity> SolicitacaoPli { get; set; }
		public virtual ICollection<PliEntity> PliEntity { get; set; }

		public EstruturaPropriaPliEntity()
		{
			SolicitacaoPli = new HashSet<SolicitacaoPliEntity>();
			PliEntity = new HashSet<PliEntity>();
		}

		[Key]
		[Column("ESP_ID")]
		public long IdEstruturaPropria { get; set; }

		[Required]
		[Column("ESP_DH_ENVIO")]
		public DateTime DataEnvio { get; set; }

		[Column("ESP_DH_INICIO_PROCESSAMENTO")]
		public DateTime? DataInicioProcessamento { get; set; }

		[Column("ESP_NU_PROTOCOLO")]
		public int? NumeroProtocolo { get; set; }

		[Column("ESP_DH_FIM_PROCESSAMENTO")]
		public DateTime? DataFimProcessamento { get; set; }

		[Column("ESP_ST_PROCESSAMENTO_ARQUIVO")]
		public byte? StatusProcessamentoArquivo { get; set; }

		[Column("ESP_QT_PLI_ARQUIVO")]
		public short? QuantidadePLIArquivo { get; set; }

		[Column("ESP_QT_PLI_PROCESSADO_SUCESSO")]
		public short QuantidadePLIProcessadoSucesso { get; set; }

		[Column("ESP_QT_PLI_PROCESSADO_FALHA")]
		public short QuantidadePLIProcessadoFalha { get; set; }

		[Column("ESP_NU_VERSAO_ESTRUTURA")]
		[StringLength(3)]
		public string VersaoEstrutura { get; set; }

		[Column("ESP_NU_LOGIN_USUARIO_ENVIO")]
		[StringLength(14)]
		public string LoginUsuarioEnvio { get; set; }

		[Column("ESP_NO_USUARIO_ENVIO")]
		[StringLength(100)]
		public string NomeUsuarioEnvio { get; set; }

		[Column("ESP_NU_LOGIN_USUARIO_IMPORTADOR")]
		[StringLength(14)]
		public string CNPJImportador { get; set; }

		[Column("ESP_NO_RAZAO_SOCIAL_IMPORTADOR")]
		[StringLength(100)]
		public string RazaoSocialImportador { get; set; }

		[Column("ESP_NU_INSCRICAO_CADASTRAL")]
		public int? InscricaoCadastral { get; set; }

		[Column("ESP_DS_NOME_ARQUIVO_ENVIO")]
		[StringLength(100)]
		public string NomeArquivoEnvio { get; set; }

		[Column("ESP_ST_PLI_TECNOLOGIA_ASSISTIVA")]
		public byte? StatusPLITecnologiaAssistiva { get; set; }

		[Column("ESP_DS_PENDENCIA_IMPORTADOR")]
		[StringLength(255)]
		public string DescricaoPendenciaImportador { get; set; }

		[Column("ESP_ST_ULTIMO_ARQ_PROCESSAMENTO")]
		public byte StatusUltimoArquivoProcessamento { get; set; }

		[Column("ESP_TP_ARQUIVO")]
		public decimal TipoArquivo { get; set; }
	}
}
