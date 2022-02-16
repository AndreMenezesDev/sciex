using FluentValidation;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Configuration;
using Suframa.Sciex.BusinessLogic.Pss;
using System.Text;
using System.Web.UI;
using Suframa.Sciex.CrossCutting.Mensagens;
using Suframa.Sciex.CrossCutting.Compressor;
using System.IO;

namespace Suframa.Sciex.BusinessLogic
{
	public class PliAnaliseVisualBll : IPliAnaliseVisualBll
	{
		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUnitOfWork _uowCadsuf;
		private readonly IUsuarioPssBll _usuarioPssBll;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;
		private readonly IViewImportadorBll _IViewImportadorBll;
		private readonly IComplementarPLIBll _complementarPLIBll;


		private long _idPLiRetorno;

		public PliAnaliseVisualBll(IUnitOfWorkSciex uowSciex, IUnitOfWork uowCadsuf,
			 IViewImportadorBll viewImportadorBll, IComplementarPLIBll complementarPLIBll,
			 IUsuarioPssBll usuarioPssBll, IUsuarioInformacoesBll usuarioInformacoesBll)
		{
			_uowSciex = uowSciex;
			_uowCadsuf = uowCadsuf;
			_IViewImportadorBll = viewImportadorBll;
			_complementarPLIBll = complementarPLIBll;
			_usuarioPssBll = usuarioPssBll;
			_usuarioInformacoesBll = usuarioInformacoesBll;
		}

		public IEnumerable<PliVM> Listar(PliVM pliVM)
		{
			var pli = _uowSciex.QueryStackSciex.Pli.Listar<PliVM>();
			return AutoMapper.Mapper.Map<IEnumerable<PliVM>>(pli);
		}

		public IEnumerable<object> Listar()
		{
			return _uowSciex.QueryStackSciex.Pli
				.Listar()
				.OrderBy(o => o.NumeroPli)
				.Select(
					s => new
					{
						id = s.IdPLI,
						text = s.CodigoCNAE + " - " + s.NumeroPli
					});
		}

		public PliVM SalvarResposta(PliVM pliVM)
		{
			if(pliVM.Anexo != null && pliVM.NomeAnexo != null)
			{
				Compressor objComprimir = new Compressor();

				string local = pliVM.LocalPastaEstruturaArquivo + @"\" + pliVM.NomeAnexo.Substring(0, pliVM.NomeAnexo.IndexOf("."));

				if (!Directory.Exists(local))
				{
					Directory.CreateDirectory(local);
				}

				string[] arquivos = Directory.GetFiles(local);

				foreach (string item in arquivos)
				{
					File.Delete(item);
				}

				if (!objComprimir.UnZIP(pliVM.Anexo, local))
				{
					foreach (string item in arquivos)
					{
						File.Delete(item);
					}
					Directory.Delete(local);
					return new PliVM() { Mensagem = "Tipo de compactação não suportada, não é possível realizar envio do arquivo. É necessário enviar arquivo com compactação ZIP" };
				}
			}

			PliAnaliseVisualEntity analiseVisual;
			long IdPliLong = Convert.ToInt64(pliVM.IdPLI.Value);
			analiseVisual = _uowSciex.QueryStackSciex.PliAnaliseVisual.Selecionar(o => o.IdPLI == IdPliLong);
			if (analiseVisual != null)
			{
				analiseVisual.DescricaoResposta = pliVM.DescricaoResposta;
				analiseVisual.StatusAnalise = 2;

				_uowSciex.CommandStackSciex.PliAnaliseVisual.Salvar(analiseVisual);
				_uowSciex.CommandStackSciex.Save();

				if (pliVM.Anexo != null && pliVM.NomeAnexo != null)
				{
					PliAnaliseVisualAnexoEntity anexo = new PliAnaliseVisualAnexoEntity();
					anexo.IdPli = IdPliLong;
					anexo.NomeArquivo = pliVM.NomeAnexo;
					anexo.Arquivo = pliVM.Anexo;
					_uowSciex.CommandStackSciex.PliAnaliseVisualAnexo.Salvar(anexo);
					_uowSciex.CommandStackSciex.Save();
				}
			}
			else
				return new PliVM() { Mensagem = "Não foi possivel Salvar a Resposta!" };

			return new PliVM() { Mensagem = "Salvo com Sucesso!" };

		}

