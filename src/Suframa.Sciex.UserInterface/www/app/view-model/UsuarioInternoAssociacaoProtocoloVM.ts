import { protocoloVM } from './ProtocoloVM';

export class usuarioInternoAssociacaoProtocoloVM {
	idUnidadeCadastradora: number;
	idUnidadeCadastradoraBusca: number;
	idUsuarioInterno: number;
	nomeUsuarioInterno: string;
	protocolos: Array<number> = new Array<number>();
	protocolosVM: Array<protocoloVM> = new Array<protocoloVM>();
}
