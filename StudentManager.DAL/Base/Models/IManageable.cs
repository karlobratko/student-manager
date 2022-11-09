namespace StudentManager.DAL.Base.Models;

public interface IManageable<K>
  where K : struct {
  DateTime CreateDate { get; set; }
  K CreatedBy { get; set; }
  DateTime UpdateDate { get; set; }
  K UpdatedBy { get; set; }
  DateTime? DeleteDate { get; set; }
  K? DeletedBy { get; set; }
}
