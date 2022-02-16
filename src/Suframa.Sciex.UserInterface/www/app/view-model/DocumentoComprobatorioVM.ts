export class documentoComprobatorioVM {
	idRequerimentoDocumento?: number;
	idRequerimento?: number;
	idTipoDocumento?: number;
	isStatusInfoComplementar?: boolean;
	descricaoTipoDocumento: string;
	numeroCertidao: string;
	dataInclusao?: Date;
	dataVencimento?: Date;
	dataExpedicao?: Date;
	horaExpedicao: string;
	status: any;
	tipoOrigem: string;
	idArquivo?: number;
	nomeArquivo: string;
}
