using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Common;

public class KeyValueDTO
{
    public Guid Id { get; set; }
    public string Value { get; set; } = String.Empty;
}