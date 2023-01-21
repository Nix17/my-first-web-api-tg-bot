using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common;

public abstract class AuditableBaseEntity : AuditableEntity
{
    public virtual Guid Id { get; set; }
}

public abstract class AuditableIntIdEntity : AuditableEntity, IAuditableIntIdEntity
{
    public int Id { get; set; }
}

public interface IAuditableIntIdEntity
{
    int Id { get; set; }
}

public interface IAuditableEntity
{
    DateTime Created { get; set; }
    string CreatedBy { get; set; }
    DateTime LastModified { get; set; }
    string LastModifiedBy { get; set; }
}

public abstract class AuditableEntity : IAuditableEntity
{
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime Created { get; set; } = new DateTime();
    public string LastModifiedBy { get; set; } = string.Empty;
    public DateTime LastModified { get; set; } = new DateTime();
}