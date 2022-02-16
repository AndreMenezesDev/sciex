import { Injectable } from '@angular/core';

@Injectable()
export class FormatCodeService {
	constructor() { }
	
	public transform(value: any, args: any): any {
		console.log(value + " " + args);
		var campo = "";

		if (args == 2)
			campo = ("00" + value).slice(-args);

		if (args == 3)
			campo = ("000" + value).slice(-args);

		if (args == 4)
			campo = ("0000" + value).slice(-args);

		if (args == 5)
			campo = ("00000" + value).slice(-args);

		if (args == 6)
			campo = ("000000" + value).slice(-args);

		if (args == 7)
			campo = ("0000000" + value).slice(-args);

		if (args == 8)
			campo = ("00000000" + value).slice(-args);

		if (args == 9)
			campo = ("000000000" + value).slice(-args);

		if (args == 10)
			campo = ("0000000000" + value).slice(-args);

		console.log(campo);

		return campo;
	}
}
