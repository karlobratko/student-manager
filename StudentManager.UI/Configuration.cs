using System.IO;

using Microsoft.Extensions.Configuration;

namespace StudentManager.UI;

public static class Configuration {
  private const string SETTINGS_FILE = "appsettings.json";

  public static IConfigurationRoot Setup() =>
    new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                              .AddJsonFile(SETTINGS_FILE,
                                           optional: false,
                                           reloadOnChange: true)
                              .Build();
}
