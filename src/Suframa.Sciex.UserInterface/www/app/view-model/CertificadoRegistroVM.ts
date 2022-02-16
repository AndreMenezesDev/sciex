export class CertificadoRegistroVM{

		// Informações Empresa Exportadora
		razaoSocial: string;
		inscricaoCadastral : string;
		cnpj: string;
		endereco : string;
		cep : string;

		// Dados Plano Aprovação
		numeroPlano : string;
		numeroProcesso : string;
		modalidade : string;
		dataDeferimento : string;
		dataValidade : string;


		// Valores Aprovados
		valorNacional : string;
		valorImportacaoFOB : string;
		valorImportacaoCFR : string;
		valorExportacao : string;

		codigoIdentificadorCRPE: string;
		listaAcrescimoSolicitacao:any;
		insumosImportadosAprovados:any;
		totalInsumosImportados:any;
		dataCancelamento:any;
		dataProrrogacao:any;
		dataProrrogacaoEspecial:any;
}