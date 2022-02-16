import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'ncm' })
export class NcmPipe implements PipeTransform {
	transform(value: any, args?: any): any {
		return value.toString().replace(/(\d{4})(\d{2})(\d{2})/g, '\$1.\$2.\$3');
	}
}
