using StudentManager.UI.Services;
using StudentManager.UI.Stores;

namespace StudentManager.UI.Commands {
  public class LogoutCommand : CommandBase {
    private readonly IAccountStore _accountStore;
    private readonly INavigationService _navigationService;

    public LogoutCommand(IAccountStore accountStore,
                         INavigationService navigationService) {
      _accountStore = accountStore;
      _navigationService = navigationService;
    }

    public override void Execute(object? parameter) {
      _accountStore.Logout();
      _navigationService.Navigate();
    }
  }
}
