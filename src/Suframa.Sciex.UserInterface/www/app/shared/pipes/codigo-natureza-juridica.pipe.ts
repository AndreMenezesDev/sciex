import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'codigoNaturezaJuridica' })
export class CodigoNaturezaJuridicaPipe implements PipeTransform {
	transform(value: any, args?: any): any {
		return value.toString().replace(/(\d{3})(\d{1})/g, '\$1-\$2');
	}
}
