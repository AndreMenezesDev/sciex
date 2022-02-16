using System;

namespace Suframa.Sciex.CrossCutting.DataTransferObject.Enum
{
    public enum EnumMotivoSituacaoInscricao
    {
        ComprovanteRegularidadeFiscal = 1,
        CertidaoFgtsVencida = 2,
        SancaoAdministrativaJudicial = 3,
        DebitoPendenteTcif = 4,
        DebitoPendenteTcifCnpj = 5,
        DebitoPendenteTsa = 6,
        DebitoPendenteTsaCnpj = 7,
        InscricaoDividaCadin = 8,
        InscricaoCnep = 9,
        InscricaoCeis = 10,
        RestricaoCnj = 11,
        RestricaoIbama = 12,
        InexecucaoTotalParcialProjeto = 13,
        NaoRealizacaoTotalParcialInvestimentoPd = 14,
        IndicadoresIndustriaisNaoAtualizados = 15,
        BloqueadaMaisSeisMeses = 31,
        InoperanteMais24MesesConsecutivos = 32,
        InativacaoMaisSeisMeses = 61,
        ImpossibilidadeConfirmacaoAutenticidadeDocumentosInformacoes = 62,
        PedidoPessoaInteressada = 63
    }
}