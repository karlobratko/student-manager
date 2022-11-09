using StudentManager.DAL.Base.Models;

namespace StudentManager.DAL.Models;

public sealed class CourseParticipant : PersistableBase<int>, IEquatable<CourseParticipant> {
  public int StudentFK { get; set; }
  public int CourseFK { get; set; }
  public byte AttendedLectureHours { get; set; }
  public byte AttendedPracticeHours { get; set; }
  public bool IsSigned { get; set; }
  public byte Grade { get; set; }

  public bool Equals(CourseParticipant? other) =>
       other is not null
    && Guid == other.Guid;

  public override bool Equals(object? obj) => Equals(obj as CourseParticipant);

  public override int GetHashCode() => Guid.GetHashCode();

  public bool IsValid() =>
       StudentFK >= 1
    && CourseFK >= 1;

  public IList<string> GetValidationMessages() {
    IList<string> valMessages = new List<string>();

    if (StudentFK < 1)
      valMessages.Add("Please select lecturer");

    if (CourseFK < 1)
      valMessages.Add("Please select course");

    return valMessages;
  }
}
