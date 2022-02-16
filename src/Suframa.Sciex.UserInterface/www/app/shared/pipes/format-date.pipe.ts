import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'formatDate' })
export class FormatDatePipe implements PipeTransform {

	constructor(

	) { }

	transform(value: any, args?: any): any {
	
		var campo = value.replace(/^(\d{4})(\d{2})(\d{2})/, '$1.$2.$3');

		return campo;
	}
}
