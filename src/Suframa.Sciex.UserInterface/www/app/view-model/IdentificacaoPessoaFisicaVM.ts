import { manterEnderecoVM } from './ManterEnderecoVM';
import { documentoComprobatorioVM } from './DocumentoComprobatorioVM';

export class identificacaoPessoaFisicaVM {
	codigo: string;
	complemento: string;
	cpf?: string;
	dataAlteracao?: Date;
	dataInclusao?: Date;
	documentosComprobatorios: documentoComprobatorioVM[];
	email: string;
	endereco: manterEnderecoVM;
	idCep: number;
	idPessoaFisica: number;
	idProtocolo?: number;
	idRequerimento?: number;
	idTipoRequerimento?: number;
	idUnidadeCadastradora?: number;
	isCredenciamentoTransportador: boolean;
	isGerarProtocolo: boolean;
	modalidadeTransportador: number;
	nome: string;
	nomeSocial: string;
	numeroEndereco: string;
	pontoReferencia: string;
	ramal?: number;
	telefone?: number;
	tipoOrigem: number;
}