		public PliVM Salvar(PliVM pliVM)
		{
			PliAnaliseVisualEntity analiseVisual;
			long IdPliLong = Convert.ToInt64(pliVM.IdPLI.Value);
			analiseVisual = _uowSciex.QueryStackSciex.PliAnaliseVisual.Selecionar(o => o.IdPLI == IdPliLong);
			if(analiseVisual != null)
			{
				analiseVisual.StatusAnalise = Convert.ToInt16(pliVM.StatusPliAnalise);
				
				analiseVisual.IdCodigoConta = pliVM.IdConta;
				analiseVisual.IdCodigoUtilizacao = pliVM.IdConta;

				if(analiseVisual.StatusAnalise == 9 || analiseVisual.StatusAnalise == 12)
				{
					analiseVisual.DataPendencia = DateTime.Now;

					PliHistoricoEntity hist = new PliHistoricoEntity();
					hist.DataEvento = DateTime.Now;
					hist.IdPLI = pliVM.IdPLI;
					hist.Observacao = pliVM.Motivo;
					hist.CPFCNPJ = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoCpfCnpj.CnpjCpfUnformat();
					hist.NomeResponsavel = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoNome;

					if(analiseVisual.StatusAnalise == 9)
					{
						hist.StatusPli = 9;
						hist.DescricaoStatusPli = "ANÁLISE VISUAL PENDENTE";
					}
					if(analiseVisual.StatusAnalise == 12)
					{
						hist.StatusPli = 12;
						hist.DescricaoStatusPli = "ANÁLISE VISUAL INDEFERIDO";
					}

					_uowSciex.CommandStackSciex.PliHistorico.Salvar(hist, true);
					_uowSciex.CommandStackSciex.Save();
				}
				else
				{
					analiseVisual.DataAnalise = DateTime.Now;
					analiseVisual.DescricaoMotivo = pliVM.Motivo;
				}
					

				_uowSciex.CommandStackSciex.PliAnaliseVisual.Salvar(analiseVisual);
				_uowSciex.CommandStackSciex.Save();
			}
			else
			{
				analiseVisual = new PliAnaliseVisualEntity();
				analiseVisual.IdPLI = pliVM.IdPLI.Value;
				analiseVisual.StatusAnalise = Convert.ToInt16(pliVM.StatusPliAnalise);
				analiseVisual.IdCodigoConta = pliVM.IdConta;
				analiseVisual.IdCodigoUtilizacao = pliVM.IdConta;

				if (pliVM.StatusPliAnalise == 9 || analiseVisual.StatusAnalise == 12)
				{
					analiseVisual.DataPendencia = DateTime.Now;

					PliHistoricoEntity hist = new PliHistoricoEntity();
					hist.DataEvento = DateTime.Now;
					hist.IdPLI = pliVM.IdPLI;
					hist.Observacao = pliVM.Motivo;
					hist.CPFCNPJ = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoCpfCnpj.CnpjCpfUnformat();
					hist.NomeResponsavel = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoNome;

					if (analiseVisual.StatusAnalise == 9)
					{
						hist.StatusPli = 9;
						hist.DescricaoStatusPli = "ANÁLISE VISUAL PENDENTE";
					}
					if (analiseVisual.StatusAnalise == 12)
					{
						hist.StatusPli = 12;
						hist.DescricaoStatusPli = "ANÁLISE VISUAL INDEFERIDO";
					}

					_uowSciex.CommandStackSciex.PliHistorico.Salvar(hist, true);
					_uowSciex.CommandStackSciex.Save();
				}
				else
				{
					analiseVisual.DataAnalise = DateTime.Now;
					analiseVisual.DescricaoMotivo = pliVM.Motivo;
				}

				_uowSciex.CommandStackSciex.PliAnaliseVisual.Salvar(analiseVisual, true);
				_uowSciex.CommandStackSciex.Save();
			}

			AuditoriaEntity audt = new AuditoriaEntity();
			audt.IdAuditoriaAplicacao = 3;
			audt.CpfCnpjResponsavel = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoCpfCnpj.CnpjCpfUnformat();
			audt.NomeResponsavel = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoNome;
			audt.TipoAcao = 1;
			audt.DataHoraAcao = DateTime.Now;
			audt.IdReferencia = pliVM.IdPLI.Value;
			audt.DescricaoAcao = "INCLUSÃO: Inserindo o registro: ["+ pliVM.IdPLI.Value + "]";

			_uowSciex.CommandStackSciex.Auditoria.Salvar(audt);
			_uowSciex.CommandStackSciex.Save();


			if (pliVM.StatusPliAnalise == 7 || pliVM.StatusPliAnalise == 11)
			{
				var pli = _uowSciex.QueryStackSciex.Pli.Selecionar(o => o.IdPLI == pliVM.IdPLI);
				pli.StatusPli = 24;

				_uowSciex.CommandStackSciex.Pli.Salvar(pli);
				_uowSciex.CommandStackSciex.Save();

				PliHistoricoEntity hist = new PliHistoricoEntity();
				hist.IdPLI = pliVM.IdPLI;
				hist.DataEvento = DateTime.Now;
				hist.Observacao = "FINALIZAÇÃO DA ANÁLISE VISUAL PARA " + pliVM.StatusPliAnalise;
				hist.CPFCNPJ = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoCpfCnpj.CnpjCpfUnformat();
				hist.NomeResponsavel = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoNome;
				hist.StatusPli = pli.StatusPli;
				hist.DescricaoStatusPli = "AGUARDANDO PROCESSAMENTO";

				_uowSciex.CommandStackSciex.PliHistorico.Salvar(hist, true);
				_uowSciex.CommandStackSciex.Save();
			}
			else if (pliVM.StatusPliAnalise == 8 || pliVM.StatusPliAnalise == 12)
			{
				var pli = _uowSciex.QueryStackSciex.Pli.Selecionar(o => o.IdPLI == pliVM.IdPLI);
				pli.StatusPli = 25;
				pli.StatusPliProcessamento = 3;

				_uowSciex.CommandStackSciex.Pli.Salvar(pli);
				_uowSciex.CommandStackSciex.Save();

				PliHistoricoEntity hist = new PliHistoricoEntity();
				hist.DataEvento = DateTime.Now;
				hist.IdPLI = pliVM.IdPLI;
				hist.Observacao = "FINALIZAÇÃO DA ANÁLISE VISUAL PARA " + pliVM.StatusPliAnalise;
				hist.CPFCNPJ = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoCpfCnpj.CnpjCpfUnformat();
				hist.NomeResponsavel = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoNome;
				hist.StatusPli = pli.StatusPli;
				hist.DescricaoStatusPli = "PLI: 25-PROCESSADO; PROCESSAMENTO: 3-REPROVADO";

				_uowSciex.CommandStackSciex.PliHistorico.Salvar(hist, true);
				_uowSciex.CommandStackSciex.Save();

				ErroProcessamentoEntity erroProc = new ErroProcessamentoEntity();
				erroProc.IdPli = pliVM.IdPLI;
				erroProc.IdErroMensagem = 416;
				erroProc.CodigoNivelErro = 1;
				erroProc.Descricao = pliVM.Motivo;
				erroProc.DataProcessamento = DateTime.Now;

				_uowSciex.CommandStackSciex.ErroProcessamento.Salvar(erroProc);
				_uowSciex.CommandStackSciex.Save();
			}

			return new PliVM() { Mensagem = "Salvo com Sucesso!"};

		}

