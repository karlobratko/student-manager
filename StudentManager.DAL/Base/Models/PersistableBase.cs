namespace StudentManager.DAL.Base.Models;

public abstract class PersistableBase<K> : IPersistable<K>
  where K : struct {
  public K Id { get; set; }
  public Guid Guid { get; set; }
  public DateTime CreateDate { get; set; }
  public K CreatedBy { get; set; }
  public DateTime UpdateDate { get; set; }
  public K UpdatedBy { get; set; }
  public DateTime? DeleteDate { get; set; }
  public K? DeletedBy { get; set; }

  public override bool Equals(object? obj) =>
       obj is PersistableBase<K> other
    && Guid == other.Guid;

  public override int GetHashCode() => Guid.GetHashCode();
}
