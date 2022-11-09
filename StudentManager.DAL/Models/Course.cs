using StudentManager.DAL.Base.Models;

namespace StudentManager.DAL.Models;

public sealed class Course : PersistableBase<int>, IEquatable<Course> {
  public int HeadLecturerFK { get; set; }
  public string Name { get; set; } = "";
  public byte ECTS { get; set; }
  public string? Description { get; set; }
  public byte MaxLectureHours { get; set; }
  public byte MaxPracticeHours { get; set; }

  public bool Equals(Course? other) =>
       other is not null
    && Guid == other.Guid;

  public override bool Equals(object? obj) => Equals(obj as Course);

  public override int GetHashCode() => Guid.GetHashCode();

  public bool IsValid() =>
       HeadLecturerFK >= 1
    && !string.IsNullOrWhiteSpace(Name)
    && ECTS is >= 1 and <= 8
    && MaxLectureHours is >= 1 and <= 100
    && MaxPracticeHours is >= 1 and <= 100;

  public IList<string> GetValidationMessages() {
    IList<string> valMessages = new List<string>();

    if (HeadLecturerFK < 1)
      valMessages.Add("Please select head lecturer");

    if (string.IsNullOrWhiteSpace(Name))
      valMessages.Add("Name name is required");

    if (ECTS is not >= 1 and <= 8)
      valMessages.Add("ECTS must be between 1 and 8");

    if (MaxLectureHours is not >= 1 and <= 100)
      valMessages.Add("Lecture hours must be between 1 and 100");

    if (MaxPracticeHours is not >= 1 and <= 100)
      valMessages.Add("Practice hours must be between 1 and 100");

    return valMessages;
  }
}
