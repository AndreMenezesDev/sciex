using System.ComponentModel;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.Enum
{
    public enum EnumTipoRequerimento
    {
        [Description("Inscrição Cadastral de Pessoa Jurídica")]
        InscricaoCadastralPessoaJuridica = 1,

        [Description("Atualização de Inscrição Cadastral de Pessoa Jurídica")]
        AtualizacaoInscricaoCadastralPessoaJuridica = 2,

        [Description("Atualização de Documentos de Pessoa Jurídica")]
        AtualizacaoDocumentosPessoaJuridica = 3,

        [Description("Reativação de Inscrição Cadastral de Pessoa Jurídica")]
        ReativacaoInscricaoCadastralPessoaJuridica = 4,

        [Description("Cancelamento de Inscrição Cadastral de Pessoa Jurídica")]
        CancelamentoInscricaoCadastralPessoaJuridica = 5,

        [Description("Enviar Recurso de Pessoa Jurídica")]
        EnviarRecursoPessoaJuridica = 6,

        [Description("Desbloqueio de Pessoa Jurídica")]
        DesbloqueioPessoaJuridica = 7,

        [Description("Inscrição Cadastral de Pessoa Física")]
        InscricaoCadastralPessoaFisica = 8,

        [Description("Atualização de Inscrição Cadastral de Pessoa Física")]
        AtualizacaoInscricaoCadastralPessoaFisica = 9,

        [Description("Atualização de Documentos de Pessoa Física")]
        AtualizacaoDocumentosPessoaFisica = 10,

        [Description("Reativação de Inscrição Cadastral de Pessoa Física")]
        ReativacaoInscricaoCadastralPessoaFisica = 11,

        [Description("Cancelamento de Inscrição Cadastral de Pessoa Física")]
        CancelamentoInscricaoCadastralPessoaFisica = 12,

        [Description("Enviar Recurso de Pessoa Física")]
        EnviarRecursoPessoaFisica = 13,

        [Description("Desbloqueio de Pessoa Física")]
        DesbloqueioPessoaFisica = 14,

        [Description("Credenciamento de Pessoa Jurídica - Auditor")]
        CredenciamentoPessoaJuridicaAuditor = 15,

        [Description("Atualização de Credenciamento de Pessoa Jurídica - Auditor")]
        AtualizacaoCredenciamentoPessoaJuridicaAuditor = 16,

        [Description("Credenciamento de Pessoa Jurídica - Transportador")]
        CredenciamentoPessoaJuridicaTransportador = 17,

        [Description("Atualização de Credenciamento de Pessoa Jurídica - Transportador")]
        AtualizacaoCredenciamentoPessoaJuridicaTransportador = 18,

        [Description("Credenciamento de Pessoa Jurídica - Remetente")]
        CredenciamentoPessoaJuridicaRemetente = 19,

        [Description("Atualização de Credenciamento de Pessoa Jurídica - Remetente")]
        AtualizacaoCredenciamentoPessoaJuridicaRemetente = 20,

        [Description("Credenciamento de Pessoa Física - Preposto")]
        CredenciamentoPessoaFisicaPreposto = 21,

        [Description("Atualização de Credenciamento de Pessoa Física - Preposto")]
        AtualizacaoCredenciamentoPessoaFisicaPreposto = 22,

        [Description("Credenciamento de Pessoa Física - Consultor")]
        CredenciamentoPessoaFisicaConsultor = 23,

        [Description("Atualização de Credenciamento de Pessoa Física - Consultor")]
        AtualizacaoCredenciamentoPessoaFisicaConsultor = 24,

        [Description("Credenciamento de Pessoa Física - Transportador")]
        CredenciamentoPessoaFisicaTransportador = 25,

        [Description("Atualização de Credenciamento de Pessoa Física - Transportador")]
        AtualizacaoCredenciamentoPessoaFisicaTransportador = 26
    }
}