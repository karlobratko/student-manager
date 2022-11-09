namespace StudentManager.DAL.Base.Models;

public interface IPersistable<K> : IIdentifiable<K>, IManageable<K>
  where K : struct {
}
