using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Extensions;

public static partial class GuidExtensions
{
    public static string ToTreeString(this Guid @this)
    {
        return @this.ToString().Replace("-", "");
    }
}