		public PagedItems<PliVM> ListarPaginado(PliVM pagedFilter)
		{
			var dataInicio = pagedFilter.DataInicio == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataInicio.Value.Year, pagedFilter.DataInicio.Value.Month, pagedFilter.DataInicio.Value.Day);
			var dataFim = pagedFilter.DataFim == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataFim.Value.Year, pagedFilter.DataFim.Value.Month, pagedFilter.DataFim.Value.Day, 23, 59, 59);
			var usuCpfCnpjEmpresaOuLogado = _usuarioInformacoesBll.ObterCNPJ().Replace(".", "").Replace("-", "").Replace("/", ""); ;

			if (pagedFilter == null) { return new PagedItems<PliVM>(); }

			var pli = new PagedItems<PliVM>();

			if (pagedFilter.StatusPli == 0)
			{
				pli = _uowSciex.QueryStackSciex.Pli.ListarPaginado<PliVM>(o =>
				(
					(
						pagedFilter.NumeroPli == -1 || o.NumeroPli == pagedFilter.NumeroPli
					) 
					&&
					(
						pagedFilter.Ano == -1 || o.Ano == pagedFilter.Ano
					) 
					&&
					(
						pagedFilter.IdPLIAplicacao == 0 || o.IdPLIAplicacao == pagedFilter.IdPLIAplicacao
					) 
					&&
					(
						pagedFilter.TipoDocumento == 0 || o.TipoDocumento == pagedFilter.TipoDocumento
					)
					&&
					(
						pagedFilter.InscricaoCadastral == null || o.InscricaoCadastral == pagedFilter.InscricaoCadastral
					)
					&&
					(
						(pagedFilter.DataInicio == null) || (o.DataCadastro >= dataInicio && o.DataCadastro <= dataFim)
					)
					&& 
					(
						(o.Cnpj == usuCpfCnpjEmpresaOuLogado && usuCpfCnpjEmpresaOuLogado.Length == 14) ||
						(o.Cnpj == usuCpfCnpjEmpresaOuLogado && usuCpfCnpjEmpresaOuLogado == o.NumeroResponsavelRegistro)
					)
					&&
					(
						o.StatusAnaliseVisual == 1
					)
				),
				pagedFilter);



			}
			else
			{
				pli = _uowSciex.QueryStackSciex.Pli.ListarPaginado<PliVM>(o =>
				(
					(
						pagedFilter.NumeroPli == -1 || o.NumeroPli == pagedFilter.NumeroPli
					) 
					&&
					(
						pagedFilter.Ano == -1 || o.Ano == pagedFilter.Ano
					) 
					&&
					(
						pagedFilter.IdPLIAplicacao == 0 || o.IdPLIAplicacao == pagedFilter.IdPLIAplicacao
					)
					&&
					(
						pagedFilter.TipoDocumento == 0 || o.TipoDocumento == pagedFilter.TipoDocumento
					)
					&&
					(
						pagedFilter.InscricaoCadastral == null || o.InscricaoCadastral == pagedFilter.InscricaoCadastral
					)
					&&
					(
						(pagedFilter.DataInicio == null) || (o.DataCadastro >= dataInicio && o.DataCadastro <= dataFim)
					)
					&& 
					(
						(o.Cnpj == usuCpfCnpjEmpresaOuLogado && usuCpfCnpjEmpresaOuLogado.Length == 14) ||
						(o.Cnpj == usuCpfCnpjEmpresaOuLogado && usuCpfCnpjEmpresaOuLogado == o.NumeroResponsavelRegistro)
					)
					&&
					(
						o.StatusAnaliseVisual == 1
					)
				),
				pagedFilter);
			}

			var analiseVisual = _uowSciex.QueryStackSciex.PliAnaliseVisual.Listar();

			var q =
					from c in pli.Items
					join p in analiseVisual on c.IdPLI equals p.IdPLI into plisAnaliseVisual
					from p in plisAnaliseVisual.DefaultIfEmpty()
					select new PliAnaliseVisualEntity() { IdPLI = p.IdPLI , IdCodigoConta = p.IdCodigoConta, IdCodigoUtilizacao = p.IdCodigoUtilizacao, CodigoConta = p.CodigoConta, CodigoUtilizacao = p.CodigoUtilizacao, StatusAnalise = p.StatusAnalise , DescricaoMotivo = p.DescricaoMotivo ,   };

