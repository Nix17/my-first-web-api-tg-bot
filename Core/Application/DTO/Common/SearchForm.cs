using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Common;

public class SearchForm
{
    public string Field { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;
}