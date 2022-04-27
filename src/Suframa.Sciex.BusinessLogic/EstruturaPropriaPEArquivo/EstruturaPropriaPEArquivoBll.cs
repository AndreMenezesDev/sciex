using FluentValidation;
using Suframa.Sciex.BusinessLogic.Pss;
using Suframa.Sciex.CrossCutting.Compressor;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.Mensagens;
using Suframa.Sciex.DataAccess;
using Suframa.Sciex.DataAccess.Database;
using Suframa.Sciex.DataAccess.Database.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;

namespace Suframa.Sciex.BusinessLogic
{
	public class EstruturaPropriaPEArquivo : IEstruturaPropriaPEArquivoBll
	{
		private Int16 quantidadeProduto;
		private string cnpjImportador;
		private string razaoSocial;

		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;
		private readonly IViewImportadorBll _viewImportadorBll;
		private readonly IUsuarioPssBll _usuarioPssBll;
		public EstruturaPropriaPEArquivo(IUsuarioLogado usuarioLogado, IUnitOfWorkSciex uowSciex,
			IUsuarioInformacoesBll usuarioInformacoesBll, IViewImportadorBll viewImportadorBll,
			IUsuarioPssBll usuarioPssBll)
		{
			_uowSciex = uowSciex;
			_usuarioInformacoesBll = usuarioInformacoesBll;
			_viewImportadorBll = viewImportadorBll;
			quantidadeProduto = 0;
			cnpjImportador = string.Empty;
			razaoSocial = string.Empty;
			_usuarioPssBll = usuarioPssBll;
		}

		public IEnumerable<EstruturaPropriaPLIArquivoVM> Listar(EstruturaPropriaPLIArquivoVM estruturaPropriaPLIArquivoVM)
		{
			var estruturapropriaarquivo = _uowSciex.QueryStackSciex.Aladi.Listar<EstruturaPropriaPLIArquivoVM>();
			return AutoMapper.Mapper.Map<IEnumerable<EstruturaPropriaPLIArquivoVM>>(estruturapropriaarquivo);
		}

		public IEnumerable<object> ListarChave(EstruturaPropriaPLIArquivoVM estruturaPropriaPLIArquivoVM)
		{
			return new List<object>();
		}

		public PagedItems<EstruturaPropriaPLIArquivoVM> ListarPaginado(EstruturaPropriaPLIArquivoVM pagedFilter)
		{
			try
			{
				if (pagedFilter == null) { return new PagedItems<EstruturaPropriaPLIArquivoVM>(); }
				var estruturapropriaarquivo = _uowSciex.QueryStackSciex.EstruturaPropriaPLIArquivo.ListarPaginado<EstruturaPropriaPLIArquivoVM>(o =>
					(
						(
							pagedFilter.IdEstruturaPropria == -1 || o.IdEstruturaPropria == pagedFilter.IdEstruturaPropria
						)
					),
					pagedFilter);

				return estruturapropriaarquivo;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message.ToString());

			}
			return new PagedItems<EstruturaPropriaPLIArquivoVM>();
		}

		public int Salvar(EstruturaPropriaPLIArquivoVM estruturapropriaarquivo)
		{
			if (estruturapropriaarquivo == null)
			{
				return 0;
			}

			EstruturaPropriaPliEntity objEstruturaPropria = new EstruturaPropriaPliEntity();

			var horadata = DateTime.Now;
			objEstruturaPropria.DataEnvio = horadata.AddHours(-1);
			objEstruturaPropria.StatusProcessamentoArquivo = (int)EnumEstruturaPropriaArquivoProcessamento.ENVIADO_A_SUFRAMA;
			objEstruturaPropria.QuantidadePLIArquivo = quantidadeProduto;
			objEstruturaPropria.VersaoEstrutura = "001";
			objEstruturaPropria.LoginUsuarioEnvio = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoCpfCnpj.Replace(".", "").Replace("-", "").Replace("/", "");
			objEstruturaPropria.NomeUsuarioEnvio = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoNome.Replace(".", "").Replace("-", "").Replace("/", "");
			objEstruturaPropria.CNPJImportador = cnpjImportador;
			objEstruturaPropria.RazaoSocialImportador = razaoSocial;
			objEstruturaPropria.InscricaoCadastral = Convert.ToInt32(estruturapropriaarquivo.NomeArquivo.Substring(0, 9));
			objEstruturaPropria.NomeArquivoEnvio = estruturapropriaarquivo.NomeArquivo;

			var lastProtocol = _uowSciex.QueryStackSciex.EstruturaPropriaPLI.Listar().Last();

			if (lastProtocol != null)
			{
				if (DateTime.Now.Year > lastProtocol.DataEnvio.Year)
					objEstruturaPropria.NumeroProtocolo = 1;
				else if (DateTime.Now.Year == lastProtocol.DataEnvio.Year)
				{
					if (lastProtocol.NumeroProtocolo == null)
						objEstruturaPropria.NumeroProtocolo = 1;
					else
						objEstruturaPropria.NumeroProtocolo = lastProtocol.NumeroProtocolo + 1;
				}
			}
			else
				objEstruturaPropria.NumeroProtocolo = 1;

			objEstruturaPropria.TipoArquivo = 3;

			objEstruturaPropria.EstruturaPropriaPliArquivo = new EstruturaPropriaPliArquivoEntity();
			objEstruturaPropria.EstruturaPropriaPliArquivo.Arquivo = estruturapropriaarquivo.Arquivo;

			_uowSciex.CommandStackSciex.EstruturaPropriaPli.Salvar(objEstruturaPropria);

			SolicitacaoPEArquivoEntity arquivo = new SolicitacaoPEArquivoEntity();
			arquivo.NomeArquivo = estruturapropriaarquivo.NomeAnexo;
			arquivo.Arquivo = estruturapropriaarquivo.Anexo;
			arquivo.EspId = objEstruturaPropria.IdEstruturaPropria;
			_uowSciex.CommandStackSciex.SolicitacaoPEArquivo.Salvar(arquivo);

			_uowSciex.CommandStackSciex.Save();

			return objEstruturaPropria.NumeroProtocolo.Value;
		}

		public EstruturaPropriaPLIArquivoVM Selecionar(int? idEstruturaPropriaPli)
		{
			var estruturaPropriaPLIVM = new EstruturaPropriaPLIArquivoVM();
			if (!idEstruturaPropriaPli.HasValue)
			{
				return estruturaPropriaPLIVM;
			}

			var estruturapropria = _uowSciex.QueryStackSciex.EstruturaPropriaPLIArquivo.Selecionar(x => x.IdEstruturaPropria == idEstruturaPropriaPli);
			var estruturapropriaVM = AutoMapper.Mapper.Map<EstruturaPropriaPLIArquivoVM>(estruturapropria);

			return estruturapropriaVM;
		}

		public void Deletar(int id)
		{
			var estruturapropria = _uowSciex.QueryStackSciex.EstruturaPropriaPLIArquivo.Selecionar(s => s.IdEstruturaPropria == id);
			if (estruturapropria != null)
			{
				_uowSciex.CommandStackSciex.EstruturaPropriaPliArquivo.Apagar(estruturapropria.IdEstruturaPropria);
			}
			_uowSciex.CommandStackSciex.Save();
		}

		public bool ValidarTipoInsumoArquivo(string[] arquivoLinhas)
		{
			try
			{
				foreach (var item in arquivoLinhas)
				{
					if (item.Length >= 1)
					{
						if (item.Substring(30, 1) != "P" && item.Substring(30, 1) != "E" && item.Substring(30, 1) != "N" && item.Substring(30, 1) != "R")
						{
							return false;
						}
					}
				}
			}
			catch
			{
				return false;
			}

			return true;
		}

		public bool ValidarModalidadeTipoLote(string[] arquivoLinhas)
		{
			try
			{
				foreach (var item in arquivoLinhas)
				{
					if (item.Length >= 1)
					{
						if(item.Substring(42, 1) != "S" && (item.Substring(43, 2) != "AP" || item.Substring(43, 2) != "CO"))
						{
							return false;
						}
					}
				}
			}
			catch
			{
				return false;
			}

			return true;
		}

		public string VerificaTipoExportaca(string[] linha)
		{
			foreach (var item in linha)
			{
				if (item.Length >= 1)
				{
					if (item.Substring(43, 2) == "AP")
					{
						return "AP";
					}else if (item.Substring(43, 2) == "CO")
					{
						return "CO";
					}
				}
			}
			return null;
		}

		private bool ValidarHorizontalProduto(string[] linhas)
		{
			foreach (var item in linhas)
			{
				if (item.Substring(0, 9) == "")
				{
					return false;
				}

				if (item.Substring(9, 10) == "")
				{
					return false;
				}

				try
				{
					Convert.ToInt32(item.Substring(19, 4));
				}
				catch
				{
					return false;
				}

				if (item.Substring(23, 8) == "")
				{
					return false;
				}

				if (item.Substring(33, 4) == "")
				{
					return false;
				}

				if (item.Substring(37, 3) == "")
				{
					return false;
				}

				if (item.Substring(40, 255) == "")
				{
					return false;
				}

				try
				{
					Convert.ToInt32(item.Substring(295, 14));
				}
				catch
				{
					return false;
				}

				try
				{
					Convert.ToInt32(item.Substring(309, 15));
				}
				catch
				{
					return false;
				}

				this.quantidadeProduto++;
			}

			return true;
		}

		private bool ValidarHorizontalProdutoPais(string[] linhas)
		{
			foreach (var item in linhas)
			{
				if (item.Substring(0, 9) == "")
				{
					return false;
				}

				if (item.Substring(9, 10) == "")
				{
					return false;
				}

				try
				{
					Convert.ToInt32(item.Substring(19, 4));
				}
				catch
				{
					return false;
				}

				if (item.Substring(23, 3) == "")
				{
					return false;
				}

				try
				{
					Convert.ToInt32(item.Substring(26, 14));
				}
				catch
				{
					return false;
				}

				try
				{
					Convert.ToInt32(item.Substring(40, 15));
				}
				catch
				{
					return false;
				}
			}

			return true;
		}

		private bool ValidarHorizontalInsumo(string[] linhas)
		{
			foreach (var item in linhas)
			{
				if (item.Substring(0, 9) == "")
				{
					return false;
				}

				if (item.Substring(9, 10) == "")
				{
					return false;
				}

				try
				{
					Convert.ToInt32(item.Substring(19, 4));
				}
				catch
				{
					return false;
				}

				try
				{
					Convert.ToInt32(item.Substring(23, 5));
				}
				catch
				{
					return false;
				}

				if (item.Substring(28, 2) == "")
				{
					return false;
				}

				if (item.Substring(30, 1) == "")
				{
					return false;
				}

				try
				{
					Convert.ToInt64(item.Substring(43, 15));
				}
				catch
				{
					return false;
				}

				if (item.Substring(88, 254) == "")
				{
					return false;
				}

				if (item.Substring(342, 3723) == "")
				{
					return false;
				}
			}

			return true;
		}

		private bool ValidarHorizontalDetalheInsumo(string[] linhas)
		{
			foreach (var item in linhas)
			{
				if (item.Substring(0, 9) == "")
				{
					return false;
				}

				if (item.Substring(9, 10) == "")
				{
					return false;
				}

				try
				{
					Convert.ToInt64(item.Substring(19, 4));
				}
				catch
				{
					return false;
				}

				try
				{
					Convert.ToInt64(item.Substring(23, 5));
				}
				catch
				{
					return false;
				}

				try
				{
					Convert.ToInt64(item.Substring(28, 2));
				}
				catch
				{
					return false;
				}

				try
				{
					Convert.ToInt64(item.Substring(36, 14));
				}
				catch
				{
					return false;
				}

				try
				{
					Convert.ToInt64(item.Substring(50, 18));
				}
				catch
				{
					return false;
				}


			}

			return true;
		}
		private bool ValidarHorizontalReDue(string[] linhas)
		{
			foreach (var item in linhas)
			{

				if(item.ToString().Length < 91) { return false; }
				if (item.Substring(0, 9) == "")
				{
					return false;
				}

				if (item.Substring(9, 10) == "")
				{
					return false;
				}

				try
				{
					Convert.ToInt64(item.Substring(19, 4));
				}
				catch
				{
					return false;
				}

				try
				{
					Convert.ToInt64(item.Substring(23, 3));
				}
				catch
				{
					return false;
				}

				if (item.Substring(26, 15) == "")
				{
					return false;
				}

				if (item.Substring(41, 10) == "")
				{
					return false;
				}

				try
				{
					Convert.ToInt64(item.Substring(51, 20));
				}
				catch
				{
					return false;
				}
				if (item.Substring(71, 20) == "")
				{
					return false;
				}
			}

			return true;
		}

		public bool ValidarDataProdutoPais(string[] linhas)
		{
			foreach (var item in linhas)
			{
				if (item.Substring(70, 10).Trim() != "")
				{
					try
					{
						string[] dados = item.Substring(70, 10).Split('_');

						int dia = int.Parse(dados[2].Substring(0, 2));
						int mes = DateTimeExtensions.RetornarNumeroMes(dados[2].Substring(2, 3));
						int ano = int.Parse(dados[2].Substring(5, 4));

						int hora = int.Parse(dados[3].Substring(0, 2));
						int min = int.Parse(dados[3].Substring(2, 2));
						int seg = int.Parse(dados[3].Substring(4, 2));


						var d = new DateTime(ano, mes, dia, hora, min, seg);
					}
					catch
					{
						return false;
					}
				}
			}

			return true;
		}
		public bool ValidarDataReDue(string[] linhas)
		{
			foreach (var item in linhas)
			{
				if (item.Substring(41, 10).Trim() != "")
				{
					try
					{
						string[] dados = item.Substring(41, 10).Split('_');

						var dia = int.Parse(dados[0].Substring(0, 2));
						var mes = int.Parse(dados[0].Substring(3, 2));
						var ano = int.Parse(dados[0].Substring(6, 4));

						var d = new DateTime(ano, mes, dia);
					}
					catch
					{
						return false;
					}
				}
			}

			return true;
		}

		public bool ValidarEmpresasRepresentadas(string cnpjEmpresa)
		{
			if (_usuarioPssBll.ListaEmpresaRepresentadas() != null &&
				_usuarioPssBll.ListaEmpresaRepresentadas().Any())
			{
				if (_usuarioPssBll.ListaEmpresaRepresentadas().Where(o => o.Cnpj.Replace(".", "").Replace("-", "").Replace("/", "") == cnpjEmpresa).Any())
				{
					return true;
				}
			}

			return false;
		}

		public bool ValidarEmpresaRepresentadaLogada(string cnpjEmpresa)
		{
			if (_usuarioInformacoesBll.ObterCNPJ().CnpjCpfUnformat() == cnpjEmpresa)
			{
				return true;
			}
			return false;
		}

		public bool ValidarFormatoNumeroArquivos(string[] arquivoLinhas)
		{

			try
			{
				foreach (var item in arquivoLinhas)
				{
					if (item.Length > 1)
					{
						if (item.Substring(9, 10).Length > 0)
						{
							if (item.Substring(9, 10).Contains("/"))
							{
								string[] valores = item.Substring(9, 10).Split('/');

								var ano = Convert.ToInt32(valores[0]);
								var numero = Convert.ToInt64(valores[1]);

								return true;
							}
							else
							{
								return false;
							}
						}
					}
				}
			}
			catch
			{
				return false;
			}

			return true;

		}

		public bool ValidarInscricaoArquivos(int inscricaoCadastralVW, string[] arquivoLinhas)
		{

			try
			{
				foreach (var item in arquivoLinhas)
				{
					if (item.Length > 1)
					{
						if (item.Substring(0, 9) != inscricaoCadastralVW.ToString())
						{
							return false;
						}
					}
				}
			}
			catch
			{
				return false;
			}

			return true;

		}

		public bool ValidarAnoArquivos(string[] arquivoLinhas, string loteAno = null, string loteNumero = null)
		{
			try
			{
				foreach (var item in arquivoLinhas)
				{
					if (item.Length > 1)
					{
						if (item.Substring(9, 4) != DateTime.Now.Year.ToString())
						{
							return false;
						}

						if (loteAno != null && loteNumero != null)
						{
							if (item.Substring(9, 4) != loteAno)
							{
								return false;
							}
							if (item.Substring(14, 5) != loteNumero)
							{
								return false;
							} 
						}
					}
				}
			}
			catch
			{
				return false;
			}

			return true;
		}

		public bool ValidarExisteLoteAprovado(string[] arquivoLinhas, int inscricao)
		{
			var numeroLote = RecuperarNumeroLoteArquivoLote(arquivoLinhas);
			var anoLote = RecuperarAnoLoteArquivoLote(arquivoLinhas);
			var planoExiste = _uowSciex.QueryStackSciex.PlanoExportacao.Existe(x => x.NumeroPlano == numeroLote && x.AnoPlano == anoLote &&
				x.NumeroInscricaoCadastral == inscricao && x.Situacao != 5 && x.TipoExportacao == "AP");
			if (planoExiste)
			{

				return true;
			}
			return false;

		}

		public bool ValidarNumeroProcessoArquivoLote(string[] arquivoLinhas)
		{
			try
			{
				foreach (var item in arquivoLinhas)
				{
					if (item.Length >= 1)
					{
						if (item.Substring(43, 2) == "CO")
						{
							if (item.Substring(28, 9).ToString().IndexOf(' ') >= 0 || item.Substring(38, 4).ToString().IndexOf(' ') >=0)
							{
								return false;
							}
							else
							{
								return true;
							}
						}
						else if (item.Substring(43, 2) == "AP")
						{
							if (item.Substring(28, 9).Trim() == "" && item.Substring(38, 4).Trim() == "")
							{
								return true;
							}
							else
							{
								return false;
							}
						}
					}
				}
			}
			catch
			{
				return false;
			}

			return true;

		}

		public bool ValidarQtdValorDolarProduto(string[] arquivoLinhas)
		{
			try
			{
				foreach (var item in arquivoLinhas)
				{
					if (item.Length >= 1)
					{
						if (item.Substring(295, 14) != "" && item.Substring(309, 15) != "")
						{
							var a = Convert.ToInt64(item.Substring(295, 14));
							var b = Convert.ToInt64(item.Substring(295, 14));

							if (a == 0 && b == 0)
							{
								return false;
							}
							else
							{
								return true;
							}
						}
					}
				}
			}
			catch
			{
				return false;
			}

			return true;
		}

		public bool ValidarQtdVlrUnitarioDetalheInsumo(string[] arquivoLinhas)
		{
			try
			{
				foreach (var item in arquivoLinhas)
				{
					if (item.Length >= 1)
					{
						if (item.Substring(36, 14) != "" && item.Substring(50, 18) != "")
						{
							var a = Convert.ToInt64(item.Substring(36, 14));
							var b = Convert.ToInt64(item.Substring(50, 18));

							if (a == 0 && b == 0)
							{
								return false;
							}
							else
							{
								return true;
							}
						}
					}
				}
			}
			catch
			{
				return false;
			}

			return true;
		}

