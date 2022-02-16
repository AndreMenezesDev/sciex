import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'searchFilter' })

export class SearchFilterPipe implements PipeTransform {
	transform(value: any, search: string): any {

		if (!search) { return value; }
		let solution = value.filter(o => {
			console.log(status);
			if (!o) { return; }
			return o.status == search;
		})
		return solution;
	}
}
