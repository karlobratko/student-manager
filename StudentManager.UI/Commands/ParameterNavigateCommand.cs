
using StudentManager.UI.Services;

namespace StudentManager.UI.Commands;

public class ParameterNavigateCommand<T> : CommandBase where T : class {
  private readonly IParameterNavigationService<T> _navigationService;

  public ParameterNavigateCommand(IParameterNavigationService<T> navigationService) =>
    _navigationService = navigationService;

  public override void Execute(object? parameter) => _navigationService.Navigate(parameter as T);
}