		public bool ValidarCodPaisCodMoedaDetalheInsumo(string tipoInsumo, string[] arquivoLinhas)
		{
			try
			{
				if (tipoInsumo == "P" || tipoInsumo == "E")
				{
					foreach (var item in arquivoLinhas)
					{
						if (item.Length >= 1)
						{
							if (item.Substring(30, 3) == "" && item.Substring(33, 3) == "")
							{
								return false;
							}
						}
					}
				}
			}
			catch
			{
				return false;
			}

			return true;
		}

		public bool ValidarQtdValorDolarProdutoPais(string[] arquivoLinhas)
		{
			try
			{
				foreach (var item in arquivoLinhas)
				{
					if (item.Length >= 1)
					{
						if (item.Substring(26, 14) != "" && item.Substring(40, 15) != "")
						{
							var a = Convert.ToInt64(item.Substring(26, 14));
							var b = Convert.ToInt64(item.Substring(40, 15));

							if (a == 0 && b == 0)
							{
								return false;
							}
						}
					}
				}
			}
			catch
			{
				return false;
			}

			return true;
		}

		public string ValidarArquivo(EstruturaPropriaPLIArquivoVM estrutura)
		{
			try
			{
				string[] dados = estrutura.NomeArquivo.Split('_');
				string CNPJArquivo = string.Empty;

				if (estrutura.NomeArquivo.Length != 34)
				{
					return "Nome do arquivo não está no padrão definido pela Suframa, não é possível realizar envio de Plano de Exportação.";
				}

				if (dados.Length != 4)
				{
					return "Nome do arquivo não está no padrão definido pela Suframa, não é possível realizar envio de Plano de Exportação.";
				}

				if (dados[1].ToUpper() != "OWN")
				{
					return "Nome do arquivo não está no padrão definido pela Suframa, não é possível realizar envio de Plano de Exportação.";
				}

				if (!dados[3].ToUpper().Trim().Substring(dados[3].IndexOf("."), dados[3].Trim().Length - dados[3].IndexOf(".")).Equals(".PEX"))
				{
					return "Nome do arquivo não está no padrão definido pela Suframa, não é possível realizar envio de Plano de Exportação.";
				}

				if (!dados[3].ToUpper().Contains(".PEX"))
				{
					return "Nome do arquivo não está no padrão definido pela Suframa, não é possível realizar envio de Plano de Exportação.";
				}


				ViewImportadorVM objVI = _viewImportadorBll.SelecionarInscricao(Convert.ToInt32(dados[0]));
				if (objVI != null)
				{
					CNPJArquivo = objVI.Cnpj;
				}

				if (_usuarioInformacoesBll.ObterCNPJ().CnpjCpfUnformat().Length == 14)
				{
					//validar empresa representada logada
					if (!ValidarEmpresaRepresentadaLogada(CNPJArquivo))
					{
						return "Este usuário (logado ou representado)não está associado a empresa especificada no nome do arquivo, não é possível realizar envio de Plano de Exportação.";
					}
				}

				if (_usuarioInformacoesBll.ObterCNPJ().CnpjCpfUnformat().Length == 11)
				{
					//validar se tem acesso a empresa representada			
					if (!ValidarEmpresasRepresentadas(CNPJArquivo))
					{
						return "Este usuário(logado ou representado)não está associado a empresa especificada no nome do arquivo, não é possível realizar envio de Plano de Exportação.";
					}

					if (_usuarioInformacoesBll.ObterCNPJ() != CNPJArquivo)
					{
						return Mensagens.ESTRUTURA_PROPRIA_EMPRESA_REPRESENTADA;
					}
				}

				try
				{
					if (objVI != null)
					{
						if (objVI.DescricaoSituacaoInscricao != "ATIVA")
						{
							return "Empresa bloqueada ou não cadastrada, não é possível realizar envio de Plano de Exportação. É necessário regularizar a situação cadastral junto à Suframa.";
						}
					}
					else
					{
						return "Empresa bloqueada ou não cadastrada, não é possível realizar envio de Plano de Exportação. É necessário regularizar a situação cadastral junto à Suframa.";
					}
				}
				catch
				{
					return "Nome do arquivo não está no padrão definido pela Suframa, não é possível realizar envio de Plano de Exportação.";
				}

				Compressor objComprimir = new Compressor();

				string local = estrutura.LocalPastaEstruturaArquivo + @"\" + estrutura.NomeArquivo.Substring(0, estrutura.NomeArquivo.IndexOf("."));

				if (!Directory.Exists(local))
				{
					Directory.CreateDirectory(local);
				}

				string[] arquivos = Directory.GetFiles(local);
				foreach (string item in arquivos)
				{
					File.Delete(item);
				}

				if (objComprimir.UnZIP(estrutura.Arquivo, local))
				{
					string nomeArquivo = estrutura.NomeArquivo.Substring(0, estrutura.NomeArquivo.IndexOf("."));
					string extensao = estrutura.NomeArquivo.Split('.')[1];
					arquivos = Directory.GetFiles(local);

					if (arquivos.Length == 0)
					{
						foreach (string item in arquivos)
						{
							File.Delete(item);
						}
						Directory.Delete(local);
						return "Não foi encontrado arquivo dentro do arquivo compactado (.PEX).";
					}
					else if (arquivos.Length < 5)
					{
						foreach (string item in arquivos)
						{
							File.Delete(item);
						}
						Directory.Delete(local);
						return "Arquivo compactado não contém os 5 itens obrigatórios. Não é possível realizar envio de Plano de Exportação.”";
					}
					else
					{
						var loteFile = arquivos.Where(o => o.Contains("PX_LOTE.TXT"));
						string[] linhasLote = File.ReadAllLines(loteFile.FirstOrDefault());
						var tipoExportacao = VerificaTipoExportaca(linhasLote);
						if (loteFile.FirstOrDefault() != null)
						{
							var filename = Path.GetFileName(loteFile.FirstOrDefault());

							string[] lines = File.ReadAllLines(loteFile.FirstOrDefault());

							if (lines.Length > 0 && lines[0].Length > 0)
							{
								if (lines[0].Substring(0, 2) == "0\0")
								{
									lines = File.ReadAllLines(filename, Encoding.Unicode);
									File.Delete(filename);
									File.WriteAllLines(filename, lines);
								}
								else
								{
									lines = File.ReadAllLines(loteFile.FirstOrDefault(), Encoding.UTF8);
								}

								if (!ValidarInscricaoArquivos(objVI.InscricaoCadastral, lines))
								{
									return "Será aceito somente Plano correspondente a empresa especificada no nome do arquivo. A empresa do arquivo de Lote está diferente, não é possível realizar envio de Plano de Exportação.";
								}

								if (!ValidarAnoArquivos(lines)) //VALIDACAO-LOTE
								{
									foreach (string item in arquivos)
									{
										File.Delete(item);
									}
									Directory.Delete(local);
									return "O ano do arquivo de Lote não corresponde ao ano corrente, não é possível realizar envio de Plano de Exportação.";
								}

								if (!ValidarFormatoNumeroArquivos(lines))
								{
									foreach (string item in arquivos)
									{
										File.Delete(item);
									}
									Directory.Delete(local);
									return "O formato do número de Plano dentro do arquivo do Lote é inválido. Utilizar o formato aaaa/nnnnn (numérico)";
								}

								if (!ValidarModalidadeTipoLote(lines))
								{
									foreach (string item in arquivos)
									{
										File.Delete(item);
									}
									Directory.Delete(local);
									return "Serão aceitos somente lote da modalidade SUSPENSÃO e tipo APROVAÇÃO. Foi identificado Lote diferente dos aceitáveis, não é possível realizar envio de Plano de Exportação.";
								}

								if (!ValidarNumeroProcessoArquivoLote(lines))
								{
									foreach (string item in arquivos)
									{
										File.Delete(item);
									}
									Directory.Delete(local);
									if(tipoExportacao == "CO")
									{
										return "O número ou ano do Processo não foi informado para o lote do tipo Comprovação.";
									}else if(tipoExportacao == "AP")
									{
										return "O número e ano do Processo não deverá ter informação para Lote do tipo Aprovação. Foi identificado informação no número ou ano do processo do arquivo de Lote.";
									}
									
								}

								if (ValidarExisteLoteAprovado(lines, (Convert.ToInt32(dados[0]))))
								{
									return "O número do Plano dentro do arquivo do Lote já existe na base de dados da SUFRAMA. Não é possível realizar envio de Plano de Exportação.";

								}

							}
							else
							{
								return "Arquivo de Lote não encontrado ou vazio, não é possível realizar envio de Plano de Exportação.";
							}
						}
						else
						{
							return "Arquivo de Lote não encontrado ou vazio, não é possível realizar envio de Plano de Exportação.";
						}

						var loteAno = File.ReadAllLines(loteFile.FirstOrDefault())[0].Substring(9, 4);
						var loteNumero = File.ReadAllLines(loteFile.FirstOrDefault())[0].Substring(14, 5);

						List<string> listaCodigoProdutoPexpam = new List<string>();

						var produtoFile = arquivos.Where(o => o.Contains("PX_PRODUTO.TXT"));
						if (produtoFile.FirstOrDefault() != null)
						{
							string[] listaProdutos = File.ReadAllLines(produtoFile.FirstOrDefault());
							foreach (var linhaProduto in listaProdutos)
							{
								listaCodigoProdutoPexpam.Add(linhaProduto.Substring(19, 4));
							}

							var filename = Path.GetFileName(produtoFile.FirstOrDefault());

							string[] lines = File.ReadAllLines(produtoFile.FirstOrDefault());

							if (lines.Length > 0 && lines[0].Length > 0)
							{
								if (lines[0].Substring(0, 2) == "0\0")
								{
									lines = File.ReadAllLines(filename, Encoding.Unicode);
									File.Delete(filename);
									File.WriteAllLines(filename, lines);
								}
								else
								{
									lines = File.ReadAllLines(produtoFile.FirstOrDefault(), Encoding.UTF8);
								}

								if (!ValidarInscricaoArquivos(objVI.InscricaoCadastral, lines))
								{
									return "Será aceito somente Plano correspondente a empresa especificada no nome do arquivo. A empresa do arquivo de País de Produto está diferente, não é possível realizar envio de Plano de Exportação.";
								}

								if (!ValidarAnoArquivos(lines, loteAno, loteNumero)) //VALIDACAO-PRODUTO
								{
									foreach (string item in arquivos)
									{
										File.Delete(item);
									}
									Directory.Delete(local);
									return "O ano do arquivo de Produto não corresponde ao ano corrente, ou o ano não corresponde ao arquivo de lote, ou o número do plano não corresponde ao número do lote. Não é possível realizar envio de Plano de Exportação.";
								}

								if (!ValidarFormatoNumeroArquivos(lines))
								{
									foreach (string item in arquivos)
									{
										File.Delete(item);
									}
									Directory.Delete(local);
									return "O formato do número do Plano dentro do arquivo de Produto é inválido. Utilizar o formato aaaa/nnnnn (numérico).";
								}

								if (!ValidarQtdValorDolarProduto(lines))
								{
									foreach (string item in arquivos)
									{
										File.Delete(item);
									}
									Directory.Delete(local);
									return "A Quantidade e o Valor Dólar devem ser superior a zero (0). Dentro do arquivo de Produto há quantidade ou valor dólar igual a zero (0).";
								}

								if (!ValidarHorizontalProduto(lines))
								{
									foreach (string item in arquivos)
									{
										File.Delete(item);
									}
									Directory.Delete(local);
									return "Arquivo de Produto com erro na estrutura, não é possível realizar envio de Plano de Exportação. É necessário seguir o padrão de Estrutura Própria definido pela Suframa.";
								}
							}
							else
							{
								return "Arquivo de Produto não encontrado ou vazio, não é possível realizar envio de Plano de Exportação.";
							}
						}
						else
						{
							return "Arquivo de Produto não encontrado ou vazio, não é possível realizar envio de Plano de Exportação.";
						}
						var prodPaisFile = arquivos.Where(o => o.Contains("PX_PRODPAIS.TXT"));
						if (prodPaisFile.FirstOrDefault() != null)
						{
							var filename = Path.GetFileName(prodPaisFile.FirstOrDefault());

							string[] lines = File.ReadAllLines(prodPaisFile.FirstOrDefault());

							if (lines.Length > 0 && lines[0].Length > 0)
							{
								if (lines[0].Substring(0, 2) == "0\0")
								{
									lines = File.ReadAllLines(filename, Encoding.Unicode);
									File.Delete(filename);
									File.WriteAllLines(filename, lines);
								}
								else
								{
									lines = File.ReadAllLines(prodPaisFile.FirstOrDefault(), Encoding.UTF8);
								}

								if (!ValidarCodigoPexpam(lines, listaCodigoProdutoPexpam))//VALICAO-COD_PEXPAM
								{
									foreach (string item in arquivos)
									{
										File.Delete(item);
									}
									Directory.Delete(local);
									return "O código do Plano do arquivo do País do Produto não corresponde ao arquivo do Produto. Não é possível realizar envio de Plano de Exportação.";
								}

								if (!ValidarInscricaoArquivos(objVI.InscricaoCadastral, lines))
								{
									return "Será aceito somente Plano correspondente a empresa especificada no nome do arquivo. A empresa do arquivo de País de Produto está diferente, não é possível realizar envio de Plano de Exportação.";
								}

								if (!ValidarAnoArquivos(lines, loteAno, loteNumero))//VALICAO-PRODPAIS
								{
									foreach (string item in arquivos)
									{
										File.Delete(item);
									}
									Directory.Delete(local);
									return "O ano do arquivo do País do Produto não corresponde ao ano corrente, ou o ano não corresponde ao arquivo de lote, ou o número do plano não corresponde ao número do lote. Não é possível realizar envio de Plano de Exportação.";
								}

								if (!ValidarFormatoNumeroArquivos(lines))
								{
									foreach (string item in arquivos)
									{
										File.Delete(item);
									}
									Directory.Delete(local);
									return "O formato do número do Plano dentro do arquivo de País do Produto é inválido. Utilizar o formato aaaa/nnnnn (numérico).";
								}

								if (!ValidarQtdValorDolarProdutoPais(lines))
								{
									foreach (string item in arquivos)
									{
										File.Delete(item);
									}
									Directory.Delete(local);
									return "A Quantidade e o Valor Dólar devem ser superior a zero (0). Dentro do arquivo de País do Produto há quantidade ou valor dólar igual a zero (0).";
								}

								if (!ValidarHorizontalProdutoPais(lines))
								{
									foreach (string item in arquivos)
									{
										File.Delete(item);
									}
									Directory.Delete(local);
									return "Arquivo de País do Produto com erro na estrutura, não é possível realizar envio de Plano de Exportação. É necessário seguir o padrão de Estrutura Própria definido pela Suframa.";
								}

								if (!ValidarDataProdutoPais(lines))
								{
									foreach (string item in arquivos)
									{
										File.Delete(item);
									}
									Directory.Delete(local);
									return "A data de embarque do arquivo de País do Produto é inválida, não é possível realizar envio de Plano de Exportação.";
								}
							}
							else
							{
								return "Arquivo de País do Produto não encontrado ou vazio, não é possível realizar envio de Plano de Exportação.";
							}
						}
						else
						{
							return "Arquivo de País do Produto não encontrado ou vazio, não é possível realizar envio de Plano de Exportação.";
						}

						var insumoFile = arquivos.Where(o => o.Contains("PX_INSUMO.TXT"));
						string tipoInsumo = string.Empty;
						if(tipoExportacao == "AP")
						{
							if (insumoFile.FirstOrDefault() != null)
							{
								var filename = Path.GetFileName(insumoFile.FirstOrDefault());

								string[] lines = File.ReadAllLines(insumoFile.FirstOrDefault());

								if (lines.Length > 0 && lines[0].Length > 0)
								{
									if (lines[0].Substring(0, 2) == "0\0")
									{
										lines = File.ReadAllLines(filename, Encoding.Unicode);
										File.Delete(filename);
										File.WriteAllLines(filename, lines);
									}
									else
									{
										lines = File.ReadAllLines(insumoFile.FirstOrDefault(), Encoding.Default);
									}

									if (!ValidarCodigoPexpam(lines, listaCodigoProdutoPexpam))//VALICAO-COD_PEXPAM
									{
										foreach (string item in arquivos)
										{
											File.Delete(item);
										}
										Directory.Delete(local);
										return "O código do Plano do arquivo do Insumo não corresponde ao arquivo do Produto. Não é possível realizar envio de Plano de Exportação.";
									}

									if (!ValidarInscricaoArquivos(objVI.InscricaoCadastral, lines))
									{
										return "Será aceito somente Plano correspondente a empresa especificada no nome do arquivo. A empresa do arquivo de “Insumo” está diferente, não é possível realizar envio de Plano de Exportação.";
									}

									if (!ValidarAnoArquivos(lines, loteAno, loteNumero))//VALIDACAO-INSUMO
									{
										foreach (string item in arquivos)
										{
											File.Delete(item);
										}
										Directory.Delete(local);
										return "O ano do arquivo de Insumo não corresponde ao ano corrente, ou o ano não corresponde ao arquivo de lote, ou o número do plano não corresponde ao número do lote. Não é possível realizar envio de Plano de Exportação.";
									}

									if (!ValidarFormatoNumeroArquivos(lines))
									{
										foreach (string item in arquivos)
										{
											File.Delete(item);
										}
										Directory.Delete(local);
										return "O formato do número do Plano dentro do arquivo de “Insumo” é inválido. Utilizar o formato aaaa/nnnnn (numérico).";
									}

									if (!ValidarTipoInsumoArquivo(lines))
									{
										foreach (string item in arquivos)
										{
											File.Delete(item);
										}
										Directory.Delete(local);
										return "Serão aceitos somente insumos dos tipos P,E,N,R. Foi identificado insumo diferente dos aceitáveis, não é possível realizar envio de Plano de Exportação.";
									}

									tipoInsumo = lines[0].Substring(30, 1);

									if (!ValidarHorizontalInsumo(lines))
									{
										foreach (string item in arquivos)
										{
											File.Delete(item);
										}
										Directory.Delete(local);
										return "Arquivo de “Insumo” com erro na estrutura, não é possível realizar envio de Plano de Exportação. É necessário seguir o padrão de Estrutura Própria definido pela Suframa.";
									}
								}
								else
								{
									return "Arquivo de Insumo não encontrado ou vazio, não é possível realizar envio de Plano de Exportação.";
								}
							}
							else
							{
								return "Arquivo de Insumo não encontrado ou vazio, não é possível realizar envio de Plano de Exportação.";
							}
						}

						if (tipoExportacao == "AP")
						{
							var detalheInsumoFile = arquivos.Where(o => o.Contains("PX_DETINSUMO.TXT"));
							if (detalheInsumoFile.FirstOrDefault() != null)
							{
								var filename = Path.GetFileName(detalheInsumoFile.FirstOrDefault());

								string[] lines = File.ReadAllLines(detalheInsumoFile.FirstOrDefault());

								if (lines.Length > 0 && lines[0].Length > 0)
								{
									if (lines[0].Substring(0, 2) == "0\0")
									{
										lines = File.ReadAllLines(filename, Encoding.Unicode);
										File.Delete(filename);
										File.WriteAllLines(filename, lines);
									}
									else
									{
										lines = File.ReadAllLines(detalheInsumoFile.FirstOrDefault(), Encoding.UTF8);
									}

									if (!ValidarCodigoPexpam(lines, listaCodigoProdutoPexpam))//VALICAO-COD_PEXPAM
									{
										foreach (string item in arquivos)
										{
											File.Delete(item);
										}
										Directory.Delete(local);
										return "O código do Plano do arquivo do Detalhe do Insumo não corresponde ao arquivo do Produto. Não é possível realizar envio de Plano de Exportação.";
									}

									if (!ValidarInscricaoArquivos(objVI.InscricaoCadastral, lines))
									{
										return "Será aceito somente Plano correspondente a empresa especificada no nome do arquivo. A empresa do arquivo de Detalhe do Insumo está diferente, não é possível realizar envio de Plano de Exportação.";
									}

									if (!ValidarAnoArquivos(lines, loteAno, loteNumero))//VALIDACAO-DETINSUMO
									{
										foreach (string item in arquivos)
										{
											File.Delete(item);
										}
										Directory.Delete(local);
										return "O ano do arquivo de Detalhe do Insumo não corresponde ao ano corrente, ou o ano não corresponde ao arquivo de lote, ou o número do plano não corresponde ao número do lote. Não é possível realizar envio de Plano de Exportação.";
									}

									if (!ValidarFormatoNumeroArquivos(lines))
									{
										foreach (string item in arquivos)
										{
											File.Delete(item);
										}
										Directory.Delete(local);
										return "O formato do número do Plano dentro do arquivo de Detalhe do Insumo é inválido. Utilizar o formato aaaa/nnnnn (numérico).";
									}

									if (!ValidarCodPaisCodMoedaDetalheInsumo(tipoInsumo, lines))
									{
										foreach (string item in arquivos)
										{
											File.Delete(item);
										}
										Directory.Delete(local);
										return "Para insumos importados o país/moeda é obrigatório. Há Detalhe do Insumo sem informação de país/moeda.";
									}

									if (!ValidarQtdVlrUnitarioDetalheInsumo(lines))
									{
										foreach (string item in arquivos)
										{
											File.Delete(item);
										}
										Directory.Delete(local);
										return "A Quantidade e o Valor Unitário devem ser superior a zero (0). Dentro do arquivo de Detalhe do Insumo há quantidade ou valor unitário igual a zero (0).";
									}

									if (!ValidarHorizontalDetalheInsumo(lines))
									{
										foreach (string item in arquivos)
										{
											File.Delete(item);
										}
										Directory.Delete(local);
										return "Arquivo de Detalhe do Insumo com erro na estrutura, não é possível realizar envio de Plano de Exportação. É necessário seguir o padrão de Estrutura Própria definido pela Suframa.";
									}
								}
								else
								{
									return "Arquivo de Detalhe do Insumo não encontrado ou vazio, não é possível realizar envio de Plano de Exportação.";
								}
							}
							else
							{
								return "Arquivo de Detalhe do Insumo não encontrado ou vazio, não é possível realizar envio de Plano de Exportação.";
							}
						}
						if(tipoExportacao == "CO")
						{
							var reDueFile = arquivos.Where(o => o.Contains("PX_RE.TXT"));

							if (reDueFile.FirstOrDefault() != null)
							{
								var filename = Path.GetFileName(reDueFile.FirstOrDefault());

								string[] lines = File.ReadAllLines(reDueFile.FirstOrDefault());

								if (lines.Length > 0 && lines[0].Length > 0)
								{
									if (lines[0].Substring(0, 2) == "0\0")
									{
										lines = File.ReadAllLines(filename, Encoding.Unicode);
										File.Delete(filename);
										File.WriteAllLines(filename, lines);
									}
									else
									{
										lines = File.ReadAllLines(reDueFile.FirstOrDefault(), Encoding.UTF8);
									}
									if (!ValidarInscricaoArquivos(objVI.InscricaoCadastral, lines))
									{
										return "Será aceito somente Plano correspondente a empresa especificada no nome do arquivo. A empresa do arquivo de RE/DUE está diferente, não é possível realizar envio de Plano de Exportação.";
									}

									if (!ValidarAnoArquivos(lines, loteAno, loteNumero))
									{
										foreach (string item in arquivos)
										{
											File.Delete(item);
										}
										Directory.Delete(local);
										return "O ano do arquivo de RE/DUE não corresponde ao ano corrente, ou o ano não corresponde ao arquivo de lote, ou o número do plano não corresponde ao número do lote. Não é possível realizar envio de Plano de Exportação.";
									}

									if (!ValidarFormatoNumeroArquivos(lines))
									{
										foreach (string item in arquivos)
										{
											File.Delete(item);
										}
										Directory.Delete(local);
										return "O formato do número do Plano dentro do arquvio de RE/DUE é inválido. Utilizar o formato aaaa/nnnnn (numérico).";
									}
									if (!ValidarHorizontalReDue(lines))
									{
										foreach (string item in arquivos)
										{
											File.Delete(item);
										}
										Directory.Delete(local);
										return "Arquivo de RE/DUE com erro na estrutura, não é possível realizar envio de Plano de Exportação. É necessário seguir o padrão de Estrutura Própria definido pela Suframa.";
									}
									if (!ValidarDataReDue(lines))
									{
										foreach (string item in arquivos)
										{
											File.Delete(item);
										}
										Directory.Delete(local);
										return "A data de embarque do arquivo de RE/DUE é inválida, não é possível realizar envio de Plano de Exportação.";
									}
								}
								else
								{
									return "Arquivo de RED/DUE não encontrado ou vazio, não é possível realizar envio de Plano de Exportação.";
								}
							}
						}	
						cnpjImportador = CNPJArquivo;
						razaoSocial = objVI.RazaoSocial;

						string retorno = Salvar(estrutura).ToString();

						return retorno;
					}
				}
				else
				{
					if (objComprimir.MensagemErro.Contains("existe"))
					{
						foreach (string item in arquivos)
						{
							File.Delete(item);
						}
						Directory.Delete(local);
						return Mensagens.ESTRUTURA_PROPRIA_JA_ENVIADO;
					}
					else
					{
						foreach (string item in arquivos)
						{
							File.Delete(item);
						}
						Directory.Delete(local);
						return "Tipo de compactação não suportada, não é possível realizar envio de Plano de Exportação. É necessário enviar arquivo com compactação ZIP.";
					}
				}
			}
			catch (Exception ex)
			{
				return ex.Message.ToString();
			}
		}

