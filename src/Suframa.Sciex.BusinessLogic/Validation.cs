using FluentValidation;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Dto;

namespace Suframa.Sciex.BusinessLogic
{
	public class Validation : IValidation
	{
		public IValidator<ArquivoDto> _arquivoSalvarValidation { get; }

		public IValidator<CaptchaDto> _captchaValidation { get; }

		public IValidator<CepDto> _cepAdicionarValidation { get; }

		public IValidator<CepDto> _cepSelecionarValidation { get; }

		public IValidator<PessoaJuridicaDto> _credenciamentoPessoaJuridicaAdicionarValidation { get; }

		public IValidator<DiligenciaDto> _diligenciaAlterarValidation { get; }

		public IValidator<PessoaJuridicaDto> _filtroCadastroSelecionarValidation { get; }

		public IValidator<PessoaFisicaDto> _identificacaoPessoaFisicaAdicionarValidation { get; }

		public IValidator<PessoaFisicaDto> _identificacaoPessoaFisicaProtocoloValidation { get; }

		public IValidator<PessoaFisicaDto> _identificacaoPessoaFisicaSelecionarValidation { get; }

		public IValidator<PessoaJuridicaDto> _identificacaoPessoaJuridicaSelecionarValidation { get; }

		public IValidator<CancelamentoDto> _inscricaoCadastralSelecionarBloqueadosValidation { get; }

		public IValidator<CancelamentoDto> _inscricaoCadastralSelecionarValidation { get; }

		public IValidator<ManterNaturezaJuridicaDto> _manterNaturezaJuridicaSalvarValidation { get; }

		public IValidator<NaturezaJuridicaDto> _naturezaJuridicaAdicionarValidation { get; }

		public IValidator<NaturezaJuridicaDto> _naturezaJuridicaApagarValidation { get; }

		public IValidator<PessoaJuridicaDto> _pessoaJuridicaSalvarValidation { get; }

		public IValidator<ProtocoloDto> _protocoloAdicionarValidation { get; }

		public IValidator<ProtocoloDto> _protocoloConsultarExistenteValidation { get; }

		public IValidator<ProtocoloDto> _protocoloConsultarTipoRequerimentoPermitidoValidation { get; }

		public IValidator<ManterSetorEmpresarialDto> _setorEmpresarialAdicionarValidation { get; }

		public IValidator<ManterSetorEmpresarialDto> _setorEmpresarialListarValidation { get; }

		public IValidator<SubClasseAtividadeDto> _subClasseAtividadeListarValidation { get; }

		public IValidator<SubClasseAtividadeDto> _subClasseAtividadeSalvarValidation { get; }

		public IValidator<ManterUnidadeCadastradoraDto> _unidadeCadastradoraAdicionarValidation { get; }

		public IValidator<ManterUnidadeCadastradoraDto> _unidadeCadastradoraDeletarValidation { get; }

		public IValidator<ManterUnidadeCadastradoraDto> _unidadeCadastradoraListarValidation { get; }

		// SCIEX
		
		//Aladi
		public IValidator<AladiDto> _aladiExisteRelacionamentoValidation { get; }
		public IValidator<AladiDto> _aladiExcluirValidation { get; }

		//NaladiDeletarValidation
		public IValidator<NaladiDto> _naladiDeletarValidation { get; }
		public IValidator<NaladiDto> _naladiExisteRelacionamentoValidation { get; }
		public IValidator<NaladiDto> _naladiExcluirValidation { get; }

		//Regime tributário Validation
		public IValidator<RegimeTributarioDto> _regimeTributarioDeletarValidation { get; }
		public IValidator<RegimeTributarioDto> _regimeTributarioExisteRelacionamentoValidation { get; }
		public IValidator<RegimeTributarioDto> _regimeTributarioExcluirValidation { get; }
		
		//Unidade Receita Federal
		public IValidator<UnidadeReceitaFederalDto> _unidadeReceitaFederalDeletarValidation { get; }
		public IValidator<UnidadeReceitaFederalDto> _unidadeReceitaFederalExisteRelacionamentoValidation { get; }
		public IValidator<UnidadeReceitaFederalDto> _unidadeReceitaFederalExcluirValidation { get; }

		//Fundamento Legal
		public IValidator<FundamentoLegalDto> _fundamentoLegalDeletarValidation { get; }
		public IValidator<FundamentoLegalDto> _fundamentoLegalExisteRelacionamentoValidation { get; }
		public IValidator<FundamentoLegalDto> _fundamentoLegalExcluirValidation { get; }

		//Fabricante
		public IValidator<FabricanteDto> _fabricanteDeletarValidation { get; }
		public IValidator<FabricanteDto> _fabricanteExisteRelacionamentoValidation { get; }
		public IValidator<FabricanteDto> _fabricanteExcluirValidation { get; }

		//Fornecedor
		public IValidator<FornecedorDto> _fornecedorDeletarValidation { get; }
		public IValidator<FornecedorDto> _fornecedorExisteRelacionamentoValidation { get; }
		public IValidator<FornecedorDto> _fornecedorExcluirValidation { get; }

		//parametros
		public IValidator<ParametrosDto> _parametroExisteRelacionamentoValidation { get; }
		public IValidator<ParametrosDto> _parametroExcluirValidation { get; }

		//pli
		public IValidator<PliDto> _pliExisteRelacionamentoValidation { get; }
		public IValidator<PliDto> _pliExcluirValidation { get; }

