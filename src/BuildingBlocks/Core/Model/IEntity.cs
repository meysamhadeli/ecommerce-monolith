namespace BuildingBlocks.Core.Model;

public interface IEntity: IVersion
{
    public DateTime? CreatedAt { get; set; }
    public long? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public long? LastModifiedBy { get; set; }
    public bool IsDeleted { get; set; }
}

public interface IVersion
{
    long Version { get; set; }
}
