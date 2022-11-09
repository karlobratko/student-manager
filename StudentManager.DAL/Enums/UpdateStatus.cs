namespace StudentManager.DAL.Enums;

public enum UpdateStatus {
  InternalError = -1,
  Success = 1,
  NotExists = 2,
  Deleted = 3,
  UniquenessViolated = 4
}
