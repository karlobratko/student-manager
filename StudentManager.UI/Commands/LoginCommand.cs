using System.Text;
using System.Windows;

using StudentManager.UI.Models;
using StudentManager.UI.Services;
using StudentManager.UI.Stores;
using StudentManager.UI.ViewModels;

namespace StudentManager.UI.Commands;

public class LoginCommand : CommandBase {
  private readonly LoginViewModel _loginViewModel;
  private readonly IAccountStore _accountStore;
  private readonly INavigationService _navigationService;

  public LoginCommand(LoginViewModel loginViewModel,
                      IAccountStore accountStore,
                      INavigationService navigationService) {
    _loginViewModel = loginViewModel;
    _accountStore = accountStore;
    _navigationService = navigationService;
  }

  public override void Execute(object? parameter) {
    if (_loginViewModel.Username == "kbratko" &&
        _loginViewModel.Password == "Pa$$w0rd") {
      _ = MessageBox.Show($"Logging in {_loginViewModel.Username}...",
                          "Login",
                          MessageBoxButton.OK,
                          MessageBoxImage.Information);

      var account = new Account {
        Email = $"{_loginViewModel.Username}@test.com",
        Username = _loginViewModel.Username
      };

      _accountStore.CurrentAccount = account;

      _navigationService.Navigate();
    }
    else {
      _ = MessageBox.Show(new StringBuilder()
                            .AppendLine("Invalid username or password")
                            .AppendLine()
                            .AppendLine("Try with these:")
                            .AppendLine("Username: kbratko")
                            .AppendLine("Password: Pa$$w0rd")
                            .ToString(),
                          "Login",
                          MessageBoxButton.OK,
                          MessageBoxImage.Information);
    }
  }
}
