using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Wrappers;

public class QueryParametersList
{
    public string? Search { get; set; } = "";
    public bool? IsActiveOnly { get; set; } = true;
}