		private bool ValidarCodigoPexpam(string[] lines, List<string> listaCodigoProdutoPexpam)
		{
			List<string> listaValidados = new List<string>();
			List<string> listaNaoValidados = new List<string>();

			foreach (var linhaListaPexpam in listaCodigoProdutoPexpam)
			{
				
				foreach (var item in lines)
				{
					var tt = item.Substring(19, 4);
					if ((item.Length > 1 && item.Substring(19, 4) == linhaListaPexpam) || listaValidados.Contains(item))
					{
						listaValidados.Add(item);
					}
					else
					{
						listaNaoValidados.Add(item);
					}
				}
			}
			var grupoValidados = listaValidados.Distinct().ToList();
			var grupoNaoValidados = listaNaoValidados.Distinct().ToList();

			var itensNaoValidados = grupoNaoValidados.Except(grupoValidados).ToList();

			if (itensNaoValidados.Count > 0)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		public void ProcessarPE()
		{
			var paridade = _uowSciex.QueryStackSciex.ParidadeCambial.Listar(o => o.DataParidade == DateTime.Today).OrderByDescending(o => o.IdParidadeCambial);

			if (paridade != null && paridade.Count() > 0)
			{
				var listaArquivosPE = _uowSciex.QueryStackSciex.EstruturaPropriaPLI.Listar(x => x.TipoArquivo == 3 && x.StatusProcessamentoArquivo == 1);

				foreach (var plano in listaArquivosPE)
				{
					if (ValidarEmpresaAtiva(plano.InscricaoCadastral.Value))
					{

						var controle = RegistrarInicioControleLog(plano);
						try
						{
							ExtrairPE(plano);

						}
						catch (Exception e)
						{
							RegistrarFimControleLog(plano, controle, false, e.Message);
							throw;
						}

					}
					else
					{
						InformarPendenciaImportador(plano);
					}
				}

				ValidarLotePE();
			}
			else
			{
				throw new Exception("SEM SUCESSO (SEM PARIDADE CAMBIAL CADASTRADA)");
			}
		}

		private void ValidarLotePE()
		{
			_uowSciex.CommandStackSciex.DetachEntries();
			var lotes = _uowSciex.QueryStackSciex.SolicitacaoPELote.Listar(x => x.Situacao == 1);
			foreach (var lote in lotes)
			{
				PlanoExportacaoEntity plano = null;
				var controle = RegistrarInicioControleLog(lote.EstruturaPropria, 25, $"Tabela: SCIEX_SOLIC_PE_LOTE, Campo slo_id:  <{lote.Id}>");
				string mensagemErro = "";
				try
				{
					var erro1 = ValidarExistenciaLote(lote);	
					var erro2 = ValidarModalidade(lote);
					var erro3 = ValidarTipoLote(lote);
					var erro4 = ValidarAnoNumeroProcesso(lote);
					var erro5 = true;
					var erro6 = true;

					if (!erro4)
					{
						erro5 = ValidarExisteProcessoNaBase(lote);

						if ("CO".Equals(lote.TipoExportacao))
						{
							var naoExistePlanoNaBase = ValidarExisteProcessoParaPlanoExportacaoNaBase(lote);
							var dueNaoValidada = ValidarInfoDue(lote.produtos);

							if (!naoExistePlanoNaBase && !dueNaoValidada)
								erro6 = false;
							
						}
						
					}

					var erro7 = ValidarProdutoPE(lote.produtos);

					lote.Situacao = 2;
					_uowSciex.CommandStackSciex.SolicitacaoPELote.Salvar(lote);

					if (!erro1 && (erro2 || erro3 || erro4 || erro5 || erro6 || erro7))
					{

						plano = GerarPlanoExportacao(lote, false);
					}
					else
					{
						if (!erro1)
						{
							plano = GerarPlanoExportacao(lote, true);
						}
						else
						{
							var produtosComFalha = lote.produtos.Count(x => x.SituacaoValidacao == 3);
							var produtosComSucesso = lote.produtos.Count(x => x.SituacaoValidacao == 2);

							lote.EstruturaPropria.QuantidadePLIProcessadoSucesso = (short)produtosComSucesso;
							lote.EstruturaPropria.QuantidadePLIProcessadoFalha = (short)produtosComFalha;
							lote.Situacao = 2;
							lote.EstruturaPropria.DataFimProcessamento = GetDateTimeNowUtc();

							lote.EstruturaPropria.StatusProcessamentoArquivo = 4;
							_uowSciex.CommandStackSciex.EstruturaPropriaPli.Salvar(lote.EstruturaPropria);
							_uowSciex.CommandStackSciex.SolicitacaoPELote.Salvar(lote);
							_uowSciex.CommandStackSciex.Save();

							RegistrarFimControleLog(lote.EstruturaPropria, controle, false, 
																		$"Plano de Exportação já existente. Número lote: <{lote.NumeroLote}{lote.Ano}>");
						}
					}

					if (plano != null)
					{
						CalcularValoresImportados(plano.IdPlanoExportacao);
						CopiarArquivo(lote, plano.IdPlanoExportacao);
					}

				}
				catch (Exception e)
				{
					RegistrarFimControleLog(lote.EstruturaPropria, controle, null, e.Message);
					throw new Exception(e.Message);
				}

				if (plano != null)
					RegistrarFimControleLog(lote.EstruturaPropria, controle, $"Tabela: SCIEX_PLANO_EXPORTACAO, Campo pex_id:  <{plano.IdPlanoExportacao}>");

			}
		}

		private bool ValidarInfoDue(ICollection<SolicitacaoPEProdutoEntity> produtos)
		{
			var erros = false;
			foreach (var produto in produtos)
			{
				
				foreach (var produtoPais in produto.PaisProduto)
				{
					foreach (var due in produtoPais.ListaSolicPEDue)
					{

						//RN22 - 1
						if (produto.SolicitacaoPELote.NumeroLote != due.NumeroLote
							|| produto.SolicitacaoPELote.Ano != due.NumeroAnoLote
							|| produto.SolicitacaoPELote.InscricaoCadastral != due.InscricaoCadastral)
						{
							var entityMensagemErro = _uowSciex.QueryStackSciex.ErroMensagem.Selecionar(o => o.IdErroMensagem == 735);
							var erro = new ErroProcessamentoEntity();
							erro.DataProcessamento = GetDateTimeNowUtc();
							erro.CNPJImportador = produto.SolicitacaoPELote.NumeroCNPJ;
							erro.Descricao = entityMensagemErro.Descricao.Replace("[sdu_nu]", $"[{due.Numero}]"); ;
							erro.IdErroMensagem = entityMensagemErro.IdErroMensagem;
							erro.CodigoNivelErro = (byte)EnumNivelErroServicoPE.RE_DUE;
							erros = true;
							_uowSciex.CommandStackSciex.ErroProcessamento.Salvar(erro);
							_uowSciex.CommandStackSciex.Save();
						}

						//RN22 - 2
						if (produto.SolicitacaoPELote.NumeroLote != due.NumeroLote
							|| produto.SolicitacaoPELote.Ano != due.NumeroAnoLote
							|| produto.SolicitacaoPELote.InscricaoCadastral != due.InscricaoCadastral
							|| produto.CodigoProdutoPexPam != due.CodigoProdutoExportacao)
						{
							var entityMensagemErro = _uowSciex.QueryStackSciex.ErroMensagem.Selecionar(o => o.IdErroMensagem == 736);
							var erro = new ErroProcessamentoEntity();
							erro.DataProcessamento = GetDateTimeNowUtc();
							erro.CNPJImportador = produto.SolicitacaoPELote.NumeroCNPJ;
							erro.Descricao = entityMensagemErro.Descricao.Replace("[sdu_co_produto_exp]", $"[{produto.CodigoProdutoPexPam}]").
																		Replace("[sdu_nu]", $"[{due.Numero}]");

							erro.IdErroMensagem = entityMensagemErro.IdErroMensagem;
							erro.CodigoNivelErro = (byte)EnumNivelErroServicoPE.RE_DUE;
							erros = true;
							_uowSciex.CommandStackSciex.ErroProcessamento.Salvar(erro);
							_uowSciex.CommandStackSciex.Save();
						}

						//RN22 - 3
						var codigoPais = int.Parse(due.SolicitacaoPEProdutoPais.CodigoPais);
						if (produto.SolicitacaoPELote.NumeroLote != due.NumeroLote
							|| produto.SolicitacaoPELote.Ano != due.NumeroAnoLote
							|| produto.SolicitacaoPELote.InscricaoCadastral != due.InscricaoCadastral
							|| due.SolicitacaoPEProdutoPais.CodigoProdutoPexPam != due.CodigoProdutoExportacao
							|| codigoPais != due.CodigoPais
							)
						{
							var entityMensagemErro = _uowSciex.QueryStackSciex.ErroMensagem.Selecionar(o => o.IdErroMensagem == 737);
							var erro = new ErroProcessamentoEntity();
							erro.DataProcessamento = GetDateTimeNowUtc();
							erro.CNPJImportador = produto.SolicitacaoPELote.NumeroCNPJ;
							erro.Descricao = entityMensagemErro.Descricao.Replace("[sdi_co_pai]", $"[{due.CodigoPais}]").
																		Replace("[spi_co_produto_exp]", $"[{due.SolicitacaoPEProdutoPais.CodigoProdutoPexPam}]")
																		.Replace("[sdu_nu]", $"[{due.Numero}]");
							erro.IdErroMensagem = entityMensagemErro.IdErroMensagem;
							erro.CodigoNivelErro = (byte)EnumNivelErroServicoPE.RE_DUE;
							erros = true;
							_uowSciex.CommandStackSciex.ErroProcessamento.Salvar(erro);
							_uowSciex.CommandStackSciex.Save();
						}


						//RN22 - 4
						var numDue = due.Numero;
						var numLote = due.NumeroLote;
						var numAnoLote = due.NumeroAnoLote;
						var inscCad = due.InscricaoCadastral;
						var codProdExp = due.CodigoProdutoExportacao;
						var codPais = due.CodigoPais;

						var existeDueDuplicada = _uowSciex.QueryStackSciex.SolicitacaoPEDue.Contar(q=> q.Numero == numDue
																									&& q.NumeroLote == numLote
																									&& q.NumeroAnoLote == numAnoLote
																									&& q.InscricaoCadastral == inscCad
																									&& q.CodigoProdutoExportacao == codProdExp
																									&& q.CodigoPais == codPais);
						if (existeDueDuplicada > 1)
						{
							erros = true;
							RegistarMensagemErro(produto.SolicitacaoPELote, 738, (byte)EnumNivelErroServicoPE.RE_DUE, new string[] { "[sdu_nu]" },
										new string[] { $"[{due.Numero}]" }, produto.Id);
						}


						//RN22 - 5
						var existeDueComPaisDistinto = _uowSciex.QueryStackSciex.SolicitacaoPEDue.Contar(q => q.Numero == numDue
																									&& q.NumeroLote == numLote
																									&& q.NumeroAnoLote == numAnoLote
																									&& q.InscricaoCadastral == inscCad
																									&& q.CodigoProdutoExportacao == codProdExp
																									&& q.CodigoPais != codPais);

						if (existeDueComPaisDistinto >= 1)
						{
							erros = true;
							RegistarMensagemErro(produto.SolicitacaoPELote, 739, (byte)EnumNivelErroServicoPE.RE_DUE, new string[] { "[sdu_nu]" },
										new string[] { $"[{due.Numero}]" }, produto.Id);
						}

						//RN22 - 6
						var dataDue = due.DataAverbacao;
						var numProcesso = int.Parse(due.SolicitacaoPEProdutoPais.SolicitacaoPEProduto.SolicitacaoPELote.NumeroProcesso);
						var anoProcesso = due.SolicitacaoPEProdutoPais.SolicitacaoPEProduto.SolicitacaoPELote.AnoProcesso;

						var validarDueDataEmbarqueNoPrazoVigenciaProcesso = _uowSciex.QueryStackSciex.Processo.Existe(q =>
																										(
																										q.ListaStatus.Any(w => w.Data < dataDue)
																										&&
																										q.DataValidade > dataDue
																										)
																										&& q.NumeroProcesso == numProcesso
																										&& q.AnoProcesso == anoProcesso
																									);

						if (!validarDueDataEmbarqueNoPrazoVigenciaProcesso)
						{
							erros = true;
							RegistarMensagemErro(produto.SolicitacaoPELote, 740, (byte)EnumNivelErroServicoPE.RE_DUE, new string[] { "[sdu_nu]" },
										new string[] { $"[{due.Numero}]" }, produto.Id);
						}
					}
				}
				

				if (!erros)
				{
					produto.SituacaoValidacao = 2;
				}
				else
				{
					produto.SolicitacaoPELote.Situacao = 3;
					produto.SituacaoValidacao = 3;
				}
				_uowSciex.CommandStackSciex.SolicitacaoPEProduto.Salvar(produto);


			}
			return erros;
		}

		private bool ValidarAnoNumeroProcesso(SolicitacaoPELoteEntity lote)
		{
			var ano = lote.AnoProcesso == null ? 0 : lote.AnoProcesso.Value;

			bool anoValido = ano != 0 && (1970 < ano && ano < 3000);


			bool processoValido = !string.IsNullOrEmpty(lote.NumeroProcesso);

			if (!anoValido || !processoValido)
			{
				var entityMensagemErro = _uowSciex.QueryStackSciex.ErroMensagem.Selecionar(o => o.IdErroMensagem == 741);
				var erro = new ErroProcessamentoEntity();
				erro.DataProcessamento = GetDateTimeNowUtc();
				erro.CNPJImportador = lote.NumeroCNPJ;
				erro.Descricao = entityMensagemErro.Descricao;
				erro.IdErroMensagem = entityMensagemErro.IdErroMensagem;
				erro.CodigoNivelErro = (byte)EnumNivelErroServicoPE.RE_DUE;
				lote.Situacao = 3;
				_uowSciex.CommandStackSciex.ErroProcessamento.Salvar(erro);
				_uowSciex.CommandStackSciex.Save();
				return true;
			}

			return false;
		}

		private bool ValidarExisteProcessoNaBase(SolicitacaoPELoteEntity lote)
		{
			var ano = lote.AnoProcesso == null ? 0 : lote.AnoProcesso.Value;

			var inscCad = lote.InscricaoCadastral == null ? 0 : int.Parse(lote.InscricaoCadastral);
			var numProc = lote.NumeroProcesso == null ? 0 : int.Parse(lote.NumeroProcesso);

			var existeProcesso = _uowSciex.QueryStackSciex.Processo.Listar(q => q.InscricaoSuframa == inscCad 
																				&& q.AnoProcesso == ano 
																				&& q.NumeroProcesso == numProc).Any();

			if (!existeProcesso)
			{
				var entityMensagemErro = _uowSciex.QueryStackSciex.ErroMensagem.Selecionar(o => o.IdErroMensagem == 741);
				var erro = new ErroProcessamentoEntity();
				erro.DataProcessamento = GetDateTimeNowUtc();
				erro.CNPJImportador = lote.NumeroCNPJ;
				erro.Descricao = entityMensagemErro.Descricao;
				erro.IdErroMensagem = entityMensagemErro.IdErroMensagem;
				erro.CodigoNivelErro = (byte)EnumNivelErroServicoPE.ANO_NUM_PROCESSO_NAO_ENCONTRADO_NA_BASE;
				lote.Situacao = 3;
				_uowSciex.CommandStackSciex.ErroProcessamento.Salvar(erro);
				_uowSciex.CommandStackSciex.Save();
				return true;
			}

			return false;
		}

		private bool ValidarExisteProcessoParaPlanoExportacaoNaBase(SolicitacaoPELoteEntity lote)
		{
			var ano = lote.AnoProcesso == null ? 0 : lote.AnoProcesso.Value;
			var inscCad = lote.InscricaoCadastral == null ? 0 : int.Parse(lote.InscricaoCadastral);
			var numProc = lote.NumeroProcesso == null ? 0 : int.Parse(lote.NumeroProcesso);


			var existeNaBaseParaPlanoComprovacao = _uowSciex.QueryStackSciex.PlanoExportacao.Listar(q=> q.NumeroInscricaoCadastral == inscCad
																										&& q.NumeroAnoProcesso == ano
																										&& q.NumeroProcesso == numProc).Any();

			if (!existeNaBaseParaPlanoComprovacao)
			{
				var entityMensagemErro = _uowSciex.QueryStackSciex.ErroMensagem.Selecionar(o => o.IdErroMensagem == 741);
				var erro = new ErroProcessamentoEntity();
				erro.DataProcessamento = GetDateTimeNowUtc();
				erro.CNPJImportador = lote.NumeroCNPJ;
				erro.Descricao = entityMensagemErro.Descricao;
				erro.IdErroMensagem = entityMensagemErro.IdErroMensagem;
				erro.CodigoNivelErro = (byte)EnumNivelErroServicoPE.ANO_NUM_PROCESSO_JA_VINCULADO_NA_BASE;
				lote.Situacao = 3;
				_uowSciex.CommandStackSciex.ErroProcessamento.Salvar(erro);
				_uowSciex.CommandStackSciex.Save();
				return true;
			}

			return false;
		}

		private void CopiarArquivo(SolicitacaoPELoteEntity lote, int idPlanoExportacao)
		{
			_uowSciex.CommandStackSciex.DetachEntries();

			var regAnexoSolicitacaoPE = _uowSciex.QueryStackSciex.SolicitacaoPEArquivo.Listar(q => q.EspId == lote.EspId).LastOrDefault();

			var regPEArquivo = _uowSciex.QueryStackSciex.PEArquivo.Listar(q => q.IdPlanoExportacao == idPlanoExportacao).LastOrDefault();


			if (regPEArquivo == null)
			{

				if (regAnexoSolicitacaoPE != null)
				{
					regPEArquivo = new PEArquivoEntity()
					{
						IdPlanoExportacao = idPlanoExportacao,
						Anexo = regAnexoSolicitacaoPE.Arquivo,
						NomeArquivo = regAnexoSolicitacaoPE.NomeArquivo
					};
				}
			}
			else
			{
				if (regAnexoSolicitacaoPE != null)
				{
					regPEArquivo.NomeArquivo = regAnexoSolicitacaoPE.NomeArquivo;
					regPEArquivo.IdPlanoExportacao = idPlanoExportacao;
					regPEArquivo.Anexo = regAnexoSolicitacaoPE.Arquivo;
				}
			}

			_uowSciex.CommandStackSciex.PEArquivo.Salvar(regPEArquivo);
			_uowSciex.CommandStackSciex.Save();

			_uowSciex.CommandStackSciex.DetachEntries();
		}

		private void CalcularValoresImportados(int idPlanoExportacao)
		{
			var listaProdutos = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Listar(q => q.IdPlanoExportacao == idPlanoExportacao).ToList();

			if (listaProdutos.Count > 0)
			{
				decimal fatorConvEmDolar = 0;
				decimal somatorioValorDolar = 0;
				decimal somatorioValorDolarFOB = 0;

				try
				{
					foreach (var regProduto in listaProdutos)
					{
						_uowSciex.CommandStackSciex.DetachEntries();

						var listaInsumos = _uowSciex.QueryStackSciex.PEInsumo.Listar(q => q.IdPEProduto == regProduto.IdPEProduto).ToList();

						if (listaInsumos.Count > 0)
						{
							foreach (var regInsumo in listaInsumos)
							{
								_uowSciex.CommandStackSciex.DetachEntries();

								var listaDetalhesInsumo = _uowSciex.QueryStackSciex.PEDetalheInsumo.Listar(q => q.IdPEInsumo == regInsumo.IdPEInsumo).ToList();

								if (listaDetalhesInsumo.Count > 0)
								{
									foreach (var regDetalheInsumo in listaDetalhesInsumo)
									{
										_uowSciex.CommandStackSciex.DetachEntries();

										decimal valorInsDolar = 0;

										if (regDetalheInsumo.Moeda.CodigoMoeda != 220)
										{
											CalcularFatorConversao(regDetalheInsumo.IdMoeda, ref fatorConvEmDolar);

											valorInsDolar = regDetalheInsumo.ValorUnitario * regDetalheInsumo.Quantidade * fatorConvEmDolar;
										}
										else
										{
											valorInsDolar = regDetalheInsumo.ValorUnitario * regDetalheInsumo.Quantidade;
										}


										regDetalheInsumo.ValorDolar = valorInsDolar + (regDetalheInsumo.ValorFrete != null ? regDetalheInsumo.ValorFrete : 0);
										regDetalheInsumo.ValorDolarFOB = valorInsDolar;
										regDetalheInsumo.ValorDolarCRF = valorInsDolar + (regDetalheInsumo.ValorFrete != null ? regDetalheInsumo.ValorFrete : 0);

										_uowSciex.CommandStackSciex.PEDetalheInsumo.Salvar(regDetalheInsumo);
										_uowSciex.CommandStackSciex.Save();

										somatorioValorDolar += regDetalheInsumo.ValorDolar != null ? (decimal)regDetalheInsumo.ValorDolar : 0;
										somatorioValorDolarFOB += regDetalheInsumo.ValorDolarFOB != null ? (decimal)regDetalheInsumo.ValorDolarFOB : 0;

										_uowSciex.CommandStackSciex.DetachEntries();
									}

									var regInsumoAtualizado = _uowSciex.QueryStackSciex.PEInsumo.Selecionar(q => q.IdPEInsumo == regInsumo.IdPEInsumo);

									regInsumoAtualizado.ValorDolar = somatorioValorDolar;

									_uowSciex.CommandStackSciex.PEInsumo.Salvar(regInsumoAtualizado);
									_uowSciex.CommandStackSciex.Save();

									#region CalculoFluxoCaixa
									var valorTotInsFOB = somatorioValorDolarFOB;
									var valorProduto = regProduto.ValorDolar;

									var fluxoCaixa = (1 - valorTotInsFOB / valorProduto) * 100;

									var regProdutoAtualizado = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Selecionar(q => q.IdPEProduto == regProduto.IdPEProduto);
									regProdutoAtualizado.ValorFluxoCaixa = fluxoCaixa;
									#endregion


									_uowSciex.CommandStackSciex.PlanoExportacaoProduto.Salvar(regProdutoAtualizado);
									_uowSciex.CommandStackSciex.Save();

								}

								_uowSciex.CommandStackSciex.DetachEntries();
							}
						}

						_uowSciex.CommandStackSciex.DetachEntries();
					}
				}
				catch (Exception e)
				{
					throw new Exception("A soma dos valores resultou em um valor acima do permitido. Por favor, revise os valores informados");
				}
			}
		}

		private void CalcularFatorConversao(int? idMoeda, ref decimal fatorConvEmDolar)
		{
			decimal fatorMoedaEstrangeira = 0;
			decimal fatorMoedaDolar = 0;
			var dataHoje = DateTime.Now.Date;

			fatorMoedaEstrangeira = _uowSciex.QueryStackSciex.ParidadeValor.Selecionar(q => q.Moeda.IdMoeda == idMoeda && q.ParidadeCambial.DataParidade == dataHoje).Valor;

			int codigoDolar = 220;
			fatorMoedaDolar = _uowSciex.QueryStackSciex.ParidadeValor.Selecionar(q => q.Moeda.CodigoMoeda == codigoDolar && q.ParidadeCambial.DataParidade == dataHoje).Valor;

			fatorConvEmDolar = fatorMoedaEstrangeira / fatorMoedaDolar;
		}

		private void RegistarMensagemErro(SolicitacaoPELoteEntity lote, int idMensagem, int nivelErro, string[] campos, string[] valores, int idProduto = 0)
		{
			var entityMensagemErro = _uowSciex.QueryStackSciex.ErroMensagem.Selecionar(o => o.IdErroMensagem == idMensagem);
			var erro = new ErroProcessamentoEntity();
			erro.IdLote = lote.Id;
			erro.DataProcessamento = GetDateTimeNowUtc();
			erro.CNPJImportador = lote.NumeroCNPJ;
			var mensagem = entityMensagemErro.Descricao;
			int i = 0;
			foreach (var campo in campos)
			{
				mensagem = mensagem.Replace(campo, valores[i++]);
			}
			erro.Descricao = mensagem;
			erro.IdErroMensagem = entityMensagemErro.IdErroMensagem;
			erro.CodigoNivelErro = 2;
			if (nivelErro > 1)
			{
				erro.IdPliMercadoriaOuPliDetalheMercadoria = idProduto;
			}


			_uowSciex.CommandStackSciex.ErroProcessamento.Salvar(erro);
			_uowSciex.CommandStackSciex.Save();
		}

		private PlanoExportacaoEntity GerarPlanoExportacao(SolicitacaoPELoteEntity lote, bool sucesso)
		{
			PlanoExportacaoEntity plano = null;
			int insc = int.Parse(lote.InscricaoCadastral);
			plano = _uowSciex.QueryStackSciex.PlanoExportacao.Selecionar(x => x.NumeroPlano == lote.NumeroLote
																	&& x.AnoPlano == lote.Ano
																	&& x.NumeroInscricaoCadastral == insc
																	&& x.Situacao == 5);
			if (plano != null)
			{
				var produtosComFalha = lote.produtos.Count(x => x.SituacaoValidacao == 3);

				var produtosComSucesso = lote.produtos.Count(x => x.SituacaoValidacao == 2);

				if (sucesso)
				{
					plano.DataStatus = GetDateTimeNowUtc();

					if (plano.Situacao == (int)EnumSituacaoPlanoExportacao.INDEFERIDO && plano.CpfResponsavel != null)
					{
						plano.Situacao = (int)EnumSituacaoPlanoExportacao.AGUARDANDO_ANÁLISE;
					}
					else
					{
						plano.Situacao = (int)EnumSituacaoPlanoExportacao.ENTREGUE;
					}

					foreach (var item in lote.produtos)
					{
						item.SituacaoValidacao = 2;
					}
					plano.DataEnvio = GetDateTimeNowUtc();
					lote.EstruturaPropria.QuantidadePLIProcessadoSucesso = (short)produtosComSucesso;
					lote.EstruturaPropria.QuantidadePLIProcessadoFalha = (short)produtosComFalha;

				}
				else
				{
					plano.Situacao = 1;
					foreach (var item in lote.produtos)
					{
						item.SituacaoValidacao = 3;
					}
					lote.EstruturaPropria.QuantidadePLIProcessadoSucesso = (short)produtosComSucesso;
					lote.EstruturaPropria.QuantidadePLIProcessadoFalha = (short)produtosComFalha;
				}
				lote.Situacao = 2;
				lote.EstruturaPropria.DataFimProcessamento = GetDateTimeNowUtc();
				//
				lote.EstruturaPropria.StatusProcessamentoArquivo = 4;
				_uowSciex.CommandStackSciex.EstruturaPropriaPli.Salvar(lote.EstruturaPropria);
				_uowSciex.CommandStackSciex.SolicitacaoPELote.Salvar(lote);


				foreach (var produto in lote.produtos)
				{
					var CodigoProdutoSuframa = int.Parse(produto.CodigoProdutoSuframa);
					var CodigoTipoProduto = int.Parse(produto.CodigoTipoProduto);
					var CodigoUnidade = int.Parse(produto.CodigoUnidade);

					var prod = plano.ListaPEProdutos.Where
								(x =>
									x.IdPlanoExportacao == plano.IdPlanoExportacao &&
									x.CodigoProdutoSuframa == CodigoProdutoSuframa &&
									x.CodigoProdutoExportacao == produto.CodigoProdutoPexPam &&
									x.CodigoTipoProduto == CodigoTipoProduto &&
									x.DescricaoModelo == produto.DescricaoModelo &&
									x.CodigoUnidade == CodigoUnidade &&
									x.CodigoNCM == produto.CodigoNCM
								).FirstOrDefault();

					if (prod != null)
					{
						if (!"CO".Equals(lote.TipoExportacao))
						{

							var listaPEInsumosReprovados = prod.ListaPEInsumo.
													Where(q => q.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.REPROVADO).ToList();

							if (listaPEInsumosReprovados.Count > 0)
							{


								try
								{
									foreach (var insumoPEAtual in listaPEInsumosReprovados)
									{

										var insumoSolic = produto.Insumos.Where(q => q.CodigoInsumo == insumoPEAtual.CodigoInsumo
																					&&
																					q.CodigoProdutoPexPam == prod.CodigoProdutoExportacao)
																				?.FirstOrDefault() ?? null;

										if (insumoSolic != null)
										{

											insumoPEAtual.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.ALTERADO;
											_uowSciex.CommandStackSciex.PEInsumo.Salvar(insumoPEAtual);


											var novoInsumoPE = new PEInsumoEntity();
											novoInsumoPE.CodigoDetalhe = int.Parse(insumoSolic.CodigoDetalhe);
											novoInsumoPE.CodigoInsumo = insumoSolic.CodigoInsumo;
											novoInsumoPE.CodigoNcm = insumoSolic.CodigoNCM;
											novoInsumoPE.CodigoUnidade = int.Parse(insumoSolic.CodigoUnidade);
											novoInsumoPE.CodigoInsumo = insumoSolic.CodigoInsumo;
											novoInsumoPE.CodigoUnidade = int.Parse(insumoSolic.CodigoUnidade);
											novoInsumoPE.TipoInsumo = insumoSolic.CodigoTipoInsumo;
											novoInsumoPE.DescricaoEspecificacaoTecnica = insumoSolic.DescricaoEspTecnica;
											novoInsumoPE.DescricaoPartNumber = insumoSolic.DescricaoPartNumber;
											novoInsumoPE.DescricaoInsumo = insumoSolic.DescricaoInsumo;
											novoInsumoPE.ValorCoeficienteTecnico = insumoSolic.ValorCoeficienteTecnico.Value;
											novoInsumoPE.ValorPercentualPerda = insumoSolic.ValorPctPerda.Value;
											novoInsumoPE.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.CORRIGIDO;
											novoInsumoPE.IdPEProduto = prod.IdPEProduto;


											foreach (var detalhePEAtualVM in insumoPEAtual.ListaPEDetalheInsumo)
											{

												//var newDet = new PEDetalheInsumoEntity();
												//newDet.IdPEInsumo = novoInsumoPE.IdPEInsumo;
												//newDet.CodigoPais = detalhePEAtualVM.CodigoPais;
												//newDet.ValorFrete = detalhePEAtualVM.ValorFrete;
												//newDet.Quantidade = detalhePEAtualVM.Quantidade;
												//newDet.NumeroSequencial = detalhePEAtualVM.NumeroSequencial;
												//newDet.ValorUnitario = detalhePEAtualVM.ValorUnitario;
												//newDet.SituacaoAnalise = detalhePEAtualVM.SituacaoAnalise;
												//newDet.IdMoeda = detalhePEAtualVM.IdMoeda;

												//novoInsumoPE.ListaPEDetalheInsumo.Add(newDet);

												////_uowSciex.CommandStackSciex.PEDetalheInsumo.Salvar(newDet);
												////_uowSciex.CommandStackSciex.Save();
												////_uowSciex.CommandStackSciex.DetachEntries();

												if (detalhePEAtualVM.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.REPROVADO)
												{
													var detalheSolic = insumoSolic.Detalhes.Where(q => q.Sequencial == detalhePEAtualVM.NumeroSequencial
																								&&
																								q.CodigoInsumo == insumoPEAtual.CodigoInsumo
																								&&
																								q.CodigoProdutoPexPam == prod.CodigoProdutoExportacao)
																								?.FirstOrDefault() ?? null;

													if (detalheSolic != null)
													{

														detalhePEAtualVM.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.ALTERADO;
														_uowSciex.CommandStackSciex.PEDetalheInsumo.Salvar(detalhePEAtualVM);

														var newDetCorrigido = new PEDetalheInsumoEntity();
														newDetCorrigido.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.CORRIGIDO;
														newDetCorrigido.CodigoPais = int.Parse(detalheSolic.CodigoPais);
														newDetCorrigido.ValorFrete = detalheSolic.ValorFrete;
														newDetCorrigido.Quantidade = detalheSolic.Quantidade.Value;
														newDetCorrigido.NumeroSequencial = detalheSolic.Sequencial;
														newDetCorrigido.ValorUnitario = detalheSolic.ValorUnitario.Value;

														//TODO moeda pode ser nulo tem que mudar na base
														if (detalheSolic.CodigoMoeda != null)
														{
															var cod = int.Parse(detalheSolic.CodigoMoeda);
															newDetCorrigido.IdMoeda = _uowSciex.QueryStackSciex.Moeda.
																								Selecionar(x => x.CodigoMoeda == cod).IdMoeda;
														}


														novoInsumoPE.ListaPEDetalheInsumo.Add(newDetCorrigido);

													}
												}

											}

											prod.ListaPEInsumo.Add(novoInsumoPE);
											_uowSciex.CommandStackSciex.PlanoExportacaoProduto.Salvar(prod);

										}


									}

								}
								catch (Exception e)
								{
									throw new Exception(e.Message);
								}


							}
						}
						else
						{ 

							foreach (var prodPaisPE in produto.PaisProduto.ToList())
							{

								foreach (var solicPEDue in prodPaisPE.ListaSolicPEDue)
								{
									_uowSciex.CommandStackSciex.DetachEntries();

									var duePEAtual = _uowSciex.QueryStackSciex.PlanoExportacaoDue.Listar(w =>
																											w.Numero == solicPEDue.Numero
																											&&
																											w.CodigoPais == solicPEDue.CodigoPais
																											).FirstOrDefault();

									if (duePEAtual != null)
									{

										if (duePEAtual.SituacaoAnalise == (int)EnumSituacaoAnalisePlanoExportacao.REPROVADO)
										{
											duePEAtual.SituacaoAnalise = (int)EnumSituacaoAnalisePlanoExportacao.ALTERADO;
											_uowSciex.CommandStackSciex.PlanoExportacaoDue.Salvar(duePEAtual);


											var novaPEDue = new PlanoExportacaoDUEEntity()
											{
												IdPEProdutoPais = solicPEDue.IdProdutoPais,
												CodigoPais = (int)solicPEDue.CodigoPais,
												DataAverbacao = (DateTime)solicPEDue.DataAverbacao,
												Numero = solicPEDue.Numero,
												Quantidade = (decimal)solicPEDue.Quantidade,
												ValorDolar = (decimal)solicPEDue.ValorDolar,
												SituacaoAnalise = (int)EnumSituacaoAnalisePEDue.CORRIGIDO
											};

											duePEAtual.PEProdutoPais.ListaPEDue.Add(novaPEDue);

											_uowSciex.CommandStackSciex.PlanoExportacaoProdutoPais.Salvar(duePEAtual.PEProdutoPais);
										}

									}
									else
									{
										var novaPEDue = new PlanoExportacaoDUEEntity()
										{
											IdPEProdutoPais = solicPEDue.IdProdutoPais,
											CodigoPais = (int)solicPEDue.CodigoPais,
											DataAverbacao = (DateTime)solicPEDue.DataAverbacao,
											Numero = solicPEDue.Numero,
											Quantidade = (decimal)solicPEDue.Quantidade,
											ValorDolar = (decimal)solicPEDue.ValorDolar,
											SituacaoAnalise = (int)EnumSituacaoAnalisePEDue.NOVO
										};

										duePEAtual.PEProdutoPais.ListaPEDue.Add(novaPEDue);

										_uowSciex.CommandStackSciex.PlanoExportacaoProdutoPais.Salvar(duePEAtual.PEProdutoPais);
									}
								}
							}
						}

					}
				}

				_uowSciex.CommandStackSciex.PlanoExportacao.Salvar(plano);
				_uowSciex.CommandStackSciex.Save();

				var historico = new PEHistoricoEntity();
				historico.Data = GetDateTimeNowUtc();
				historico.CpfResponsavel = "25";
				historico.NomeResponsavel = "SERVIÇO: VALIDAR ESTRUTURA PRÓPRIA DE PLANO";
				historico.DescricaoObservacao = "CORREÇÃO DO PLANO DE EXPORTAÇÃO POR ESTRUTURA PRÓPRIA";
				_uowSciex.CommandStackSciex.PEHistorico.Salvar(historico);
				_uowSciex.CommandStackSciex.Save();

				_uowSciex.CommandStackSciex.PlanoExportacao.Salvar(plano);
				_uowSciex.CommandStackSciex.Save();

			}
			else
			{
				plano = new PlanoExportacaoEntity();
				plano.AnoPlano = lote.Ano.Value;
				plano.NumeroPlano = lote.NumeroLote.Value;
				plano.RazaoSocial = lote.RazaoSocial;
				plano.TipoExportacao = lote.TipoExportacao;
				plano.TipoModalidade = lote.TipoModalidade;
				plano.NumeroInscricaoCadastral = int.Parse(lote.InscricaoCadastral);
				plano.Cnpj = lote.NumeroCNPJ;
				plano.DataCadastro = GetDateTimeNowUtc();

				if ("CO".Equals(lote.TipoExportacao))
				{
					plano.NumeroProcesso = int.Parse(lote.NumeroProcesso);
					plano.NumeroAnoProcesso = lote.AnoProcesso;
				}

				var produtosComFalha = lote.produtos.Count(x => x.SituacaoValidacao == 3);
				var produtosComSucesso = lote.produtos.Count(x => x.SituacaoValidacao == 2);

				if (sucesso)
				{
					plano.DataStatus = GetDateTimeNowUtc();
					plano.Situacao = 2; //ENTREGUE
					plano.DataEnvio = GetDateTimeNowUtc();
					lote.EstruturaPropria.QuantidadePLIProcessadoSucesso = (short)produtosComSucesso;
					lote.EstruturaPropria.QuantidadePLIProcessadoFalha = (short)produtosComFalha;

				}
				else
				{ 
					plano.Situacao = 1; //EM ELABORAÇÃO
					lote.EstruturaPropria.QuantidadePLIProcessadoSucesso = (short)produtosComSucesso;
					lote.EstruturaPropria.QuantidadePLIProcessadoFalha = (short)produtosComFalha;
				}
				lote.Situacao = 2;
				lote.EstruturaPropria.DataFimProcessamento = GetDateTimeNowUtc();

				lote.EstruturaPropria.StatusProcessamentoArquivo = 4;
				_uowSciex.CommandStackSciex.EstruturaPropriaPli.Salvar(lote.EstruturaPropria);
				_uowSciex.CommandStackSciex.SolicitacaoPELote.Salvar(lote);

				foreach (var produto in lote.produtos)
				{
					var prod = new PEProdutoEntity();
					prod.CodigoNCM = produto.CodigoNCM;
					prod.CodigoProdutoExportacao = produto.CodigoProdutoPexPam;
					prod.CodigoProdutoSuframa = int.Parse(produto.CodigoProdutoSuframa);
					prod.CodigoTipoProduto = int.Parse(produto.CodigoTipoProduto);
					prod.CodigoUnidade = int.Parse(produto.CodigoUnidade);
					prod.DescricaoModelo = produto.DescricaoModelo;
					prod.Qtd = produto.Quantidade.Value;
					prod.ValorDolar = produto.ValorDolar.Value;
					prod.ValorFluxoCaixa = produto.ValorNacional.Value; // TODO CONFIRMAR

					foreach (var pais in produto.PaisProduto)
					{
						var paisEntity = new PEProdutoPaisEntity();
						paisEntity.CodigoPais = int.Parse(pais.CodigoPais);
						paisEntity.Quantidade = pais.Quantidade.Value;
						paisEntity.ValorDolar = pais.ValorDolar.Value;

						foreach (var due in pais.ListaSolicPEDue)
						{
							var dueEntity = new PlanoExportacaoDUEEntity();
							dueEntity.Numero = due.Numero;
							dueEntity.DataAverbacao = (DateTime)(due.DataAverbacao);
							dueEntity.ValorDolar = due.ValorDolar.Value;
							dueEntity.Quantidade = due.Quantidade.Value;
							dueEntity.CodigoPais = (int)due.CodigoPais;

							paisEntity.ListaPEDue.Add(dueEntity);
						}

						prod.ListaPEProdutoPais.Add(paisEntity);
					}

					foreach (var insumo in produto.Insumos)
					{
						var insumoEntity = new PEInsumoEntity();
						insumoEntity.CodigoDetalhe = int.Parse(insumo.CodigoDetalhe);
						insumoEntity.CodigoInsumo = insumo.CodigoInsumo;
						insumoEntity.CodigoNcm = insumo.CodigoNCM;
						insumoEntity.CodigoUnidade = int.Parse(insumo.CodigoUnidade);
						insumoEntity.CodigoInsumo = insumo.CodigoInsumo;
						insumoEntity.CodigoUnidade = int.Parse(insumo.CodigoUnidade);
						insumoEntity.TipoInsumo = insumo.CodigoTipoInsumo;
						insumoEntity.DescricaoEspecificacaoTecnica = insumo.DescricaoEspTecnica;
						insumoEntity.DescricaoPartNumber = insumo.DescricaoPartNumber;
						insumoEntity.DescricaoInsumo = insumo.DescricaoInsumo;
						insumoEntity.ValorCoeficienteTecnico = insumo.ValorCoeficienteTecnico.Value;
						insumoEntity.ValorPercentualPerda = insumo.ValorPctPerda.Value;

						foreach (var detalhe in insumo.Detalhes)
						{
							var det = new PEDetalheInsumoEntity();
							det.CodigoPais = int.Parse(detalhe.CodigoPais);
							det.ValorFrete = detalhe.ValorFrete;
							det.Quantidade = detalhe.Quantidade.Value;
							det.NumeroSequencial = detalhe.Sequencial;
							det.ValorUnitario = detalhe.ValorUnitario.Value;

							//TODO moeda pode ser nulo tem que mudar na base
							if (detalhe.CodigoMoeda != null)
							{
								var cod = int.Parse(detalhe.CodigoMoeda);
								det.IdMoeda = _uowSciex.QueryStackSciex.Moeda.Selecionar(x => x.CodigoMoeda == cod).IdMoeda;
							}

							insumoEntity.ListaPEDetalheInsumo.Add(det);
						}

						prod.ListaPEInsumo.Add(insumoEntity);


					}


					plano.ListaPEProdutos.Add(prod);
				}

				_uowSciex.CommandStackSciex.PlanoExportacao.Salvar(plano);
				_uowSciex.CommandStackSciex.Save();



				var historico = new PEHistoricoEntity();
				historico.Data = GetDateTimeNowUtc();
				historico.CpfResponsavel = "25";
				historico.NomeResponsavel = "SERVIÇO: VALIDAR ESTRUTURA PRÓPRIA DE PLANO";
				historico.DescricaoObservacao = "CADASTRO DO PLANO DE EXPORTAÇÃO POR ESTRUTURA PRÓPRIA";

				_uowSciex.CommandStackSciex.PEHistorico.Salvar(historico);
				_uowSciex.CommandStackSciex.Save();

				_uowSciex.CommandStackSciex.PlanoExportacao.Salvar(plano);
				_uowSciex.CommandStackSciex.Save();
			}



			return plano;
		}

		private bool ValidarPaisProdutoPE(ICollection<SolicitacaoPEProdutoPaisEntity> paises)
		{
			var erros = false;
			foreach (var pais in paises)
			{
				var produto = pais.SolicitacaoPEProduto;
				var existePais = _uowSciex.QueryStackSciex.ViewPais.Existe(x => x.CodigoPais == pais.CodigoPais);
				if (!existePais)
				{
					erros = true;
					RegistarMensagemErro(produto.SolicitacaoPELote, 710, 2, new string[] { "[prp_co_pais]" },
						new string[] { $"[{pais.CodigoPais}]" }, produto.Id);
				}

				if (pais.AnoLote != pais.SolicitacaoPEProduto.SolicitacaoPELote.Ano || pais.NumeroLote != pais.SolicitacaoPEProduto.SolicitacaoPELote.NumeroLote ||
					 pais.InscricaoCadastral != pais.SolicitacaoPEProduto.SolicitacaoPELote.InscricaoCadastral)
				{
					erros = true;
					RegistarMensagemErro(produto.SolicitacaoPELote, 711, 2, new string[] { "[prp_co_pais]" },
						new string[] { $"[{pais.CodigoPais}]" }, produto.Id);

				}

				if (pais.CodigoProdutoPexPam != pais.SolicitacaoPEProduto.CodigoProdutoPexPam ||
					 pais.InscricaoCadastral != pais.SolicitacaoPEProduto.InscricaoCadastral)
				{
					erros = true;
					RegistarMensagemErro(produto.SolicitacaoPELote, 712, 2, new string[] { "[prp_co_pais]", "[spp_co_produto_exp]" },
						new string[] { $"[{produto.CodigoProdutoSuframa}]", $"[{produto.CodigoProdutoPexPam}]" }, produto.Id);
				}

				var existePaisDuplicado = _uowSciex.QueryStackSciex.SolicitacaoPaisProduto.Contar(x => x.CodigoProdutoPexPam == pais.CodigoProdutoPexPam &&
				x.CodigoPais == pais.CodigoPais && x.InscricaoCadastral == pais.InscricaoCadastral && x.ProdutoId == pais.ProdutoId);
				if (existePaisDuplicado > 1)
				{
					erros = true;
					RegistarMensagemErro(produto.SolicitacaoPELote, 713, 2, new string[] { "[prp_co_pais]", "[spp_co_produto_exp]" },
						new string[] { $"[{produto.CodigoProdutoSuframa}]", $"[{produto.CodigoProdutoPexPam}]" }, produto.Id);
				}

				if (paises.Sum(x => x.Quantidade) != pais.SolicitacaoPEProduto.Quantidade)
				{
					erros = true;
					RegistarMensagemErro(produto.SolicitacaoPELote, 714, 2, new string[] { "[spp_co_produto_exp]" },
						new string[] { $"[{produto.CodigoProdutoPexPam}]" }, produto.Id);
				}

				if (paises.Sum(x => x.ValorDolar) != pais.SolicitacaoPEProduto.ValorDolar)
				{
					erros = true;
					RegistarMensagemErro(produto.SolicitacaoPELote, 715, 2, new string[] { "[spp_co_produto_exp]" },
						new string[] { $"[{produto.CodigoProdutoPexPam}]" }, produto.Id);
				}
			}

			return erros;
		}

		private bool ValidarDetalheInsumosProduto(ICollection<SolicitacaoPEDetalheEntity> detalhes)
		{
			var erros = false;

			foreach (var detalhe in detalhes)
			{
				var produto = detalhe.SolicitacaoPEInsumoEntity.SolicitacaoPEProdutoEntity;

				var lote = produto.SolicitacaoPELote;

				if (detalhe.AnoLote != lote.Ano || detalhe.NumeroLote != lote.NumeroLote ||
					 detalhe.InscricaoCadastral != lote.InscricaoCadastral)
				{
					erros = true;
					RegistarMensagemErro(produto.SolicitacaoPELote, 728, 2, new string[] { "[sdi_nu_seq]", "[sdi_co_insumo]" },
						new string[] { $"[{detalhe.Sequencial}]", $"[{detalhe.CodigoInsumo}]" }, produto.Id);
				}

				var paisExiste = _uowSciex.QueryStackSciex.ViewPais.Existe(x => x.CodigoPais == detalhe.CodigoPais);
				if (!paisExiste)
				{
					erros = true;
					RegistarMensagemErro(produto.SolicitacaoPELote, 731, 2, new string[] { "[sdi_co_pais]" },
						new string[] { $"[{detalhe.CodigoPais}]" }, produto.Id);
				}


				var codMoeda = short.Parse(detalhe.CodigoMoeda);
				var moedaExiste = _uowSciex.QueryStackSciex.Moeda.Existe(x => x.CodigoMoeda == codMoeda);
				if (!moedaExiste)
				{
					erros = true;
					RegistarMensagemErro(produto.SolicitacaoPELote, 732, 2, new string[] { "[sdi_co_moeda]" },
						new string[] { $"[{codMoeda}]" }, produto.Id);
				}

				var existeDetalheDuplicado = _uowSciex.QueryStackSciex.SolicitacaoPEDetalhe.Contar(x => x.CodigoProdutoPexPam == detalhe.CodigoProdutoPexPam &&
				x.Id == detalhe.Id && x.InscricaoCadastral == detalhe.InscricaoCadastral);
				if (existeDetalheDuplicado > 1)
				{
					erros = true;
					RegistarMensagemErro(produto.SolicitacaoPELote, 733, 2, new string[] { "[sdi_nu_seq]", "[sdi_co_insumo]", "[spp_co_produto_exp]" },
						new string[] { $"[{detalhe.Sequencial}]", $"[{detalhe.CodigoInsumo}]", $"[{produto.CodigoProdutoPexPam}]" }, produto.Id);
				}

				int insc = int.Parse(produto.InscricaoCadastral);
				var planoIndeferidoExiste = _uowSciex.QueryStackSciex.PlanoExportacao.Selecionar(x => x.NumeroPlano == produto.NumeroLote && x.AnoPlano == produto.AnoLote &&
				   x.NumeroInscricaoCadastral == insc && x.Situacao == 5);
				if (planoIndeferidoExiste != null)
				{
					var CodigoProdutoSuframa = int.Parse(produto.CodigoProdutoSuframa);
					var CodigoTipoProduto = int.Parse(produto.CodigoTipoProduto);
					var CodigoUnidade = int.Parse(produto.CodigoUnidade);
					var peProduto = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Selecionar
																(x =>
																	x.IdPlanoExportacao == planoIndeferidoExiste.IdPlanoExportacao &&
																	x.CodigoProdutoSuframa == CodigoProdutoSuframa &&
																	x.CodigoProdutoExportacao == produto.CodigoProdutoPexPam &&
																	x.CodigoTipoProduto == CodigoTipoProduto &&
																	x.DescricaoModelo == produto.DescricaoModelo &&
																	x.CodigoUnidade == CodigoUnidade &&
																	x.CodigoNCM == produto.CodigoNCM
																);
					if (peProduto != null)
					{
						var peInsumo = _uowSciex.QueryStackSciex.PEInsumo.Listar(x => x.CodigoInsumo == detalhe.SolicitacaoPEInsumoEntity.CodigoInsumo && x.IdPEProduto == peProduto.IdPEProduto);
						if (peInsumo.Count() > 0)
						{
							var idPEInsumo = peInsumo.OrderBy(x => x.IdPEInsumo).LastOrDefault().IdPEInsumo;

							var peDetalheInsumo = _uowSciex.QueryStackSciex.PEDetalheInsumo.Selecionar(x => x.NumeroSequencial == detalhe.Sequencial
																										&& x.IdPEInsumo == idPEInsumo);
							if (peDetalheInsumo == null)
							{
								erros = true;
								RegistarMensagemErro(produto.SolicitacaoPELote, 736, 2, new string[] { "[sdi_nu_seq]", "[sdi_co_insumo]", "[spp_co_produto_exp]" },
									new string[] { $"[{detalhe.Sequencial}]", $"[{detalhe.CodigoInsumo}]", $"[{produto.CodigoProdutoPexPam}]" }, produto.Id);
							}
						}
					}
				}
			}

			return erros;
		}

		private bool ValidarInsumosProduto(ICollection<SolicitacaoPEInsumoEntity> insumos)
		{
			var erros = false;
			foreach (var insumo in insumos)
			{
				var produto = insumo.SolicitacaoPEProdutoEntity;
				var lote = produto.SolicitacaoPELote;
				var inscSuf = int.Parse(insumo.InscricaoCadastral);
				var existeInsumo = _uowSciex.QueryStackSciex.LEInsumo.Existe(x => x.CodigoInsumo == insumo.CodigoInsumo 
																			&& x.CodigoProduto == insumo.CodigoProdutoPexPam &&
																		x.LEProduto.InscricaoCadastral == inscSuf && x.SituacaoInsumo == 1);
				if (!existeInsumo)
				{
					erros = true;
					RegistarMensagemErro(produto.SolicitacaoPELote, 716, 3, new string[] { "[spi_co_insumo]", "[spi_co_produto_exp]" },
						new string[] { $"[{insumo.CodigoInsumo}]", $"[{insumo.CodigoProdutoPexPam}]" }, produto.Id);
				}

				if (insumo.AnoLote != lote.Ano || insumo.NumeroLote != lote.NumeroLote ||
					 insumo.InscricaoCadastral != lote.InscricaoCadastral)
				{
					erros = true;
					RegistarMensagemErro(produto.SolicitacaoPELote, 717, 3, new string[] { "[spi_co_insumo]" },
						new string[] { $"[{insumo.CodigoInsumo}]" }, produto.Id);

				}

				if (insumo.AnoLote != produto.AnoLote || insumo.NumeroLote != produto.NumeroLote ||
					 insumo.InscricaoCadastral != produto.InscricaoCadastral)
				{
					erros = true;
					RegistarMensagemErro(produto.SolicitacaoPELote, 718, 3, new string[] { "[spi_co_insumo]", "[spi_co_produto_exp]" },
						new string[] { $"[{insumo.CodigoInsumo}]", $"[{insumo.CodigoProdutoPexPam}]" }, produto.Id);

				}
				var codUnidade = int.Parse(insumo.CodigoUnidade);
				var existeUnidadeInsumo = _uowSciex.QueryStackSciex.LEInsumo.Existe(x => x.CodigoUnidadeMedida == codUnidade && x.CodigoProduto == insumo.CodigoProdutoPexPam &&
				x.LEProduto.InscricaoCadastral == inscSuf && x.SituacaoInsumo == 1);

				if (!existeUnidadeInsumo)
				{
					erros = true;
					RegistarMensagemErro(produto.SolicitacaoPELote, 719, 3, new string[] { "[spi_co_unidade]", "[spi_co_produto_exp]" },
						new string[] { $"[{insumo.CodigoUnidade}]", $"[{insumo.CodigoProdutoPexPam}]" }, produto.Id);
				}

				var existeTipoInsumo = _uowSciex.QueryStackSciex.LEInsumo.Existe(x => x.TipoInsumo == insumo.CodigoTipoInsumo && x.CodigoProduto == insumo.CodigoProdutoPexPam &&
				x.LEProduto.InscricaoCadastral == inscSuf && x.SituacaoInsumo == 1);

				if (!existeTipoInsumo)
				{
					erros = true;
					RegistarMensagemErro(produto.SolicitacaoPELote, 720, 3, new string[] { "[spi_tp_insumo]", "[spi_co_produto_exp]" },
						new string[] { $"[{insumo.CodigoTipoInsumo}]", $"[{insumo.CodigoProdutoPexPam}]" }, produto.Id);
				}

				var existeNCMInsumo = _uowSciex.QueryStackSciex.LEInsumo.Existe(x => x.CodigoNCM == insumo.CodigoNCM && x.CodigoProduto == insumo.CodigoProdutoPexPam &&
				x.LEProduto.InscricaoCadastral == inscSuf && x.SituacaoInsumo == 1);

				if (!existeNCMInsumo)
				{
					erros = true;
					RegistarMensagemErro(produto.SolicitacaoPELote, 721, 3, new string[] { "[spi_co_ncm]", "[spi_co_produto_exp]" },
						new string[] { $"[{insumo.CodigoNCM}]", $"[{insumo.CodigoProdutoPexPam}]" }, produto.Id);
				}
				var codDetalhe = int.Parse(insumo.CodigoDetalhe);
				var existeCodDetalhensumo = _uowSciex.QueryStackSciex.LEInsumo.Existe(x => x.CodigoDetalhe == codDetalhe && x.CodigoProduto == insumo.CodigoProdutoPexPam &&
				x.LEProduto.InscricaoCadastral == inscSuf && x.SituacaoInsumo == 1);

				if (!existeCodDetalhensumo)
				{
					erros = true;
					RegistarMensagemErro(produto.SolicitacaoPELote, 722, 3, new string[] { "[spi_co_detalhe]", "[spi_co_produto_exp]" },
						new string[] { $"[{insumo.CodigoDetalhe}]", $"[{insumo.CodigoProdutoPexPam}]" }, produto.Id);
				}

				var existeDescricaoInsumo = _uowSciex.QueryStackSciex.LEInsumo.Existe(x => x.DescricaoInsumo == insumo.DescricaoInsumo
																				&& x.CodigoProduto == insumo.CodigoProdutoPexPam &&
																				x.LEProduto.InscricaoCadastral == inscSuf
																				&& x.SituacaoInsumo == 1);

				if (!existeDescricaoInsumo)
				{
					erros = true;
					RegistarMensagemErro(produto.SolicitacaoPELote, 723, 3, new string[] { "[spi_co_insumo]", "[spi_co_produto_exp]" },
						new string[] { $"[{insumo.CodigoInsumo}]", $"[{insumo.CodigoProdutoPexPam}]" }, produto.Id);
				}

				var existeDescricaoEspTecnicaInsumo = _uowSciex.QueryStackSciex.LEInsumo.Existe(x => x.DescricaoEspecTecnica == insumo.DescricaoEspTecnica
																								&& x.CodigoProduto == insumo.CodigoProdutoPexPam &&
																								x.LEProduto.InscricaoCadastral == inscSuf
																								&& x.SituacaoInsumo == 1);

				if (!existeDescricaoEspTecnicaInsumo)
				{
					erros = true;
					RegistarMensagemErro(produto.SolicitacaoPELote, 724, 3, new string[] { "[spi_co_insumo]", "[spi_co_produto_exp]" },
						new string[] { $"[{insumo.CodigoInsumo}]", $"[{insumo.CodigoProdutoPexPam}]" }, produto.Id);
				}

				var existeValorEspTecnicaInsumo = _uowSciex.QueryStackSciex.LEInsumo.Existe(x => x.ValorCoeficienteTecnico == insumo.ValorCoeficienteTecnico && x.CodigoProduto == insumo.CodigoProdutoPexPam &&
				x.LEProduto.InscricaoCadastral == inscSuf && x.SituacaoInsumo == 1);

				if (!existeValorEspTecnicaInsumo)
				{
					erros = true;
					RegistarMensagemErro(produto.SolicitacaoPELote, 725, 3, new string[] { "[spi_co_insumo]", "[spi_co_produto_exp]" },
						new string[] { $"[{insumo.CodigoInsumo}]", $"[{insumo.CodigoProdutoPexPam}]" }, produto.Id);
				}

				var existeBloqueioInsumo = _uowSciex.QueryStackSciex.LEInsumo.Existe(x => x.CodigoInsumo == insumo.CodigoInsumo && x.CodigoProduto == insumo.CodigoProdutoPexPam &&
				 x.LEProduto.InscricaoCadastral == inscSuf && x.LEProduto.StatusLE == 5);

				if (existeBloqueioInsumo)
				{
					erros = true;
					RegistarMensagemErro(produto.SolicitacaoPELote, 726, 3, new string[] { "[spi_co_insumo]", "[spi_co_produto_exp]" },
						new string[] { $"[{insumo.CodigoInsumo}]", $"[{insumo.CodigoProdutoPexPam}]" }, produto.Id);
				}

				var existeInsumoDuplicado = _uowSciex.QueryStackSciex.SolicitacaoPEInsumo.Contar(x => x.CodigoInsumo == insumo.CodigoInsumo && x.CodigoProdutoPexPam == insumo.CodigoProdutoPexPam &&
				 x.InscricaoCadastral == insumo.InscricaoCadastral && x.ProdutoId == insumo.ProdutoId);
				if (existeInsumoDuplicado > 1)
				{
					erros = true;
					RegistarMensagemErro(produto.SolicitacaoPELote, 727, 3, new string[] { "[spi_co_insumo]", "[spp_co_produto_exp]" },
						new string[] { $"[{insumo.CodigoInsumo}]", $"[{produto.CodigoProdutoPexPam}]" }, produto.Id);
				}

				int insc = int.Parse(produto.InscricaoCadastral);
				var planoIndeferidoExiste = _uowSciex.QueryStackSciex.PlanoExportacao.Selecionar(x => x.NumeroPlano == produto.NumeroLote && x.AnoPlano == produto.AnoLote &&
				   x.NumeroInscricaoCadastral == insc && x.Situacao == 5);
				if (planoIndeferidoExiste != null)
				{
					var CodigoProdutoSuframa = int.Parse(produto.CodigoProdutoSuframa);
					var CodigoTipoProduto = int.Parse(produto.CodigoTipoProduto);
					var CodigoUnidade = int.Parse(produto.CodigoUnidade);
					var peProduto = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Selecionar
																(x =>
																	x.IdPlanoExportacao == planoIndeferidoExiste.IdPlanoExportacao &&
																	x.CodigoProdutoSuframa == CodigoProdutoSuframa &&
																	x.CodigoProdutoExportacao == produto.CodigoProdutoPexPam &&
																	x.CodigoTipoProduto == CodigoTipoProduto &&
																	x.DescricaoModelo == produto.DescricaoModelo &&
																	x.CodigoUnidade == CodigoUnidade &&
																	x.CodigoNCM == produto.CodigoNCM
																);
					if (peProduto != null)
					{
						var peInsumo = _uowSciex.QueryStackSciex.PEInsumo.Listar(x => x.CodigoInsumo == insumo.CodigoInsumo && x.IdPEProduto == peProduto.IdPEProduto);
						if (peInsumo.Count() == 0)
						{
							erros = true;
							RegistarMensagemErro(produto.SolicitacaoPELote, 735, 3, new string[] { "[spi_co_insumo]", "[spp_co_produto_exp]" },
								new string[] { $"[{insumo.CodigoInsumo}]", $"[{produto.CodigoProdutoPexPam}]" }, produto.Id);
						}
					}
				}

				ValidarDetalheInsumosProduto(insumo.Detalhes);
			}
			return erros;
		}

		private bool ValidarProdutoPE(ICollection<SolicitacaoPEProdutoEntity> produtos)
		{
			var erros = false;
			foreach (var produto in produtos)
			{
				if (produto.AnoLote != produto.SolicitacaoPELote.Ano 
					|| produto.NumeroLote != produto.SolicitacaoPELote.NumeroLote 
					|| produto.InscricaoCadastral != produto.SolicitacaoPELote.InscricaoCadastral)
				{
					var entityMensagemErro = _uowSciex.QueryStackSciex.ErroMensagem.Selecionar(o => o.IdErroMensagem == 703);
					var erro = new ErroProcessamentoEntity();
					erro.DataProcessamento = GetDateTimeNowUtc();
					erro.CNPJImportador = produto.SolicitacaoPELote.NumeroCNPJ;
					erro.Descricao = entityMensagemErro.Descricao.Replace("[spp_co_produto_exp]", $"[{produto.CodigoProdutoPexPam}]"); ;
					erro.IdErroMensagem = entityMensagemErro.IdErroMensagem;
					erro.CodigoNivelErro = 2;
					erros = true;
					_uowSciex.CommandStackSciex.ErroProcessamento.Salvar(erro);
					_uowSciex.CommandStackSciex.Save();
				}

				var codSuf = int.Parse(produto.CodigoProdutoSuframa);
				var inscSuf = int.Parse(produto.InscricaoCadastral);
				var codTipoProduto = int.Parse(produto.CodigoTipoProduto);
				var codUnidade = int.Parse(produto.CodigoUnidade);


				var existeLEProduto = _uowSciex.QueryStackSciex.LEProduto.Existe(x => x.CodigoProdutoSuframa == codSuf &&
				x.CodigoProduto == produto.CodigoProdutoPexPam && x.InscricaoCadastral == inscSuf);
				if (!existeLEProduto)
				{
					erros = true;
					RegistarMensagemErro(produto.SolicitacaoPELote, 704, 2, new string[] { "[spp_co_produto_suframa]", "[spp_co_produto_exp]" },
						new string[] { $"[{produto.CodigoProdutoSuframa}]", $"[{produto.CodigoProdutoPexPam}]" }, produto.Id);
				}

				var existeLETipoProduto = _uowSciex.QueryStackSciex.LEProduto.Existe(x => x.CodigoTipoProduto == codTipoProduto &&
				x.CodigoProduto == produto.CodigoProdutoPexPam && x.InscricaoCadastral == inscSuf);
				if (!existeLETipoProduto)
				{
					erros = true;
					RegistarMensagemErro(produto.SolicitacaoPELote, 705, 2, new string[] { "[spp_co_tp_produto]", "[spp_co_produto_exp]" },
								new string[] { $"[{produto.CodigoTipoProduto}]", $"[{produto.CodigoProdutoPexPam}]" }, produto.Id);
				}

				var existeLEDescricaoModeloProduto = _uowSciex.QueryStackSciex.LEProduto.Existe(x => x.DescricaoModelo == produto.DescricaoModelo &&
				x.CodigoProduto == produto.CodigoProdutoPexPam && x.InscricaoCadastral == inscSuf);
				if (!existeLEDescricaoModeloProduto)
				{
					erros = true;
					RegistarMensagemErro(produto.SolicitacaoPELote, 706, 2, new string[] { "[spp_co_produto_exp]" },
								new string[] { $"[{produto.CodigoProdutoPexPam}]" }, produto.Id);
				}

				var existeLECodigoUnidadeProduto = _uowSciex.QueryStackSciex.LEProduto.Existe(x => x.CodigoUnidadeMedida == codUnidade &&
				x.CodigoProduto == produto.CodigoProdutoPexPam && x.InscricaoCadastral == inscSuf);
				if (!existeLECodigoUnidadeProduto)
				{
					erros = true;
					RegistarMensagemErro(produto.SolicitacaoPELote, 707, 2, new string[] { "[spp_co_unidade]", "[spp_co_produto_exp]" },
								new string[] { $"[{produto.CodigoUnidade}]", $"[{produto.CodigoProdutoPexPam}]" }, produto.Id);
				}

				var existeLENCMProduto = _uowSciex.QueryStackSciex.LEProduto.Existe(x => x.CodigoNCM == produto.CodigoNCM &&
				x.CodigoProduto == produto.CodigoProdutoPexPam && x.InscricaoCadastral == inscSuf);
				if (!existeLENCMProduto)
				{
					erros = true;
					RegistarMensagemErro(produto.SolicitacaoPELote, 708, 2, new string[] { "[spp_co_ncm]", "[spp_co_produto_exp]" },
								new string[] { $"[{produto.CodigoNCM}]", $"[{produto.CodigoProdutoPexPam}]" }, produto.Id);
				}

				var existeLEProdutoDuplicado = _uowSciex.QueryStackSciex.SolicitacaoPEProduto.Contar(x => x.NumeroLote == produto.NumeroLote && x.AnoLote == produto.AnoLote &&
				x.CodigoProdutoPexPam == produto.CodigoProdutoPexPam && x.InscricaoCadastral == produto.InscricaoCadastral && x.SolicitacaoPELote.EspId == produto.SolicitacaoPELote.EspId);
				if (existeLEProdutoDuplicado > 1)
				{
					erros = true;
					RegistarMensagemErro(produto.SolicitacaoPELote, 709, 2, new string[] { "[spp_co_produto_exp]" },
								new string[] { $"[{produto.CodigoProdutoPexPam}]" }, produto.Id);
				}

				int insc = int.Parse(produto.SolicitacaoPELote.InscricaoCadastral);
				var planoIndeferidoExiste = _uowSciex.QueryStackSciex.PlanoExportacao.Selecionar(x => x.NumeroPlano == produto.SolicitacaoPELote.NumeroLote && x.AnoPlano == produto.SolicitacaoPELote.Ano &&
				   x.NumeroInscricaoCadastral == insc && x.Situacao == 5);
				if (planoIndeferidoExiste != null)
				{
					var CodigoProdutoSuframa = int.Parse(produto.CodigoProdutoSuframa);
					var CodigoTipoProduto = int.Parse(produto.CodigoTipoProduto);
					var CodigoUnidade = int.Parse(produto.CodigoUnidade);
					var peProduto = _uowSciex.QueryStackSciex.PlanoExportacaoProduto.Selecionar
																(x =>
																	x.IdPlanoExportacao == planoIndeferidoExiste.IdPlanoExportacao &&
																	x.CodigoProdutoSuframa == CodigoProdutoSuframa &&
																	x.CodigoProdutoExportacao == produto.CodigoProdutoPexPam &&
																	x.CodigoTipoProduto == CodigoTipoProduto &&
																	x.DescricaoModelo == produto.DescricaoModelo &&
																	x.CodigoUnidade == CodigoUnidade &&
																	x.CodigoNCM == produto.CodigoNCM
																);
					if (peProduto == null)
					{
						erros = true;
						var entityMensagemErro = _uowSciex.QueryStackSciex.ErroMensagem.Selecionar(o => o.IdErroMensagem == 734);
						var erro = new ErroProcessamentoEntity();
						erro.DataProcessamento = GetDateTimeNowUtc();
						erro.CNPJImportador = produto.SolicitacaoPELote.NumeroCNPJ;
						erro.Descricao = entityMensagemErro.Descricao.Replace("[SPP_CO_PRODUTO_EXP] ", $"[{produto.CodigoProdutoPexPam}]");
						erro.IdErroMensagem = entityMensagemErro.IdErroMensagem;
						erro.CodigoNivelErro = 1;

						_uowSciex.CommandStackSciex.ErroProcessamento.Salvar(erro);
						_uowSciex.CommandStackSciex.Save();
					}
				}


				var paisErro = ValidarPaisProdutoPE(produto.PaisProduto);
				var insumosErro = ValidarInsumosProduto(produto.Insumos);
				if (paisErro || insumosErro)
				{
					erros = true;
				}
				if (!erros)
				{
					produto.SituacaoValidacao = 2;
				}
				else
				{
					produto.SolicitacaoPELote.Situacao = 3;
					produto.SituacaoValidacao = 3;
				}
				_uowSciex.CommandStackSciex.SolicitacaoPEProduto.Salvar(produto);


			}
			return erros;
		}

		private bool ValidarExistenciaLote(SolicitacaoPELoteEntity lote)
		{
			int insc = int.Parse(lote.InscricaoCadastral);
			int _indeferido = 5;
			var planoExiste = _uowSciex.QueryStackSciex.PlanoExportacao.Existe(x => 
			
			x.NumeroPlano == lote.NumeroLote 
			&& 
			x.AnoPlano == lote.Ano 
			&&
			x.NumeroInscricaoCadastral == insc 
			&& 
			x.Situacao != _indeferido
			);

			if (planoExiste)
			{
				byte nivelErroProduto = 2;
				var entityMensagemErro = _uowSciex.QueryStackSciex.ErroMensagem.Selecionar(o => o.IdErroMensagem == 700);
				var erro = new ErroProcessamentoEntity();
				erro.DataProcessamento = GetDateTimeNowUtc();
				erro.CNPJImportador = lote.NumeroCNPJ;
				erro.Descricao = entityMensagemErro.Descricao.Replace("[SLO_NU_ANO_LOTE/SLO_NU_LOTE]", $"[{lote.Ano}/{lote.NumeroLote}]");
				erro.IdErroMensagem = entityMensagemErro.IdErroMensagem;
				erro.CodigoNivelErro = nivelErroProduto;
				erro.IdLote = lote.Id;
				lote.Situacao = 3;

				_uowSciex.CommandStackSciex.ErroProcessamento.Salvar(erro);
				_uowSciex.CommandStackSciex.Save();
				return true;
			}

			return false;
		}

		private bool ValidarModalidade(SolicitacaoPELoteEntity lote)
		{
			var valido = !String.IsNullOrEmpty(lote.TipoModalidade) && (lote.TipoModalidade == "S" || lote.TipoModalidade == "I" || lote.TipoModalidade == "R");
			if (!valido)
			{
				var entityMensagemErro = _uowSciex.QueryStackSciex.ErroMensagem.Selecionar(o => o.IdErroMensagem == 701);
				var erro = new ErroProcessamentoEntity();
				erro.DataProcessamento = GetDateTimeNowUtc();
				erro.CNPJImportador = lote.NumeroCNPJ;
				erro.Descricao = entityMensagemErro.Descricao;
				erro.IdErroMensagem = entityMensagemErro.IdErroMensagem;
				erro.CodigoNivelErro = 1;
				lote.Situacao = 3;
				_uowSciex.CommandStackSciex.ErroProcessamento.Salvar(erro);
				_uowSciex.CommandStackSciex.Save();
				return true;

			}
			return false;
		}

		private bool ValidarTipoLote(SolicitacaoPELoteEntity lote)
		{
			var valido = !String.IsNullOrEmpty(lote.TipoModalidade) && (lote.TipoExportacao == "AP" || lote.TipoExportacao == "CO");
			if (!valido)
			{
				var entityMensagemErro = _uowSciex.QueryStackSciex.ErroMensagem.Selecionar(o => o.IdErroMensagem == 702);
				var erro = new ErroProcessamentoEntity();
				erro.DataProcessamento = GetDateTimeNowUtc();
				erro.CNPJImportador = lote.NumeroCNPJ;
				erro.Descricao = entityMensagemErro.Descricao;
				erro.IdErroMensagem = entityMensagemErro.IdErroMensagem;
				erro.CodigoNivelErro = 1;
				lote.Situacao = 3;
				_uowSciex.CommandStackSciex.ErroProcessamento.Salvar(erro);
				_uowSciex.CommandStackSciex.Save();
				return true;
			}
			return false;
		}

		public string RecuperarNumeroProcessoArquivoLote(string[] arquivoLinhas)
		{
			if (arquivoLinhas.Length > 0)
			{
				return arquivoLinhas.FirstOrDefault().Substring(28, 10).Trim();
			}

			return null;
		}

		public string RecuperarTipoModalidadeArquivoLote(string[] arquivoLinhas)
		{
			if (arquivoLinhas.Length > 0)
			{
				return arquivoLinhas.FirstOrDefault().Substring(42, 1).Trim();
			}

			return null;
		}

		public string RecuperarTipoLoteArquivoLote(string[] arquivoLinhas)
		{
			if (arquivoLinhas.Length > 0)
			{
				return arquivoLinhas.FirstOrDefault().Substring(43, 2).Trim();
			}

			return null;
		}

		public int? RecuperarAnoProcessoArquivoLote(string[] arquivoLinhas)
		{
			if (arquivoLinhas.Length > 0)
			{
				var ano = arquivoLinhas.FirstOrDefault().Substring(38, 4).Trim();
				int anoInt;
				bool bNum = int.TryParse(ano, out anoInt);
				if (bNum)
				{
					return anoInt;
				}
			}
			return null;
		}

		public int? RecuperarAnoLoteArquivoLote(string[] arquivoLinhas)
		{
			if (arquivoLinhas.Length > 0)
			{
				var ano = arquivoLinhas.FirstOrDefault().Substring(9, 4).Trim();
				int anoInt;
				bool bNum = int.TryParse(ano, out anoInt);
				if (bNum)
				{
					return anoInt;
				}
			}
			return null;
		}

		public int? RecuperarNumeroLoteArquivoLote(string[] arquivoLinhas)
		{
			if (arquivoLinhas.Length > 0)
			{
				var ano = arquivoLinhas.FirstOrDefault().Substring(14, 5).Trim();
				int anoInt;
				bool bNum = int.TryParse(ano, out anoInt);
				if (bNum)
				{
					return anoInt;
				}
			}
			return null;
		}

		public decimal? RecuperarValorDolarArquivoProduto(string linha)
		{
			if (linha.Length > 0)
			{
				var valorLinha = linha.Substring(309, 15).Trim().InsertDecimal(2);
				decimal valor;
				bool bNum = decimal.TryParse(valorLinha, out valor);
				if (bNum)
				{
					return valor;
				}
			}
			return null;
		}
		public decimal? RecuperarValorNacionalArquivoProduto(string linha)
		{
			if (linha.Length > 0)
			{
				var valorLinha = linha.Substring(324, 15).Trim().InsertDecimal(2);
				decimal valor;
				bool bNum = decimal.TryParse(valorLinha, out valor);
				if (bNum)
				{
					return valor;
				}
			}
			return null;
		}

		public decimal? RecuperarQuantidadeArquivoProduto(string linha)
		{
			if (linha.Length > 0)
			{
				var valorLinha = linha.Substring(295, 14).Trim().InsertDecimal(5);
				decimal valor;
				bool bNum = decimal.TryParse(valorLinha, out valor);
				if (bNum)
				{
					return valor;
				}
			}
			return null;
		}

		public decimal? RecuperarQuantidadeArquivoDetalhe(string linha)
		{
			if (linha.Length > 0)
			{
				var valorLinha = linha.Substring(36, 14).Trim().InsertDecimal(5);
				decimal valor;
				bool bNum = decimal.TryParse(valorLinha, out valor);
				if (bNum)
				{
					return valor;
				}
			}
			return null;
		}

		public decimal? RecuperarValorUnitarioDetalhe(string linha)
		{
			if (linha.Length > 0)
			{
				var valorLinha = linha.Substring(50, 18).Trim().InsertDecimal(7);
				decimal valor;
				bool bNum = decimal.TryParse(valorLinha, out valor);
				if (bNum)
				{
					return valor;
				}
			}
			return null;
		}

		public decimal? RecuperarValorFreteDetalhe(string linha)
		{
			if (linha.Length > 0)
			{
				var valorLinha = linha.Substring(68, 18).Trim().InsertDecimal(7);
				decimal valor;
				bool bNum = decimal.TryParse(valorLinha, out valor);
				if (bNum)
				{
					return valor;
				}
			}
			return null;
		}

		public decimal? RecuperarCoeficienteTecArquivoInsumo(string linha)
		{
			if (linha.Length > 0)
			{
				var valorLinha = linha.Substring(43, 15).Trim().InsertDecimal(8);

				decimal valor;
				bool bNum = decimal.TryParse(valorLinha, out valor);
				if (bNum)
				{
					return valor;
				}
			}
			return null;
		}

		public decimal? RecuperarPctPerdaArquivoInsumo(string linha)
		{
			if (linha.Length > 0)
			{
				var valorLinha = linha.Substring(78, 10).Trim().InsertDecimal(7);
				decimal valor;
				bool bNum = decimal.TryParse(valorLinha, out valor);
				if (bNum)
				{
					return 0;
				}
			}
			return null;
		}

		public string RecuperarNCMArquivoProduto(string arquivoLinhas)
		{
			if (arquivoLinhas.Length > 0)
			{
				return arquivoLinhas.Substring(25, 8).Trim();
			}
			return null;
		}

		public string RecuperarNCMArquivoInsumo(string arquivoLinhas)
		{
			if (arquivoLinhas.Length > 0)
			{
				return arquivoLinhas.Substring(31, 8).Trim();
			}
			return null;
		}

		public string RecuperarDetalheArquivoInsumo(string arquivoLinhas)
		{
			if (arquivoLinhas.Length > 0)
			{
				return arquivoLinhas.Substring(39, 4).Trim();
			}
			return null;
		}

		public string RecuperarPartNumberArquivoInsumo(string arquivoLinhas)
		{
			if (arquivoLinhas.Length > 0)
			{
				return arquivoLinhas.Substring(58, 20).Trim();
			}
			return null;
		}

		public string RecuperarDescInsumoArquivoInsumo(string arquivoLinhas)
		{
			if (arquivoLinhas.Length > 0)
			{
				return arquivoLinhas.Substring(88, 254).Trim();
			}
			return null;
		}

		public string RecuperarEspTecnicaArquivoInsumo(string arquivoLinhas)
		{
			if (arquivoLinhas.Length > 0)
			{
				return arquivoLinhas.Substring(342, 3723).Trim();
			}
			return null;
		}

		public string RecuperarUnidadeInsumo(string arquivoLinhas)
		{
			if (arquivoLinhas.Length > 0)
			{
				return arquivoLinhas.Substring(28, 2).Trim();
			}
			return null;
		}

		public string RecuperarCodigoPaisDetalhe(string arquivoLinhas)
		{
			if (arquivoLinhas.Length > 0)
			{
				return arquivoLinhas.Substring(30, 3).Trim();
			}
			return null;
		}

		public string RecuperarCodigoMoedaDetalhe(string arquivoLinhas)
		{
			if (arquivoLinhas.Length > 0)
			{
				return arquivoLinhas.Substring(33, 3).Trim();
			}
			return null;
		}

		public string RecuperarNumeroDocumentoDetalhe(string arquivoLinhas)
		{
			if (arquivoLinhas.Length > 0)
			{
				return arquivoLinhas.Substring(86, 20).Trim();
			}
			return null;
		}

		public string RecuperarCnpjRemetenteDetalhe(string arquivoLinhas)
		{
			if (arquivoLinhas.Length > 0)
			{
				return arquivoLinhas.Substring(106, 14).Trim();
			}
			return null;
		}

		public string RecuperarDescricaoModArquivoProduto(string arquivoLinhas)
		{
			if (arquivoLinhas.Length > 0)
			{
				return arquivoLinhas.Substring(40, 255).Trim();
			}
			return null;
		}

		public string RecuperarCodigoUnidaderAquivoProduto(string arquivoLinhas)
		{
			if (arquivoLinhas.Length > 0)
			{
				return arquivoLinhas.Substring(23, 2).Trim();
			}
			return null;
		}

		public string RecuperarCodigoPPArquivoProduto(string arquivoLinhas)
		{
			if (arquivoLinhas.Length > 0)
			{
				return arquivoLinhas.Substring(33, 4).Trim();
			}
			return null;
		}

		public string RecuperarCodigoTipoPPArquivoProduto(string arquivoLinhas)
		{
			if (arquivoLinhas.Length > 0)
			{
				return arquivoLinhas.Substring(37, 3).Trim();
			}
			return null;
		}

		public string RecuperarTipoInsumoArquivoInsumo(string arquivoLinhas)
		{
			if (arquivoLinhas.Length > 0)
			{
				return arquivoLinhas.Substring(30, 1).Trim();
			}
			return null;
		}

		public int RecuperarCodigoPexPamArquivoProduto(string linha)
		{
			if (linha.Length > 0)
			{
				var valorLinha = linha.Substring(19, 4).Trim();
				int valor;
				bool bNum = int.TryParse(valorLinha, out valor);
				if (bNum)
				{
					return valor;
				}
			}
			return 0;
		}

		public int RecuperarCodigoInsumoArquivoInsumo(string linha)
		{
			if (linha.Length > 0)
			{
				var valorLinha = linha.Substring(23, 5).Trim();
				int valor;
				bool bNum = int.TryParse(valorLinha, out valor);
				if (bNum)
				{
					return valor;
				}
			}
			return 0;
		}

		public int RecuperarSequencialArquivoDetalhe(string linha)
		{
			if (linha.Length > 0)
			{
				var valorLinha = linha.Substring(28, 2).Trim();
				int valor;
				bool bNum = int.TryParse(valorLinha, out valor);
				if (bNum)
				{
					return valor;
				}
			}
			return 0;
		}

		public decimal? RecuperarQuantidadeArquivoPais(string linha)
		{
			if (linha.Length > 0)
			{
				var valorLinha = linha.Substring(26, 14).Trim().InsertDecimal(5);
				decimal valor;
				bool bNum = decimal.TryParse(valorLinha, out valor);
				if (bNum)
				{
					return valor;
				}
			}
			return null;
		}

		public decimal? RecuperarValorDolarArquivoPais(string linha)
		{
			if (linha.Length > 0)
			{
				var valorLinha = linha.Substring(40, 15).Trim().InsertDecimal(2);
				decimal valor;
				bool bNum = decimal.TryParse(valorLinha, out valor);
				if (bNum)
				{
					return valor;
				}
			}
			return null;
		}

		public string RecuperarCodigoPaisArquivoPais(string arquivoLinhas)
		{
			if (arquivoLinhas.Length > 0)
			{
				return arquivoLinhas.Substring(23, 3).Trim();
			}
			return null;
		}

		public string RecuperarRegistroExportacaoArquivoPais(string arquivoLinhas)
		{
			if (arquivoLinhas.Length > 0)
			{
				return arquivoLinhas.Substring(55, 15).Trim();
			}
			return null;
		}

		public void RegistrarInicioExtracao(EstruturaPropriaPliEntity plano)
		{
			plano.DataInicioProcessamento = GetDateTimeNowUtc();
			plano.StatusProcessamentoArquivo = 2;
			plano.DescricaoPendenciaImportador = null;
			_uowSciex.CommandStackSciex.EstruturaPropriaPli.Salvar(plano);
			_uowSciex.CommandStackSciex.Save();
		}

		public void RegistrarInicioValidacao(EstruturaPropriaPliEntity plano)
		{
			//plano.DataFimProcessamento = GetDateTimeNowUtc();
			plano.StatusProcessamentoArquivo = 3;
			_uowSciex.CommandStackSciex.EstruturaPropriaPli.Salvar(plano);
			_uowSciex.CommandStackSciex.Save();
		}

		public void encodeTest()
		{
			//tirar daqui a proxima linha
			var estrutura = new EstruturaPropriaPLIArquivoVM();
			estrutura.LocalPastaEstruturaArquivo = System.Web.HttpContext.Current.Server.MapPath("EstruturaPropriaPEVal");

			string local = estrutura.LocalPastaEstruturaArquivo;
			string[] arquivos = Directory.GetFiles(local);
			var insumoFile = arquivos.Where(o => o.Contains("PX_INSUMO.TXT"));
			if (insumoFile.FirstOrDefault() != null)
			{
				var filename = Path.GetFileName(insumoFile.FirstOrDefault());

				string[] lines = File.ReadAllLines(insumoFile.FirstOrDefault());

				if (lines[0].Length > 0)
				{
					if (lines[0].Substring(0, 2) == "0\0")
					{
						lines = File.ReadAllLines(filename, Encoding.Unicode);
						File.Delete(filename);
						File.WriteAllLines(filename, lines);
					}
					else
					{
						lines = File.ReadAllLines(insumoFile.FirstOrDefault(), Encoding.UTF8);
					}

					foreach (var item in lines)
					{
						Console.WriteLine(item);
					}
				}
			}
		}

		public void ExtrairPE(EstruturaPropriaPliEntity plano)
		{
			RegistrarInicioExtracao(plano);
			var estrutura = new EstruturaPropriaPLIArquivoVM();
			//tirar daqui a proxima linha
			estrutura.LocalPastaEstruturaArquivo = System.Web.HttpContext.Current.Server.MapPath("EstruturaPropriaPEVal");
			estrutura.NomeArquivo = plano.NomeArquivoEnvio;
			estrutura.Arquivo = plano.EstruturaPropriaPliArquivo.Arquivo;
			Compressor objComprimir = new Compressor();

			string local = estrutura.LocalPastaEstruturaArquivo + @"\" + estrutura.NomeArquivo.Substring(0, estrutura.NomeArquivo.IndexOf("."));

			if (!Directory.Exists(local))
			{
				Directory.CreateDirectory(local);
			}

			string[] arquivos = Directory.GetFiles(local);
			foreach (string item in arquivos)
			{
				File.Delete(item);
			}
			var lote = new SolicitacaoPELoteEntity();
			if (objComprimir.UnZIP(estrutura.Arquivo, local))
			{
				string nomeArquivo = estrutura.NomeArquivo.Substring(0, estrutura.NomeArquivo.IndexOf("."));
				string extensao = estrutura.NomeArquivo.Split('.')[1];
				arquivos = Directory.GetFiles(local);


				var loteFile = arquivos.Where(o => o.Contains("PX_LOTE.TXT"));
				if (loteFile.FirstOrDefault() != null)
				{
					var filename = Path.GetFileName(loteFile.FirstOrDefault());

					string[] lines = File.ReadAllLines(loteFile.FirstOrDefault());

					if (lines[0].Length > 0)
					{
						if (lines[0].Substring(0, 2) == "0\0")
						{
							lines = File.ReadAllLines(filename, Encoding.Unicode);
							File.Delete(filename);
							File.WriteAllLines(filename, lines);
						}
						else
						{
							lines = File.ReadAllLines(loteFile.FirstOrDefault(), Encoding.Default);
						}
						lote.InscricaoCadastral = plano.InscricaoCadastral.ToString();
						lote.NumeroCNPJ = plano.CNPJImportador;
						lote.RazaoSocial = plano.RazaoSocialImportador;
						lote.Ano = RecuperarAnoLoteArquivoLote(lines);
						lote.NumeroLote = RecuperarNumeroLoteArquivoLote(lines);
						lote.NumeroProcesso = RecuperarNumeroProcessoArquivoLote(lines);
						lote.AnoProcesso = RecuperarAnoProcessoArquivoLote(lines);
						lote.TipoModalidade = RecuperarTipoModalidadeArquivoLote(lines);
						lote.TipoExportacao = RecuperarTipoLoteArquivoLote(lines);
						lote.Situacao = 1;

					}

				}

				var produtoFile = arquivos.Where(o => o.Contains("PX_PRODUTO.TXT"));
				var produto = new SolicitacaoPEProdutoEntity();
				if (produtoFile.FirstOrDefault() != null)
				{
					var filename = Path.GetFileName(produtoFile.FirstOrDefault());

					string[] lines = File.ReadAllLines(produtoFile.FirstOrDefault());

					if (lines[0].Length > 0)
					{
						if (lines[0].Substring(0, 2) == "0\0")
						{
							lines = File.ReadAllLines(filename, Encoding.Unicode);
							File.Delete(filename);
							File.WriteAllLines(filename, lines);
						}
						else
						{
							lines = File.ReadAllLines(produtoFile.FirstOrDefault(), Encoding.Default);
						}

						foreach (var item in lines)
						{
							var anoLote = RecuperarAnoLoteArquivoLote(lines);
							var numLote = RecuperarNumeroLoteArquivoLote(lines);
							var codPexPam = RecuperarCodigoPexPamArquivoProduto(item);
							//considerando insercao sequencial nos arquivos...
							//if (anoLote == lote.Ano && numLote == lote.NumeroLote)
							//{
							produto.ValorDolar = RecuperarValorDolarArquivoProduto(item);
							produto.Quantidade = RecuperarQuantidadeArquivoProduto(item);
							produto.CodigoNCM = RecuperarNCMArquivoProduto(item);
							produto.CodigoProdutoSuframa = RecuperarCodigoPPArquivoProduto(item);
							produto.CodigoTipoProduto = RecuperarCodigoTipoPPArquivoProduto(item);
							produto.CodigoUnidade = RecuperarCodigoUnidaderAquivoProduto(item);
							produto.CodigoProdutoPexPam = RecuperarCodigoPexPamArquivoProduto(item);
							produto.InscricaoCadastral = plano.InscricaoCadastral.ToString();
							produto.AnoLote = lote.Ano;
							produto.NumeroLote = lote.NumeroLote;
							produto.DescricaoModelo = RecuperarDescricaoModArquivoProduto(item);
							produto.ValorNacional = RecuperarValorNacionalArquivoProduto(item);
							//produto.SituacaoValidacao = lote.Situacao; //confirmar TODO

							var insumos = RecuperarInsumos(arquivos, produto);
							foreach (var insumo in insumos)
							{
								produto.Insumos.Add(insumo);
							}
							lote.produtos.Add(produto);
							//}
						}
					}

				}

				var prodPaisFile = arquivos.Where(o => o.Contains("PX_PRODPAIS.TXT"));
				if (prodPaisFile.FirstOrDefault() != null)
				{
					var filename = Path.GetFileName(prodPaisFile.FirstOrDefault());

					string[] lines = File.ReadAllLines(prodPaisFile.FirstOrDefault());

					if (lines[0].Length > 0)
					{
						if (lines[0].Substring(0, 2) == "0\0")
						{
							lines = File.ReadAllLines(filename, Encoding.Unicode);
							File.Delete(filename);
							File.WriteAllLines(filename, lines);
						}
						else
						{
							lines = File.ReadAllLines(prodPaisFile.FirstOrDefault(), Encoding.Default);
						}
						foreach (var item in lines)
						{
							var produtoPais = new SolicitacaoPEProdutoPaisEntity();


							var anoLote = RecuperarAnoLoteArquivoLote(lines);
							var numLote = RecuperarNumeroLoteArquivoLote(lines);
							var codPexPam = RecuperarCodigoPexPamArquivoProduto(item);
							//considerando insercao sequencial nos arquivos...
							if (anoLote == produto.AnoLote && numLote == produto.NumeroLote && codPexPam == produto.CodigoProdutoPexPam)
							{
								produtoPais.InscricaoCadastral = plano.InscricaoCadastral.ToString();
								produtoPais.AnoLote = anoLote;
								produtoPais.NumeroLote = numLote;
								produtoPais.CodigoProdutoPexPam = codPexPam;
								produtoPais.Quantidade = RecuperarQuantidadeArquivoPais(item);
								produtoPais.ValorDolar = RecuperarValorDolarArquivoPais(item);
								produtoPais.CodigoPais = RecuperarCodigoPaisArquivoPais(item);
								produtoPais.RegistroExportacao = RecuperarRegistroExportacaoArquivoPais(item);
								produtoPais.DataEmbarque = DateTime.Now; // nao tem o exemplo do formato TODO

								var Dues = RecuperarDues(arquivos, produtoPais);
								foreach (var Due in Dues)
								{
									produtoPais.ListaSolicPEDue.Add(Due);
								}
								produto.PaisProduto.Add(produtoPais);


								lote.produtos.Add(produto);
							}
						}
					}

				}
				lote.EspId = plano.IdEstruturaPropria;
				lote.Situacao = 1; //aguardando validacao
				_uowSciex.CommandStackSciex.SolicitacaoPELote.Salvar(lote);
				_uowSciex.CommandStackSciex.Save();


				foreach (string item in arquivos)
				{
					File.Delete(item);
				}
				Directory.Delete(local);

				RegistrarInicioValidacao(plano);
			}
		}

		public ICollection<SolicitacaoPEDueEntity> RecuperarDues(string[] arquivos, SolicitacaoPEProdutoPaisEntity produtopais)
		{
			var listaDues = new HashSet<SolicitacaoPEDueEntity>();
			var dueFile = arquivos.Where(o => o.Contains("PX_RE.TXT"));

			if (dueFile.FirstOrDefault() != null)
			{
				var filename = Path.GetFileName(dueFile.FirstOrDefault());

				string[] lines = File.ReadAllLines(dueFile.FirstOrDefault());

				if (lines.Length > 0)
				{
					if (lines[0].Length > 0)
					{
						if (lines[0].Substring(0, 2) == "0\0")
						{
							lines = File.ReadAllLines(filename, Encoding.Unicode);
							File.Delete(filename);
							File.WriteAllLines(filename, lines);
						}
						else
						{
							lines = File.ReadAllLines(dueFile.FirstOrDefault(), Encoding.Default);
						}

						foreach (var item in lines)
						{
							var due = new SolicitacaoPEDueEntity();
							var anoLote = RecuperarAnoLoteArquivoLote(lines);
							var numLote = RecuperarNumeroLoteArquivoLote(lines);
							var codPexPam = RecuperarCodigoPexPamArquivoProduto(item);
							var codPais = RecuperarCodigoPais(item);
							//considerando insercao sequencial nos arquivos...
							if (anoLote == produtopais.AnoLote
								&& numLote == produtopais.NumeroLote
								&& codPexPam == produtopais.CodigoProdutoPexPam
								&& codPais == int.Parse(produtopais.CodigoPais)
								)
							{
								due.InscricaoCadastral = produtopais.InscricaoCadastral.ToString();
								due.NumeroAnoLote = anoLote;
								due.NumeroLote = numLote;
								due.CodigoProdutoExportacao = codPexPam;
								due.CodigoPais = RecuperarCodigoPais(item);
								due.Numero = RecuperarNumeroRegExportacao(item);
								due.DataAverbacao = RecuperarDataAverbacao(item);
								due.Quantidade = RecuperarQuantidade(item);
								due.ValorDolar = RecuperarValorDolar(item);

								listaDues.Add(due);
							}
						}
					} 
				}

			}

			return listaDues;
		}

		private decimal? RecuperarValorDolar(string arquivoLinhas)
		{

			if (arquivoLinhas.Length > 0)
			{
				var valorLinha = arquivoLinhas.Substring(71, 20).Trim().InsertDecimal(7);

				decimal valor;
				bool bNum = decimal.TryParse(valorLinha, out valor);
				if (bNum)
				{
					return valor;
				}
			}
			return null;
		}

		private decimal? RecuperarQuantidade(string arquivoLinhas)
		{

			if (arquivoLinhas.Length > 0)
			{
				var valorLinha = arquivoLinhas.Substring(51, 20).Trim().InsertDecimal(7);

				decimal valor;
				bool bNum = decimal.TryParse(valorLinha, out valor);
				if (bNum)
				{
					return valor;
				}
			}
			return null;
		}

		private DateTime? RecuperarDataAverbacao(string arquivoLinhas)
		{
			if (arquivoLinhas.Length > 0)
			{
				return Convert.ToDateTime(arquivoLinhas.Substring(41, 10).Trim());
			}
			return null;
		}

		private string RecuperarNumeroRegExportacao(string arquivoLinhas)
		{
			if (arquivoLinhas.Length > 0)
			{
				return arquivoLinhas.Substring(26, 15).Trim();
			}
			return null;
		}

		private int? RecuperarCodigoPais(string arquivoLinhas)
		{
			if (arquivoLinhas.Length > 0)
			{
				return Convert.ToInt32(arquivoLinhas.Substring(23, 3).Trim());
			}
			return null;
		}

		public ICollection<SolicitacaoPEInsumoEntity> RecuperarInsumos(string[] arquivos, SolicitacaoPEProdutoEntity produto)
		{
			var insumos = new HashSet<SolicitacaoPEInsumoEntity>();
			var insumoFile = arquivos.Where(o => o.Contains("PX_INSUMO.TXT"));

			if (insumoFile.FirstOrDefault() != null)
			{
				var filename = Path.GetFileName(insumoFile.FirstOrDefault());

				string[] lines = File.ReadAllLines(insumoFile.FirstOrDefault());

				if (lines.Length > 0)
				{
					if (lines[0].Length > 0)
					{
						if (lines[0].Substring(0, 2) == "0\0")
						{
							lines = File.ReadAllLines(filename, Encoding.Unicode);
							File.Delete(filename);
							File.WriteAllLines(filename, lines);
						}
						else
						{
							lines = File.ReadAllLines(insumoFile.FirstOrDefault(), Encoding.Default);
						}

						foreach (var item in lines)
						{
							var insumo = new SolicitacaoPEInsumoEntity();
							var anoLote = RecuperarAnoLoteArquivoLote(lines);
							var numLote = RecuperarNumeroLoteArquivoLote(lines);
							var codPexPam = RecuperarCodigoPexPamArquivoProduto(item);
							//considerando insercao sequencial nos arquivos...
							if (anoLote == produto.AnoLote && numLote == produto.NumeroLote && codPexPam == produto.CodigoProdutoPexPam)
							{
								insumo.InscricaoCadastral = produto.InscricaoCadastral.ToString();
								insumo.AnoLote = anoLote;
								insumo.NumeroLote = numLote;
								insumo.CodigoProdutoPexPam = codPexPam;
								insumo.CodigoInsumo = RecuperarCodigoInsumoArquivoInsumo(item);
								insumo.CodigoUnidade = RecuperarUnidadeInsumo(item);
								insumo.CodigoTipoInsumo = RecuperarTipoInsumoArquivoInsumo(item);
								insumo.CodigoNCM = RecuperarNCMArquivoInsumo(item);
								insumo.CodigoDetalhe = RecuperarDetalheArquivoInsumo(item);
								insumo.ValorCoeficienteTecnico = RecuperarCoeficienteTecArquivoInsumo(item);
								insumo.DescricaoPartNumber = RecuperarPartNumberArquivoInsumo(item);
								insumo.ValorPctPerda = RecuperarPctPerdaArquivoInsumo(item);
								insumo.DescricaoInsumo = RecuperarDescInsumoArquivoInsumo(item);
								insumo.DescricaoEspTecnica = RecuperarEspTecnicaArquivoInsumo(item);
								var detalhes = RecuperarDetalheInsumos(arquivos, produto, insumo);
								foreach (var detalhe in detalhes)
								{
									insumo.Detalhes.Add(detalhe);
								}
								insumos.Add(insumo);
							}
						}
					} 
				}

			}

			return insumos;
		}

		public ICollection<SolicitacaoPEDetalheEntity> RecuperarDetalheInsumos(string[] arquivos, SolicitacaoPEProdutoEntity produto,
			SolicitacaoPEInsumoEntity insumo)
		{
			var detalhes = new HashSet<SolicitacaoPEDetalheEntity>();
			var detalheInsumoFile = arquivos.Where(o => o.Contains("PX_DETINSUMO.TXT"));
			if (detalheInsumoFile.FirstOrDefault() != null)
			{
				var filename = Path.GetFileName(detalheInsumoFile.FirstOrDefault());

				string[] lines = File.ReadAllLines(detalheInsumoFile.FirstOrDefault());

				if (lines[0].Length > 0)
				{
					if (lines[0].Substring(0, 2) == "0\0")
					{
						lines = File.ReadAllLines(filename, Encoding.Unicode);
						File.Delete(filename);
						File.WriteAllLines(filename, lines);
					}
					else
					{
						lines = File.ReadAllLines(detalheInsumoFile.FirstOrDefault(), Encoding.Default);
					}

					foreach (var item in lines)
					{
						var detalhe = new SolicitacaoPEDetalheEntity();
						var anoLote = RecuperarAnoLoteArquivoLote(lines);
						var numLote = RecuperarNumeroLoteArquivoLote(lines);
						var codPexPam = RecuperarCodigoPexPamArquivoProduto(item);
						var codigoDetalheInsumo = RecuperarCodigoInsumoArquivoInsumo(item);
						//considerando insercao sequencial nos arquivos...
						if (anoLote == produto.AnoLote && numLote == produto.NumeroLote && codPexPam == produto.CodigoProdutoPexPam &&
							codigoDetalheInsumo == insumo.CodigoInsumo)
						{
							detalhe.InscricaoCadastral = produto.InscricaoCadastral.ToString();
							detalhe.AnoLote = anoLote;
							detalhe.NumeroLote = numLote;
							detalhe.CodigoProdutoPexPam = codPexPam;
							detalhe.CodigoInsumo = RecuperarCodigoInsumoArquivoInsumo(item);
							detalhe.Sequencial = RecuperarSequencialArquivoDetalhe(item);
							detalhe.CodigoPais = RecuperarCodigoPaisDetalhe(item);
							detalhe.CodigoMoeda = RecuperarCodigoMoedaDetalhe(item);
							detalhe.Quantidade = RecuperarQuantidadeArquivoDetalhe(item);
							detalhe.ValorUnitario = RecuperarValorUnitarioDetalhe(item);
							detalhe.ValorFrete = RecuperarValorFreteDetalhe(item);
							detalhe.NumeroDocumento = RecuperarNumeroDocumentoDetalhe(item);
							detalhe.CnpjRemetente = RecuperarCnpjRemetenteDetalhe(item);
							detalhe.DataEmissaoNF = DateTime.Now; // TODO ajustar data;

							detalhes.Add(detalhe);
						}
					}
				}

			}

			return detalhes;
		}
		public void InformarPendenciaImportador(EstruturaPropriaPliEntity plano)
		{
			plano.DescricaoPendenciaImportador = "SITUAÇÃO CADASTRAL DO IMPORTADOR NÃO ESTÁ ATIVA";
			_uowSciex.CommandStackSciex.EstruturaPropriaPli.Salvar(plano);
			_uowSciex.CommandStackSciex.Save();
		}

		public bool ValidarEmpresaAtiva(int inscricaoCadastral)
		{

			var dadosEmpresa = _uowSciex.QueryStackSciex.ViewImportador.Selecionar(o => o.InscricaoCadastral == inscricaoCadastral);
			if (inscricaoCadastral == 0 || dadosEmpresa == null || dadosEmpresa.IdSituacaoInscricao != 1) // nao ativo
			{
				return false;
			}
			return true;
		}

		public ControleExecucaoServicoEntity RegistrarInicioControleLog(EstruturaPropriaPliEntity plano)
		{
			ControleExecucaoServicoEntity _controleExecucaoServicoVM = new ControleExecucaoServicoEntity();
			_controleExecucaoServicoVM.DataHoraExecucaoInicio = GetDateTimeNowUtc();
			_controleExecucaoServicoVM.IdListaServico = 24; //SALVAR ARQUIVO DE LI-CANCELAMENTO
			_controleExecucaoServicoVM.MemoObjetoEnvio = $"Tabela: SCIEX_ESTRUTURA_PROPRIA_ARQUIVO, Campo esp_id: {plano.IdEstruturaPropria}";
			_controleExecucaoServicoVM.MemoObjetoRetorno = $"Tabela: SCIEX_SOLIC_PE_LOTE, Campo esp_id: {plano.IdEstruturaPropria}";
			_controleExecucaoServicoVM.DataHoraExecucaoFim = GetDateTimeNowUtc();
			_controleExecucaoServicoVM.NomeCPFCNPJInteressado = plano.RazaoSocialImportador;
			_controleExecucaoServicoVM.NumeroCPFCNPJInteressado = plano.CNPJImportador;
			_controleExecucaoServicoVM.StatusExecucao = 0;
			_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_controleExecucaoServicoVM);
			_uowSciex.CommandStackSciex.Save();

			return _controleExecucaoServicoVM;
		}

