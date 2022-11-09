namespace StudentManager.DAL.Models;

public sealed class Student : UserBase, IEquatable<Student> {
  public string JMBAG { get; set; } = "";
  public byte YearOfStudy { get; set; }

  public bool Equals(Student? other) =>
     other is not null
  && Guid == other.Guid;

  public override bool Equals(object? obj) => Equals(obj as Student);

  public override int GetHashCode() => Guid.GetHashCode();

  public override string ToString() => $"{FName} {LName} ({JMBAG})";

  public override bool IsValid() => 
       base.IsValid()
    && !string.IsNullOrWhiteSpace(JMBAG)
    && YearOfStudy is >= 1 and <= 5;

  public override IList<string> GetValidationMessages() {
    IList<string> list = base.GetValidationMessages();

    if (string.IsNullOrWhiteSpace(JMBAG))
      list.Add("JMBAG is required");

    if (YearOfStudy is not (>= 1 and <= 5))
      list.Add("Year of study must be between 1 and 5");

    return list;
  }
}
