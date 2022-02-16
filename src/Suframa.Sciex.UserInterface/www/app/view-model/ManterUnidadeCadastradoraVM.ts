import { municipioVM } from './MunicipioVM';

export class manterUnidadeCadastradoraVM {
	idUnidadeCadastradora?: number;
	descricao: string;
	uf: string;
	ufSecundaria: string;
	idMunicipio?: number;
	idMunicipioSecundario?: number;
	municipiosSecundarios: municipioVM[];
}
