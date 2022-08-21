using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Client;
public class EndPoint
{
    protected virtual void CheckResponse(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"API Response was not successful: {response.StatusCode}");
        }
    }

    protected virtual void ThrowIfNull<T>(T obj)
    {
        if (obj is null)
        {
            throw new Exception($"API returned a null value to a method where null values are not allowed.");
        }
    }
}
