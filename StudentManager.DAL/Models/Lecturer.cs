namespace StudentManager.DAL.Models;

public sealed class Lecturer : UserBase, IEquatable<Lecturer> {
  public bool Equals(Lecturer? other) =>
       other is not null
    && Guid == other.Guid;

  public override bool Equals(object? obj) => Equals(obj as Lecturer);

  public override int GetHashCode() => Guid.GetHashCode();

  public override string ToString() => $"{FName} {LName}";

  public override bool IsValid() => base.IsValid();

  public override IList<string> GetValidationMessages() => base.GetValidationMessages();
}
