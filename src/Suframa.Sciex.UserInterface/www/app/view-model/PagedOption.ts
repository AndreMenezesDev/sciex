import { SortOption } from './SortOption';

export class PagedOption extends SortOption {
    page : number;
    reverse : boolean;
    size : number;
    sort : string;
    sortManny : Array<SortOption>;    
}