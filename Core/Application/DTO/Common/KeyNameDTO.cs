using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Common;

public interface IKeyName
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class KeyNameDTO : IKeyName
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}