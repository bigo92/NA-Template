import { IpagingModel } from './IpagingModel';
export class PagingModel implements IpagingModel {
  order: string;
  page: number;
  size: number;
  sizeitem: number;
  asc: boolean;
  totalPage: number;
  recordPosition: number;
  keyword: string;
  count: number;
  constructor() {
    this.page = 1;
    this.size = 10;
    this.asc = true;
    this.recordPosition = 0;
    this.keyword = '';
    this.order = 'id ASC';
  }
}