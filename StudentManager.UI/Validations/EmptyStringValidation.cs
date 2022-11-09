using System.Windows.Controls;

namespace StudentManager.UI.Validations;
public class EmptyStringValidation : ValidationRule {
  public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo) {
    string? test = value as string;
    return new ValidationResult(string.IsNullOrWhiteSpace(test), "Field is required");
  }
}