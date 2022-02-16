export class consultaPublicaVM {
	cpfCnpj: string;
	dataRestricao?: Date;
	idConsultaPublica?: number;
	idProtocolo?: number;
	idPessoaFisica?: number;
	idPessoaJuridica?: number;
	idPessoaJuridicaAdministrador?: number;
	idPessoaJuridicaSocio?: number;
	idArquivo?: number;
	idTipoConsultaPublica?: number;
	nomeArquivo: string;
	nomeConsulta: string;
	nomeRazaoSocial: string;
	tipoOrigem: number;
	statusRestricao?: boolean;
}
