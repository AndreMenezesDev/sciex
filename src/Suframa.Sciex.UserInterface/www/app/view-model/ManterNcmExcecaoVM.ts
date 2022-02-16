import { viewMunicipioVM } from "./ViewMunicipioVM";

export class manterNcmExcecaoVM {
	idNcmExcecao?: number;
	codigo: number;
	descricaoNcm: string;
	status: number;
	codigoMunicipio: number;
	descricaoMunicipio: string;
	codigoSetor: number;
	descricaoSetor: string;
	dataInicioVigencia: Date;
	uf: string;

	// Complemento de Classe
	listaMunicipios: Array<manterNcmExcecaoVM>;
	mensagemErro: string;
	buscarPorMunicipiosAssociados: boolean;
	checkbox: boolean;
	dataInicioVigenciaFormatado: string;
	codigoNCM: string;
	dataVigenciaInicio?: Date;
	dataVigenciaFim?: Date;
	descricaoMunicipioCodigo: string;

}