		public ControleExecucaoServicoEntity RegistrarInicioControleLog(EstruturaPropriaPliEntity plano, int idListaServico, string mensagemEnvio)
		{
			ControleExecucaoServicoEntity _controleExecucaoServicoVM = new ControleExecucaoServicoEntity();
			_controleExecucaoServicoVM.DataHoraExecucaoInicio = GetDateTimeNowUtc();
			_controleExecucaoServicoVM.IdListaServico = idListaServico; //SALVAR ARQUIVO DE LI-CANCELAMENTO
			_controleExecucaoServicoVM.MemoObjetoEnvio = mensagemEnvio;
			_controleExecucaoServicoVM.DataHoraExecucaoFim = GetDateTimeNowUtc();
			_controleExecucaoServicoVM.NomeCPFCNPJInteressado = plano.RazaoSocialImportador;
			_controleExecucaoServicoVM.NumeroCPFCNPJInteressado = plano.CNPJImportador;
			_controleExecucaoServicoVM.StatusExecucao = 0;
			_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_controleExecucaoServicoVM);
			_uowSciex.CommandStackSciex.Save();

			return _controleExecucaoServicoVM;
		}

		public void RegistrarFimControleLog(EstruturaPropriaPliEntity plano, ControleExecucaoServicoEntity _controleExecucaoServicoVM,
			string mensagemSucesso, string mensagemErro = null)
		{
			_controleExecucaoServicoVM.DataHoraExecucaoFim = GetDateTimeNowUtc();
			_controleExecucaoServicoVM.StatusExecucao = mensagemErro == null ? 1 : 2;
			if (mensagemErro == null)
			{
				_controleExecucaoServicoVM.MemoObjetoRetorno = mensagemSucesso;
			}
			else
			{
				_controleExecucaoServicoVM.MemoObjetoRetorno = mensagemErro;
			}

			_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_controleExecucaoServicoVM);
			_uowSciex.CommandStackSciex.Save();
		}

		public void RegistrarFimControleLog(EstruturaPropriaPliEntity plano, ControleExecucaoServicoEntity _controleExecucaoServicoVM,
			bool sucesso, string mensagemErro)
		{
			_controleExecucaoServicoVM.DataHoraExecucaoFim = GetDateTimeNowUtc();
			_controleExecucaoServicoVM.StatusExecucao = sucesso == true ? 1 : 2;
			//_controleExecucaoServicoVM.MemoObjetoRetorno = $"Tabela: SCIEX_SOLIC_PE_LOTE, Campo esp_id: {plano.IdEstruturaPropria}";
			if (!sucesso)
			{
				_controleExecucaoServicoVM.MemoObjetoRetorno = _controleExecucaoServicoVM.MemoObjetoRetorno + "Erro: " + mensagemErro;
			}

			_uowSciex.CommandStackSciex.ControleExecucaoServico.Salvar(_controleExecucaoServicoVM);
			_uowSciex.CommandStackSciex.Save();
		}

		public DateTime GetDateTimeNowUtc()
		{
			var manausTime = TimeZoneInfo.ConvertTime(DateTime.Now,
				 TimeZoneInfo.FindSystemTimeZoneById("SA Western Standard Time"));

			return manausTime;

		}


	}

	public static class StringExtension
	{
		public static String InsertDecimal(this String @this, Int32 precision)
		{
			if (@this != null && @this != "")
			{
				String padded = @this.PadLeft(precision, '0');
				return padded.Insert(padded.Length - precision, ",");
			}
			return @this;
		}
	}
}