using System.Windows.Input;

using StudentManager.UI.Commands;
using StudentManager.UI.Services;
using StudentManager.UI.Stores;

namespace StudentManager.UI.ViewModels;

public class LoginViewModel : ViewModelBase {
  private string? _username;
  public string? Username {
    get => _username;
    set {
      _username = value;
      OnPropertyChanged(nameof(Username));
    }
  }

  private string? _password;
  public string? Password {
    get => _password;
    set {
      _password = value;
      OnPropertyChanged(nameof(Password));
    }
  }
  public ICommand LoginCommand { get; }

  public LoginViewModel(IAccountStore accountStore,
                        INavigationService homeNavigationService) {
    LoginCommand = new LoginCommand(this,
                                    accountStore,
                                    homeNavigationService);
  }
}
