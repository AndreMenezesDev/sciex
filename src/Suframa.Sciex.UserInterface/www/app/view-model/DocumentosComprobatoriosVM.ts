import { documentoComprobatorioVM } from './DocumentoComprobatorioVM';

export class documentosComprobatoriosVM {
	idRequerimento?: number;
	idPessoaJuridica?: number;
	idTipoRequerimento?: number;
	idProtocolo?: number;
	tipoOrigem?: number;
	isGerarProtocolo: boolean;
	isQuadroSocietario: boolean;

	documentosComprobatorios = new Array<documentoComprobatorioVM>();
	documentosComprobatoriosVigentes = new Array<documentoComprobatorioVM>();
}
