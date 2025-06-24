namespace Dsw2025Tpi.Domain.Entities;

public abstract class EntityBase
{
    public EntityBase()
    {
        Id = Guid.NewGuid();
    }
    public Guid Id { get; set; }
}
