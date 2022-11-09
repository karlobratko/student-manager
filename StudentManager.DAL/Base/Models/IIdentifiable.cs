namespace StudentManager.DAL.Base.Models;

public interface IIdentifiable<K>
  where K : struct {
  K Id { get; set; }
  Guid Guid { get; set; }
}
