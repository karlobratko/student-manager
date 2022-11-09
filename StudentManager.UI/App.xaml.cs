using System;
using System.Windows;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using StudentManager.UI.Services;

namespace StudentManager.UI;

public partial class App : Application {
  private readonly IServiceProvider _serviceProvider;

  public App() {
    IConfiguration _configuration = Configuration.Setup();
    _serviceProvider = Service.Setup(_configuration);
  }

  protected override void OnStartup(StartupEventArgs e) {
    INavigationService initialNavigationService = _serviceProvider.GetRequiredService<INavigationService>();
    initialNavigationService.Navigate();

    MainWindow mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
    mainWindow.Show();

    base.OnStartup(e);
  }
}
