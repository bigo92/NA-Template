export class PagingModel2 {
    public itemsPerPage: number;
    public currentPage: number;
    public totalItems: number;
    public keyword: string;
    public order: string;
    constructor() {
        this.itemsPerPage = 1;
        this.currentPage = 10;
        this.totalItems = 0;
        this.keyword = '';
        this.order = 'id desc';
    }
}