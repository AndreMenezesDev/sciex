using System;
using Newtonsoft.Json;
using Suframa.Sciex.CrossCutting.Configuration;
using Suframa.Sciex.DataAccess.RestService.ApiDto;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;

namespace Suframa.Sciex.DataAccess.RestService
{
	public class CadastroApi
	{
		private string _endPoint;
		private string _endPointInscsuf;
		private string _endPointPf;
		private string _endPointPj;
		private string _endPointBloqueio;

		public CadastroApi()
		{
			var url = CrossCutting.Web.UriHelper.Slashfy(PrivateSettings.URL_CADASTRO);

			_endPointPj = PrivateSettings.URL_CADASTRO + "usuario/dados/pj/";
			_endPointPf = PrivateSettings.URL_CADASTRO + "usuario/dados/pf/";
			_endPointInscsuf = PrivateSettings.URL_CADASTRO + "usuario/dados/inscsuf/";
			_endPointBloqueio = PrivateSettings.URL_INTEGRACAO_LEGADO + "cadastroempresa/enviarbloqDesbloqLegado/";
		}

		public InscricaoSuframaApiDto ObterInscricaoSuframa(string inscsuf)
		{
			var client = new RestClientApi(_endPointInscsuf, HttpVerb.GET);
			InscricaoSuframaApiDto inscricaoSuframa = new InscricaoSuframaApiDto();
			try
			{
				var respostaOrigem = client.MakeRequest(inscsuf);
				var resposta = JsonConvert.DeserializeObject<InscricaoSuframaApiDto>(respostaOrigem);
				inscricaoSuframa.cnpj = resposta.cnpj;
				inscricaoSuframa.nome = resposta.nome;
				inscricaoSuframa.inscsuf = resposta.inscsuf;
				//de para do legado para o cadsuf
				inscricaoSuframa.situacao = resposta.situacao == 2 ? 1 : 2;
				inscricaoSuframa.validade = resposta.validade;
				inscricaoSuframa.setor = resposta.setor;
				inscricaoSuframa.localidade = resposta.localidade;
			}
			catch (ApplicationException applicationException)
			{
				Console.WriteLine("{0} Exception caught.", applicationException);
				return null;
			}
			return inscricaoSuframa;
		}

		public PessoaFisicaApiDto ObterPF(string cpf)
		{
			var client = new RestClientApi(_endPointPf, HttpVerb.GET);
			var respostaOrigem = client.MakeRequest(cpf);

			var resposta = JsonConvert.DeserializeObject<PessoaFisicaApiDto>(respostaOrigem);
			PessoaFisicaApiDto pessoaFisicaApiDto = new PessoaFisicaApiDto();
			if (resposta != null)
			{
				pessoaFisicaApiDto = new PessoaFisicaApiDto()
				{
					pfCpf = resposta.pfCpf,
					pfNome = resposta.pfNome,
					pfLogradouro = resposta.pfLogradouro,
					pfNumero = resposta.pfNumero,
					pfBairro = resposta.pfBairro,
					pfCep = resposta.pfCep,
					pfComplemento = resposta.pfComplemento,
					ufSigla = resposta.ufSigla,
					munNome = resposta.munNome
				};
			}
			return pessoaFisicaApiDto;
		}

		public PessoaJuridicaApiDto ObterPJ(string cnpj)
		{
			var client = new RestClientApi(_endPointPj, HttpVerb.GET);
			var respostaOrigem = client.MakeRequest(cnpj);

			var resposta = JsonConvert.DeserializeObject<PessoaJuridicaApiDto>(respostaOrigem);

			PessoaJuridicaApiDto pessoaJuridicaApiDto = new PessoaJuridicaApiDto();

			if (resposta != null)
			{
				pessoaJuridicaApiDto.empCnpj = resposta.empCnpj;
				pessoaJuridicaApiDto.empRazaoSocial = resposta.empRazaoSocial;
				pessoaJuridicaApiDto.empNomeFantasia = resposta.empNomeFantasia;
				pessoaJuridicaApiDto.empLogradouro = resposta.empLogradouro;
				pessoaJuridicaApiDto.empNumero = resposta.empNumero;
				pessoaJuridicaApiDto.empBairro = resposta.empBairro;
				pessoaJuridicaApiDto.empCep = resposta.empCep;
				pessoaJuridicaApiDto.empComplemento = resposta.empComplemento;
				pessoaJuridicaApiDto.ufSigla = resposta.ufSigla;
				pessoaJuridicaApiDto.munNome = resposta.munNome;
			}
			else
			{
				pessoaJuridicaApiDto.empCnpj = cnpj;
			}

			return pessoaJuridicaApiDto;
		}

		
		public BloqueioDesbloqueioRetornoVM EnviarBloqueioDesbloqueio(ListaDto<BloqueioDesbloqueioVM> lista)
		{
			var client = new RestClientApi(_endPointBloqueio, HttpVerb.POST);
			var respostaOrigem = client.MakePostRequest(lista);

			var resposta = JsonConvert.DeserializeObject<BloqueioDesbloqueioRetornoVM>(respostaOrigem);
			BloqueioDesbloqueioRetornoVM bloq = new BloqueioDesbloqueioRetornoVM();
			if (resposta != null)
			{
				bloq = new BloqueioDesbloqueioRetornoVM()
				{
					id = resposta.id,
					erro = resposta.erro,
					tpOperacao = resposta.tpOperacao
				};
				return bloq;
			}
			return null;
		}
	}
}