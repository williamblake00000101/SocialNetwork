export interface Pagination {
  pageIndex: number;
  pageSize: number;
  count: number;
  data: object | undefined;
}

export class PaginatedResult<T> {
  result?: T;
  pagination?: Pagination;
}
