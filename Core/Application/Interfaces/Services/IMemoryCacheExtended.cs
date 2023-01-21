using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Caching.Memory;

namespace Application.Interfaces.Services;

public interface IMemoryCacheExtended : IEnumerable<KeyValuePair<object, object>>, IMemoryCache
{
    /// <summary>
    /// Clears all cache entries.
    /// </summary>
    void Clear();
}