import { Injectable } from '@angular/core';

@Injectable()
export class ExtractNumberService {
	constructor() { }

	extractNumbers(obj: any) {
		if (!obj) {
			return '';
		}

		return obj.toString().replace(/\D/g, '');
	}
}
