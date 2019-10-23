import { ErrorModel } from './errorModel';
import { PagingModel } from './pagingModel';

export class HttpModel<T> {
    constructor(
        public success: boolean,
        public error: ErrorModel[],
        public data: T,
        public paging: PagingModel,
    ) {}
}