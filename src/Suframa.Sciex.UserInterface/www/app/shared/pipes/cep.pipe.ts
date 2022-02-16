import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'cep' })
export class CepPipe implements PipeTransform {
	transform(value: any, args?: any): any {
		return value.toString().replace(/(\d{5})(\d{3})/g, '\$1-\$2');
	}
}