			return pli;

		}

		public PagedItems<PliVM> ListarPaginadoSql(PliVM pagedFilter)
		{
			var ret = _uowSciex.QueryStackSciex.ListarPaginadoSql<PliVM>(MontarConsulta(pagedFilter), pagedFilter);

			foreach (var item in ret.Items)
			{
				item.NumeroPliConcatenado = item.Ano + "/" + item.NumeroPli;

				item.DescricaoTipoDocumento = (item.TipoDocumento == 1 ? EnumPliTipoDocumento.NORMAL.ToString() : (item.TipoDocumento == 2) ? EnumPliTipoDocumento.SUBSTITUTIVO.ToString() : (item.TipoDocumento == 3) ? "RETIFICADORA" : "");

				if (item.IdPliAnalise == null || item.StatusPliAnalise == 02)
					item.StatusPliAnaliseFormatado = "EM ANÁLISE VISUAL";
				else if (item.StatusPliAnalise == 07)
					item.StatusPliAnaliseFormatado = "ANÁLISE VISUAL OK";
				else if (item.StatusPliAnalise == 08)
					item.StatusPliAnaliseFormatado = "ANÁLISE VISUAL NÃO OK";
				else if (item.StatusPliAnalise == 09)
					item.StatusPliAnaliseFormatado = "ANÁLISE VISUAL PENDENTE";
				else if (item.StatusPliAnalise == 11)
					item.StatusPliAnaliseFormatado = "ANÁLISE VISUAL DEFERIDO";
				else if (item.StatusPliAnalise == 12)
					item.StatusPliAnaliseFormatado = "ANÁLISE VISUAL INDEFERIDO";

				if (item.IdConta != null)
				{
					var ccods = _uowSciex.QueryStackSciex.CodigoConta.Selecionar(o => o.IdCodigoConta == item.IdConta);
					item.CodigoContaFormatada = ccods.Codigo.ToString("D2") + " | " + ccods.Descricao;
				}
				if (item.IdUtilizacao != null)
				{
					var cutds = _uowSciex.QueryStackSciex.CodigoUtilizacao.Selecionar(o => o.IdCodigoUtilizacao == item.IdUtilizacao);
					item.CodigoUtilizacaoFormatada = cutds.Codigo.ToString("D2") + " | " + cutds.Descricao;
				}

				if (!String.IsNullOrEmpty(item.DescricaoResposta)){
					var anexo = _uowSciex.QueryStackSciex.PliAnaliseVisualAnexo.Selecionar(o => o.IdPli == item.IdPLI);
					if(anexo != null)
					{
						item.Anexo = anexo.Arquivo;
						item.NomeAnexo = anexo.NomeArquivo;
					}
				}
			}

			return ret;
		}

		private string MontarConsulta(PliVM pagedFilter)
		{
			var dataInicio = pagedFilter.DataInicio == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataInicio.Value.Year, pagedFilter.DataInicio.Value.Month, pagedFilter.DataInicio.Value.Day);
			var dataFim = pagedFilter.DataFim == null ? new DateTime(1, 1, 1) : new DateTime(pagedFilter.DataFim.Value.Year, pagedFilter.DataFim.Value.Month, pagedFilter.DataFim.Value.Day, 23, 59, 59);
			var usuCpfCnpjEmpresaOuLogado = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoCpfCnpj.CnpjCpfUnformat();

			StringBuilder sb = new StringBuilder();

			sb.AppendLine("select p.PLI_ID as IdPli, p.PLI_NU as NumeroPli , p.PLI_NU_ANO as Ano, p.pli_tp_documento as TipoDocumento , p.INS_CO as InscricaoCadastral,");
			sb.AppendLine("		p.IMP_DS_RAZAO_SOCIAL as RazaoSocial , Convert(Varchar(10), p.PLI_DH_ENVIO, 103) as DataEnvioPliFormatada,");
			sb.AppendLine("		Convert(Varchar(10), a.pav_dh_pendencia , 103) as DataPendenciaFormatada ,");
			sb.AppendLine("		Convert(Varchar(10), a.pav_dh_analise, 103) as DataAnaliseFormatada,");
			sb.AppendLine("		a.pav_no_user as NomeResponsavelRegistro, a.pav_st_analise as StatusPliAnalise ,");
			sb.AppendLine("		a.pli_id as IdPliAnalise, a.pav_ds_motivo as Motivo, a.pav_ds_resposta as DescricaoResposta,");
			sb.AppendLine("		a.cut_id as IdUtilizacao, a.cco_id as IdConta,");
			sb.AppendLine("		h.PHI_NO_RESPONSAVEL as NomeAnalistaPendencia, h.PHI_DS_OBSERVACAO as MotivoPendencia");
			sb.AppendLine("from SCIEX_PLI as p");
			sb.AppendLine("LEFT OUTER JOIN SCIEX_PLI_ANALISE_VISUAL as a on (p.PLI_ID = a.pli_id)");
			sb.AppendLine("LEFT OUTER JOIN SCIEX_PLI_HISTORICO as h on(p.PLI_ID = h.pli_id and h.PLI_ST_PLI in (9,12))");
			sb.AppendLine("where 1 = 1 ");
			sb.AppendLine("AND p.PLI_ST_PLI in (23,24,25)");
			if(pagedFilter.NumeroPli != -1 && pagedFilter.Ano != -1)
			{
				sb.AppendLine("AND p.PLI_NU = '" + pagedFilter.NumeroPli.ToString() + "'");
				sb.AppendLine("AND p.PLI_NU_ANO = '"+ pagedFilter.Ano.ToString() +"'");
			} 
			if(pagedFilter.IdPLIAplicacao != 0)
				sb.AppendLine("AND p.PAP_ID = " + pagedFilter.IdPLIAplicacao.ToString());
			if(pagedFilter.TipoDocumento != 0)
				sb.AppendLine("AND p.PLI_TP_DOCUMENTO = " + pagedFilter.TipoDocumento.ToString());
			if(pagedFilter.InscricaoCadastral != null)
				sb.AppendLine("AND p.INS_CO = " + pagedFilter.InscricaoCadastral.ToString());
			if (pagedFilter.DataInicio != null)
				sb.AppendLine("AND p.PLI_DH_ENVIO >= convert(datetime, '" + dataInicio.ToString() + "' , 103) AND p.PLI_DH_ENVIO <= convert(datetime, '" + dataFim.ToString() + "' , 103)");

			if (!String.IsNullOrEmpty(pagedFilter.RazaoSocial))
			{
				sb.AppendLine("AND p.IMP_DS_RAZAO_SOCIAL like '%"+pagedFilter.RazaoSocial+"%'");
			}

			if (usuCpfCnpjEmpresaOuLogado != null)
				sb.AppendLine("AND a.PAV_NU_USER = '" + usuCpfCnpjEmpresaOuLogado + "'");

			if (pagedFilter.StatusPliSelecionado != null && pagedFilter.StatusPliSelecionado > 0)
				sb.AppendLine("AND a.pav_st_analise = " + pagedFilter.StatusPliSelecionado.ToString());
			else
			{
				sb.AppendLine("AND a.pav_st_analise in (2,7,8,9,11,12)");
			}
				
			sb.AppendLine("and p.PLI_ST_ANALISE_VISUAL = 1");


			return sb.ToString();
		}

		public PliVM Selecionar(long? idPli)
		{
			var pliVM = new PliVM();
			if (!idPli.HasValue)
			{
				return pliVM;
			}

			var pli = _uowSciex.QueryStackSciex.Pli.Selecionar(x => x.IdPLI == idPli);
			var pliTaxaDebito= _uowSciex.QueryStackSciex.TaxaPliDebito.Listar(p => p.IdPli == pli.IdPLI).OrderBy(k => k.IdDebito).FirstOrDefault();
			var pliTaxa = _uowSciex.QueryStackSciex.TaxaPli.Selecionar(p => p.IdPli == pli.IdPLI);

			if (pli == null)
			{
				return null;
			}
			
			var importador = _uowSciex.QueryStackSciex.ViewImportador.Selecionar(o => o.Cnpj == pli.Cnpj);
			var cnae = _uowCadsuf.QueryStack.ViewAtividadeEconomicaPrincipal.Selecionar(o => o.CNPJ == pli.Cnpj);

			var uticon = _uowSciex.QueryStackSciex.ControleImportacao.Listar(o => o.IdPliAplicacao == pli.IdPLIAplicacao).FirstOrDefault();

			pliVM = AutoMapper.Mapper.Map<PliVM>(pli);
			pliVM.Endereco = importador.Endereco;
			pliVM.Numero = importador.Numero;
			pliVM.Complemento = importador.Complemento;
			pliVM.Bairro = importador.Bairro;
			pliVM.CodigoMunicipio = importador.CodigoMunicipio.ToString();
			pliVM.TemProjetoAprovado = importador.TemProjetoAprovado;


			if (pliVM.DataDebitoGeracao == null)
			{
				pliVM.DescricaoDebito = " - ";
				pliVM.Situacao = " - ";
				pliVM.DescricaoValorGeralTcif = " - ";
			}
			else
			{
				pliVM.DescricaoDebito = pliTaxaDebito == null ? " - " : pliTaxaDebito.NumeroDebito.ToString() + "/" + pliTaxaDebito.AnoDebito.ToString();
				if (pliTaxaDebito == null)
				{
					pliVM.Situacao = " - ";
				}
				else
				{
					if (pliTaxaDebito.NumeroControleCobrancaTCIF == 0)
						pliVM.Situacao = "Cobrar Débito";
					if (pliTaxaDebito.NumeroControleCobrancaTCIF == 1)
						pliVM.Situacao = "Não Cobrar Débito";
					if (pliTaxaDebito.NumeroControleCobrancaTCIF == 2)
						pliVM.Situacao = "Suspender Débito";
				}
				pliVM.DescricaoValorGeralTcif = pliTaxa == null ? " - " : pliTaxa.ValorGeralTCIF.ToString();
			}


			pliVM.Municipio = importador.Municipio;
			pliVM.UF = importador.UF;
			pliVM.CEP = string.Format("{0:00000-000}", importador.CEP);
			pliVM.DescricaoCNAE = cnae.Descricao;
			pliVM.PaisCodigo = importador.CodigoPais;
			pliVM.PaisDescricao = importador.DescricaoPais;
			pliVM.Telefone = (importador.Telefone.Length == 10 ? string.Format("{0:(00) 0000-0000}", Convert.ToDecimal(importador.Telefone)) : string.Format("{0:(##) #####-####}", Convert.ToDecimal(importador.Telefone)));
			if(uticon != null)
			{
				pliVM.CodigoUtilizacao = uticon.CodigoUtilizacao.Codigo.ToString();
				pliVM.DescricaoUtilizacao = uticon.CodigoUtilizacao.Descricao.ToString();
				pliVM.CodigoConta = uticon.CodigoConta.Codigo.ToString();
				pliVM.DescricaoConta = uticon.CodigoConta.Descricao.ToString();
			}
			if (pli.NumeroLIReferencia != null)
			{
				if (!String.IsNullOrEmpty(pli.NumeroLIReferencia.Trim()))
				{
					var numLi = Int64.Parse(pli.NumeroLIReferencia);

					var idLi = _uowSciex.QueryStackSciex.Li.Selecionar(o => o.NumeroLi == numLi).IdPliMercadoria;
					pliVM.NumeroALISubstitutiva = _uowSciex.QueryStackSciex.Ali.Selecionar(o => o.IdPliMercadoria == idLi).NumeroAli;

					pliVM.IdLiReferencia = idLi;

					var idPLi = _uowSciex.QueryStackSciex.PliMercadoria.Selecionar(o => o.IdPliMercadoria == idLi).IdPLI;
					pliVM.NumeroPLISubstitutivo = _uowSciex.QueryStackSciex.Pli.Selecionar(o => o.IdPLI == idPLi).NumeroPli;

					pliVM.IdPLISubstitutivo = idPLi;
					pliVM.IdPliMercadoriaSubstitutivo = _uowSciex.QueryStackSciex.Ali.Selecionar(o => o.IdPliMercadoria == idLi).IdPliMercadoria;

					var NumeroPLISubstitutivoInt = Convert.ToInt32(pliVM.NumeroPLISubstitutivo);

					pliVM.AnoPliSubstitutivo = _uowSciex.QueryStackSciex.Pli.Selecionar(o => o.IdPLI == idPLi).Ano;
					pliVM.NumeroPliSubstitutivoConcatenado = (pliVM.AnoPliSubstitutivo + "/" + NumeroPLISubstitutivoInt.ToString("d6"));
				} 
			}

			if (pliVM.StatusAnaliseVisual == 1)
			{
				var plianalise = _uowSciex.QueryStackSciex.PliAnaliseVisual.Selecionar(o => o.IdPLI == pliVM.IdPLI);

				if (plianalise == null || plianalise.IdPLI == null || plianalise.StatusAnalise == 02)
				{
					pliVM.StatusPliAnalise = 2;
					pliVM.StatusPliAnaliseFormatado = "EM ANÁLISE VISUAL";
				}
				else if (plianalise.StatusAnalise == 07)
				{
					pliVM.StatusPliAnalise = plianalise.StatusAnalise;
					pliVM.StatusPliAnaliseFormatado = "ANÁLISE VISUAL OK";
				}
				else if (plianalise.StatusAnalise == 08)
				{
					pliVM.StatusPliAnalise = plianalise.StatusAnalise;
					pliVM.StatusPliAnaliseFormatado = "ANÁLISE VISUAL NÃO OK";
				}
				else if (plianalise.StatusAnalise == 09)
				{
					pliVM.StatusPliAnalise = plianalise.StatusAnalise;
					pliVM.StatusPliAnaliseFormatado = "ANÁLISE VISUAL PENDENTE";
				}

				if (plianalise != null && !String.IsNullOrEmpty(plianalise.DescricaoResposta))
				{
					pliVM.DescricaoResposta = plianalise.DescricaoResposta;
					var anexo = _uowSciex.QueryStackSciex.PliAnaliseVisualAnexo.Selecionar(o => o.IdPli == pliVM.IdPLI);
					if(anexo != null)
					{
						pliVM.AnaliseVisualNomeAnexo = anexo.NomeArquivo;
						pliVM.AnaliseVisualAnexo = anexo.Arquivo;
					}
				}
			}

			var pliMotivoPendencia = _uowSciex.QueryStackSciex.PliHistorico.Selecionar(o => o.IdPLI == pli.IdPLI && o.StatusPli == 9);
			if(pliMotivoPendencia != null)
			{
				pliVM.MotivoPendencia = pliMotivoPendencia.Observacao;
			}
			

			return pliVM;
		}

		public IEnumerable<RelatorioRetificacoesVM> ListarRelatorio(long? idPli)
		{
			var relatorio = new List<RelatorioRetificacoesVM>();
			if (!idPli.HasValue)
			{
				return relatorio;
			}

			var pli = _uowSciex.QueryStackSciex.Pli.Selecionar(x => x.IdPLI == idPli);

			foreach (var item in pli.PLIMercadoria)
			{
				RelatorioRetificacoesVM itemRel = new RelatorioRetificacoesVM();
				long liReferencia = Convert.ToInt64(item.NumeroLiRetificador);
				itemRel.NumeroLiRetificador = item.NumeroLiRetificador.ToString();
				var idPliMercadoriaReferencia = _uowSciex.QueryStackSciex.Li.Selecionar(o => o.NumeroLi == liReferencia).IdPliMercadoria;
				var pliMercadoriaReferencia = _uowSciex.QueryStackSciex.PliMercadoria.Selecionar(o => o.IdPliMercadoria == idPliMercadoriaReferencia);
				#region Comparações de Campos
				//SCIEX_PLI_MERCADORIA
				itemRel.Lista = new List<CampoRetificacaoVM>();
				if (item.CodigoProduto != pliMercadoriaReferencia.CodigoProduto)
					itemRel.Lista.Add(new CampoRetificacaoVM() { Campo = "Produto", ValorAntigo = pliMercadoriaReferencia.CodigoProduto.ToString() , ValorNovo = item.CodigoProduto.ToString() });
				if (item.CodigoNCMMercadoria != pliMercadoriaReferencia.CodigoNCMMercadoria)
					itemRel.Lista.Add(new CampoRetificacaoVM() { Campo = "NCM", ValorAntigo = pliMercadoriaReferencia.CodigoNCMMercadoria.ToString(), ValorNovo = item.CodigoNCMMercadoria.ToString() });
				if (item.PesoLiquido != pliMercadoriaReferencia.PesoLiquido)
					itemRel.Lista.Add(new CampoRetificacaoVM() { Campo = "Peso Líquido", ValorAntigo = pliMercadoriaReferencia.PesoLiquido.ToString(), ValorNovo = item.PesoLiquido.ToString() });
				if (item.QuantidadeUnidadeMedidaEstatistica != pliMercadoriaReferencia.QuantidadeUnidadeMedidaEstatistica)
					itemRel.Lista.Add(new CampoRetificacaoVM() { Campo = "Quantidade na Medida Estatística", ValorAntigo = pliMercadoriaReferencia.QuantidadeUnidadeMedidaEstatistica.ToString(), ValorNovo = item.QuantidadeUnidadeMedidaEstatistica.ToString() });
				if (item.IdMoeda != pliMercadoriaReferencia.IdMoeda)
					itemRel.Lista.Add(new CampoRetificacaoVM() { Campo = "Moeda", ValorAntigo = pliMercadoriaReferencia.Moeda.Descricao, ValorNovo = item.Moeda.Descricao });
				if (item.IdIncoterms != pliMercadoriaReferencia.IdIncoterms)
					itemRel.Lista.Add(new CampoRetificacaoVM() { Campo = "INCOTERMS", ValorAntigo = pliMercadoriaReferencia.Incoterms.Descricao, ValorNovo = item.Incoterms.Descricao });
				if (item.DescricaoPais != pliMercadoriaReferencia.DescricaoPais)
					itemRel.Lista.Add(new CampoRetificacaoVM() { Campo = "País Origem Mercadoria", ValorAntigo = pliMercadoriaReferencia.DescricaoPais, ValorNovo = item.DescricaoPais });
				if (item.TipoCOBCambial != pliMercadoriaReferencia.TipoCOBCambial)
					itemRel.Lista.Add(new CampoRetificacaoVM() { Campo = "Tipo de Cobertura", ValorAntigo = GetTipoCoberturaCambial(pliMercadoriaReferencia.TipoCOBCambial.Value), ValorNovo = GetTipoCoberturaCambial(item.TipoCOBCambial.Value) });
				if (item.IdAladi != pliMercadoriaReferencia.IdAladi)
					itemRel.Lista.Add(new CampoRetificacaoVM() { Campo = "Acordo ALADI", ValorAntigo = pliMercadoriaReferencia.Aladi == null ? "-" : pliMercadoriaReferencia.Aladi.Descricao, ValorNovo = item.Aladi == null ? "-" : item.Aladi.Descricao });
				if (item.IdNaladi != pliMercadoriaReferencia.IdNaladi)
					itemRel.Lista.Add(new CampoRetificacaoVM() { Campo = "NALADI/SH", ValorAntigo = pliMercadoriaReferencia.Naladi == null ? "-" : pliMercadoriaReferencia.Naladi.Descricao, ValorNovo = item.Naladi == null ? "-" : item.Naladi.Descricao });
				if (item.TipoAcordoTarifario != pliMercadoriaReferencia.TipoAcordoTarifario)
					itemRel.Lista.Add(new CampoRetificacaoVM() { Campo = "Tipo de Acordo Tarifário", ValorAntigo = GetTipoAcordoTarifario(pliMercadoriaReferencia.TipoAcordoTarifario.Value), ValorNovo = GetTipoAcordoTarifario(item.TipoAcordoTarifario.Value) });
				if (item.DescricaoInformacaoComplementar != pliMercadoriaReferencia.DescricaoInformacaoComplementar)
					itemRel.Lista.Add(new CampoRetificacaoVM() { Campo = "Informações Complementares", ValorAntigo = pliMercadoriaReferencia.DescricaoInformacaoComplementar, ValorNovo = item.DescricaoInformacaoComplementar });
				if (item.DescricaoPaisOrigemFabricante != pliMercadoriaReferencia.DescricaoPaisOrigemFabricante)
					itemRel.Lista.Add(new CampoRetificacaoVM() { Campo = "País Origem Fabricante", ValorAntigo = pliMercadoriaReferencia.DescricaoPaisOrigemFabricante, ValorNovo = item.DescricaoPaisOrigemFabricante });
				//SCIEX_PLI_DETALHE_MERCADORIA
				if (item.PliDetalheMercadoria.Count != pliMercadoriaReferencia.PliDetalheMercadoria.Count)
					itemRel.Lista.Add(new CampoRetificacaoVM() { Campo = "Detalhes", ValorAntigo = pliMercadoriaReferencia.PliDetalheMercadoria.Count.ToString() , ValorNovo = item.PliDetalheMercadoria.Count.ToString() });
				if (item.PliDetalheMercadoria.Sum(o => o.ValorCondicaoVenda) != pliMercadoriaReferencia.PliDetalheMercadoria.Sum(o => o.ValorCondicaoVenda))
					itemRel.Lista.Add(new CampoRetificacaoVM() { Campo = "Valor Total dos Itens", ValorAntigo = pliMercadoriaReferencia.PliDetalheMercadoria.Sum(o => o.ValorCondicaoVenda).ToString(), ValorNovo = item.PliDetalheMercadoria.Sum(o => o.ValorCondicaoVenda).ToString() });
				
				//SCIEX_PLI_FORNECEDOR_FABRICANTE
				var pliFornecedorFabricanteRef = _uowSciex.QueryStackSciex.PliFornecedorFabricante.Selecionar(u => u.IdPliMercadoria == idPliMercadoriaReferencia);
				var pliFornecedorFabricante = _uowSciex.QueryStackSciex.PliFornecedorFabricante.Selecionar(u=>u.IdPliMercadoria == item.IdPliMercadoria);
				if (pliFornecedorFabricante.DescricaoFornecedor != pliFornecedorFabricanteRef.DescricaoFornecedor)
					itemRel.Lista.Add(new CampoRetificacaoVM() { Campo = "Fornecedor", ValorAntigo = pliFornecedorFabricanteRef == null ? "-"  : pliFornecedorFabricanteRef == null ? "-"  : pliFornecedorFabricanteRef.DescricaoFornecedor, ValorNovo = pliFornecedorFabricante == null ? "-" : pliFornecedorFabricante.DescricaoFornecedor });
				if (pliFornecedorFabricante.DescricaoPaisFornecedor != pliFornecedorFabricanteRef.DescricaoPaisFornecedor)
					itemRel.Lista.Add(new CampoRetificacaoVM() { Campo = "Fornecedor: País", ValorAntigo = pliFornecedorFabricanteRef == null ? "-"  : pliFornecedorFabricanteRef.DescricaoPaisFornecedor, ValorNovo = pliFornecedorFabricante == null ? "-" : pliFornecedorFabricante.DescricaoPaisFornecedor });
				if (pliFornecedorFabricante.DescricaoLogradouroFornecedor != pliFornecedorFabricanteRef.DescricaoLogradouroFornecedor)
					itemRel.Lista.Add(new CampoRetificacaoVM() { Campo = "Fornecedor: Logradouro", ValorAntigo = pliFornecedorFabricanteRef == null ? "-" : pliFornecedorFabricanteRef.DescricaoLogradouroFornecedor, ValorNovo = pliFornecedorFabricante == null ? "-" : pliFornecedorFabricante.DescricaoLogradouroFornecedor });
				if (pliFornecedorFabricante.DescricaoCidadeFornecedor != pliFornecedorFabricanteRef.DescricaoCidadeFornecedor)
					itemRel.Lista.Add(new CampoRetificacaoVM() { Campo = "Fornecedor: Cidade", ValorAntigo = pliFornecedorFabricanteRef == null ? "-" : pliFornecedorFabricanteRef.DescricaoCidadeFornecedor, ValorNovo = pliFornecedorFabricante == null ? "-" : pliFornecedorFabricante.DescricaoCidadeFornecedor });
				if (pliFornecedorFabricante.DescricaoEstadoFornecedor != pliFornecedorFabricanteRef.DescricaoEstadoFornecedor)
					itemRel.Lista.Add(new CampoRetificacaoVM() { Campo = "Fornecedor: Estado", ValorAntigo = pliFornecedorFabricanteRef == null ? "-" : pliFornecedorFabricanteRef.DescricaoEstadoFornecedor, ValorNovo = pliFornecedorFabricante == null ? "-" : pliFornecedorFabricante.DescricaoEstadoFornecedor });
				if (pliFornecedorFabricante.DescricaoFabricante != pliFornecedorFabricanteRef.DescricaoFabricante)
					itemRel.Lista.Add(new CampoRetificacaoVM() { Campo = "Fabricante", ValorAntigo = pliFornecedorFabricanteRef == null ? "-" : pliFornecedorFabricanteRef.DescricaoFabricante, ValorNovo = pliFornecedorFabricante == null ? "-" : pliFornecedorFabricante.DescricaoFabricante });
				if (pliFornecedorFabricante.DescricaoPaisFabricante != pliFornecedorFabricanteRef.DescricaoPaisFabricante)
					itemRel.Lista.Add(new CampoRetificacaoVM() { Campo = "Fabricante: País", ValorAntigo = pliFornecedorFabricanteRef == null ? "-" : pliFornecedorFabricanteRef.DescricaoPaisFabricante, ValorNovo = pliFornecedorFabricante == null ? "-" : pliFornecedorFabricante.DescricaoPaisFabricante });
				if (pliFornecedorFabricante.DescricaoLogradouroFabricante != pliFornecedorFabricanteRef.DescricaoLogradouroFabricante)
					itemRel.Lista.Add(new CampoRetificacaoVM() { Campo = "Fabricante: Logradouro", ValorAntigo = pliFornecedorFabricanteRef == null ? "-" : pliFornecedorFabricanteRef.DescricaoLogradouroFabricante, ValorNovo = pliFornecedorFabricante == null ? "-" : pliFornecedorFabricante.DescricaoLogradouroFabricante });
				if (pliFornecedorFabricante.DescricaoCidadeFabricante != pliFornecedorFabricanteRef.DescricaoCidadeFabricante)
					itemRel.Lista.Add(new CampoRetificacaoVM() { Campo = "Fabricante: Cidade", ValorAntigo = pliFornecedorFabricanteRef == null ? "-" : pliFornecedorFabricanteRef.DescricaoCidadeFabricante, ValorNovo = pliFornecedorFabricante == null ? "-" : pliFornecedorFabricante.DescricaoCidadeFabricante });
				if (pliFornecedorFabricante.DescricaoEstadoFabricante != pliFornecedorFabricanteRef.DescricaoEstadoFabricante)
					itemRel.Lista.Add(new CampoRetificacaoVM() { Campo = "Fabricante: Estado", ValorAntigo = pliFornecedorFabricanteRef == null ? "-" : pliFornecedorFabricanteRef.DescricaoEstadoFabricante, ValorNovo = pliFornecedorFabricante == null ? "-" : pliFornecedorFabricante.DescricaoEstadoFabricante });
				#endregion

				relatorio.Add(itemRel);
			}



			return relatorio;
		}

		public String GetTipoAcordoTarifario(int idAcordoTarifario)
		{
			string retorno = "";
			switch (idAcordoTarifario)
			{
				case 0:
					retorno = "-";
					break;
				case 2:
					retorno = "ALADI";
					break;
				case 3:
					retorno = "OMC";
					break;
				case 4:
					retorno = "SGPC";
					break;
			}
			return retorno;
		}

		public String GetTipoCoberturaCambial(int idCobertura)
		{
			string retorno = "";
			switch (idCobertura)
			{
				case 1: 
					retorno = "Até 180 dias";
					break;
				case 2:
					retorno = "De 180 até 360 dias";
					break;
				case 3:
					retorno = "Acima de 360 dias";
					break;
				case 4:
					retorno = "Sem cobertura";
					break;
			}
			return retorno;
		}

		public void Deletar(long id)
		{
			var pli = _uowSciex.QueryStackSciex.Pli.Selecionar(s => s.IdPLI == id);


			if (pli != null)
			{
				_uowSciex.CommandStackSciex.Pli.Apagar(pli.IdPLI);
			}
			_uowSciex.CommandStackSciex.Save();
		}

	}
}