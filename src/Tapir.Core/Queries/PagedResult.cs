﻿namespace Tapir.Core.Queries
{
    public class PagedResult<T>
    {
        public required List<T> Items { get; set; }
        public required int PageNumber { get; set; }
        public required int PageSize { get; set; }
        public required int TotalCount { get; set; }
    }
}
