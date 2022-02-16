export class StringService {
	public replaceAll(str, search, replacement): string {
		search = search.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
		return str.replace(new RegExp(search, 'g'), replacement);
	}
}
