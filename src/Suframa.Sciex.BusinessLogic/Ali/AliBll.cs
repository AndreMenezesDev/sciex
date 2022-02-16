using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.Ftp;
using Suframa.Sciex.CrossCutting.Texto;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Suframa.Sciex.BusinessLogic
{
	public class AliBll : IAliBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUnitOfWork _uowCadsuf;
		private readonly IUsuarioLogado _IUsuarioLogado;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;
		private readonly IViewImportadorBll _IViewImportadorBll;
		private readonly IComplementarPLIBll _complementarPLIBll;
		private readonly IControleExecucaoServicoBll _controleExecucaoServicoBll;
		private string CNPJ { get; set; }
		private String erroCampo { get; set; }
		public AliBll(IUnitOfWorkSciex uowSciex, IUnitOfWork uowCadsuf,
			IUsuarioLogado usuarioLogado, IViewImportadorBll viewImportadorBll, IComplementarPLIBll complementarPLIBll,
			IUsuarioInformacoesBll usuarioInformacoesBll, IControleExecucaoServicoBll controleExecucaoServicoBll)
		{
			_uowSciex = uowSciex;
			_uowCadsuf = uowCadsuf;
			_IUsuarioLogado = usuarioLogado;
			_IViewImportadorBll = viewImportadorBll;
			_complementarPLIBll = complementarPLIBll;
			_usuarioInformacoesBll = usuarioInformacoesBll;
			_controleExecucaoServicoBll = controleExecucaoServicoBll;
			this.CNPJ = _usuarioInformacoesBll.ObterCNPJ();
		}

		public IEnumerable<AliVM> Listar(AliVM aliVM)
		{
			var ali = _uowSciex.QueryStackSciex.Ali.Listar<AliVM>();
			return AutoMapper.Mapper.Map<IEnumerable<AliVM>>(ali);
		}

		public IEnumerable<object> ListarChave(AliVM aliVM)
		{
			var lista = _uowSciex.QueryStackSciex.Ali
				.Listar().Where(o =>
						(aliVM.IdPliMercadoria == -1 || o.IdPliMercadoria == aliVM.IdPliMercadoria)

					)
				.OrderBy(o => o.DataCadastro)
				.Select(
					s => new
					{
						id = s.NumeroAli,
						text = s.DataCadastro
					});
			return lista;
		}

		public PagedItems<AliVM> ListarPaginado(AliVM pagedFilter)
		{
			if (pagedFilter == null) { return new PagedItems<AliVM>(); }
			var ali = _uowSciex.QueryStackSciex.Ali.ListarPaginado<AliVM>(o =>
				(
				   pagedFilter.IdPliMercadoria == -1 || o.IdPliMercadoria == pagedFilter.IdPliMercadoria
				),
				pagedFilter);
			return ali;
		}

		public AliVM RegrasSalvar(AliVM aliVM)
		{
			var entityALI = AutoMapper.Mapper.Map<AliEntity>(aliVM);
			_uowSciex.CommandStackSciex.Ali.Salvar(entityALI);
			_uowSciex.CommandStackSciex.Save();

			var _aliVM = AutoMapper.Mapper.Map<AliVM>(entityALI);
			return _aliVM;
		}

		public AliVM Selecionar(int? numeroAli)
		{
			var aliVM = new AliVM();
			if (!numeroAli.HasValue) { return aliVM; }

			var ali = _uowSciex.QueryStackSciex.Ali.Selecionar(x => x.NumeroAli == numeroAli);
			if (ali == null) { return aliVM; }

			aliVM = AutoMapper.Mapper.Map<AliVM>(ali);
			return aliVM;
		}

		public AliVM Visualizar(AliVM aliVM)
		{
			var entity = _uowSciex.QueryStackSciex.Ali.Selecionar(x => x.NumeroAli == aliVM.NumeroAli);
			var retorno = AutoMapper.Mapper.Map<AliVM>(entity);
			return retorno;
		}

		public void Salvar(AliVM aliVM)
		{
			RegrasSalvar(aliVM);
		}

		public void Deletar(int id)
		{
			var ali = _uowSciex.QueryStackSciex.Ali.Selecionar(s => s.NumeroAli == id);
			if (ali != null)
			{
				_uowSciex.CommandStackSciex.Ali.Apagar(ali.NumeroAli);
			}
		}

		public DateTime GetDateTimeNowUtc()
		{
			var manausTime = TimeZoneInfo.ConvertTime(DateTime.Now,
				 TimeZoneInfo.FindSystemTimeZoneById("SA Western Standard Time"));

			return manausTime;

		}


		public void GerarAquivoEnvio()
		{
			#region RN05
			ControleExecucaoServicoEntity _controleExecucaoServicoEntity = new ControleExecucaoServicoEntity();
			_controleExecucaoServicoEntity.DataHoraExecucaoInicio = GetDateTimeNowUtc();
			_controleExecucaoServicoEntity.StatusExecucao = (int)EnumControleServico.ServicoIniciado;
			_controleExecucaoServicoEntity.NumeroCPFCNPJInteressado = "04407029000143";
			_controleExecucaoServicoEntity.NomeCPFCNPJInteressado = "Administrador do Sistema - SUFRAMA";
			_controleExecucaoServicoEntity.IdListaServico = (int)EnumListaServico.GerarArquivoALI;
			#endregion

			try
			{
				StringBuilder arquivoALI = new StringBuilder();

				List<long> _PLis = new List<long>();
				decimal _valorTotalReal = 0;
				decimal _valorTotalDolar = 0;

				#region RN01
				var listaALI = _uowSciex.QueryStackSciex.Ali.Listar(o => o.Status == (int)EnumAliStatus.ALI_GERADA && (o.TipoAli == 1 || o.TipoAli == 2 || o.TipoAli == 3) && o.IdAliArquivoEnvio.HasValue);
				#endregion


				if (listaALI.Any())
				{
					#region CABEÇALHO

					//TIPO DE REGISTRO 00

					//identificação SUBRAMA
					arquivoALI.Append("SU");

					//CNPJ da SUFRAMA
					arquivoALI.Append("04407029000143");

					//Dia Juliano
					string dataInicio = "01/01/" + GetDateTimeNowUtc().Year.ToString();
					string dataAtual = GetDateTimeNowUtc().Day.ToString() + "/" + GetDateTimeNowUtc().Month.ToString() + "/" + GetDateTimeNowUtc().Year.ToString();
					TimeSpan date = Convert.ToDateTime(dataAtual) - Convert.ToDateTime(dataInicio);
					arquivoALI.Append((date.Days + 1).ToString("D3"));

					//HORA
					arquivoALI.Append(GetDateTimeNowUtc().Hour.ToString("D2") + GetDateTimeNowUtc().Minute.ToString("D2") + GetDateTimeNowUtc().Second.ToString("D2"));

					//Indicador do processo de Host
					arquivoALI.Append("02");

					//Indicador LI
					arquivoALI.Append("02");

					//Indicador para obtenção de diagnóstico
					arquivoALI.Append("02");

					//Código da Transação
					arquivoALI.Append("850");

					//Dia Juliano
					arquivoALI.Append((date.Days + 1).ToString("D3"));

					//HORA
					arquivoALI.Append(GetDateTimeNowUtc().Hour.ToString("D2") + GetDateTimeNowUtc().Minute.ToString("D2") + GetDateTimeNowUtc().Second.ToString("D2"));

					arquivoALI.AppendLine();
					#endregion
					foreach (AliEntity itemAli in listaALI)
					{
						StringBuilder arquivoALI1 = new StringBuilder();
						try
						{
							var mercadoria = _uowSciex.QueryStackSciex.PliMercadoria.Selecionar(o => o.IdPliMercadoria == itemAli.IdPliMercadoria);

							if (mercadoria != null)
							{
								erroCampo = "Tabela Pli Mercadoria | IdPliMercadoria " + mercadoria.IdPliMercadoria.ToString() + " | Campo: IdPLI";
								PliEntity itemPLI = _uowSciex.QueryStackSciex.Pli.Selecionar(o => o.IdPLI == mercadoria.IdPLI);

								_valorTotalDolar += mercadoria.ValorTotalCondicaoVendaDolar.HasValue ? mercadoria.ValorTotalCondicaoVendaDolar.Value : 0;
								_valorTotalReal += mercadoria.ValorTotalCondicaoVendaReal.HasValue ? mercadoria.ValorTotalCondicaoVendaReal.Value : 0;

								var pliVM = new PliVM();
								erroCampo = "Tabela Pli | IdPli " + itemPLI.IdPLI.ToString() + " | Problema ao executar a ViewImportador (o => o.Cnpj == " + itemPLI.Cnpj + ")";
								var importador = _uowSciex.QueryStackSciex.ViewImportador.Selecionar(o => o.Cnpj == itemPLI.Cnpj);

								erroCampo = "Tabela Pli | IdPli " + itemPLI.IdPLI.ToString() + mercadoria.IdPliMercadoria.ToString() + " | Problema ao executar a ViewAtividadeEconomicaPrincipal (o => o.Cnpj == " + itemPLI.Cnpj + ")";
								var cnae = _uowCadsuf.QueryStack.ViewAtividadeEconomicaPrincipal.Selecionar(o => o.CNPJ == itemPLI.Cnpj);

								erroCampo = "Tabela Pli | IdPliAplicacao " + itemPLI.IdPLIAplicacao.ToString() + " | Problema ao selecionar a tabela ControleImportacao (o => o.IdPliAplicacao == " + itemPLI.IdPLIAplicacao.ToString() + ")";
								var uticon = _uowSciex.QueryStackSciex.ControleImportacao.Listar(o => o.IdPliAplicacao == itemPLI.IdPLIAplicacao).FirstOrDefault();

								erroCampo = "Tabela Pli | IdPLI " + itemPLI.IdPLI.ToString() + " | Problema ao selecionar PLI (o => o.IdPLI == " + itemPLI.IdPLI + ")";
								var pli = _uowSciex.QueryStackSciex.Pli.Selecionar(o => o.IdPLI == mercadoria.IdPLI);

								pliVM = AutoMapper.Mapper.Map<PliVM>(pli);
								pliVM.Endereco = importador.Endereco;
								pliVM.Numero = importador.Numero;
								pliVM.Complemento = importador.Complemento;
								pliVM.Bairro = importador.Bairro;
								erroCampo = "ViewImportador | CNPJ " + itemPLI.Cnpj.ToString() + " | Campo: CodigoMunicipio";
								pliVM.CodigoMunicipio = importador.CodigoMunicipio.ToString();

								pliVM.Municipio = importador.Municipio;
								pliVM.UF = importador.UF;

								erroCampo = "Tabela Pli | IdPli " + itemPLI.IdPLI.ToString() + " | ViewImportador | Campo: CEP";
								pliVM.CEP = string.Format("{0:00000-000}", importador.CEP);

								pliVM.DescricaoCNAE = cnae.Descricao;
								pliVM.PaisCodigo = importador.CodigoPais;
								pliVM.PaisDescricao = importador.DescricaoPais;
								pliVM.Telefone = (importador.Telefone.Length == 10 ? string.Format("{0:(00) 0000-0000}", Convert.ToDecimal(importador.Telefone)) : string.Format("{0:(##) #####-####}", Convert.ToDecimal(importador.Telefone)));

								erroCampo = "Tabela Pli | IdPli " + itemPLI.IdPLI.ToString() + " | Tabela CodigoUtilizacao | Campo: Codigo";
								pliVM.CodigoUtilizacao = uticon.CodigoUtilizacao.Codigo.ToString();

								erroCampo = "Tabela Pli | IdPli " + itemPLI.IdPLI.ToString() + " | Tabela CodigoUtilizacao | Campo: Descricao";
								pliVM.DescricaoUtilizacao = uticon.CodigoUtilizacao.Descricao.ToString();

								erroCampo = "Tabela Pli | IdPli " + itemPLI.IdPLI.ToString() + " | Tabela CodigoConta | Campo: Codigo";
								pliVM.CodigoConta = uticon.CodigoConta.Codigo.ToString();

								erroCampo = "Tabela Pli | IdPli " + itemPLI.IdPLI.ToString() + " | Tabela CodigoConta | Campo: Descricao";
								pliVM.DescricaoConta = uticon.CodigoConta.Descricao.ToString();

								//verifica a quantidade de PLIS existentes
								if (!_PLis.Exists(o => o == pliVM.IdPLI.Value))
								{
									_PLis.Add(pliVM.IdPLI.Value);
								}

								//CNPJ do IMPORTADOR
								long cnpj = Convert.ToInt64(pliVM.Cnpj.Replace(".", "").Replace("/", "").Replace("-", ""));

								//busca os dados do NALADI
								erroCampo = "Tabela Pli Mercadoria | IdPliMercadoria " + mercadoria.IdPliMercadoria.ToString() + " | Tabela Naladi | Campo: IdNaladi";
								var registroNALADI =
									AutoMapper.Mapper.Map<NaladiVM>(_uowSciex.QueryStackSciex.Naladi.Selecionar(o => o.IdNaladi == mercadoria.IdNaladi));

								//buscar modalidade de Pagamento
								erroCampo = "Tabela Pli Mercadoria | IdPliMercadoria " + mercadoria.IdPliMercadoria.ToString() + " | Tabela ModalidadePagamento | Campo: IdModalidadePagamento";
								var registroMP =
									AutoMapper.Mapper.Map<ModalidadePagamentoVM>(_uowSciex.QueryStackSciex.ModalidadePagamento.Selecionar(o => o.IdModalidadePagamento == mercadoria.IdModalidadePagamento));

								//buscar instituição financeira
								erroCampo = "Tabela Pli Mercadoria | IdPliMercadoria " + mercadoria.IdPliMercadoria.ToString() + " | Tabela InstituicaoFinanceira | Campo: IdInstituicaoFinanceira";
								var registroIF =
									AutoMapper.Mapper.Map<InstituicaoFinanceiraVM>(_uowSciex.QueryStackSciex.InstituicaoFinanceira.Selecionar(o => o.IdInstituicaoFinanceira == mercadoria.IdInstituicaoFinanceira));

								//buscar Motivo
								erroCampo = "Tabela Pli Mercadoria | IdPliMercadoria " + mercadoria.IdPliMercadoria.ToString() + " | Tabela Motivo | Campo: IdMotivo";
								var registroMotivo =
									AutoMapper.Mapper.Map<MotivoVM>(_uowSciex.QueryStackSciex.Motivo.Selecionar(o => o.IdMotivo == mercadoria.IdMotivo));

								//buscar Unidade de Despacho
								erroCampo = "Tabela Pli Mercadoria | IdPliMercadoria " + mercadoria.IdPliMercadoria.ToString() + " | Tabela UnidadeReceitaFederal | Campo: IdUnidadeReceitaFederal";
								var registroUnidadeDespacho =
									AutoMapper.Mapper.Map<UnidadeReceitaFederalVM>(_uowSciex.QueryStackSciex.UnidadeReceitaFederal.Selecionar(o => o.IdUnidadeReceitaFederal == mercadoria.IdURFDespacho));

								//buscar Lista de processos anuentes
								erroCampo = "Tabela Pli Mercadoria | IdPliMercadoria " + mercadoria.IdPliMercadoria.ToString() + " | Tabela PliProcessoAnuente | Campo: IdPliMercadoria";
								var registroProcessoAnuente =
									AutoMapper.Mapper.Map<IEnumerable<PliProcessoAnuenteVM>>(_uowSciex.QueryStackSciex.PliProcessoAnuente.Listar(o => o.IdPliMercadoria == mercadoria.IdPliMercadoria));

								#region TIPO DE REGISTRO 01 - INFORMAÇÕES GERAIS
								//tipo de registro
								arquivoALI1.Append("01");

								//numero da ALI
								erroCampo = "Tabela Pli Mercadoria | Campo: NumeroALI";
								arquivoALI1.Append(mercadoria.Ali.NumeroAli.ToString().PadRight(15, ' '));

								//Numero do protocolo
								arquivoALI1.Append("0000000000");

								//Código de autorização de transmissão
								arquivoALI1.Append("0");

								//Código do tipo de importador
								arquivoALI1.Append("1");

								//Identificação do importador
								arquivoALI1.Append(cnpj.ToString("D14"));

								//País do importador
								erroCampo = "Tabela Pli | IdPli " + pliVM.IdPLI.ToString() + " Campo: CodigoPais";
								arquivoALI1.Append("000");

								//Nome do Importador
								erroCampo = "Tabela Pli | IdPli " + pliVM.IdPLI.ToString() + " ViewImportador | Campo: RazaoSocial";
								arquivoALI1.Append(Texto.RemoveCaracteres(importador.RazaoSocial).PadRight(60, ' '));

								//Número do telefone do importador
								arquivoALI1.Append(pliVM.Telefone.Replace("(", "").Replace(")", "").PadRight(15, ' '));

								//Endereço do importador
								arquivoALI1.Append(Texto.RemoveCaracteres(pliVM.Endereco).PadRight(40, ' '));

								//Numero endereço do importador
								arquivoALI1.Append(pliVM.Numero.PadRight(6, ' '));

								//complemento endereço do importador
								arquivoALI1.Append((pliVM.Complemento != null ? Texto.RemoveCaracteres(pliVM.Complemento).PadRight(21, ' ') : "".PadRight(21, ' ')));

								//bairro endereço do importador
								arquivoALI1.Append(Texto.RemoveCaracteres(pliVM.Bairro).PadRight(25, ' '));

								//municipio endereço do importador
								arquivoALI1.Append(Texto.RemoveCaracteres(pliVM.Municipio).PadRight(25, ' '));

								//uf endereço do importador
								arquivoALI1.Append(pliVM.UF.PadRight(2, ' '));

								//cep endereço do importador
								erroCampo = "Tabela Pli | IdPli " + pliVM.IdPLI.ToString() + " ViewImportador | Campo: CEP";
								arquivoALI1.Append(importador.CEP.ToString());

								//Atividade econômica do importador
								arquivoALI1.Append(pliVM.CodigoCNAE.Substring(0, 4));

								//RFB de entrada da mercadoria
								erroCampo = "Tabela Pli Mercadoria | IdPliMercadoria " + mercadoria.IdPliMercadoria.ToString() + " Campo: EntradaMercadoria";
								arquivoALI1.Append(Convert.ToInt32(mercadoria.UnidadeReceitaFederalEntrada.Codigo).ToString("D7"));

								//busca os dados do fornecedor fabricante
								erroCampo = "Tabela Pli Mercadoria | IdPliMercadoria " + mercadoria.IdPliMercadoria.ToString() + " | Tabela Fornecedor | Campo: IdFornecedor";
								var registroFornecedorFabricante =
									AutoMapper.Mapper.Map<PliFornecedorFabricanteVM>(_uowSciex.QueryStackSciex.PliFornecedorFabricante.Selecionar(o => o.IdPliMercadoria == mercadoria.IdPliMercadoria));

								#region regra nova

								#region DADOS FORNECEDOR

								//nome do fornecedor estrangeiro
								arquivoALI1.Append(registroFornecedorFabricante != null && registroFornecedorFabricante.DescricaoFornecedor != null ? Texto.RemoveCaracteres(registroFornecedorFabricante.DescricaoFornecedor).PadRight(60, ' ') : "".PadRight(60, ' '));

								//endereco do fornecedor estrangeiro
								arquivoALI1.Append(registroFornecedorFabricante != null && registroFornecedorFabricante.DescricaoLogradouroFornecedor != null ? Texto.RemoveCaracteres(registroFornecedorFabricante.DescricaoLogradouroFornecedor).PadRight(40, ' ') : "".PadRight(40, ' '));

								//numero endereco do fornecedor estrangeiro
								arquivoALI1.Append(registroFornecedorFabricante != null && registroFornecedorFabricante.NumeroFornecedor != null ? registroFornecedorFabricante.NumeroFornecedor.PadRight(6, ' ') : "".PadRight(6, ' '));

								//complemento endereco do fornecedor estrangeiro
								arquivoALI1.Append(registroFornecedorFabricante != null && registroFornecedorFabricante.DescricaoComplementoFornecedor != null ? Texto.RemoveCaracteres(registroFornecedorFabricante.DescricaoComplementoFornecedor).PadRight(21, ' ') : "".PadRight(21, ' '));

								//estado endereco do fornecedor estrangeiro
								arquivoALI1.Append(registroFornecedorFabricante != null && registroFornecedorFabricante.DescricaoEstadoFornecedor != null ? Texto.RemoveCaracteres(registroFornecedorFabricante.DescricaoEstadoFornecedor).PadRight(25, ' ') : "".PadRight(25, ' '));

								//cidade endereco do fornecedor estrangeiro
								arquivoALI1.Append(registroFornecedorFabricante != null && registroFornecedorFabricante.DescricaoCidadeFornecedor != null ? Texto.RemoveCaracteres(registroFornecedorFabricante.DescricaoCidadeFornecedor).PadRight(25, ' ') : "".PadRight(25, ' '));

								//pais de aquisição da mercadoria
								arquivoALI1.Append(registroFornecedorFabricante != null && registroFornecedorFabricante.CodigoPaisFornecedor != null && registroFornecedorFabricante.CodigoPaisFornecedor.Trim().Length > 0 ? Convert.ToInt32(registroFornecedorFabricante.CodigoPaisFornecedor).ToString("D3") : "000");

								#endregion

								//Mercadoria NCM
								arquivoALI1.Append(mercadoria.CodigoNCMMercadoria);

								//País de procedência da mercadoria
								arquivoALI1.Append(mercadoria.CodigoPais != null && mercadoria.CodigoPais != "" ?
													Convert.ToInt32(mercadoria.CodigoPais).ToString("D3") : "000");

								//Mercadoria NCM
								arquivoALI1.Append(mercadoria.TipoFornecedor.ToString());

								#region DADOS FABRICANTE
								//nome do fabricante estrangeiro
								arquivoALI1.Append(registroFornecedorFabricante != null && registroFornecedorFabricante.DescricaoFabricante != null ? Texto.RemoveCaracteres(registroFornecedorFabricante.DescricaoFabricante).PadRight(60, ' ') : "".PadRight(60, ' '));

								//endereco do fabricante estrangeiro
								arquivoALI1.Append(registroFornecedorFabricante != null && registroFornecedorFabricante.DescricaoLogradouroFabricante != null ? Texto.RemoveCaracteres(registroFornecedorFabricante.DescricaoLogradouroFabricante).PadRight(40, ' ') : "".PadRight(40, ' '));

								//numero endereco do fabricante estrangeiro
								arquivoALI1.Append(registroFornecedorFabricante != null && registroFornecedorFabricante.NumeroFabricante != null ? registroFornecedorFabricante.NumeroFabricante.PadRight(6, ' ') : "".PadRight(6, ' '));

								//complemento endereco do fabricante estrangeiro
								arquivoALI1.Append(registroFornecedorFabricante != null && registroFornecedorFabricante.DescricaoComplementoFabricante != null ? Texto.RemoveCaracteres(registroFornecedorFabricante.DescricaoComplementoFabricante).PadRight(21, ' ') : "".PadRight(21, ' '));

								//estado endereco do fabricante estrangeiro
								arquivoALI1.Append(registroFornecedorFabricante != null && registroFornecedorFabricante.DescricaoEstadoFabricante != null ? Texto.RemoveCaracteres(registroFornecedorFabricante.DescricaoEstadoFabricante).PadRight(25, ' ') : "".PadRight(25, ' '));

								//cidade endereco do fabricante estrangeiro
								arquivoALI1.Append(registroFornecedorFabricante != null && registroFornecedorFabricante.DescricaoCidadeFabricante != null ? Texto.RemoveCaracteres(registroFornecedorFabricante.DescricaoCidadeFabricante).PadRight(25, ' ') : "".PadRight(25, ' '));

								//pais endereco do fabricante estrangeiro
								if (registroFornecedorFabricante.CodigoAusenciaFabricante == 2)
								{
									arquivoALI1.Append(registroFornecedorFabricante != null && registroFornecedorFabricante.CodigoPaisFabricante != null ? Convert.ToInt32(registroFornecedorFabricante.CodigoPaisFabricante).ToString("D3") : "000");
								}
								else
								{
									arquivoALI1.Append(mercadoria.CodigoPaisOrigemFabricante);
								}

								#endregion

								#endregion

								//NALADI
								arquivoALI1.Append(registroNALADI != null ? registroNALADI.Codigo.ToString("D8") : "00000000");

								//Peso líquido da mercadoria
								string[] peso = mercadoria.PesoLiquido.Value.ToString().Split(',');

								arquivoALI1.Append
									(
										Convert.ToInt64(peso[0].Replace(".", "")).ToString("D10") + peso[1].PadRight(5, '0')
									);

								//Quantidade na medida estatistica
								string[] medida = mercadoria.QuantidadeUnidadeMedidaEstatistica.Value.ToString().Split(',');

								arquivoALI1.Append
									(
										Convert.ToInt64(medida[0].Replace(".", "")).ToString("D9") + medida[1].PadRight(5, '0')
									);

								//Código da moeda negociada
								arquivoALI1.Append(mercadoria.Moeda.CodigoMoeda.ToString("D3"));


								//Código da INCOTERMS
								arquivoALI1.Append(Texto.RemoveCaracteres(mercadoria.Incoterms.Codigo).PadRight(3, ' '));


								//Valor da mercadoria na moeda negociada
								erroCampo = "Tabela Pli Mercadoria | IdPliMercadoria " + mercadoria.IdPliMercadoria.ToString() + " | Campo: ValorTotalCondicaoVenda";


								if (mercadoria.ValorTotalCondicaoVenda != null)
								{
									string[] valornegociado = mercadoria.ValorTotalCondicaoVenda.Value.ToString().Split(',');

									arquivoALI1.Append
									(
										Convert.ToInt64(valornegociado[0].Replace(".", "")).ToString("D13") + valornegociado[1].PadRight(2, '0')
									);
								}
								else
								{
									arquivoALI1.Append
									(
										Convert.ToInt64(0).ToString("D15")
									);
								}

								//arquivoALI.Append
								//	(
								//		Convert.ToInt64(valornegociado[0].Replace(".", "")).ToString("D13") + valornegociado[1].PadRight(2, '0')
								//	);

								//Tipo de acordo tarifário
								arquivoALI1.Append(mercadoria.TipoAcordoTarifario);

								//Codigo Acordo ALADI
								arquivoALI1.Append(mercadoria.Aladi != null && mercadoria.Aladi.Codigo.ToString().Length > 0 ? Convert.ToInt64(mercadoria.Aladi.Codigo).ToString().PadRight(3, ' ') : "".PadRight(3, ' '));

								//Código regime tributário
								arquivoALI1.Append(mercadoria.RegimeTributario.Codigo.ToString());

								//Código fundamento legal
								arquivoALI1.Append(mercadoria.FundamentoLegal != null && mercadoria.FundamentoLegal.Codigo.ToString().Length > 0 ? Convert.ToInt32(mercadoria.FundamentoLegal.Codigo).ToString("D2") : "00");

								//Código Cobertura Cambial
								arquivoALI1.Append(mercadoria.TipoCOBCambial.ToString());

								//Código Modalidade de Pagamento
								arquivoALI1.Append(registroMP != null ? Convert.ToInt32(registroMP.Codigo).ToString("D2") : "00");

								//Quantidade de dias limite de pagamento
								arquivoALI1.Append(mercadoria.NumeroCOBCambialLimiteDiasPagamento.HasValue ?
									mercadoria.NumeroCOBCambialLimiteDiasPagamento.Value.ToString("D3") : "000");

								//Código do instituição financeira
								arquivoALI1.Append(registroIF != null ? Convert.ToInt32(registroIF.Codigo).ToString("D2") : "00");

								//Código do motivo importação sem cobertura
								arquivoALI1.Append(registroMotivo != null ? Convert.ToInt32(registroMotivo.Codigo).ToString("D2") : "00");

								//Código da agência SECEX
								arquivoALI1.Append(mercadoria.NumeroAgenciaSecex != null ? mercadoria.NumeroAgenciaSecex.PadRight(5, ' ') : "".PadRight(5, ' '));

								//RFB de despacho
								arquivoALI1.Append(registroUnidadeDespacho != null ? registroUnidadeDespacho.Codigo.ToString("D7") : "00000000");

								//Indicador de material usado
								arquivoALI1.Append(mercadoria.TipoMaterialUsado == null || mercadoria.TipoMaterialUsado.Value == 0 ? "N" : "S");

								//Indicador de bem fabricado sob encomenda
								arquivoALI1.Append(mercadoria.TipoBemEncomenda == null || mercadoria.TipoBemEncomenda.Value == 0 ? "N" : "S");

								//Número do ato Drawback
								arquivoALI1.Append(mercadoria.NumeroAtoDrawback != null ? mercadoria.NumeroAtoDrawback.PadRight(13, ' ') : "".PadRight(13, ' '));

								//Comunicado de compra
								arquivoALI1.Append(mercadoria.NumeroComunicadoCompra != null ? mercadoria.NumeroComunicadoCompra.PadRight(13, ' ') : "".PadRight(13, ' '));

								if(pliVM.TipoDocumento == 2)
								{
									//Número da LI de referência
									if (string.IsNullOrEmpty(pliVM.NumeroLIReferencia) || pliVM.NumeroLIReferencia.Trim() == "" || pliVM.NumeroLIReferencia.Contains("0000000000"))
									{

										pliVM.NumeroLIReferencia = "0000000000";
										arquivoALI1.Append(pliVM.NumeroLIReferencia);
									}
									else
									{
										arquivoALI1.Append(Convert.ToInt64(pliVM.NumeroLIReferencia).ToString("D10"));
									}
								}
								else if (pliVM.TipoDocumento == 3)
								{
									//Número da LI de referência
									if (mercadoria.NumeroLiRetificador == null || mercadoria.NumeroLiRetificador == 0)
									{

										pliVM.NumeroLIReferencia = "0000000000";
										arquivoALI1.Append(pliVM.NumeroLIReferencia);
									}
									else
									{
										arquivoALI1.Append(Convert.ToInt64(mercadoria.NumeroLiRetificador).ToString("D10"));
									}
								}
								

								//arquivoALI.Append(pliVM.NumeroLIReferencia != null ? Convert.ToInt64(pliVM.NumeroLIReferencia).ToString("D10") : "0000000000");

								//Código de origem da LI (SUFRAMA = 4 – FIXO)
								arquivoALI1.Append("4");

								//Número do CPF do representante legal
								arquivoALI1.Append(pliVM.NumCPFRepLegalSISCO.Replace(".", "").Replace("-", ""));

								//Indicador de registro Drawback
								arquivoALI1.Append("3");

								//Número do registro Drawback
								arquivoALI1.Append("00000000000");

								arquivoALI1.AppendLine();

								#endregion
								var pliDetalheMercadoriaaaaa2 = _uowSciex.QueryStackSciex.PliDetalheMercadoria.Listar(o => o.IdPliMercadoria == mercadoria.IdPliMercadoria);
								var listaDetalhe = AutoMapper.Mapper.Map<IEnumerable<PliDetalheMercadoriaVM>>(pliDetalheMercadoriaaaaa2);
								int itemNCM = 1;

								foreach (var itemDetalhe in listaDetalhe)
								{
									StringBuilder sblistaDetalhe = new StringBuilder();
									try
									{
										#region TIPO DE REGISTRO 03 - MERCADORIA DETALHE

										sblistaDetalhe.Append("03");
										//numero da ALI
										sblistaDetalhe.Append(mercadoria.Ali.NumeroAli.ToString().PadRight(15, ' '));
										//Número sequencial do produto
										sblistaDetalhe.Append(itemNCM.ToString("D2"));

										//Valor da mercadoria na moeda negociada
										erroCampo = "Tabela Pli Detalhe Mercadoria | IdPliDetalheMercadoria " + itemDetalhe.IdPliDetalheMercadoria.ToString() + " | Campo: QuantidadeComercializada";
										string[] qtdUnidadeComercializada = itemDetalhe.QuantidadeComercializada.Value.ToString().Split(',');

										sblistaDetalhe.Append
											(
												Convert.ToInt64(qtdUnidadeComercializada[0].Replace(".", "")).ToString("D9") + qtdUnidadeComercializada[1].PadRight(5, '0')
											);

										//Unidade de medida comercializada
										erroCampo = "Tabela Pli Detalhe Mercadoria | IdPliDetalheMercadoria " + itemDetalhe.IdPliDetalheMercadoria.ToString() + " | Campo: DescricaoUnidadeMedida";
										if (itemDetalhe.DescricaoUnidadeMedida.Length > 20)
										{
											sblistaDetalhe.Append(Texto.RemoveCaracteres(itemDetalhe.DescricaoUnidadeMedida).Substring(0, 20).PadRight(20, ' '));
										}
										else
										{
											sblistaDetalhe.Append(Texto.RemoveCaracteres(itemDetalhe.DescricaoUnidadeMedida).PadRight(20, ' '));
										}

										//Valor da unidade na condição de venda
										erroCampo = "Tabela Pli Detalhe Mercadoria | IdPliDetalheMercadoria " + itemDetalhe.IdPliDetalheMercadoria.ToString() + " | Campo: ValorUnitarioCondicaoVenda";
										string[] valorUnitario = itemDetalhe.ValorUnitarioCondicaoVenda.Value.ToString().Split(',');

										sblistaDetalhe.Append
											(
												Convert.ToInt64(valorUnitario[0].Replace(".", "")).ToString("D13") + valorUnitario[1].PadRight(7, '0')
											);
										//Número do item Drawback
										sblistaDetalhe.Append("000");
										//Qtde.do produto Drawback
										sblistaDetalhe.Append("00000000000000");
										//Valor do produto Drawback
										sblistaDetalhe.Append("000000000000000");

										sblistaDetalhe.AppendLine();

										#endregion

										#region TIPO REGISTRO 06 - DETALHE

										//Tipo de registro
										sblistaDetalhe.Append("06");
										//numero da ALI
										sblistaDetalhe.Append(mercadoria.Ali.NumeroAli.ToString().PadRight(15, ' '));
										//Número sequencial do produto
										sblistaDetalhe.Append(itemNCM.ToString("D2"));
										//Descrição de detalhes da mercadoria
										sblistaDetalhe.Append(Texto.RemoveCaracteres(itemDetalhe.DescricaoDetalhe).PadRight(254, ' '));

										if (pliVM.DescricaoAplicacao == "INDUSTRIALIZAÇÃO")
										{
											string complemento = "**SUFRAMA**" +
																 itemDetalhe.DescricaoComplemento +
																 itemDetalhe.DescricaoREFFabricante +
																 itemDetalhe.DescricaoPartNumber +
																 itemDetalhe.DescricaoMateriaPrimaBasica;

											//Complemento da descrição
											if (complemento.Length < 4048)
												sblistaDetalhe.Append(Texto.RemoveCaracteres(complemento).Substring(0, complemento.Length));
											else
												sblistaDetalhe.Append(Texto.RemoveCaracteres(complemento).Substring(0, 4048));
										}
										else
										{
											//Complemento da descrição
											if (itemDetalhe.DescricaoComplemento != null)
											{
												if (itemDetalhe.DescricaoComplemento.Length < 4048)
												{
													sblistaDetalhe.Append(itemDetalhe.DescricaoComplemento != null ? Texto.RemoveCaracteres(itemDetalhe.DescricaoComplemento).Substring(0, itemDetalhe.DescricaoComplemento.Length) : "");
												}
												else
												{
													sblistaDetalhe.Append(itemDetalhe.DescricaoComplemento != null ? Texto.RemoveCaracteres(itemDetalhe.DescricaoComplemento).Substring(0, 4048) : "");
												}
											}
										}
										sblistaDetalhe.AppendLine();

										#endregion

										itemNCM++;
									}
									catch (Exception)
									{
										throw;
									}
									arquivoALI1.Append(sblistaDetalhe.ToString());
								}

								#region TIPO REGISTRO 04 - PROCESSO ANUENTE

								if (registroProcessoAnuente != null && registroProcessoAnuente.Any())
								{
									//Tipo de Registro - Processo Anuente
									arquivoALI1.Append("04");
									//numero da ALI
									arquivoALI1.Append(mercadoria.Ali.NumeroAli.ToString().PadRight(15, ' '));
									//Número do processo anuente
									arquivoALI1.Append(registroProcessoAnuente.FirstOrDefault().NumeroProcesso.ToString().PadRight(20, ' '));
									//Sigla do orgão processo anuente
									arquivoALI1.Append(Texto.RemoveCaracteres(registroProcessoAnuente.FirstOrDefault().Sigla).ToString().PadRight(10, ' '));

									arquivoALI1.AppendLine();
								}

								#endregion

								#region TIPO REGISTRO 05 - DESTAQUE NCM 

								if (mercadoria.NumeroNCMDestaque != null && mercadoria.NumeroNCMDestaque.Length > 0)
								{
									//Tipo de Registro - DESTAQUE NCM
									arquivoALI1.Append("05");
									//numero da ALI
									arquivoALI1.Append(mercadoria.Ali.NumeroAli.ToString().PadRight(15, ' '));
									//Número do destaque da NCM
									arquivoALI1.Append(Texto.RemoveCaracteres(mercadoria.NumeroNCMDestaque).Substring(0, 3).PadLeft(3, '0'));

									arquivoALI1.AppendLine();
								}

								#endregion

								#region TIPO REGISTRO 07 - INFORMAÇÕES COMPLEMENTARES

								//Tipo de Registro - DESTAQUE NCM
								arquivoALI1.Append("07");
								//numero da ALI
								arquivoALI1.Append(mercadoria.Ali.NumeroAli.ToString().PadRight(15, ' '));
								if (mercadoria.DescricaoInformacaoComplementar != null && mercadoria.DescricaoInformacaoComplementar.Length > 0)
								{
									//Número do destaque da NCM
									if (mercadoria.DescricaoInformacaoComplementar.Length < 4048)
									{
										arquivoALI1.Append(Texto.RemoveCaracteres(mercadoria.DescricaoInformacaoComplementar).Substring(0, mercadoria.DescricaoInformacaoComplementar.Length));
									}
									else
									{
										arquivoALI1.Append(Texto.RemoveCaracteres(mercadoria.DescricaoInformacaoComplementar).Substring(0, 4048));
									}
								}

								arquivoALI1.AppendLine();

								#endregion

							}
						}
						catch (Exception ex)
						{
							//fim da execução do controle de serviço
							_controleExecucaoServicoEntity.DataHoraExecucaoFim = GetDateTimeNowUtc();
							_controleExecucaoServicoEntity.MemoObjetoRetorno = "Erro geração do arquivo de ALI: " + itemAli.IdPliMercadoria + " " + erroCampo + ex.Message.ToString();
							_controleExecucaoServicoEntity.StatusExecucao = (int)EnumControleServico.ServicoExecutadoSemSucesso;
							_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_controleExecucaoServicoEntity);
							_uowSciex.CommandStackSciex.Save();

							continue;
						}
						arquivoALI.Append(arquivoALI1.ToString());
					}
				}


				if (listaALI.Any())
				{
					string fileName = "ENVIO_ALI_NORMAL_" +
						GetDateTimeNowUtc().Day.ToString("D2") +
						GetDateTimeNowUtc().Month.ToString("D2") +
						GetDateTimeNowUtc().Year.ToString("D4") + "_" +
						GetDateTimeNowUtc().Hour.ToString("D2") +
						GetDateTimeNowUtc().Minute.ToString("D2");


					////retirar as ultimas quebras de linhas
					int ultimoIndice = arquivoALI.ToString().LastIndexOf("\r\n");
					string arquivoGerado = arquivoALI.ToString().Substring(0, ultimoIndice);

					//converte de string para salvar base em varbinary
					byte[] arquivo = Encoding.ASCII.GetBytes(arquivoGerado);

					//inserir o registro de ARQUIVO_ENVIO DA ALI
					AliArquivoEnvioEntity _aliArquivoEnvioEntity = new AliArquivoEnvioEntity();
					_aliArquivoEnvioEntity.NomeArquivo = fileName;
					_aliArquivoEnvioEntity.DataGeracao = GetDateTimeNowUtc();
					_aliArquivoEnvioEntity.CodigoStatusEnvioSiscomex = (int)EnumAliAquivoStatus.ALI_ARQUIVO_NAO_ENVIADO_AO_SISCOMEX;
					_aliArquivoEnvioEntity.QuantidadeALIs = (short)listaALI.Count;
					_aliArquivoEnvioEntity.QuantidadePLIs = (short)_PLis.Count;
					_aliArquivoEnvioEntity.ValorTotalReal = _valorTotalReal;
					_aliArquivoEnvioEntity.ValorTotalDolar = _valorTotalDolar;
					_aliArquivoEnvioEntity.TipoArquivo = (byte)EnumAliTipoArquivo.ALI_NORMAL;
					_aliArquivoEnvioEntity.TentativasEnvio = 0;

					_aliArquivoEnvioEntity.AliArquivo = new AliArquivoEntity();
					_aliArquivoEnvioEntity.AliArquivo.Arquivo = arquivo;

					_uowSciex.CommandStackSciex.AliArquivoEnvio.Salvar(_aliArquivoEnvioEntity);
					_uowSciex.CommandStackSciex.Save();

					//atualizando o codigo do aqruivo na tabela de ALI
					foreach (AliEntity item in listaALI)
					{


						//atualizar Status ALI
						AliEntity AliAtual = _uowSciex.QueryStackSciex.Ali.Selecionar(o => o.NumeroAli == item.NumeroAli && o.Status == 1 && (o.TipoAli == 1 || o.TipoAli == 2 || o.TipoAli == 3));
					
						//AliAtual.Status = (int)EnumAliStatus.ALI_ENVIADA_AO_SISCOMEX;
						AliAtual.IdAliArquivoEnvio = _aliArquivoEnvioEntity.IdAliArquivoEnvio;
						AliAtual.NomeArquivo = fileName;

						_uowSciex.CommandStackSciex.Ali.Salvar(AliAtual);
						_uowSciex.CommandStackSciex.Save();
					
						//Inserir o historico de envio da ALI no arquivo
						AliEntradaArquivoEntity _aliEntradaArquivoEntity = new AliEntradaArquivoEntity();
						_aliEntradaArquivoEntity.IdPliMercadoria = AliAtual.IdPliMercadoria;
						_aliEntradaArquivoEntity.IdAliArquivoEnvio = _aliArquivoEnvioEntity.IdAliArquivoEnvio;
						_aliEntradaArquivoEntity.DataEnvioArquivoRetorno = GetDateTimeNowUtc();

						_uowSciex.CommandStackSciex.AliEntradaArquivo.Salvar(_aliEntradaArquivoEntity);
						_uowSciex.CommandStackSciex.Save();
					
					}


					_controleExecucaoServicoEntity.DataHoraExecucaoFim = GetDateTimeNowUtc();
					_controleExecucaoServicoEntity.StatusExecucao = (int)EnumControleServico.ServicoExecutadoComSucesso;
					_controleExecucaoServicoEntity.MemoObjetoRetorno = "Tabela: SCIEX_ALI_ARQUIVO_ENVIO, Campo aae_id: "+ _aliArquivoEnvioEntity.IdAliArquivoEnvio.ToString();
					_controleExecucaoServicoEntity.MemoObjetoEnvio = arquivoALI.ToString();

					_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_controleExecucaoServicoEntity);
					_uowSciex.CommandStackSciex.Save();
					return;

				}

				_controleExecucaoServicoEntity.DataHoraExecucaoFim = GetDateTimeNowUtc();
				_controleExecucaoServicoEntity.MemoObjetoRetorno = "Sem ALI's para geração de arquivo.";
				_controleExecucaoServicoEntity.StatusExecucao = (int)EnumControleServico.ServicoExecutadoComSucesso;
				_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_controleExecucaoServicoEntity);
				_uowSciex.CommandStackSciex.Save();

		}
			catch (Exception ex)
			{
				//fim da execução do controle de serviço
				_controleExecucaoServicoEntity.DataHoraExecucaoFim = GetDateTimeNowUtc();
		_controleExecucaoServicoEntity.MemoObjetoRetorno = "Erro geração do arquivo de ALI: " + erroCampo + ex.Message.ToString();
				_controleExecucaoServicoEntity.StatusExecucao = (int) EnumControleServico.ServicoExecutadoSemSucesso;
		_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_controleExecucaoServicoEntity);
				_uowSciex.CommandStackSciex.Save();

				throw;
			}

}


		public string GerarResposta()
		{
			string texto = @"
				 Resposta
			";
			return texto;
		}

		private bool GerarArquivoRespostaFTP(string arquivo, string usuario, string senha)
		{
			try
			{

				FtpWebRequest request = (FtpWebRequest)WebRequest.Create(arquivo);
				request.Method = WebRequestMethods.Ftp.UploadFile;
				request.Credentials = new NetworkCredential(usuario, senha);

				byte[] fileContents;
				fileContents = Encoding.UTF8.GetBytes("Arquivo de retorno");

				request.ContentLength = fileContents.Length;

				using (Stream requestStream = request.GetRequestStream())
				{
					requestStream.Write(fileContents, 0, fileContents.Length);
				}

				using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
				{
					Console.WriteLine($"Upload File Complete, status {response.StatusDescription}");
				}
				return true;
			}
			catch (Exception ex)
			{
				return false;

			}
		}

		public bool DowwloadArquivoAli()
		{
			try
			{
				GerarArquivoRespostaFTP(@"ftp://192.168.0.251/ArquivoRetornoSISCOMEX/RETORNO_ALI_NORMAL.txt", "ctis", "ctis");
				//ReceberArquivoFTP(@"ftp://192.168.0.251/ArquivoRetornoSISCOMEX/RETORNO_ALI_NORMAL.txt", "ctis", "ctis");

				return true;
			}
			catch (Exception ex)
			{
				return false;
			}

		}

		public string EnviarArquivoALI()
		{
			string mensagem = string.Empty;

			try
			{
				ControleExecucaoServicoEntity _ControleExecucaoServicoVM = new ControleExecucaoServicoEntity();
				_ControleExecucaoServicoVM.DataHoraExecucaoInicio = GetDateTimeNowUtc();
				_ControleExecucaoServicoVM.IdListaServico = 10; // ENVIAR ARQUIVO ALI AO SISCOMEX

				// consulta arquivos pendentes de envio
				var _arquivoALI = _uowSciex.QueryStackSciex.AliArquivoEnvio.Listar(o => o.CodigoStatusEnvioSiscomex == (int)EnumAliAquivoStatus.ALI_ARQUIVO_NAO_ENVIADO_AO_SISCOMEX && o.TipoArquivo == (int)EnumAliTipoArquivo.ALI_NORMAL).OrderBy(o => o.DataGeracao).FirstOrDefault();
				var conteudo = System.Text.Encoding.Default.GetString(_arquivoALI.AliArquivo.Arquivo);
				var configuracoesFTP = _uowSciex.QueryStackSciex.ParametroConfiguracao.Selecionar(o => o.IdParametroConfiguracao == 7).Valor;
				var configuracoesUsuario = _uowSciex.QueryStackSciex.ParametroConfiguracao.Selecionar(o => o.IdParametroConfiguracao == 8).Valor;
				var configuracoesSenha = _uowSciex.QueryStackSciex.ParametroConfiguracao.Selecionar(o => o.IdParametroConfiguracao == 9).Valor;
				var qtdTentativasArquivoEnvio = _uowSciex.QueryStackSciex.ParametroConfiguracao.Selecionar(o => o.Descricao == "Quantidade máxima para tentativa de envio de arquivo" && o.IdListaServico == 10);

				if (!Ftp.VerificarSeExisteArquivo(configuracoesFTP, configuracoesUsuario, configuracoesSenha))
				{
					mensagem = Ftp.EnviarArquivo(configuracoesFTP, configuracoesUsuario, configuracoesSenha, conteudo);
					if (mensagem == "enviado")
					{
						mensagem = "Enviado ao SISCOMEX";
						// atualiza status envio para enviando = 1
						_arquivoALI.CodigoStatusEnvioSiscomex = (int)EnumAliAquivoStatus.ALI_ARQUIVO_ENVIADO_AO_SISCOMEX;
						_ControleExecucaoServicoVM.StatusExecucao = 1;
						_uowSciex.CommandStackSciex.AliArquivoEnvio.Salvar(_arquivoALI);
						_uowSciex.CommandStackSciex.Save();

						List<AliEntity> lista = _uowSciex.QueryStackSciex.Ali.Listar(o => o.IdAliArquivoEnvio == _arquivoALI.IdAliArquivoEnvio);

						if (lista.Count > 0)
						{
							foreach (AliEntity item in lista)
							{
								byte statusAnteriorALI = item.Status;

								AliEntity aliAtualizar = _uowSciex.QueryStackSciex.Ali.Selecionar(x => x.NumeroAli == item.NumeroAli);
								aliAtualizar.Status = (int)EnumAliStatus.ALI_ENVIADA_AO_SISCOMEX;
								_uowSciex.CommandStackSciex.Ali.Salvar(aliAtualizar);
								_uowSciex.CommandStackSciex.Save();

								//inserir o histórico de geração do envio da ALI
								AliHistoricoEntity _aliHistoricoEntity = new AliHistoricoEntity();
								_aliHistoricoEntity.IdPliMercadoria = item.IdPliMercadoria;
								_aliHistoricoEntity.DataOperacao = GetDateTimeNowUtc();
								_aliHistoricoEntity.StatusAliAnterior = statusAnteriorALI;
								_aliHistoricoEntity.CPFCNPJResponsavel = "04407029000143";
								_aliHistoricoEntity.NomeResponsavel = "Administrador do Sistema – SUFRAMA";
								_aliHistoricoEntity.Observacao = "ALI enviada ao SISCOMEX";

								_uowSciex.CommandStackSciex.AliHistorico.Salvar(_aliHistoricoEntity);
								_uowSciex.CommandStackSciex.Save();
							}
						}
					}
					else
					{
						//verificar o numero de tentativa de envio do arquivo
						AliArquivoEnvioEntity _aliArquivoEnvioEntity = _uowSciex.QueryStackSciex.AliArquivoEnvio.Selecionar(o => o.IdAliArquivoEnvio == _arquivoALI.IdAliArquivoEnvio);
						byte tentativas = (byte)(_aliArquivoEnvioEntity.TentativasEnvio + 1);

						if (tentativas <= Convert.ToByte(qtdTentativasArquivoEnvio.Valor))
						{
							_aliArquivoEnvioEntity.TentativasEnvio = tentativas;
							_aliArquivoEnvioEntity.CodigoStatusEnvioSiscomex = (int)EnumAliAquivoStatus.ALI_ARQUIVO_NAO_ENVIADO_AO_SISCOMEX; ;
							_uowSciex.CommandStackSciex.AliArquivoEnvio.Salvar(_aliArquivoEnvioEntity);
							_uowSciex.CommandStackSciex.Save();
						}
						else
						{
							//disponibilizar ALI dos arquivos
							var listaALI = _uowSciex.QueryStackSciex.Ali.Listar(o => o.IdAliArquivoEnvio == _aliArquivoEnvioEntity.IdAliArquivoEnvio);
							if (listaALI != null && listaALI.Count > 0)
							{
								foreach (AliEntity item in listaALI)
								{
									item.IdAliArquivoEnvio = null;
									item.NomeArquivo = string.Empty;									

									_uowSciex.CommandStackSciex.Ali.Salvar(item);
									_uowSciex.CommandStackSciex.Save();
									
								}
							}

							_aliArquivoEnvioEntity.CodigoStatusEnvioSiscomex = (int)EnumAliAquivoStatus.ALI_ARQUIVO_NAO_ENVIADO_TENTATIVA_DE_ENVIO_EXCEDEU_O_LIMITE; ;
							_uowSciex.CommandStackSciex.AliArquivoEnvio.Salvar(_aliArquivoEnvioEntity);
							_uowSciex.CommandStackSciex.Save();
						}

						mensagem = "Não enviado ao SISCOMEX: " + mensagem;
						_ControleExecucaoServicoVM.StatusExecucao = 2;
					}
				}
				else
				{
					//verificar o numero de tentativa de envio do arquivo
					AliArquivoEnvioEntity _aliArquivoEnvioEntity = _uowSciex.QueryStackSciex.AliArquivoEnvio.Selecionar(o => o.IdAliArquivoEnvio == _arquivoALI.IdAliArquivoEnvio);

					byte tentativas = (byte)(_aliArquivoEnvioEntity.TentativasEnvio + 1);

					if (tentativas <= Convert.ToByte(qtdTentativasArquivoEnvio.Valor))
					{
						_aliArquivoEnvioEntity.TentativasEnvio = tentativas;
						_aliArquivoEnvioEntity.CodigoStatusEnvioSiscomex = (int)EnumAliAquivoStatus.ALI_ARQUIVO_NAO_ENVIADO_AO_SISCOMEX; ;
						_uowSciex.CommandStackSciex.AliArquivoEnvio.Salvar(_aliArquivoEnvioEntity);
						_uowSciex.CommandStackSciex.Save();
					}
					else
					{
						//disponibilizar ALI dos arquivos
						var listaALI = _uowSciex.QueryStackSciex.Ali.Listar(o => o.IdAliArquivoEnvio == _aliArquivoEnvioEntity.IdAliArquivoEnvio);
						if (listaALI != null && listaALI.Count > 0)
						{
							foreach (AliEntity item in listaALI)
							{
								item.IdAliArquivoEnvio = null;
								item.NomeArquivo = string.Empty;

								_uowSciex.CommandStackSciex.Ali.Salvar(item);
								_uowSciex.CommandStackSciex.Save();
								
							}
						}

						_aliArquivoEnvioEntity.CodigoStatusEnvioSiscomex = (int)EnumAliAquivoStatus.ALI_ARQUIVO_NAO_ENVIADO_TENTATIVA_DE_ENVIO_EXCEDEU_O_LIMITE; ;
						_uowSciex.CommandStackSciex.AliArquivoEnvio.Salvar(_aliArquivoEnvioEntity);
						_uowSciex.CommandStackSciex.Save();
					}

					mensagem = "Arquivo existente na pasta no momento do envio";
					_ControleExecucaoServicoVM.StatusExecucao = 2;
				}


				_ControleExecucaoServicoVM.MemoObjetoEnvio = "Tabela: SCIEX_ALI_ARQUIVO_ENVIO e SCIEX_ALI_ARQUIVO Campo: aae_id " + _arquivoALI.AliArquivo.IdAliArquivoEnvio;
				_ControleExecucaoServicoVM.MemoObjetoRetorno = mensagem;
				_ControleExecucaoServicoVM.DataHoraExecucaoFim = GetDateTimeNowUtc();
				_ControleExecucaoServicoVM.NomeCPFCNPJInteressado = "Administrador do Sistema - SUFRAMA";
				_ControleExecucaoServicoVM.NumeroCPFCNPJInteressado = "04407029000143";

				_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_ControleExecucaoServicoVM);
				_uowSciex.CommandStackSciex.Save();
			}
			catch
			{
				mensagem = "Nenhum arquivo disponivel para envio";
			}

			return mensagem;
		}

		public PagedItems<AliVM> ListarPaginadoRelatorioAli(AliVM pagedFilter)
		{
			try
			{
				if (pagedFilter == null) { return new PagedItems<AliVM>(); }
				var ali = _uowSciex.QueryStackSciex.Ali.ListarPaginado<AliVM>(o =>
					(
							o.PliMercadoria.IdPLI == pagedFilter.IdPli
					),
					pagedFilter);

				return ali;
			}
			catch (Exception ex)
			{
				//ChamaErro("Sistema Aladi: Nenhum registro encontrado.");

			}
			return new PagedItems<AliVM>();
		}

		public string GerarArquivoCancelamento()
		{
			ControleExecucaoServicoEntity _controleExecucaoServicoEntity = new ControleExecucaoServicoEntity();

			try
			{
				List<long> _PLis = new List<long>();
				decimal _valorTotalReal = 0;
				decimal _valorTotalDolar = 0;

				//Controle Execucao de serviço			
				_controleExecucaoServicoEntity.DataHoraExecucaoInicio = GetDateTimeNowUtc();
				_controleExecucaoServicoEntity.StatusExecucao = (int)EnumControleServico.ServicoIniciado;
				_controleExecucaoServicoEntity.IdListaServico = (int)EnumListaServico.GerarArquivoAliCancelamento;
				_controleExecucaoServicoEntity.NomeCPFCNPJInteressado = "Administrador do Sistema - SUFRAMA";
				_controleExecucaoServicoEntity.NumeroCPFCNPJInteressado = "04407029000143";

				// Consulta de ALI's para cancelamento
				var listaAli = _uowSciex.QueryStackSciex.Ali.Listar(o =>
					o.Status == (byte)EnumAliStatus.ALI_SOLICITADA_PARA_CANCELAMENTO
					&& o.PliMercadoria.Li.Status == (byte)EnumLiStatus.LI_DEFERIDA

					&& !(o.PliMercadoria.AliEntradaArquivo.Any(x => x.AliArquivoEnvio.CodigoStatusEnvioSiscomex ==
					(byte)EnumAliAquivoStatus.ALI_ARQUIVO_NAO_ENVIADO_AO_SISCOMEX && x.AliArquivoEnvio.TipoArquivo == (byte)EnumAliTipoArquivo.ALI_CANCELAMENTO)));


				if (!listaAli.Any())
				{
					//Atualiza controle execucao serviço					
					_controleExecucaoServicoEntity.StatusExecucao = (int)EnumControleServico.ServicoExecutadoComSucesso;
					_controleExecucaoServicoEntity.MemoObjetoRetorno = "Sem registros";
					_controleExecucaoServicoEntity.DataHoraExecucaoFim = GetDateTimeNowUtc();
					_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_controleExecucaoServicoEntity);
					_uowSciex.CommandStackSciex.Save();
					return "Sem ALIS condicionada a regra RN01";
				}

				// Configuração do arquivo
				string fileName = "ENVIO_ALI_CANC_" +
				GetDateTimeNowUtc().Day.ToString("D2") +
				GetDateTimeNowUtc().Month.ToString("D2") +
				GetDateTimeNowUtc().Year.ToString("D4") + "_" +
				GetDateTimeNowUtc().Hour.ToString("D2") +
				GetDateTimeNowUtc().Minute.ToString("D2");

				StringBuilder arquivoALI = new StringBuilder();

				#region início cabeçalho

				//identificação SUFRAMA
				arquivoALI.Append("SU");
				//CNPJ da SUFRAMA
				arquivoALI.Append("04407029000143");

				//Dia Juliano
				string dataInicio = "01/01/" + GetDateTimeNowUtc().Year.ToString();
				string dataAtual = GetDateTimeNowUtc().Day.ToString() + "/" + GetDateTimeNowUtc().Month.ToString() + "/" + GetDateTimeNowUtc().Year.ToString();
				TimeSpan date = Convert.ToDateTime(dataAtual) - Convert.ToDateTime(dataInicio);
				arquivoALI.Append((date.Days + 1).ToString("D3"));

				//HORA
				arquivoALI.Append(GetDateTimeNowUtc().Hour.ToString("D2") + GetDateTimeNowUtc().Minute.ToString("D2") + GetDateTimeNowUtc().Second.ToString("D2"));
				//Indicador do processo de Host
				arquivoALI.Append("02");
				//Indicador LI
				arquivoALI.Append("02");
				//Indicador para obtenção de diagnóstico
				arquivoALI.Append("02");
				//Código da Transação
				arquivoALI.Append("850");
				//Dia Juliano
				arquivoALI.Append((date.Days + 1).ToString("D3"));
				//HORA
				arquivoALI.Append(GetDateTimeNowUtc().Hour.ToString("D2") + GetDateTimeNowUtc().Minute.ToString("D2") + GetDateTimeNowUtc().Second.ToString("D2"));
				arquivoALI.AppendLine();

				#endregion

				foreach (var item in listaAli)
				{
					//tipo de registro
					arquivoALI.Append("09");
					//numero da ALI
					arquivoALI.Append(item.PliMercadoria.Li.NumeroLi.ToString().PadLeft(10, '0'));
					//Numero do protocolo
					arquivoALI.Append(item.PliMercadoria.Pli.NumCPFRepLegalSISCO);
					arquivoALI.AppendLine();

					//verifica a quantidade de PLIS existentes
					if (!_PLis.Exists(o => o == item.PliMercadoria.Pli.IdPLI))
					{
						_PLis.Add(item.PliMercadoria.Pli.IdPLI);
					}

					//Valor total ALI Dolar e Real - RN2
					_valorTotalDolar += item.PliMercadoria.ValorTotalCondicaoVendaDolar.HasValue ? item.PliMercadoria.ValorTotalCondicaoVendaDolar.Value : 0;
					_valorTotalReal += item.PliMercadoria.ValorTotalCondicaoVendaReal.HasValue ? item.PliMercadoria.ValorTotalCondicaoVendaReal.Value : 0;

				}

				//retirar as ultimas quebras de linhas
				int ultimoIndice = arquivoALI.ToString().LastIndexOf("\r\n");
				string arquivoGerado = arquivoALI.ToString().Substring(0, ultimoIndice);

				//converte de string para salvar base em varbinary
				byte[] arquivo = Encoding.ASCII.GetBytes(arquivoGerado);


				//inserir o registro de ARQUIVO_ENVIO DA ALI
				AliArquivoEnvioEntity _aliArquivoEnvioEntity = new AliArquivoEnvioEntity();
				_aliArquivoEnvioEntity.NomeArquivo = fileName;
				_aliArquivoEnvioEntity.DataGeracao = GetDateTimeNowUtc();
				_aliArquivoEnvioEntity.CodigoStatusEnvioSiscomex = (int)EnumAliAquivoStatus.ALI_ARQUIVO_NAO_ENVIADO_AO_SISCOMEX;
				_aliArquivoEnvioEntity.QuantidadeALIs = (short)listaAli.Count;
				_aliArquivoEnvioEntity.QuantidadePLIs = (short)_PLis.Count;
				_aliArquivoEnvioEntity.ValorTotalReal = _valorTotalReal;
				_aliArquivoEnvioEntity.ValorTotalDolar = _valorTotalDolar;
				_aliArquivoEnvioEntity.TipoArquivo = (byte)EnumAliTipoArquivo.ALI_CANCELAMENTO;
				_aliArquivoEnvioEntity.TentativasEnvio = 0;
				_aliArquivoEnvioEntity.AliArquivo = new AliArquivoEntity();
				_aliArquivoEnvioEntity.AliArquivo.Arquivo = arquivo;
				_uowSciex.CommandStackSciex.AliArquivoEnvio.Salvar(_aliArquivoEnvioEntity);
				_uowSciex.CommandStackSciex.Save();


				foreach (var item in listaAli)
				{
					//Inserir o registro no Ali entrada Arquivo
					AliEntradaArquivoEntity _aliEntradaArquivo = new AliEntradaArquivoEntity();
					_aliEntradaArquivo.IdPliMercadoria = item.IdPliMercadoria;
					_aliEntradaArquivo.IdAliArquivoEnvio = _aliArquivoEnvioEntity.IdAliArquivoEnvio;
					_aliEntradaArquivo.DataEnvioArquivoRetorno = _aliArquivoEnvioEntity.DataGeracao;
					_uowSciex.CommandStackSciex.AliEntradaArquivo.Salvar(_aliEntradaArquivo);
					_uowSciex.CommandStackSciex.Save();

				}


				//Atualiza controle execucao serviço					
				_controleExecucaoServicoEntity.StatusExecucao = (int)EnumControleServico.ServicoExecutadoComSucesso;
				_controleExecucaoServicoEntity.MemoObjetoRetorno = arquivoALI.ToString();
				_controleExecucaoServicoEntity.DataHoraExecucaoFim = GetDateTimeNowUtc();
				_controleExecucaoServicoEntity.MemoObjetoEnvio = "Tabela: SCIEX_ALI_ARQUIVO_ENVIO e SCIEX_ALI_ARQUIVO Campo: aae_id: " + _aliArquivoEnvioEntity.IdAliArquivoEnvio.ToString();
				_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_controleExecucaoServicoEntity);

				_uowSciex.CommandStackSciex.Save();

			}
			catch (Exception ex)
			{
				//Atualiza controle execucao serviço				
				_controleExecucaoServicoEntity.StatusExecucao = (int)EnumControleServico.ServicoExecutadoSemSucesso;
				_controleExecucaoServicoEntity.MemoObjetoRetorno = "Exceção: " + ex.Message;
				_controleExecucaoServicoEntity.DataHoraExecucaoFim = GetDateTimeNowUtc();
				_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_controleExecucaoServicoEntity);
				_uowSciex.CommandStackSciex.Save();

				return "Erro na geração do arquivo";
			}

			return "Sucesso";
		}

		/// <summary>
		/// Estória 28
		/// </summary>
		/// <returns></returns>
		public string EnviarArquivoALICancelamento()
		{
			string mensagem = string.Empty;

			try
			{
				var conteudo = "";

				var _arquivoALI = _uowSciex.QueryStackSciex.AliArquivoEnvio.Listar(o => o.CodigoStatusEnvioSiscomex == 1
				&& o.TentativasEnvio < 3 &&
				o.TipoArquivo == 2).OrderBy(o => o.DataGeracao).FirstOrDefault(); //Mais antigo

				if (_arquivoALI != null)
				{
					// consulta arquivos pendentes de envio

					conteudo = System.Text.Encoding.Default.GetString(_arquivoALI.AliArquivo.Arquivo);
					var listaFTP = _uowSciex.QueryStackSciex.ParametroConfiguracao.Listar(o => o.IdListaServico == (int)EnumListaServico.EnviarArquivoAliCancelamento);
					var configuracoesFTP = _uowSciex.QueryStackSciex.ParametroConfiguracao.Selecionar(o => o.IdParametroConfiguracao == 10).Valor;
					var configuracoesUsuario = _uowSciex.QueryStackSciex.ParametroConfiguracao.Selecionar(o => o.IdParametroConfiguracao == 11).Valor;
					var configuracoesSenha = _uowSciex.QueryStackSciex.ParametroConfiguracao.Selecionar(o => o.IdParametroConfiguracao == 12).Valor;

					//RN03
					var qtdTentativasArquivoEnvio = _uowSciex.QueryStackSciex.ParametroConfiguracao.Listar(o => o.Descricao == "Quantidade máxima para tentativa de envio de arquivo").FirstOrDefault();

					if (!Ftp.VerificarSeExisteArquivo(configuracoesFTP, configuracoesUsuario, configuracoesSenha))
					{
						mensagem = Ftp.EnviarArquivo(configuracoesFTP, configuracoesUsuario, configuracoesSenha, conteudo);
						if (mensagem == "enviado")
						{
							var listaALIEntrada = _uowSciex.QueryStackSciex.AliEntradaArquivo.Listar(o => o.IdAliArquivoEnvio == _arquivoALI.IdAliArquivoEnvio);
							foreach (var item in listaALIEntrada)
							{
								item.PliMercadoria.Li.Status = (int)EnumLiStatus.LI_ENVIADA_PARA_CANCELAMENTO;
								_uowSciex.CommandStackSciex.Li.Salvar(item.PliMercadoria.Li);
								_uowSciex.CommandStackSciex.Save();

								//inserir historico da ali
								AliHistoricoEntity _aliHistorico = new AliHistoricoEntity();
								_aliHistorico.IdPliMercadoria = item.PliMercadoria.IdPliMercadoria;
								_aliHistorico.DataOperacao = GetDateTimeNowUtc();
								_aliHistorico.StatusAliAnterior = 6;
								_aliHistorico.StatusLiAnterior = 1;
								_aliHistorico.NomeResponsavel = "Administrador do Sistema - SUFRAMA";
								_aliHistorico.CPFCNPJResponsavel = "04407029000143";
								_aliHistorico.Observacao = "FOI GERADO ARQUIVO DE LI DE CANCELAMENTO E ENVIADO AO SISCOMEX";
								_uowSciex.CommandStackSciex.AliHistorico.Salvar(_aliHistorico);
								_uowSciex.CommandStackSciex.Save();


								//Registra o início da execucao
								ControleExecucaoServicoEntity _ControleExecucaoServicoVM = new ControleExecucaoServicoEntity();
								_ControleExecucaoServicoVM.DataHoraExecucaoInicio = GetDateTimeNowUtc();
								_ControleExecucaoServicoVM.StatusExecucao = 1;
								_ControleExecucaoServicoVM.NomeCPFCNPJInteressado = "Administrador do Sistema - SUFRAMA";
								_ControleExecucaoServicoVM.NumeroCPFCNPJInteressado = "04407029000143";
								_ControleExecucaoServicoVM.IdListaServico = (int)EnumListaServico.EnviarArquivoAliCancelamento; // ENVIAR ARQUIVO ALI AO SISCOMEX

								//Registra o fim da execucao
								_ControleExecucaoServicoVM.DataHoraExecucaoInicio = GetDateTimeNowUtc();
								_ControleExecucaoServicoVM.StatusExecucao = 1;
								_ControleExecucaoServicoVM.MemoObjetoEnvio = "Tabela: SCIEX_ALI_ARQUIVO_ENVIO e SCIEX_ALI_ARQUIVO Campo: aae_id: " + item.AliArquivoEnvio.IdAliArquivoEnvio;
								_ControleExecucaoServicoVM.IdListaServico = (int)EnumListaServico.EnviarArquivoAliCancelamento; // ENVIAR ARQUIVO ALI AO SISCOMEX
								_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_ControleExecucaoServicoVM);
								_uowSciex.CommandStackSciex.Save();
							}

							string SQL =
								 "UPDATE SCIEX_ALI_ARQUIVO_ENVIO SET AAE_ST_ENVIO_SISCOMEX = " + (int)EnumAliAquivoStatus.ALI_ARQUIVO_ENVIADO_AO_SISCOMEX +
								 " WHERE AAE_ID = " + _arquivoALI.IdAliArquivoEnvio.ToString();
							mensagem = "Enviado ao SISCOMEX";

							//RN01						
							_uowSciex.CommandStackSciex.Salvar(SQL);

						}
						else
						{
							//verificar o numero de tentativa de envio do arquivo
							AliArquivoEnvioEntity _aliArquivoEnvioEntity = _uowSciex.QueryStackSciex.AliArquivoEnvio.Selecionar(o => o.IdAliArquivoEnvio == _arquivoALI.IdAliArquivoEnvio);
							byte tentativas = (byte)(_aliArquivoEnvioEntity.TentativasEnvio + 1);

							if (tentativas <= Convert.ToByte(qtdTentativasArquivoEnvio.Valor))
							{
								_aliArquivoEnvioEntity.TentativasEnvio = tentativas;
								_aliArquivoEnvioEntity.CodigoStatusEnvioSiscomex = (int)EnumAliAquivoStatus.ALI_ARQUIVO_NAO_ENVIADO_AO_SISCOMEX; ;
								_uowSciex.CommandStackSciex.AliArquivoEnvio.Salvar(_aliArquivoEnvioEntity);
								_uowSciex.CommandStackSciex.Save();
							}
							else
							{
								//disponibilizar ALI dos arquivos
								var listaALI = _uowSciex.QueryStackSciex.Ali.Listar(o => o.IdAliArquivoEnvio == _aliArquivoEnvioEntity.IdAliArquivoEnvio);
								if (listaALI != null && listaALI.Count > 0)
								{
									foreach (AliEntity item in listaALI)
									{
										item.IdAliArquivoEnvio = null;
										item.Status = (int)EnumAliStatus.ALI_SOLICITADA_PARA_CANCELAMENTO;
										_uowSciex.CommandStackSciex.Ali.Salvar(item);
										_uowSciex.CommandStackSciex.Save();
									}
								}

								_aliArquivoEnvioEntity.CodigoStatusEnvioSiscomex = (int)EnumAliAquivoStatus.ALI_ARQUIVO_NAO_ENVIADO_TENTATIVA_DE_ENVIO_EXCEDEU_O_LIMITE; ;
								_uowSciex.CommandStackSciex.AliArquivoEnvio.Salvar(_aliArquivoEnvioEntity);
								_uowSciex.CommandStackSciex.Save();
							}

							mensagem = "Não enviado ao SISCOMEX: " + mensagem;
						}
					}
					else
					{
						//verificar o numero de tentativa de envio do arquivo
						AliArquivoEnvioEntity _aliArquivoEnvioEntity = _uowSciex.QueryStackSciex.AliArquivoEnvio.Selecionar(o => o.IdAliArquivoEnvio == _arquivoALI.IdAliArquivoEnvio);

						byte tentativas = (byte)(_aliArquivoEnvioEntity.TentativasEnvio + 1);

						if (tentativas <= Convert.ToByte(qtdTentativasArquivoEnvio.Valor))
						{
							_aliArquivoEnvioEntity.TentativasEnvio = tentativas;
							_aliArquivoEnvioEntity.CodigoStatusEnvioSiscomex = (int)EnumAliAquivoStatus.ALI_ARQUIVO_NAO_ENVIADO_AO_SISCOMEX; ;
							_uowSciex.CommandStackSciex.AliArquivoEnvio.Salvar(_aliArquivoEnvioEntity);
							_uowSciex.CommandStackSciex.Save();
						}
						else
						{
							//disponibilizar ALI dos arquivos
							var listaALI = _uowSciex.QueryStackSciex.Ali.Listar(o => o.IdAliArquivoEnvio == _aliArquivoEnvioEntity.IdAliArquivoEnvio);
							if (listaALI != null && listaALI.Count > 0)
							{
								foreach (AliEntity item in listaALI)
								{
									item.IdAliArquivoEnvio = null;
									item.Status = (int)EnumAliStatus.ALI_SOLICITADA_PARA_CANCELAMENTO;
									_uowSciex.CommandStackSciex.Ali.Salvar(item);
									_uowSciex.CommandStackSciex.Save();
								}
							}

							_aliArquivoEnvioEntity.CodigoStatusEnvioSiscomex = (int)EnumAliAquivoStatus.ALI_ARQUIVO_NAO_ENVIADO_TENTATIVA_DE_ENVIO_EXCEDEU_O_LIMITE; ;
							_uowSciex.CommandStackSciex.AliArquivoEnvio.Salvar(_aliArquivoEnvioEntity);
							_uowSciex.CommandStackSciex.Save();
						}

						mensagem = "Arquivo existente na pasta no momento do envio";


						//Registra o início da execucao
						ControleExecucaoServicoEntity _ControleExecucaoServicoVM = new ControleExecucaoServicoEntity();
						_ControleExecucaoServicoVM.DataHoraExecucaoInicio = GetDateTimeNowUtc();
						_ControleExecucaoServicoVM.StatusExecucao = 1;
						_ControleExecucaoServicoVM.NomeCPFCNPJInteressado = "Administrador do Sistema - SUFRAMA";
						_ControleExecucaoServicoVM.NumeroCPFCNPJInteressado = "04407029000143";
						_ControleExecucaoServicoVM.IdListaServico = (int)EnumListaServico.EnviarArquivoAliCancelamento; // ENVIAR ARQUIVO ALI AO SISCOMEX

						//Registra o fim da execucao
						_ControleExecucaoServicoVM.DataHoraExecucaoInicio = GetDateTimeNowUtc();
						_ControleExecucaoServicoVM.StatusExecucao = 2;
						_ControleExecucaoServicoVM.IdListaServico = (int)EnumListaServico.EnviarArquivoAliCancelamento; // ENVIAR ARQUIVO ALI AO SISCOMEX
						_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_ControleExecucaoServicoVM);
						_uowSciex.CommandStackSciex.Save();

					}
				}
				else
				{

					ControleExecucaoServicoEntity _ControleExecucaoServicoVM = new ControleExecucaoServicoEntity();
					_ControleExecucaoServicoVM.DataHoraExecucaoInicio = GetDateTimeNowUtc();
					_ControleExecucaoServicoVM.StatusExecucao = 0;
					_ControleExecucaoServicoVM.NomeCPFCNPJInteressado = "Administrador do Sistema - SUFRAMA";
					_ControleExecucaoServicoVM.NumeroCPFCNPJInteressado = "04407029000143";
					_ControleExecucaoServicoVM.IdListaServico = (int)EnumListaServico.EnviarArquivoAliCancelamento; // ENVIAR ARQUIVO ALI AO SISCOMEX
																													// Registra o encerramento da execução RN01
					_ControleExecucaoServicoVM.MemoObjetoEnvio = "Regra RN02 não satisfeita";
					_ControleExecucaoServicoVM.MemoObjetoRetorno = "Não enviado ao SISCOMEX";
					_ControleExecucaoServicoVM.DataHoraExecucaoFim = GetDateTimeNowUtc();
					_ControleExecucaoServicoVM.NomeCPFCNPJInteressado = "Administrador do Sistema - SUFRAMA";
					_ControleExecucaoServicoVM.NumeroCPFCNPJInteressado = "04407029000143";
					_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_ControleExecucaoServicoVM);
					_uowSciex.CommandStackSciex.Save();

					return "Falha ao enviar o arquivo";
				}
			}
			catch (Exception ex)
			{
				//Registra o início
				ControleExecucaoServicoEntity _ControleExecucaoServicoVM = new ControleExecucaoServicoEntity();
				_ControleExecucaoServicoVM.DataHoraExecucaoInicio = GetDateTimeNowUtc();
				_ControleExecucaoServicoVM.StatusExecucao = 2;
				_ControleExecucaoServicoVM.IdListaServico = (int)EnumListaServico.EnviarArquivoAliCancelamento; // ENVIAR ARQUIVO ALI AO SISCOMEX

				//Registra o fim
				_ControleExecucaoServicoVM.MemoObjetoRetorno = "Erro ao enviar Ali's para cancelamento: " + ex.Message;
				_ControleExecucaoServicoVM.DataHoraExecucaoFim = GetDateTimeNowUtc();
				_ControleExecucaoServicoVM.NomeCPFCNPJInteressado = "Administrador do Sistema - SUFRAMA";
				_ControleExecucaoServicoVM.NumeroCPFCNPJInteressado = "04407029000143";
				_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_ControleExecucaoServicoVM);
				_uowSciex.CommandStackSciex.Save();
			}

			return mensagem;
		}

		public void ProcessarArquivoALICancelamento(string path)
		{
			GerarArquivoSimulacaoALICancelamento();

			bool arquivoExistente = false;
			int contador = 1;
			string linha;
			long codigoLiArquivoRetorno = 0;


			string enderecoFTP = "192.168.0.251";
			string pastaFTP = "ArquivoRetornoSISCOMEX";
			string nomeDoArquivo = "SERPRO.FILE";

			var configuracoesFTP = _uowSciex.QueryStackSciex.ParametroConfiguracao.Selecionar(o => o.Descricao == "FTP envio SISCOMEX");
			var configuracoesUsuario = ConfigurationManager.AppSettings["FTP_USUARIO"];
			var configuracoesSenha = ConfigurationManager.AppSettings["FTP_SENHA"];

			//Arquivo de processamento da LI
			string localAqruivoFTP = configuracoesFTP.Valor;
			string usuario = configuracoesUsuario;
			string senha = configuracoesSenha;

			StreamReader file = null;

			#region REGRA DE NEGOCIO 03

			if (Ftp.VerificarSeExisteArquivo(localAqruivoFTP, usuario, senha))
			{
				arquivoExistente = false;
				file =
					new StreamReader(
						 new MemoryStream(Ftp.ReceberArquivo(localAqruivoFTP, usuario, senha)),
						 Encoding.Default)
					;
			}
			else
			{
				var registro = _uowSciex.QueryStackSciex.LiArquivoRetorno.Listar(o => o.StatusLeituraArquivo == 0).OrderBy(o => o.DataRecepcaoArquivo).FirstOrDefault();
				if (registro.LiArquivo.ArquivoLIRetorno != null)
				{
					arquivoExistente = true;
					file =
						new StreamReader(
							 new MemoryStream(registro.LiArquivo.ArquivoLIRetorno), Encoding.Default)
						;

					codigoLiArquivoRetorno = registro.IdLiArquivoRetorno;
				}
			}

			#endregion

			#region REGRA DE NEGOCIO 01

			if (!arquivoExistente)
			{
				byte[] arquivo = Ftp.ReceberArquivo(localAqruivoFTP, usuario, senha);
				LiArquivoRetornoEntity _liArquivoRetornoEntity = new LiArquivoRetornoEntity();

				_liArquivoRetornoEntity.NomeArquivo = nomeDoArquivo;
				_liArquivoRetornoEntity.StatusLeituraArquivo = 0;
				_liArquivoRetornoEntity.DataRecepcaoArquivo = GetDateTimeNowUtc();
				_liArquivoRetornoEntity.QuantidadeErroLI = 0;
				_liArquivoRetornoEntity.QuantidadeLI = 0;
				_liArquivoRetornoEntity.QuantidadeLIDeferida = 0;
				_liArquivoRetornoEntity.QuantidadeLIIndeferida = 0;
				_liArquivoRetornoEntity.TipoArquivoLI = (byte)EnumLiTipoArquivo.LI_ARQUIVO_NORMAL;

				_liArquivoRetornoEntity.LiArquivo = new LiArquivoEntity();
				_liArquivoRetornoEntity.LiArquivo.ArquivoLIRetorno = arquivo;

				_uowSciex.CommandStackSciex.LiArquivoRetorno.Salvar(_liArquivoRetornoEntity);
				_uowSciex.CommandStackSciex.Save();

				codigoLiArquivoRetorno = _liArquivoRetornoEntity.IdLiArquivoRetorno;

				ControleExecucaoServicoEntity _controleExecucaoServicoVM = new ControleExecucaoServicoEntity();
				_controleExecucaoServicoVM.DataHoraExecucaoInicio = GetDateTimeNowUtc();
				_controleExecucaoServicoVM.IdListaServico = 12; // SALVAR ARQUIVO DE LI-NORMAL
				_controleExecucaoServicoVM.MemoObjetoEnvio = localAqruivoFTP;
				_controleExecucaoServicoVM.DataHoraExecucaoFim = GetDateTimeNowUtc();
				_controleExecucaoServicoVM.NomeCPFCNPJInteressado = "Administrador do Sistema - SUFRAMA";
				_controleExecucaoServicoVM.NumeroCPFCNPJInteressado = "04407029000143";

				_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_controleExecucaoServicoVM);
				_uowSciex.CommandStackSciex.Save();

				//exclui o arquivo do FTP
				Ftp.DeleteFile(localAqruivoFTP, usuario, senha);
			}

			#endregion

			//gerar 
			if (file != null)
			{
				StringBuilder SQLExecutar = new StringBuilder();

				SQLExecutar.AppendLine(
				"USE dbd_sciex " +
				"DECLARE @CONTROLE_SERVICO_COD INT " +

				"BEGIN TRY " +
				"BEGIN TRANSACTION; " +

				" INSERT INTO SCIEX_CONTROLE_EXEC_SERVICO " +
				"(CES_DH_EXECUCAO_INICIO, CES_ME_OBJETO_ENVIO, CES_ST_EXECUCAO, CES_NU_CPF_CNPJ_INTERESSADO, CES_NO_CPF_CNPJ_INTERESSADO, LSE_ID)" +
				"VALUES (" +
				" CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')) , '" + localAqruivoFTP + "',0,'04407029000143','Administrador do Sistema – SUFRAMA', 14" +
				") " +

				"SET @CONTROLE_SERVICO_COD = @@identity"

				);

				int qtdLI = 0;
				int qtdLIDeferida = 0;
				int qtdLIIndeferida = 0;
				int qtdErros = 0;

				while ((linha = file.ReadLine()) != null)
				{
					//cabeçalho
					if (contador == 1)
					{
						Console.WriteLine(linha);
					}
					else
					{
						if (linha.Length > 1)
						{
							qtdLI = qtdLI + 1;

							string tipoRegistro = linha.Substring(0, 2);

							string numeroALI = string.Empty;
							string numeroLIProtocolda = string.Empty;
							string numeroLI = string.Empty;
							string codigoStatusDiagnostico = string.Empty;
							string dataGeracaoDiagnostico = string.Empty;
							string mensagemErro = string.Empty;

							#region REGRA DE NEGOCIO 04
							switch (tipoRegistro)
							{
								case "01": //LI GERADA COM SUCESSO
									{
										qtdLIDeferida = qtdLIDeferida + 1;

										numeroALI = linha.Substring(2, 15).Trim();
										numeroLIProtocolda = linha.Substring(17, 10);
										numeroLI = linha.Substring(17, 10);
										dataGeracaoDiagnostico = linha.Substring(37, 8);

										long numeroALIPesquisa = Convert.ToInt64(numeroALI);
										var ali = _uowSciex.QueryStackSciex.Ali.Selecionar(o => o.NumeroAli == numeroALIPesquisa);
										var listaLI = _uowSciex.QueryStackSciex.Li.Listar(o => o.IdPliMercadoria == ali.IdPliMercadoria);

										string ano = dataGeracaoDiagnostico.Substring(0, 4);
										string mes = dataGeracaoDiagnostico.Substring(4, 2);
										string dia = dataGeracaoDiagnostico.Substring(6, 2);

										#region REGRA DE NEGOCIO 05

										if (listaLI.Count > 0)
										{
											//Insere Tabela de li_historico
											SQLExecutar.AppendLine(
												"INSERT INTO SCIEX_LI_HISTORICO_ERRO " +
												"(" +
												"	LI_NU," +
												"	LI_NU_LI_PROTOCOLADA," +
												"	LI_DT_GERACAO," +
												"	LI_DS_MENSAGEM," +
												"	LI_NU_DIAGNOSTICO_ERRO," +
												"	LI_ST," +
												"	LHE_TP_ERRO," +
												"	LI_DH_CADASTRO," +
												"	LI_DH_CANCELAMENTO" +
												")" +
												"" +
												"SELECT " +
												"	LI_NU, " +
												"	LI_NU_LI_PROTOCOLADA, " +
												"	LI_DT_GERACAO, " +
												"	LI_DS_MENSAGEM, " +
												"	LI_NU_DIAGNOSTICO_ERRO, " +
												"	LI_ST, " +
												"	1, " +
												"	CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')) , " +
												"	LI_DH_CANCELAMENTO " +
												"FROM SCIEX_LI WHERE LI_NU = " + listaLI[0].NumeroLi.ToString()
												);

											//Atualiza LI
											SQLExecutar.AppendLine(
												"UPDATE SCIEX_LI SET " +
													"LAR_ID = " + codigoLiArquivoRetorno.ToString() +
													",LI_NU = " + numeroLI +
													",LI_ST = 1" +
													",LI_TP = 1" +
													",LI_DH_CADASTRO = '" + GetDateTimeNowUtc().ToString() + "'" +
													",LI_NU_PROTOCOLADA = " + numeroLIProtocolda +
													",LI_DT_GERACAO = '" + ano + "-" + mes + "-" + dia + "'" +
													",LI_DS_MENSAGEM = NULL " +
													",LI_NU_DIAGNOSTICO_ERRO = NULL " +
													",LI_DH_CANCELAMENTO = NULL " +
													"FROM SCIEX_LI WHERE LI_NU = " + numeroLI
												);
										}
										else
										{
											//INSERIR A LI
											SQLExecutar.AppendLine(
												"INSERT INTO SCIEX_LI " +
												"( PME_ID," +
												"  LAR_ID, " +
												"  LI_NU, " +
												"  LI_ST, " +
												"  LI_TP, " +
												"  LI_DH_CADASTRO, " +
												"  LI_NU_LI_PROTOCOLADA, " +
												"  LI_DT_GERACAO) " +
												"VALUES (" +
													ali.IdPliMercadoria.ToString() + "," +
													codigoLiArquivoRetorno.ToString() + "," +
													numeroLI + "," +
													"1," +
													"1," +
													"'" + GetDateTimeNowUtc().ToString() + "'," +
													numeroLIProtocolda + "," +
													"'" + ano + "-" + mes + "-" + dia + "'" +
												") ");

											//atualizar a ALI
											SQLExecutar.AppendLine(
												" UPDATE SCIEX_ALI SET " +
												 "  ALI_ST = " + (int)EnumAliStatus.ALI_DEFERIDA +
												"  ,ALI_DH_RESPOSTA_SISCOMEX = '" + ano + "-" + mes + "-" + dia + "'" +
												" WHERE ALI_NU = " + ali.NumeroAli.ToString()
												);

										}

										#endregion

										break;
									}
								case "99": //LI COM ERRO
									{
										qtdLIIndeferida = qtdLIIndeferida + 1;

										numeroALI = linha.Substring(2, 15).Trim();
										numeroLIProtocolda = linha.Substring(17, 10);
										codigoStatusDiagnostico = linha.Substring(27, 1);
										dataGeracaoDiagnostico = linha.Substring(28, 8);
										mensagemErro = linha.Substring(36, linha.Length - 36);

										long numeroALIPesquisa = Convert.ToInt64(numeroALI);
										var ali = _uowSciex.QueryStackSciex.Ali.Selecionar(o => o.NumeroAli == numeroALIPesquisa);
										var listaLI = _uowSciex.QueryStackSciex.Li.Listar(o => o.IdPliMercadoria == ali.IdPliMercadoria);

										string ano = dataGeracaoDiagnostico.Substring(0, 4);
										string mes = dataGeracaoDiagnostico.Substring(4, 2);
										string dia = dataGeracaoDiagnostico.Substring(6, 2);

										#region REGRA DE NEGOCIO¨06

										if (listaLI.Count > 0)
										{
											//Insere Tabela de li_historico
											SQLExecutar.AppendLine(
												"INSERT INTO SCIEX_LI_HISTORICO_ERRO " +
												"(" +
												"	LI_NU," +
												"	LI_NU_LI_PROTOCOLADA," +
												"	LI_DT_GERACAO," +
												"	LI_DS_MENSAGEM," +
												"	LI_NU_DIAGNOSTICO_ERRO," +
												"	LI_ST," +
												"	LHE_TP_ERRO," +
												"	LI_DH_CADASTRO," +
												"	LI_DH_CANCELAMENTO" +
												")" +
												"" +
												"SELECT " +
												"	LI_NU, " +
												"	LI_NU_LI_PROTOCOLADA, " +
												"	LI_DT_GERACAO, " +
												"	LI_DS_MENSAGEM, " +
												"	LI_NU_DIAGNOSTICO_ERRO, " +
												"	LI_ST, " +
												"	1, " +
												"	CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')) , " +
												"	LI_DH_CANCELAMENTO " +
												"FROM SCIEX_LI WHERE LI_NU = " + listaLI[0].NumeroLi.ToString()
												);

											//Atualiza LI
											SQLExecutar.AppendLine(
												"UPDATE SCIEX_LI SET " +
													"LI_NU = NULL, " +
													",LI_NU_PROTOCOLADA = " + numeroLIProtocolda +
													",LI_DT_GERACAO = '" + ano + "-" + mes + "-" + dia + "'" +
													",LI_DS_MENSAGEM =  '" + mensagemErro + "'" +
													",LI_NU_DIAGNOSTICO_ERRO = NULL " +
													",LI_DH_CANCELAMENTO = NULL " +
													"FROM SCIEX_LI WHERE LI_NU = " + numeroLI
												);
										}
										else
										{
											if (!mensagemErro.Contains("LICENCIAMENTO COM ERRO – NAO PROCESSADO"))
											{
												//INSERIR A LI
												SQLExecutar.AppendLine(
													"INSERT INTO SCIEX_LI " +
													"( PME_ID," +
													"  LAR_ID, " +
													"  LI_NU_LI_PROTOCOLADA, " +
													"  LI_NU_DIAGNOSTICO_ERRO, " +
													"  LI_DS_MENSAGEM, " +
													"  LI_ST, " +
													"  LI_TP, " +
													"  LI_DH_CADASTRO, " +
													"  LI_DT_GERACAO) " +
													"VALUES (" +
														ali.IdPliMercadoria.ToString() + "," +
														codigoLiArquivoRetorno.ToString() + "," +
														numeroLIProtocolda + "," +
														codigoStatusDiagnostico + "," +
														"'" + mensagemErro + "'," +
														"2," +
														"1," +
														"'" + GetDateTimeNowUtc().ToString() + "'," +
														"'" + ano + "-" + mes + "-" + dia + "'" +
													") ");

												//atualizar a ALI
												SQLExecutar.AppendLine(
													" UPDATE SCIEX_ALI SET " +
													 "  ALI_ST = " + (int)EnumAliStatus.ALI_INDEFERIDA_PELO_SISCOMEX +
													"  ,ALI_DH_RESPOSTA_SISCOMEX = '" + ano + "-" + mes + "-" + dia + "'" +
													" WHERE ALI_NU = " + ali.NumeroAli.ToString()
													);
											}
											else
											{
												//INSERIR A LI
												SQLExecutar.AppendLine(
													"INSERT INTO SCIEX_LI " +
													"( PME_ID," +
													"  LAR_ID, " +
													"  LI_NU_LI_PROTOCOLADA, " +
													"  LI_NU_DIAGNOSTICO_ERRO, " +
													"  LI_DS_MENSAGEM, " +
													"  LI_ST, " +
													"  LI_TP, " +
													"  LI_DH_CADASTRO, " +
													"  LI_DT_GERACAO) " +
													"VALUES (" +
														ali.IdPliMercadoria.ToString() + "," +
														codigoLiArquivoRetorno.ToString() + "," +
														numeroLIProtocolda + "," +
														codigoStatusDiagnostico + "," +
														"'" + mensagemErro + "'," +
														"2," +
														"1," +
														"'" + GetDateTimeNowUtc().ToString() + "'," +
														"'" + ano + "-" + mes + "-" + dia + "'" +
													") ");

												//atualizar a ALI
												SQLExecutar.AppendLine(
													" UPDATE SCIEX_ALI SET " +
													"  ALI_ST = " + (int)EnumAliStatus.ALI_GERADA +
													"  ,ALI_DH_RESPOSTA_SISCOMEX = '" + ano + "-" + mes + "-" + dia + "'" +
													" WHERE ALI_NU = " + ali.NumeroAli.ToString()
													);
											}
										}

										#endregion

										break;
									}
								case "98": //ERRO DE ESTRUTURA DO ARQUIVO
									{
										qtdErros = qtdErros + 1;

										numeroALI = linha.Substring(2, 15).Trim();
										mensagemErro = linha.Substring(17, linha.Length - 17);

										long numeroALIPesquisa = Convert.ToInt64(numeroALI);
										var ali = _uowSciex.QueryStackSciex.Ali.Selecionar(o => o.NumeroAli == numeroALIPesquisa);
										var listaLI = _uowSciex.QueryStackSciex.Li.Listar(o => o.IdPliMercadoria == ali.IdPliMercadoria);

										#region REGRA DE NEGOCIO 07

										if (listaLI.Count > 0)
										{
											//Insere Tabela de li_historico
											SQLExecutar.AppendLine(
												"INSERT INTO SCIEX_LI_HISTORICO_ERRO " +
												"(" +
												"	LI_NU," +
												"	LI_NU_LI_PROTOCOLADA," +
												"	LI_DT_GERACAO," +
												"	LI_DS_MENSAGEM," +
												"	LI_NU_DIAGNOSTICO_ERRO," +
												"	LI_ST," +
												"	LHE_TP_ERRO," +
												"	LI_DH_CADASTRO," +
												"	LI_DH_CANCELAMENTO" +
												")" +
												"" +
												"SELECT " +
												"	LI_NU, " +
												"	LI_NU_LI_PROTOCOLADA, " +
												"	LI_DT_GERACAO, " +
												"	LI_DS_MENSAGEM, " +
												"	LI_NU_DIAGNOSTICO_ERRO, " +
												"	LI_ST, " +
												"	1, " +
												"	CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')) , " +
												"	LI_DH_CANCELAMENTO " +
												"FROM SCIEX_LI WHERE LI_NU = " + listaLI[0].NumeroLi.ToString()
												);

											//Atualiza LI
											SQLExecutar.AppendLine(
												"UPDATE SCIEX_LI SET " +
													"LI_NU = NULL, " +
													",LI_NU_PROTOCOLADA = NULL " +
													",LI_DS_MENSAGEM =  '" + mensagemErro + "'" +
													",LI_NU_DIAGNOSTICO_ERRO = NULL " +
													",LI_DH_CANCELAMENTO = NULL " +
													",LI_DH_CANCELAMENTO = NULL " +
													",LI_ST = 2 " +
													"FROM SCIEX_LI WHERE LI_NU = " + numeroLI
												);

											//atualizar a ALI
											SQLExecutar.AppendLine(
												" UPDATE SCIEX_ALI SET " +
													"  ALI_ST = " + (int)EnumAliStatus.ALI_GERADA +
												"  ,ALI_DH_RESPOSTA_SISCOMEX = CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00'))" +
												" WHERE ALI_NU = " + ali.NumeroAli.ToString()
												);
										}
										else
										{
											//INSERIR A LI
											SQLExecutar.AppendLine(
												"INSERT INTO SCIEX_LI " +
												"( PME_ID," +
												"  LAR_ID, " +
												"  LI_DS_MENSAGEM, " +
												"  LI_ST, " +
												"  LI_TP, " +
												"  LI_DH_CADASTRO) " +
												"VALUES (" +
													ali.IdPliMercadoria.ToString() + "," +
													codigoLiArquivoRetorno.ToString() + "," +
													"'" + mensagemErro + "'," +
													"2," +
													"1," +
													"'" + GetDateTimeNowUtc().ToString() + "'," +
												") ");

											//atualizar a ALI
											SQLExecutar.AppendLine(
												" UPDATE SCIEX_ALI SET " +
													"  ALI_ST = " + (int)EnumAliStatus.ALI_GERADA +
												"  ,ALI_DH_RESPOSTA_SISCOMEX = CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00'))" +
												" WHERE ALI_NU = " + ali.NumeroAli.ToString()
												);
										}

										#endregion

										break;
									}
								default:
									break;
							}
							#endregion
						}
					}
					contador++;
				}
				file.Close();

				#region REGRA DE NEGOCIO 08
				SQLExecutar.AppendLine(
					"UPDATE SCIEX_LI_ARQUIVO_RETORNO SET " +
					   "LAR_QT_LI = " + qtdLI.ToString() +
					   ",LAR_QT_LI_DEFERIDA = " + qtdLIDeferida.ToString() +
					   ",LAR_QT_LI_INDEFERIDA = " + qtdLIIndeferida.ToString() +
					   ",LAR_QT_LI_ERRO = " + qtdErros.ToString() +
					"WHERE LAR_ID =" + codigoLiArquivoRetorno
				);
				#endregion

				SQLExecutar.AppendLine(
				"					UPDATE SCIEX_CONTROLE_EXEC_SERVICO " +
				"					SET " +
				"						CES_DH_EXECUCAO_FIM = CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')), " +
				"						CES_ST_EXECUCAO = 2, " +
				"						CES_ME_OBJETO_RETORNO = 'Arquivo de LI (" + codigoLiArquivoRetorno.ToString() + " lido com sucesso' " +
				"					WHERE CES_ID = @CONTROLE_SERVICO_COD " +
					"COMMIT; " +
				"END TRY   " +
				"BEGIN CATCH " +
				"	IF @@TRANCOUNT > 0 " +
				"	BEGIN " +
				"		ROLLBACK " +
				"		SELECT " +
				"			 ERROR_NUMBER() AS ErrorNumber " +
				"			, ERROR_SEVERITY() AS ErrorSeverity " +
				"			, ERROR_STATE() AS ErrorState " +
				"			, ERROR_PROCEDURE() AS ErrorProcedure " +
				"			, ERROR_LINE() AS ErrorLine " +
				"			, ERROR_MESSAGE() AS ErrorMessage; " +
				"				BEGIN TRANSACTION " +
				"					UPDATE SCIEX_CONTROLE_EXEC_SERVICO " +
				"					SET " +
				"						CES_DH_EXECUCAO_FIM = CONVERT(DATETIME, SWITCHOFFSET(GETDATE(), '-01:00')), " +
				"						CES_ST_EXECUCAO = 2, " +
				"						CES_ME_OBJETO_RETORNO = ERROR_MESSAGE() " +
				"					WHERE CES_ID = @CONTROLE_SERVICO_COD " +
				"				COMMIT " +
				"	END " +
				"END CATCH ");

				string sql = SQLExecutar.ToString();
				_uowSciex.CommandStackSciex.Salvar(sql);
			}


			//arquivo de cancelamento da LI
			nomeDoArquivo = "ARQCANC.FILE";
			localAqruivoFTP = @"ftp://" + enderecoFTP + "/" + pastaFTP + "/" + nomeDoArquivo;

			if (Ftp.VerificarSeExisteArquivo(localAqruivoFTP, usuario, senha))
			{
				//TO-DO
			}
		}

		public void GerarArquivoSimulacaoALICancelamento()
		{
			StringBuilder arquivo = new StringBuilder();

			arquivo.Append("24");
			arquivo.Append("04407029000143");

			string dataInicio = "01/01/" + GetDateTimeNowUtc().Year.ToString();
			string dataAtual = GetDateTimeNowUtc().Day.ToString() + "/" + GetDateTimeNowUtc().Month.ToString() + "/" + GetDateTimeNowUtc().Year.ToString();
			TimeSpan date = Convert.ToDateTime(dataAtual) - Convert.ToDateTime(dataInicio);
			arquivo.Append((date.Days + 1).ToString("D3"));

			arquivo.Append(GetDateTimeNowUtc().Hour.ToString("D2") + GetDateTimeNowUtc().Minute.ToString("D2") + GetDateTimeNowUtc().Second.ToString("D2"));
			arquivo.Append("02");
			arquivo.Append("02");
			arquivo.Append("02");
			arquivo.Append("850");
			arquivo.AppendLine();

			List<AliEntity> listaALI = _uowSciex.QueryStackSciex.Ali.Listar(o => o.Status == (int)EnumAliStatus.ALI_ENVIADA_AO_SISCOMEX);

			long codigoLIProtocolada = 2459230332;
			long codigoLI = 2459230332;

			int contador = 1;
			foreach (AliEntity item in listaALI)
			{
				if (contador % 6 == 0)
				{
					codigoLIProtocolada++;
					arquivo.Append("99");
					arquivo.Append(item.NumeroAli.ToString().PadRight(15, ' '));
					arquivo.Append(codigoLIProtocolada.ToString("D10"));
					arquivo.Append("2");
					arquivo.Append(GetDateTimeNowUtc().Year.ToString("D4") + GetDateTimeNowUtc().Month.ToString("D2") + GetDateTimeNowUtc().Day.ToString("D2"));
					arquivo.Append("NUMERO DO DESTAQUE DA NCM PARA ANUENCIA - NAO INFORMADO");
				}
				else

				if (contador % 10 == 0)
				{
					codigoLIProtocolada++;
					arquivo.Append("99");
					arquivo.Append(item.NumeroAli.ToString().PadRight(15, ' '));
					arquivo.Append(codigoLIProtocolada.ToString("D10"));
					arquivo.Append("2");
					arquivo.Append(GetDateTimeNowUtc().Year.ToString("D4") + GetDateTimeNowUtc().Month.ToString("D2") + GetDateTimeNowUtc().Day.ToString("D2"));
					arquivo.Append("DECLARACAO NAO ACEITA - USUARIO NAO CADASTRADO COMO REPRESENTANTE LEGAL DO IMPORTADOR PARA AS ATIVIDADES DE IMPORTACAO");
				}
				else
				{
					arquivo.Append("01");
					arquivo.Append(item.NumeroAli.ToString().PadRight(15, ' '));
					arquivo.Append(codigoLIProtocolada.ToString("D10"));
					arquivo.Append(codigoLI.ToString("D10"));
					arquivo.Append(GetDateTimeNowUtc().Year.ToString("D4") + GetDateTimeNowUtc().Month.ToString("D2") + GetDateTimeNowUtc().Day.ToString("D2"));
				}

				arquivo.AppendLine();

				codigoLIProtocolada++;
				codigoLI++;
				contador++;
			}

			if (!Ftp.VerificarSeExisteArquivo(@"ftp://192.168.0.251/ArquivoRetornoSISCOMEX/SERPRO.FILE", "ctis", "ctis"))
			{
				string mensagem = Ftp.EnviarArquivo(@"ftp://192.168.0.251/ArquivoRetornoSISCOMEX/SERPRO.FILE", "ctis", "ctis", arquivo.ToString());

				//if (File.Exists(fileName))
				//{
				//	File.Delete(fileName);
				//}

				//using (StreamWriter sw = File.CreateText(fileName))
				//{
				//	sw.Write(arquivo.ToString());
				//}
			}

		}


	}
}
