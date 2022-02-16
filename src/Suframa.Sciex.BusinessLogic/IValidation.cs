using FluentValidation;
using Suframa.Sciex.CrossCutting.DataTransferObject;

namespace Suframa.Sciex.BusinessLogic
{
    public interface IValidation
    {
        IValidator<CepDto> _cepAdicionarValidation { get; }
        IValidator<PessoaJuridicaDto> _filtroCadastroSelecionarValidation { get; }
        IValidator<ManterNaturezaJuridicaDto> _manterNaturezaJuridicaSalvarValidation { get; }
        IValidator<NaturezaJuridicaDto> _naturezaJuridicaAdicionarValidation { get; }
        IValidator<NaturezaJuridicaDto> _naturezaJuridicaApagarValidation { get; }
        IValidator<PessoaJuridicaDto> _pessoaJuridicaSalvarValidation { get; }
    }
}