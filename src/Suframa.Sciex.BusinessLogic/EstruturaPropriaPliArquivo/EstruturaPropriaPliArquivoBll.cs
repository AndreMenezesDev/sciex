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
	public class EstruturaPropriaPliArquivo : IEstruturaPropriaPliArquivoBll
	{
		private bool estruturaCompleta { get; set; }
		private string listaPLI;
		private Int16 quantidadePLI;
		private string cnpjImportador;
		private string razaoSocial;
		private bool itenspliproblema { get; set; }
		private string CNPJ { get; set; }

		private readonly IUnitOfWorkSciex _uowSciex;
		private readonly Validation _validation;
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;
		private readonly IUsuarioLogado _IUsuarioLogado;
		private readonly IViewImportadorBll _viewImportadorBll;
		private readonly IUsuarioPssBll _usuarioPssBll;
		public EstruturaPropriaPliArquivo(IUsuarioLogado usuarioLogado, IUnitOfWorkSciex uowSciex,
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
			listaPLI = String.Empty;
			quantidadePLI = 0;
			cnpjImportador = string.Empty;
			razaoSocial = string.Empty;
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

		public int Salvar(EstruturaPropriaPLIArquivoVM estruturapropriaarquivo)
		{
			if (estruturapropriaarquivo == null)
			{
				return 0;
			}

			EstruturaPropriaPliEntity objEstruturaPropria = new EstruturaPropriaPliEntity();

			var horadata = DateTime.Now;
			objEstruturaPropria.DataEnvio = horadata.AddHours(-1);
			objEstruturaPropria.InscricaoCadastral = Convert.ToInt32(estruturapropriaarquivo.NomeArquivo.Substring(0, 9));
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
			objEstruturaPropria.VersaoEstrutura = estruturapropriaarquivo.VersaoEstrutura;
			objEstruturaPropria.StatusProcessamentoArquivo = (int)EnumEstruturaPropriaArquivoProcessamento.ENVIADO_A_SUFRAMA;
			objEstruturaPropria.CNPJImportador = cnpjImportador;
			objEstruturaPropria.RazaoSocialImportador = razaoSocial;
			objEstruturaPropria.QuantidadePLIArquivo = quantidadePLI;
			objEstruturaPropria.LoginUsuarioEnvio = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoCpfCnpj.Replace(".", "").Replace("-", "").Replace("/", ""); ;
			objEstruturaPropria.NomeUsuarioEnvio = _usuarioPssBll.ObterUsuarioLogado().usuarioLogadoNome.Replace(".", "").Replace("-", "").Replace("/", ""); ;
			objEstruturaPropria.StatusPLITecnologiaAssistiva = (estruturapropriaarquivo.TecnologiaAssistida ? Convert.ToByte(1) : Convert.ToByte(0));
			objEstruturaPropria.QuantidadePLIProcessadoFalha = 0;
			objEstruturaPropria.QuantidadePLIProcessadoSucesso = 0;
			objEstruturaPropria.TipoArquivo = 1;

			objEstruturaPropria.EstruturaPropriaPliArquivo = new EstruturaPropriaPliArquivoEntity();
			objEstruturaPropria.EstruturaPropriaPliArquivo.Arquivo = estruturapropriaarquivo.Arquivo;

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
					if (item.Length > 1)
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
					if (item.Length > 1)
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

		/// <summary>
		/// Metodo para validar estrutura vertical
		/// </summary>
		/// <param name="arquivoLinhas"></param>
		/// <returns>Retorna verdadeiro se a validação da estrutura vertical está correta</returns>
		/// 
		//
		public bool ValidarEstrutura(string[] arquivoLinhas, string[] dados)
		{
			bool primeiraLinha = true;

			string tipoRegistroAnterior = "";
			string tipoRegistroAtual = "00";
			string tpDoc = null;
			int qtdLinhasRegistro6 = 0;
			int qtdLinhasRegistro7 = 0;

			int qtdLinhaImportadorNormal = 0;
			int qtdLinhaImportadorSubstutivo = 0;



			foreach (var item in arquivoLinhas)
			{
				if (item.Length > 1)
				{
					if (primeiraLinha)
					{
						tipoRegistroAtual = item.Substring(0, 2);
						if (tipoRegistroAtual != "01")
						{
							return false;
						}

						primeiraLinha = false;

						if (!ValidarHorizontalRegistro01(item, dados ,ref qtdLinhaImportadorNormal, ref qtdLinhaImportadorSubstutivo))
						{
							return false;
						}

						tpDoc = item.Substring(256, 1).ToString();
					}
					else
					{
						tipoRegistroAnterior = tipoRegistroAtual;
						tipoRegistroAtual = item.Substring(0, 2);

						

						if (!ValidarRegistroVertical(tipoRegistroAtual, tipoRegistroAnterior))
						{
							return false;
						}

						//validação horizontal
						switch (tipoRegistroAtual)
						{
							case "01":
								{
									qtdLinhasRegistro7 = 0;
									qtdLinhasRegistro6 = 0;
									itenspliproblema = false;
									if (!ValidarHorizontalRegistro01(item, dados, ref qtdLinhaImportadorNormal, ref qtdLinhaImportadorSubstutivo))
									{
										return false;
									}
									break;
								}
							case "09":
								{
									if(tpDoc == "2" && (
										tipoRegistroAnterior == "06" && tipoRegistroAtual == "09" ||
										tipoRegistroAnterior == "04" && tipoRegistroAtual == "09" ||
										tipoRegistroAnterior == "05" && tipoRegistroAtual == "09" ||
										tipoRegistroAnterior == "07" && tipoRegistroAtual == "09")
									  )
									{//RN23
										throw new ValidationException("Há PLI do tipo Substitutivo no arquivo com mais de 1 mercadoria, não é possível realizar envio de PLI.");
									}
									if (!ValidarHorizontalRegistro09(item))
									{
										return false;
									}
									break;
								}
							case "08":
								{
									qtdLinhasRegistro7 = 0;
									qtdLinhasRegistro6 = 0;
									itenspliproblema = false;
									if (!ValidarHorizontalRegistro08(item))
									{
										return false;
									}
									break;
								}
							case "10":
								{
									qtdLinhasRegistro7 = 0;
									qtdLinhasRegistro6 = 0;
									itenspliproblema = false;
									if (!ValidarHorizontalRegistro10(item))
									{
										return false;
									}
									break;
								}
							case "03":
								{
									qtdLinhasRegistro7 = 0;
									qtdLinhasRegistro6 = 0;
									itenspliproblema = false;
									if (!ValidarHorizontalRegistro03(item))
									{
										return false;
									}
									break;
								}
							case "06":
								{
									qtdLinhasRegistro6 = qtdLinhasRegistro6 + 1;
									if (qtdLinhasRegistro6 > 15)
									{
										itenspliproblema = true;
										return false;
									}

									if (!ValidarHorizontalRegistro06(item))
									{
										return false;
									}
									break;
								}
							case "04":
								{
									itenspliproblema = false;
									qtdLinhasRegistro7 = 0;
									qtdLinhasRegistro6 = 0;
									if (!ValidarHorizontalRegistro04(item))
									{
										return false;
									}
									break;
								}
							case "05":
								{
									itenspliproblema = false;
									qtdLinhasRegistro7 = 0;
									qtdLinhasRegistro6 = 0;
									if (!ValidarHorizontalRegistro05(item))
									{
										return false;
									}
									break;
								}
							case "07":
								{
									qtdLinhasRegistro7 = qtdLinhasRegistro7 + 1;
									if (qtdLinhasRegistro7 > 16)
									{
										itenspliproblema = true;
										return false;
									}
									qtdLinhasRegistro6 = 0;
									if (!ValidarHorizontalRegistro07(item))
									{
										return false;
									}
									break;
								}
						}
					}
				}
			}

			if(qtdLinhaImportadorNormal > 0 && qtdLinhaImportadorSubstutivo > 0)
			{//RN10
				throw new ValidationException("Serão aceitos somente PLI Normal ou Substitutivo. Foi identificado PLI com tipo de documento diferente dos aceitáveis, não é possível realizar envio de PLI.");
			}

			if (!estruturaCompleta)
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Validar a linha anterior com a atual se está na estrurura correta vertical
		/// </summary>
		/// <param name="registroAtual"></param>
		/// <param name="registroAnterior"></param>
		/// <returns>Retorna Verdadeiro se estiver correta a validação</returns>
		/// 
		private bool ValidarRegistroVertical(string registroAtual, string registroAnterior)
		{
			//somente para  a primeira linha lida do arquivo
			if (registroAtual == "01" && registroAnterior == "01")
			{
				return true;
			}

			switch (registroAnterior)
			{
				case "01":
				case "02":
					{
						estruturaCompleta = false;
						if (registroAtual == "09")
						{
							return true;
						}
						else
						{
							return false;
						}
					}
				case "09":
					{
						estruturaCompleta = false;
						if (registroAtual == "08")
						{
							return true;
						}
						else
						{
							return false;
						}
					}
				case "08":
					{
						estruturaCompleta = false;
						if (registroAtual == "10")
						{
							return true;
						}
						else
						{
							return false;
						}
					}
				case "10":
					{
						if (registroAtual == "03")
						{
							estruturaCompleta = true;
							return true;
						}
						else
						{
							return false;
						}
					}
				case "03":
					{
						if (registroAtual == "01" || registroAtual == "03" || registroAtual == "04" || registroAtual == "05" || registroAtual == "06" || registroAtual == "07" || registroAtual == "09")
						{
							if (registroAtual == "01" || registroAtual == "09")
								estruturaCompleta = false;
							return true;
						}
						else
						{
							return false;
						}
					}
				case "06":
					{
						if (registroAtual == "01" || registroAtual == "03" || registroAtual == "04" || registroAtual == "05" || registroAtual == "06" || registroAtual == "07" || registroAtual == "09")
						{
							if (registroAtual == "09")
								estruturaCompleta = false;

							if (registroAtual == "03")
								estruturaCompleta = true;

							return true;
						}
						else
						{
							return false;
						}
					}
				case "04":
					{
						if (registroAtual == "01" || registroAtual == "05" || registroAtual == "07" || registroAtual == "09")
						{
							if (registroAtual == "01" || registroAtual == "09")
								estruturaCompleta = false;
							return true;
						}
						else
						{
							return false;
						}
					}
				case "05":
					{
						if (registroAtual == "01" || registroAtual == "07" || registroAtual == "09")
						{
							if (registroAtual == "01" || registroAtual == "09")
								estruturaCompleta = false;
							return true;
						}
						else
						{
							return false;
						}
					}
				case "07":
					{
						if (registroAtual == "07")
						{
							return true;
						}
						else
						if (registroAtual == "09" || registroAtual == "02" || registroAtual == "01")
						{
							estruturaCompleta = false;
							return true;
						}
						else
						{
							return false;
						}
					}
			}

			return true;
		}

		private bool ValidarHorizontalRegistro01(string linha, string[] dados ,ref int qtdLinhaImportadorNormal , ref int qtdLinhaImportadorSubstutivo)
		{
			//tamanho da linha
			if (linha.Length != 343)
			{
				return false;
			}
			//tipo de registro
			if (linha.Substring(0, 2) != "01")
			{
				return false;
			}

			//numero do PLI com a barra			
			if (linha.Substring(2, 10).IndexOf("/") < 0)
			{
				return false;
			}
			else
			{
				listaPLI = listaPLI + linha.Substring(2, 10) + "; ";
				quantidadePLI++;
			}

			//isncrição cadastral
			try
			{
				Convert.ToInt32(linha.Substring(12, 9));
			}
			catch
			{
				return false;
			}

			//codigo do tipo de importador
			try
			{
				Convert.ToInt32(linha.Substring(21, 1));
			}
			catch
			{
				return false;
			}

			//CNPJ Importador
			//if (linha.Substring(22, 14).Trim().Length != 14)
			//{
			//	return false;
			//}

			cnpjImportador = linha.Substring(22, 14);


			//codigo do pais do importador
			try
			{
				Convert.ToInt32(linha.Substring(36, 3));
			}
			catch
			{
				return false;
			}

			//Nome do importador
			//if (linha.Substring(39, 60).Trim().Length == 0)
			//{
			//	return false;
			//}

			razaoSocial = linha.Substring(39, 60);

			//numero do telefone do importador
			//linha.Substring(99, 15)

			//Endereço do importador
			//if (linha.Substring(114, 40).Trim().Length == 0)
			//{
			//	return false;
			//}

			//numero do endereco do importador
			//linha.Substring(154, 6)

			//complemento do endereco do importador
			//linha.Substring(160, 21)

			//bairro do endereco do importador
			//if (linha.Substring(181, 25).Trim().Length == 0)
			//{
			//	return false;
			//}

			//municipio do endereco do importador
			//if (linha.Substring(206, 25).Trim().Length == 0)
			//{
			//	return false;
			//}

			//uf do endereco do importador
			//if (linha.Substring(232, 2).Trim().Length == 0)
			//{
			//	return false;
			//}

			//cep do endereco do importador
			try
			{
				if (Convert.ToInt32(linha.Substring(233, 8).Trim()) == 0)
				{
					return false;
				}
			}
			catch
			{
				return false;
			}

			//codigo da atividade economica
			try
			{
				if (Convert.ToInt32(linha.Substring(241, 4).Trim()) == 0)
				{
					return false;
				}
			}
			catch
			{
				return false;
			}

			//CPF do representante legal
			try
			{
				if (linha.Substring(245, 11).Trim().Length == 0)
				{
					return false;
				}

				if (Convert.ToInt64(linha.Substring(245, 11).Trim()) == 0)
				{
					return false;
				}
			}
			catch
			{
				return false;
			}

			//codigo do tipo de documento
			if (linha.Substring(256, 1).Trim().Length == 0)
			{
				return false;
			}
			else
			{
				if (linha.Substring(256, 1) == "3")
				{
					return false;
				}
			}

			if ((linha.Substring(256, 1).Trim() != "1" &&
				(linha.Substring(256, 1).Trim() != "2") &&
				(linha.Substring(256, 1).Trim() != "3")))
			{
				return false;
			}

			var tpDoc = linha.Substring(256, 1).ToString();
			if (tpDoc == "1")
				qtdLinhaImportadorNormal++;
			else if (tpDoc == "2")
				qtdLinhaImportadorSubstutivo++;

			//numero do documento de referencia
			//linha.Substring(257, 8)

			//tipo de aplicação do PLI
			try
			{
				Convert.ToInt32(linha.Substring(265, 2).Trim());
			}
			catch
			{
				return false;
			}

			//numero do li de referencia
			if(tpDoc == "1") //NORMAL
			{
				var liRef = linha.Substring(267, 10).Trim();
				if (liRef == "" || liRef == "0000000000")
				{
					try
					{
						Convert.ToInt32(linha.Substring(267, 10).Trim());
					}
					catch
					{
						return true;
					}
				}//RN18
				else
					throw new ValidationException("PLI do tipo Normal não deve conter LI de referência.Há PLI no arquivo do tipo Normal e com informação de LI de referência, não é possível realizar envio de PLI.");
			}
			if (tpDoc == "2")//SUBSTITUTIVO
			{
				try
				{
					Convert.ToInt32(linha.Substring(267, 10).Trim());				
				}
				catch
				{//RN18
					throw new ValidationException("PLI do tipo Substitutivo deve conter obrigatoriamente LI de referência. Há PLI no arquivo do tipo Substitutivo sem informação de LI de referência, não é possível realizar envio de PLI.");
				}
				long liRef = Convert.ToInt64(linha.Substring(267, 10).TrimStart('0'));
				if (liRef.ToString() == "")
				{
					throw new ValidationException("PLI do tipo Substitutivo deve conter obrigatoriamente LI de referência. Há PLI no arquivo do tipo Substitutivo sem informação de LI de referência, não é possível realizar envio de PLI.");
				}

				//Verifica se Li de Referencia pertence ao importador passado no arquivo RN19
				var validaImp = _uowSciex.QueryStackSciex.ValidaLIReferenciaPertenceaImpotador(dados[0].ToString(), liRef.ToString());
				if (validaImp.Count == 0)
					throw new ValidationException("Há PLI no arquivo com LI de referência que não pertence ao Importador, não é possível realizar envio de PLI.");

				
				//Validar se LI de Referência está Apta a ser Substituída RN20
				var validaLi = _uowSciex.QueryStackSciex.Li.Selecionar(o => o.NumeroLi == liRef);
				if (validaLi.IdDI != null && validaLi.Status != 1)
					throw new ValidationException("Para ser substituída, a LI de referência deve está Deferida e sem DI.Há PLI no arquivo com LI de referência que não está apta a ser substituída, não é possível realizar envio de PLI.");

				//Validar se LI e referencia e ou deferida 
				var liParaImportador = _uowSciex.QueryStackSciex.VerificaLiIndeferidoCancelado(liRef.ToString());
				if (liParaImportador.Count > 0)
					throw new ValidationException("Há PLI no arquivo com LI de referência que já está sendo referenciada em outro PLI, não é possível realizar envio de PLI.");

				//valida se li ja foi substituida 3 vezes e lança RN22
				var liReferec = _uowSciex.QueryStackSciex.SelecionarIdOrigemLiReferencia(liRef.ToString());
				if (liReferec != null && liReferec.Count >= 3)
				{
					throw new ValidationException("Há PLI no arquivo com LI de referência que excedeu o limite máximo de 3 (três) substituições, não é possível realizar envio de PLI.");
				}

				int pliaplicacao = Convert.ToInt32(linha.Substring(265, 2).Trim());

				var liaplicacao = _uowSciex.QueryStackSciex.VerificaAplicacaoLideReferencia(liRef.ToString());

				if(pliaplicacao != liaplicacao)
				{
					throw new ValidationException("Há PLI Substitutivo com a Aplicação diferente da Aplicação da LI de Referência informada, não é possível realizar o envio de PLI");
				}
				
			}

			//numero do processo de exportacao
			try
			{
				Convert.ToInt32(linha.Substring(277, 8).Trim());
			}
			catch
			{
				return false;
			}

			//ano do processo de exportação
			try
			{
				Convert.ToInt32(linha.Substring(285, 4).Trim());
			}
			catch
			{
				return false;
			}

			//email
			//linha.Substring(289, 50)

			//versao
			//linha.Substring(339, 3)

			//inidicação de LI em exigência
			//linha.Substring(342, 1)

			return true;
		}

		private bool ValidarHorizontalRegistro09(string linha)
		{
			//tamanho da linha
			if (linha.Length != 135)
			{
				return false;
			}

			if (linha.Substring(0, 2) != "09")
			{
				return false;
			}

			//numero do PLI com a barra
			if (linha.Substring(2, 10).IndexOf("/") < 0)
			{
				return false;
			}

			//inscrição cadastral
			try
			{
				Convert.ToInt32(linha.Substring(12, 9));
			}
			catch
			{
				return false;
			}

			//Mercadoria NCM
			if (linha.Substring(21, 8).Trim().Length == 0)
			{
				return false;
			}

			//codigo do produto + tipo + modelo
			try
			{
				Convert.ToInt64(linha.Substring(29, 11));
			}
			catch
			{
				return false;
			}

			//Mercadoria NALADI-SH
			//linha.Substring(40, 8)

			//Peso Liquido da Mercadoria
			try
			{
				if (Convert.ToInt64(linha.Substring(48, 15)) == 0)
				{
					return false;
				}
			}
			catch
			{
				return false;
			}

			//Quantidade na medida estatistica
			try
			{
				if (Convert.ToInt64(linha.Substring(63, 14)) == 0)
				{
					return false;
				}
			}
			catch
			{
				return false;
			}

			//Código da Aplicação da mercadoria
			//linha.Substring(77, 1)

			//codigo da moeda negociada
			try
			{
				if (Convert.ToInt32(linha.Substring(78, 3)) == 0)
				{
					return false;
				}
			}
			catch
			{
				return false;
			}

			//codigo INCOTERMS
			//if (linha.Substring(81, 3).Trim().Length == 0)
			//{
			//	return false;
			//}

			//codigo da moeda negociada
			try
			{
				if (Convert.ToInt64(linha.Substring(84, 15)) == 0)
				{
					return false;
				}
			}
			catch
			{
				return false;
			}

			//Indicador de material usado
			//linha.Substring(99, 1)

			//indicador de bem fabricado sob encomenda
			//linha.Substring(100, 1)

			//numero do comunicado de compra
			//linha.Substring(101, 13)

			//RFB de entrada da mercadoria
			try
			{
				if (Convert.ToInt64(linha.Substring(114, 7)) == 0)
				{
					return false;
				}
			}
			catch
			{
				return false;
			}

			//Pais de procedencia da mercadoria
			try
			{
				if (Convert.ToInt64(linha.Substring(121, 3)) == 0)
				{
					return false;
				}
			}
			catch
			{
				return false;
			}

			//RFB de despacho
			try
			{
				if (Convert.ToInt64(linha.Substring(124, 7)) == 0)
				{
					return false;
				}
			}
			catch
			{
				return false;
			}

			//Aliquota sobre o II
			try
			{
				Convert.ToDecimal(linha.Substring(131, 2) + "." + linha.Substring(133, 2));
			}
			catch
			{
				return false;
			}
			//linha.Substring(131, 4)

			return true;
		}

		private bool ValidarHorizontalRegistro08(string linha)
		{
			//tamanho da linha
			if (linha.Length != 401)
			{
				return false;
			}

			if (linha.Substring(0, 2) != "08")
			{
				return false;
			}

			//numero do PLI com a barra
			if (linha.Substring(2, 10).IndexOf("/") < 0)
			{
				return false;
			}

			//inscrição cadastral
			try
			{
				Convert.ToInt32(linha.Substring(12, 9));
			}
			catch
			{
				return false;
			}

			//Mercadoria NCM
			if (linha.Substring(21, 8).Trim().Length == 0)
			{
				return false;
			}

			//codigo do produto + tipo + modelo
			try
			{
				Convert.ToInt64(linha.Substring(29, 11));
			}
			catch
			{
				return false;
			}

			//Nome do fornecedor estrangeiro
			//if (linha.Substring(40, 60).Trim().Length == 0)
			//{
			//	return false;
			//}

			//endereco do fornecedor estrangeiro
			//if (linha.Substring(100, 40).Trim().Length == 0)
			//{
			//	return false;
			//}

			//numero endereco do fornecedor estrangeiro
			//linha.Substring(140, 6)

			//complemento endereco do fornecedor estrangeiro
			//linha.Substring(146, 21)

			//estado endereco do fornecedor estrangeiro
			//linha.Substring(167, 25)

			//cidade endereco do fornecedor estrangeiro
			//linha.Substring(192, 25)

			//país endereco do fornecedor estrangeiro
			try
			{
				Convert.ToInt64(linha.Substring(217, 3));
			}
			catch
			{
				return false;
			}

			//tipo de fornecedor
			try
			{
				Convert.ToInt16(linha.Substring(220, 1));
			}
			catch
			{
				return false;
			}

			//nome do fabricante
			//linha.Substring(221, 60)

			//endereco do fabricante estrangeiro
			//if (linha.Substring(281, 40).Trim().Length == 0)
			//{
			//	return false;
			//}

			//numero endereco do fabricante estrangeiro
			//linha.Substring(321, 6)

			//complemento endereco do fabricante estrangeiro
			//linha.Substring(327, 21)

			//estado endereco do fabricante estrangeiro
			//linha.Substring(348, 25)

			//cidade endereco do fabricante estrangeiro
			//linha.Substring(373, 25)

			//país endereco do fabricante estrangeiro
			//try
			//{
			//	Convert.ToInt64(linha.Substring(398, 3));
			//}
			//catch
			//{
			//	return false;
			//}

			return true;
		}

		private bool ValidarHorizontalRegistro10(string linha)
		{
			//tamanho da linha
			if (linha.Length != 75)
			{
				return false;
			}

			if (linha.Substring(0, 2) != "10")
			{
				return false;
			}

			//numero do PLI com a barra
			if (linha.Substring(2, 10).IndexOf("/") < 0)
			{
				return false;
			}

			//inscrição cadastral
			try
			{
				Convert.ToInt32(linha.Substring(12, 9));
			}
			catch
			{
				return false;
			}

			//Mercadoria NCM
			//if (linha.Substring(21, 8).Trim().Length == 0)
			//{
			//	return false;
			//}

			//codigo do produto + tipo + modelo
			try
			{
				Convert.ToInt64(linha.Substring(29, 11));
			}
			catch
			{
				return false;
			}

			//tipo de acordo tarifario
			try
			{
				Convert.ToInt16(linha.Substring(40, 1));
			}
			catch
			{
				return false;
			}

			//codigo acordo ALADI
			//try
			//{
			//	Convert.ToInt16(linha.Substring(41, 3));
			//}
			//catch
			//{
			//	return false;
			//}

			//codigo regime tributário
			try
			{
				Convert.ToInt16(linha.Substring(44, 1));
			}
			catch
			{
				return false;
			}

			//codigo fundamento legal
			try
			{
				Convert.ToInt16(linha.Substring(45, 2));
			}
			catch
			{
				return false;
			}

			//codigo cobertura cambial
			try
			{
				Convert.ToInt16(linha.Substring(47, 1));
			}
			catch
			{
				return false;
			}

			//codigo modalidade de pagamento
			try
			{
				Convert.ToInt16(linha.Substring(49, 2));
			}
			catch
			{
				return false;
			}

			//quantidade de dias limite de pagamento
			try
			{
				Convert.ToInt16(linha.Substring(50, 3));
			}
			catch
			{
				return false;
			}

			//codigo instituição financeira
			try
			{
				Convert.ToInt16(linha.Substring(53, 2));
			}
			catch
			{
				return false;
			}

			//codigo motivo importação sem cobertura
			try
			{
				Convert.ToInt16(linha.Substring(55, 2));
			}
			catch
			{
				return false;
			}

			//codigo agencia SECEX
			//linha.Substring(57, 5)

			//numero do ato drawback
			//linha.Substring(62, 13)

			return true;
		}

		private bool ValidarHorizontalRegistro03(string linha)
		{
			//tamanho da linha
			if (linha.Length != 410)
			{
				return false;
			}
			//tipo de registro
			if (linha.Substring(0, 2) != "03")
			{
				return false;
			}

			//numero do PLI com a barra
			if (linha.Substring(2, 10).IndexOf("/") < 0)
			{
				return false;
			}

			//isncrição cadastral
			try
			{
				Convert.ToInt32(linha.Substring(12, 9));
			}
			catch
			{
				return false;
			}

			//mercadoria NCM
			//linha.Substring(21, 8);

			//codigo detalhe
			try
			{
				Convert.ToInt32(linha.Substring(29, 4));
			}
			catch
			{
				return false;
			}

			//codigo do produto + tipo + modelo
			try
			{
				Convert.ToInt64(linha.Substring(33, 11));
			}
			catch
			{
				return false;
			}

			//quantidade da mercadoria na unidade comercializada
			try
			{
				Convert.ToInt64(linha.Substring(44, 14));
			}
			catch
			{
				return false;
			}

			//unidade de medida comercializada
			//linha.Substring(21, 8);

			//valor da unidade na condição de venda
			try
			{
				Convert.ToInt64(linha.Substring(78, 18));
			}
			catch
			{
				return false;
			}

			//descricao de detalhes da mercadoria
			//linha.Substring(96, 254);

			//referencia fabricante
			//linha.Substring(350, 20);

			//part number
			//linha.Substring(370, 20);

			//matéria prima básica
			//linha.Substring(390, 20);
			return true;
		}

		private bool ValidarHorizontalRegistro06(string linha)
		{
			//tamanho da linha
			if (linha.Length != 300)
			{
				return false;
			}
			//tipo de registro
			if (linha.Substring(0, 2) != "06")
			{
				return false;
			}

			//numero do PLI com a barra
			if (linha.Substring(2, 10).IndexOf("/") < 0)
			{
				return false;
			}

			//isncrição cadastral
			try
			{
				Convert.ToInt32(linha.Substring(12, 9));
			}
			catch
			{
				return false;
			}

			//mercadoria NCM
			//linha.Substring(21, 8);

			//codigo detalhe
			try
			{
				Convert.ToInt32(linha.Substring(29, 4));
			}
			catch
			{
				return false;
			}

			//codigo do produto + tipo + modelo
			try
			{
				Convert.ToInt64(linha.Substring(33, 11));
			}
			catch
			{
				return false;
			}

			//sequencial
			//linha.Substring(44, 3);

			//texto da descricao
			//linha.Substring(47, 253);

			return true;
		}

		private bool ValidarHorizontalRegistro04(string linha)
		{
			//tamanho da linha
			if (linha.Length != 70)
			{
				return false;
			}
			//tipo de registro
			if (linha.Substring(0, 2) != "04")
			{
				return false;
			}

			//numero do PLI com a barra
			if (linha.Substring(2, 10).IndexOf("/") < 0)
			{
				return false;
			}

			//isncrição cadastral
			try
			{
				Convert.ToInt32(linha.Substring(12, 9));
			}
			catch
			{
				return false;
			}

			//mercadoria NCM
			//linha.Substring(21, 8);

			//numero do processo anuente
			//linha.Substring(29, 20);

			//sigla do orgao do processo anuente
			//linha.Substring(49, 10);

			//codigo do produto + tipo + modelo
			try
			{
				Convert.ToInt64(linha.Substring(59, 11));
			}
			catch
			{
				return false;
			}

			return true;
		}

		private bool ValidarHorizontalRegistro05(string linha)
		{
			//tamanho da linha
			if (linha.Length != 43)
			{
				return false;
			}
			//tipo de registro
			if (linha.Substring(0, 2) != "05")
			{
				return false;
			}

			//numero do PLI com a barra
			if (linha.Substring(2, 10).IndexOf("/") < 0)
			{
				return false;
			}

			//isncrição cadastral
			try
			{
				Convert.ToInt32(linha.Substring(12, 9));
			}
			catch
			{
				return false;
			}

			//mercadoria NCM
			//linha.Substring(21, 8);

			// numero destaque da ncm
			try
			{
				Convert.ToInt16(linha.Substring(29, 3));
			}
			catch
			{
				return false;
			}

			//codigo do produto + tipo + modelo
			try
			{
				Convert.ToInt64(linha.Substring(32, 11));
			}
			catch
			{
				return false;
			}

			return true;
		}

		private bool ValidarHorizontalRegistro07(string linha)
		{
			//tamanho da linha
			if (linha.Length != 296)
			{
				return false;
			}
			//tipo de registro
			if (linha.Substring(0, 2) != "07")
			{
				return false;
			}

			//numero do PLI com a barra
			if (linha.Substring(2, 10).IndexOf("/") < 0)
			{
				return false;
			}

			//isncrição cadastral
			try
			{
				Convert.ToInt32(linha.Substring(12, 9));
			}
			catch
			{
				return false;
			}

			//mercadoria NCM
			//linha.Substring(21, 8);

			//codigo do produto + tipo + modelo
			try
			{
				Convert.ToInt64(linha.Substring(29, 11));
			}
			catch
			{
				return false;
			}

			//numero sequencial do produto
			//try
			//{
			//	Convert.ToInt64(linha.Substring(40, 3));
			//}
			//catch
			//{
			//	return false;
			//}

			//informação complementar
			//linha.Substring(43, 253);

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
					if (item.Length > 1)
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
					if (item.Length > 1)
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
					if (item.Length > 1)
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
					if (item.Length > 1)
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
					if (item.Length > 1)
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

		public string ValidarArquivo(EstruturaPropriaPLIArquivoVM estrutura)
		{
			try
			{
				string[] dados = estrutura.NomeArquivo.Split('_');
				string CNPJArquivo = string.Empty;

				if (estrutura.NomeArquivo.Length != 37)
				{
					return Mensagens.ESTRUTURA_PROPRIA_FORA_PADRAO;
				}

				if (dados.Length != 4)
				{
					return Mensagens.ESTRUTURA_PROPRIA_FORA_PADRAO;
				}

				if (dados[1].ToUpper() != "OWN")
				{
					return Mensagens.ESTRUTURA_PROPRIA_FORA_PADRAO;
				}

				if (dados[3].IndexOf(".") > 6)
				{
					return Mensagens.ESTRUTURA_PROPRIA_FORA_PADRAO;
				}

				if (!dados[3].ToUpper().Trim().Substring(dados[3].IndexOf("."), dados[3].Trim().Length - dados[3].IndexOf(".")).Equals(".PL5ZIP"))
				{
					return Mensagens.ESTRUTURA_PROPRIA_FORA_PADRAO;
				}

				if (!dados[3].ToUpper().Contains(".PL5ZIP"))
				{
					return Mensagens.ESTRUTURA_PROPRIA_FORA_PADRAO;
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
						return Mensagens.ESTRUTURA_PROPRIA_EMPRESA_REPRESENTADA;
					}
				}

				if (_usuarioInformacoesBll.ObterCNPJ().CnpjCpfUnformat().Length == 11)
				{
					//validar se tem acesso a empresa representada			
					if (!ValidarEmpresasRepresentadas(CNPJArquivo))
					{
						return Mensagens.ESTRUTURA_PROPRIA_EMPRESA_REPRESENTADA;
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
							return Mensagens.ESTRUTURA_PROPRIA_INSCRICAO_BLOQUEADA;
						}
					}
					else
					{
						return Mensagens.ESTRUTURA_PROPRIA_INSCRICAO_BLOQUEADA;
					}
				}
				catch
				{
					return Mensagens.ESTRUTURA_PROPRIA_FORA_PADRAO;
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
					string arquivoAtual = string.Empty;
					string nomeArquivo = estrutura.NomeArquivo.Substring(0, estrutura.NomeArquivo.IndexOf("."));
					string extensao = estrutura.NomeArquivo.Split('.')[1];
					arquivos = Directory.GetFiles(local);

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
						Directory.Delete(local);
						return Mensagens.ESTRUTURA_PROPRIA_ESTRUTURA_NENHUM_ARQUIVO_ENCONTRADO;
					}
					else if (arquivos.Length > 1)
					{
						foreach (string item in arquivos)
						{
							File.Delete(item);
						}
						Directory.Delete(local);
						return Mensagens.ESTRUTURA_PROPRIA_ARQUIVOS;
					}
					else
					{
						//validar nomes dos arquivos
						if (Path.GetFileName(arquivoAtual.ToUpper()) != estrutura.NomeArquivo.ToUpper())
						{
							foreach (string item in arquivos)
							{
								File.Delete(item);
							}
							Directory.Delete(local);
							return Mensagens.ESTRUTURA_PROPRIA_NOME_ARQUIVOS_DIFERENTE;
						}

						//valida o nome do arquivo zipado
						if (!ValidarDataHoraArquivo(Path.GetFileName(estrutura.NomeArquivo)))
						{
							foreach (string item in arquivos)
							{
								File.Delete(item);
							}
							Directory.Delete(local);
							return Mensagens.ESTRUTURA_PROPRIA_FORA_PADRAO;
						}

						//valida inscrição cadastral dos arquivos
						String inscricaoArquivo = Path.GetFileName(arquivoAtual).Substring(0, 9);
						String inscricaoZip = estrutura.NomeArquivo.Substring(0, 9);

						if (inscricaoArquivo != inscricaoZip)
						{
							foreach (string item in arquivos)
							{
								File.Delete(item);
							}
							Directory.Delete(local);
							return Mensagens.ESTRUTURA_PROPRIA_CNPJ_DIFERENTE;
						}

						//valida o nome do arquivo dentro do arquivo zipado
						if (!ValidarDataHoraArquivo(Path.GetFileName(arquivoAtual)))
						{
							foreach (string item in arquivos)
							{
								File.Delete(item);
							}
							Directory.Delete(local);
							return Mensagens.ESTRUTURA_PROPRIA_FORA_PADRAO;
						}

						string[] linhas = File.ReadAllLines(arquivoAtual);

						if (linhas[0].Length > 0)
						{
							if (linhas[0].Substring(0, 2) == "0\0")
							{
								linhas = File.ReadAllLines(arquivoAtual, Encoding.Unicode);
								File.Delete(arquivoAtual);
								File.WriteAllLines(arquivoAtual, linhas);
							}
							else
							{
								linhas = File.ReadAllLines(arquivoAtual, Encoding.UTF8);
							}
						}

						objVI = _viewImportadorBll.SelecionarInscricao(Convert.ToInt32(dados[0]));
						if (!ValidarCnpjArquivo(objVI.Cnpj, linhas))
						{
							return Mensagens.ESTRUTURA_PROPRIA_CNPJ_DIFERENTE;
						}

						if (!ValidarInscricaoArquivo(objVI.InscricaoCadastral, linhas))
						{
							return Mensagens.ESTRUTURA_PROPRIA_CNPJ_DIFERENTE;
						}

						//validar ano do PLI
						if (!ValidarAnoPLI(linhas))
						{
							foreach (string item in arquivos)
							{
								File.Delete(item);
							}
							Directory.Delete(local);
							return Mensagens.ESTRUTURA_PROPRIA_ANO_CORRENTE_INVALIDO;
						}

						//validar formatação do PLI
						if (!ValidarFormatacaoPLI(linhas))
						{
							foreach (string item in arquivos)
							{
								File.Delete(item);
							}
							Directory.Delete(local);
							return Mensagens.ESTRUTURA_PROPRIA_FORMATACAO_PLI;
						}

						if (linhas.Any())//pegar a versão do arquivo de estrutura
						{
							try
							{
								estrutura.VersaoEstrutura = linhas[0].Substring(339, 3);
							}
							catch { }
						}

						if (!ValidarPLIsEmpresa(linhas))
						{
							foreach (string item in arquivos)
							{
								File.Delete(item);
							}
							Directory.Delete(local);
							return Mensagens.ESTRUTURA_PROPRIA_CNPJ_DIFERENTE;
						}

						//validar o tipo de pli
						if (!ValidarTipoPLi(linhas))
						{
							foreach (string item in arquivos)
							{
								File.Delete(item);
							}
							Directory.Delete(local);
							return Mensagens.ESTRUTURA_PROPRIA_TIPO_DOCUMENTO;
						}

						if (!ValidarCodigoAplicacao(linhas))
						{
							foreach (string item in arquivos)
							{
								File.Delete(item);
							}
							Directory.Delete(local);
							return Mensagens.ESTRUTURA_PROPRIA_CODIGO_APLICACAO_INVALIDO;
						}

						if (!ValidarEstrutura(linhas, dados))
						{
							if (ValidarItensPli())
							{
								foreach (string item in arquivos)
								{
									File.Delete(item);
								}
								Directory.Delete(local);
								return Mensagens.ESTRUTURA_PROPRIA_ESTRUTURA_ERRADA_ITENS;
							}

							foreach (string item in arquivos)
							{
								if (item.Contains(estrutura.NomeArquivo.Split('.')[0] +"."+estrutura.NomeArquivo.Split('.')[1].ToUpper()) || item.Contains(estrutura.NomeArquivo.Split('.')[0] + "." + estrutura.NomeArquivo.Split('.')[1].ToLower()))
								{
									File.Delete(item);
								}
							}
							Directory.Delete(local);
							return Mensagens.ESTRUTURA_PROPRIA_ESTRUTURA_ERRADA;
						}
						else
						{
							try
							{
								estrutura.VersaoEstrutura = linhas[0].Substring(339, 3);
							}
							catch { }
							estrutura.Arquivo = File.ReadAllBytes(arquivoAtual);
							string retorno = Salvar(estrutura).ToString();

							File.Delete(arquivoAtual);
							Directory.Delete(local);
							return retorno;
						}
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
						return Mensagens.ESTRUTURA_PROPRIA_COMPACTACAO;
					}

				}
			}
			catch (Exception ex)
			{
				return ex.Message.ToString();
			}
		}
	}
}