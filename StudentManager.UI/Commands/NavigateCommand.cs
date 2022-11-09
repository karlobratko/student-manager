using StudentManager.UI.Services;

namespace StudentManager.UI.Commands; 

public class NavigateCommand : CommandBase {
  private readonly INavigationService _navigationService;

  public NavigateCommand(INavigationService navigationService) =>
    _navigationService = navigationService;

  public override void Execute(object? parameter) => _navigationService.Navigate();
}