		//pli produto
		public IValidator<PliProdutoDto> _pliProdutoExcluirValidation { get; }

		//pliAplicacao
		public IValidator<PliAplicacaoDto> _pliAplicacaoExisteRelacionamentoValidation { get; }
		public IValidator<PliAplicacaoDto> _pliAplicacaoExcluirValidation { get; }

		//pliDetalheMercadoria
		public IValidator<PliDetalheMercadoriaDto> _pliDetalheMercadoriaExisteRelacionamentoValidation { get; }
		public IValidator<PliDetalheMercadoriaDto> _pliDetalheMercadoriaExcluirValidation { get; }

		//pliHistorico
		public IValidator<PliHistoricoDto> _pliHistoricoExisteRelacionamentoValidation { get; }
		public IValidator<PliHistoricoDto> _pliHistoricoExcluirValidation { get; }

		//pliMercadoria
		public IValidator<PliMercadoriaDto> _pliMercadoriaExisteRelacionamentoValidation { get; }
		public IValidator<PliMercadoriaDto> _pliMercadoriaExcluirValidation { get; }

		//pliprocessoAnuente
		public IValidator<PliProcessoAnuenteDto> _pliProcessoAnuenteExisteRelacionamentoValidation { get; }
		public IValidator<PliProcessoAnuenteDto> _pliProcessoAnuenteExcluirValidation { get; }

		//pliStatus
		public IValidator<PliStatusDto> _pliStatusExisteRelacionamentoValidation { get; }
		public IValidator<PliStatusDto> _pliStatusExcluirValidation { get; }

		//pliTipo
		public IValidator<PliTipoDto> _pliTipoExisteRelacionamentoValidation { get; }
		public IValidator<PliTipoDto> _pliTipoExcluirValidation { get; }

		//ncmAmazoniaOcidental
		public IValidator<NcmDto> _ncmExcluirValidation { get; }

		public Validation()
		{
			// Validation
			_arquivoSalvarValidation = new ArquivoSalvarValidation();
			_captchaValidation = new CaptchaValidation();

			//aladi
			_aladiExcluirValidation = new AladiExcluirValidation();
			_aladiExisteRelacionamentoValidation = new AladiExisteRelacionamentoValidation();

			//naladi
			_naladiExcluirValidation = new NaladiExcluirValidation();
			_naladiExisteRelacionamentoValidation = new NaladiExisteRelacionamentoValidation();
			_naladiDeletarValidation = new NaladiDeletarValidation();

			//urf
			_unidadeReceitaFederalExcluirValidation = new UnidadeReceitaFederalExcluirValidation();
			_unidadeReceitaFederalExisteRelacionamentoValidation = new UnidadeReceitaFederalExcluirRelacionamentoValidation();
			_unidadeReceitaFederalDeletarValidation = new UnidadeReceitaFederalDeletarValidation();

			//fundamento legal
			_fundamentoLegalDeletarValidation = new FundamentoLegalDeletarValidation();
			_fundamentoLegalExisteRelacionamentoValidation = new FundamentoLegalExisteRelacionamentoValidation();
			_fundamentoLegalExcluirValidation = new FundamentoLegalExcluirValidation();

			//fabricante
			_fabricanteDeletarValidation = new FabricanteDeletarValidation();
			_fabricanteExisteRelacionamentoValidation = new FabricanteExisteRelacionamentoValidation();
			_fabricanteExcluirValidation = new FabricanteExcluirValidation();

			//fornecedor
			_fornecedorDeletarValidation = new FornecedorDeletarValidation();
			_fornecedorExisteRelacionamentoValidation = new FornecedorExisteRelacionamentoValidation();
			_fornecedorExcluirValidation = new FornecedorExcluirValidation();

			//parametros
			_parametroExisteRelacionamentoValidation = new ParametrosExisteRelacionamentoValidation();
			_parametroExcluirValidation = new ParametrosExcluirValidation();

			//pli
			_pliExisteRelacionamentoValidation = new PliExisteRelacionamentoValidation();
			_pliExcluirValidation = new PliExcluirValidation();

			//pli produto
			_pliProdutoExcluirValidation = new PliProdutoExcluirValidation();

			//pliAplicacao
			_pliAplicacaoExisteRelacionamentoValidation = new PliAplicacaoExisteRelacionamentoValidation();
			_pliAplicacaoExcluirValidation = new PliAplicacaoExcluirValidation();

			//pliDetalheMercadoria
			_pliDetalheMercadoriaExisteRelacionamentoValidation = new PliDetalheMercadoriaExisteRelacionamentoValidation();
			_pliDetalheMercadoriaExcluirValidation = new PliDetalheMercadoriaExcluirValidation();

			//pliHistorico
			_pliHistoricoExisteRelacionamentoValidation = new PliHistoricoExisteRelacionamentoValidation();
			_pliHistoricoExcluirValidation = new PliHistoricoExcluirValidation();

			//pliMercadoria
			_pliMercadoriaExisteRelacionamentoValidation = new PliMercadoriaExisteRelacionamentoValidation();
			_pliMercadoriaExcluirValidation = new PliMercadoriaExcluirValidation();

			//pliProcessoAnuente
			_pliProcessoAnuenteExisteRelacionamentoValidation = new PliProcessoAnuenteExisteRelacionamentoValidation();
			_pliProcessoAnuenteExcluirValidation = new PliProcessoAnuenteExcluirValidation();		

			//ncmAmazoniaOcidental
			_ncmExcluirValidation = new NcmExcluirValidation();
		}
	}
}