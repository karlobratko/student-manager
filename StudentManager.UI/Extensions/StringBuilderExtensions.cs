using System.Collections.Generic;
using System.Text;

namespace StudentManager.UI.Extensions;

public static class StringBuilderExtensions {
  public static StringBuilder AppendListInLines<T>(this StringBuilder builder, IEnumerable<T> items, string? prefix = null) {
    foreach (T item in items)
      _ = builder.AppendLine($"{prefix}{item}");
    return builder;
  }
}
