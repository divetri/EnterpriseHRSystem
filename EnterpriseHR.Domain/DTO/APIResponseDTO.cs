using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseHR.Domain.DTOs;

public class PagedResultDTO<T>
{
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
}

public class APIResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }

    public static APIResponse<T> Ok(T? data, string message = "Success")
        => new() { Success = true, Message = message, Data = data };

    public static APIResponse<T> Fail(string message, T? data = default)
        => new() { Success = false, Message = message, Data = data };
}