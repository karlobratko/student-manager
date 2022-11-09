namespace StudentManager.DAL.Enums;

public enum CreateStatus {
  InternalError = -1,
  Success = 1,
  UniquenessViolated = 2,
  Recreated = 3
}
