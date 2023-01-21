using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.DTO.Common;

public interface IAuditableDTO
{
    public string CreatedBy { get; set; }
    public DateTime Created { get; set; }
    public string LastModifiedBy { get; set; }
    public DateTime LastModified { get; set; }
}

public abstract class AuditableDTO : IAuditableDTO
{
    [JsonPropertyOrder(96)]
    public string CreatedBy { get; set; } = String.Empty;
    [JsonPropertyOrder(97)]
    public DateTime Created { get; set; } = new DateTime();
    [JsonPropertyOrder(98)]
    public string LastModifiedBy { get; set; } = String.Empty;
    [JsonPropertyOrder(99)]
    public DateTime LastModified { get; set; } = new DateTime();
}

public abstract class AuditableBaseDTO : AuditableDTO
{
    [JsonPropertyOrder(-1)]
    public virtual Guid Id { get; set; }
}

public abstract class AuditableIntIdDTO : AuditableDTO
{
    [JsonPropertyOrder(-1)]
    public virtual int Id { get; set; }
}