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
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;

namespace Suframa.Sciex.BusinessLogic
{
	public class EstruturaPropriaLEArquivo : IEstruturaPropriaLEArquivoBll
	{
		private bool estruturaCompleta { get; set; }
		private bool itenspliproblema { get; set; }
		private string CNPJ { get; set; }

		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;
		private readonly IUsuarioLogado _IUsuarioLogado;
		private readonly IViewImportadorBll _viewImportadorBll;
		private readonly IUsuarioPssBll _usuarioPssBll;
		public EstruturaPropriaLEArquivo(IUsuarioLogado usuarioLogado, IUnitOfWorkSciex uowSciex,
			IUsuarioInformacoesBll usuarioInformacoesBll, IViewImportadorBll viewImportadorBll,
			IUsuarioPssBll usuarioPssBll)
		{
			itenspliproblema = false;
			_uowSciex = uowSciex;
			_usuarioInformacoesBll = usuarioInformacoesBll;
			_IUsuarioLogado = usuarioLogado;
			_validation = new Validation();
			_viewImportadorBll = viewImportadorBll;

			estruturaCompleta = false;
			_usuarioPssBll = usuarioPssBll;


			this.CNPJ = _usuarioInformacoesBll.ObterCNPJ().CnpjCpfUnformat();
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

		public int Salvar(EstruturaPropriaLEVM estruturapropriaarquivo, int qtdLinhas)
		{
			if (estruturapropriaarquivo == null)
			{
				return 0;
			}

			EstruturaPropriaPliEntity objEstruturaPropria = new EstruturaPropriaPliEntity();

			var horadata = DateTime.Now;
			objEstruturaPropria.DataEnvio = horadata.AddHours(-1);
			objEstruturaPropria.InscricaoCadastral = estruturapropriaarquivo.InscricaoCadastral;
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
			
			objEstruturaPropria.NomeArquivoEnvio = estruturapropriaarquivo.NomeArquivo;
			objEstruturaPropria.VersaoEstrutura = "001";
			objEstruturaPropria.StatusProcessamentoArquivo = (int)EnumEstruturaPropriaArquivoProcessamento.ENVIADO_A_SUFRAMA;
			objEstruturaPropria.CNPJImportador = estruturapropriaarquivo.CNPJImportador;
			objEstruturaPropria.RazaoSocialImportador = estruturapropriaarquivo.RazaoSocialImportador;
			objEstruturaPropria.QuantidadePLIArquivo = Convert.ToInt16(qtdLinhas);
			objEstruturaPropria.LoginUsuarioEnvio = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoCpfCnpj.Replace(".", "").Replace("-", "").Replace("/", "");
			objEstruturaPropria.NomeUsuarioEnvio = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoNome.Replace(".", "").Replace("-", "").Replace("/", "");
			objEstruturaPropria.QuantidadePLIProcessadoFalha = 0;
			objEstruturaPropria.QuantidadePLIProcessadoSucesso = 0;
			objEstruturaPropria.TipoArquivo = 2;

			objEstruturaPropria.EstruturaPropriaPliArquivo = new EstruturaPropriaPliArquivoEntity();
			objEstruturaPropria.EstruturaPropriaPliArquivo.Arquivo = estruturapropriaarquivo.Arquivo;

			objEstruturaPropria.EstruturaPropriaLE = new EstruturaPropriaLEEntity();
			objEstruturaPropria.EstruturaPropriaLE.CodigoProduto = estruturapropriaarquivo.CodigoProduto;
			objEstruturaPropria.EstruturaPropriaLE.DescricaoProduto = estruturapropriaarquivo.DescricaoProduto;
			objEstruturaPropria.EstruturaPropriaLE.CodigoTipoProduto = estruturapropriaarquivo.CodigoTipoProduto;
			objEstruturaPropria.EstruturaPropriaLE.DescricaoTipoProduto = estruturapropriaarquivo.DescricaoTipoProduto;
			objEstruturaPropria.EstruturaPropriaLE.CodigoNCM = estruturapropriaarquivo.CodigoNCM;
			objEstruturaPropria.EstruturaPropriaLE.DescricaoNCM = estruturapropriaarquivo.DescricaoNCM;
			objEstruturaPropria.EstruturaPropriaLE.CodigoUnidadeMedida = Convert.ToInt16(estruturapropriaarquivo.IdCodigoUnidadeMedida);
			objEstruturaPropria.EstruturaPropriaLE.DescricaoCentroCusto = estruturapropriaarquivo.DescricaoCentroCusto;
			objEstruturaPropria.EstruturaPropriaLE.DescricaoModelo = estruturapropriaarquivo.DescricaoModelo;
			objEstruturaPropria.EstruturaPropriaLE.CodigoModeloEmpresa = estruturapropriaarquivo.DescricaoModeloExportador;
			objEstruturaPropria.EstruturaPropriaLE.IdEstruturaPropria = objEstruturaPropria.IdEstruturaPropria;

			_uowSciex.CommandStackSciex.EstruturaPropriaPli.Salvar(objEstruturaPropria);
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

		public bool ValidarTipoPLi(string[] arquivoLinhas)
		{
			try
			{
				foreach (var item in arquivoLinhas)
				{
					if (item.Length >= 1)
					{
						switch (item.Substring(0, 2))
						{
							case "01":
								{
									if (item.Substring(256, 1) != "1" && item.Substring(256, 1) != "2")
									{
										return false;
									}

									break;
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

		public bool ValidarCodigoAplicacao(string[] arquivoLinhas)
		{
			try
			{
				foreach (var item in arquivoLinhas)
				{
					if (item.Length >= 1)
					{
						switch (item.Substring(0, 2))
						{
							//Codigo aplicacao --Indutrializacao(01) --Apenas
							case "01":
								{
									if (item.Substring(265, 2) != "00" && item.Substring(265, 2) != "01" && item.Substring(265, 2) != "02" && item.Substring(265, 2) != "03")
									{
										return false;
									}

									break;
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

		public bool ValidarEstruturaLE(string[] arquivoLinhas)
		{
			foreach (var linha in arquivoLinhas)
			{
				if (linha.Length > 1)
				{
					//validar tamanho de cada linha
					if (linha.Length != 4037)
					{
						return false;
					}

					//tipo de registro
					if (linha.Substring(0, 1) != "P" && linha.Substring(0, 1) != "E" && linha.Substring(0, 1) != "N" && linha.Substring(0, 1) != "R")
					{
						return false;
					}

					var tipoReg = linha.Substring(0, 1);

					//valida ncm 		
					if (linha.Substring(1, 8).Trim() == "" || Convert.ToInt32(linha.Substring(1, 8).Trim()) == 0)
					{
						return false;
					}

					//valida destaque
					if( tipoReg == "P")
					{
						if (linha.Substring(9, 4).Trim() == "" || Convert.ToInt32(linha.Substring(9, 4).Trim()) == 0)
						{
							return false;
						}
					}
					//valida descricao
					else
					{
						if (linha.Substring(13, 254).Trim() == "")
						{
							return false;
						}
					}

					//valida coeficiente tec
					if (linha.Substring(267, 15).Trim() == "" || Convert.ToDecimal(linha.Substring(267, 15).Trim()) == 0)
					{
						return false;
					}

					//valida unidade
					if (linha.Substring(282, 2).Trim() == "" || Convert.ToInt32(linha.Substring(282, 2).Trim()) == 0)
					{
						return false;
					}

					if(tipoReg == "P" || tipoReg == "E")
					{
						//valida espec tec
						if (linha.Substring(314, 3723).Trim() == "")
						{
							return false;
						}
					}
				}
			}

			return true;
		}

		public bool ValidarDataHoraArquivo(string data)
		{

			try
			{
				string[] dados = data.Split('_');

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

			return true;
		}

		public bool ValidarItensPli()
		{
			return itenspliproblema;
		}

		public bool ValidarPLIsEmpresa(string[] arquivoLinhas)
		{
			if (arquivoLinhas != null)
			{
				foreach (var item in arquivoLinhas)
				{
					if (item.Length >= 1)
					{
						if (item.Substring(0, 2) == "01")
						{
							if (item.Substring(22, 14) != CNPJ)
							{
								return false;
							}
						}
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

		public bool ValidarCnpjArquivo(string cnpjEmpresaVW, string[] arquivoLinhas)
		{

			try
			{
				foreach (var item in arquivoLinhas)
				{
					if (item.Length >= 1)
					{
						switch (item.Substring(0, 2))
						{
							case "01":
								{
									String cnpj = item.Substring(22, 14);
									if (cnpj != cnpjEmpresaVW)
									{
										return false;
									}

									break;
								}
							case "02":
								{
									String cnpj = item.Substring(22, 14);
									if (cnpj != cnpjEmpresaVW)
									{
										return false;
									}

									break;
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

		public bool ValidarInscricaoArquivo(int inscricaoCadastralVW, string[] arquivoLinhas)
		{

			try
			{
				foreach (var item in arquivoLinhas)
				{
					if (item.Length >= 1)
					{
						if (item.Substring(12, 9) != inscricaoCadastralVW.ToString())
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

		public bool ValidarAnoPLI(string[] arquivoLinhas)
		{
			try
			{
				foreach (var item in arquivoLinhas)
				{
					if (item.Length >= 1)
					{
						if (item.Substring(2, 4) != DateTime.Now.Year.ToString())
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

		public bool ValidarFormatacaoPLI(string[] arquivoLinhas)
		{
			try
			{
				foreach (var item in arquivoLinhas)
				{
					if (item.Length >= 1)
					{
						String pliFormato = item.Substring(2, 11);

						if (pliFormato.Replace(" ", "").Length != 11 || !(pliFormato.Contains("/") && pliFormato.IndexOf("/") == 4))
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

		public string ValidarArquivo(EstruturaPropriaLEVM estrutura)
		{
			//try
			//{
				string[] dados = estrutura.NomeArquivo.Split('.');
				string CNPJArquivo = string.Empty;

				if (dados.Length != 2)
				{
					return "Nome do arquivo não está no padrão definido pela SUFRAMA, não é possível realizar envio de LE.";
				}

				if (dados[1].ToUpper() != "TXT")
				{
					return "Nome do arquivo não está no padrão definido pela SUFRAMA, não é possível realizar envio de LE.";
				}

				ViewImportadorVM objVI = _viewImportadorBll.SelecionarInscricao(estrutura.InscricaoCadastral.Value);
				if (objVI != null)
				{
					estrutura.CNPJImportador = objVI.Cnpj;
					estrutura.InscricaoCadastral = objVI.InscricaoCadastral;
					estrutura.RazaoSocialImportador = objVI.RazaoSocial;
				}

				try
				{					
					if (objVI != null)
					{
						if (objVI.DescricaoSituacaoInscricao != "ATIVA")
						{
							return "Empresa bloqueada ou não cadastrada, não é possível realizar envio de LE. É necessário regularizar a situação cadastral junto à SUFRAMA.";
						}
					}
					else
					{
						return "Empresa bloqueada ou não cadastrada, não é possível realizar envio de LE. É necessário regularizar a situação cadastral junto à SUFRAMA.";
					}
				}
				catch
				{
					return "Nome do arquivo não está no padrão definido pela SUFRAMA, não é possível realizar envio de LE.";
				}

				if (!Directory.Exists(estrutura.LocalPastaEstruturaArquivo))
				{
					Directory.CreateDirectory(estrutura.LocalPastaEstruturaArquivo);
				}

				string[] arquivos = Directory.GetFiles(estrutura.LocalPastaEstruturaArquivo);
				foreach (string item in arquivos)
				{
					File.Delete(item);
				}

				File.WriteAllBytes(estrutura.LocalPastaEstruturaArquivo + @"\" + estrutura.NomeArquivo.Substring(0, estrutura.NomeArquivo.IndexOf(".")) , estrutura.Arquivo);

				string arquivoAtual = string.Empty;
				string nomeArquivo = estrutura.NomeArquivo.Substring(0, estrutura.NomeArquivo.IndexOf("."));
				arquivos = Directory.GetFiles(estrutura.LocalPastaEstruturaArquivo);

				foreach (string item in arquivos)
				{
					if (item.Contains(nomeArquivo))
					{
						arquivoAtual = item;
					}
				}

				if (arquivos.Length == 0)
				{
					foreach (string item in arquivos)
					{
						File.Delete(item);
					}
					Directory.Delete(estrutura.LocalPastaEstruturaArquivo);
					return "Arquivo com erro na estrutura, não é possível realizar envio de LE. É necessário seguir o padrão de LE de Estrutura Própria definido pela SUFRAMA.";
				}
				else
				{
					string[] linhas = File.ReadAllLines(arquivoAtual, Encoding.UTF8);

					if (!ValidarEstruturaLE(linhas))
					{
						foreach (string arq in arquivos)
						{
							File.Delete(arq);
						}
						Directory.Delete(estrutura.LocalPastaEstruturaArquivo);
						return "Arquivo com erro na estrutura, não é possível realizar envio de LE. É necessário seguir o padrão de LE de Estrutura Própria definido pela SUFRAMA.";

					}
					else
					{
						var produto = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao.Selecionar(o => o.IdProdutoEmpresaExportacao == estrutura.IdCodigoProdutoSuframa);
						estrutura.CodigoProduto = produto.CodigoProduto;
						estrutura.DescricaoProduto = produto.DescricaoProduto;
						var tipoProduto = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao.Selecionar(o => o.IdProdutoEmpresaExportacao == estrutura.IdCodigoTipoProduto);
						estrutura.CodigoTipoProduto = tipoProduto.CodigoTipoProduto;
						estrutura.DescricaoTipoProduto = tipoProduto.DescricaoTipoProduto;
						var ncm = _uowSciex.QueryStackSciex.ViewProdutoEmpresaExportacao.Selecionar(o => o.IdProdutoEmpresaExportacao == estrutura.IdCodigoNCM);
						estrutura.CodigoNCM = ncm.CodigoNCM;
						estrutura.DescricaoNCM = ncm.DescricaoNCM;

						estrutura.Arquivo = File.ReadAllBytes(arquivoAtual);
						int quantidadelinhas = 0;
						foreach (var item in linhas)
						{ 
							if (item.Length > 1)
							{
							quantidadelinhas = quantidadelinhas + 1;
							}
						}	
						string retorno = Salvar(estrutura, quantidadelinhas).ToString();

						File.Delete(arquivoAtual);
						Directory.Delete(estrutura.LocalPastaEstruturaArquivo);
						return retorno;
					}
				}
			//}
			//catch (Exception ex)
			//{
			//	return ex.Message.ToString();
			//}
		}

		public EstruturaPropriaLEVM BuscarArquivo(EstruturaPropriaLEVM estrutura)
		{

			if (estrutura == null || estrutura.IdEstruturaPropria == 0) { return null; }

			estrutura.Arquivo = _uowSciex.QueryStackSciex.EstruturaPropriaPLIArquivo.Selecionar(q => q.IdEstruturaPropria == estrutura.IdEstruturaPropria).Arquivo;

			return estrutura;
		}
	}
}