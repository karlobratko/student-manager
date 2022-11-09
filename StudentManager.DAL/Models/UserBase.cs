using System.Text.RegularExpressions;

using StudentManager.DAL.Base.Models;

namespace StudentManager.DAL.Models;

public abstract class UserBase : PersistableBase<int> {
  public string FName { get; set; } = "";
  public string LName { get; set; } = "";
  public DateTime BirthDate { get; set; } = DateTime.Now;
  public string Email { get; set; } = "";
  public string? PhoneNumber { get; set; }
  public string? Address { get; set; }
  public byte[]? Image { get; set; }

  public virtual bool IsValid() => 
       !string.IsNullOrWhiteSpace(FName)
    && !string.IsNullOrWhiteSpace(LName)
    && !string.IsNullOrWhiteSpace(Email)
    && Regex.IsMatch(Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

  public virtual IList<string> GetValidationMessages() {
    IList<string> valMessages = new List<string>();

    if (string.IsNullOrWhiteSpace(FName))
      valMessages.Add("First name is required");

    if (string.IsNullOrWhiteSpace(LName))
      valMessages.Add("Last name is required");

    if (string.IsNullOrWhiteSpace(Email) || !Regex.IsMatch(Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
      valMessages.Add("Email field must be valid Email");

    return valMessages;
  }
}
