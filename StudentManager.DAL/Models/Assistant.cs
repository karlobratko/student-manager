using System.Xml.Linq;

using StudentManager.DAL.Base.Models;

namespace StudentManager.DAL.Models;

public sealed class Assistant : PersistableBase<int>, IEquatable<Assistant> {
  public int LecturerFK { get; set; }
  public int CourseFK { get; set; }

  public bool Equals(Assistant? other) => 
       other is not null
    && Guid == other.Guid;

  public override bool Equals(object? obj) => Equals(obj as Assistant);

  public override int GetHashCode() => Guid.GetHashCode();

  public bool IsValid() =>
     LecturerFK >= 1
  && CourseFK >= 1;

  public IList<string> GetValidationMessages() {
    IList<string> valMessages = new List<string>();

    if (LecturerFK < 1)
      valMessages.Add("Please select lecturer");

    if (CourseFK < 1)
      valMessages.Add("Please select course");

    return valMessages;
  }
}
