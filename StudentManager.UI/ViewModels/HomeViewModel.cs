using System;
using System.Windows.Input;

using StudentManager.UI.Commands;
using StudentManager.UI.Services;
using StudentManager.UI.Stores;

namespace StudentManager.UI.ViewModels;

public sealed class HomeViewModel : ViewModelBase {
  public static string WelcomeMessage => "Welcome to Student Manager";

  private readonly IAccountStore _accountStore;
  public string? Username => _accountStore.CurrentAccount?.Username;
  public string? Email => _accountStore.CurrentAccount?.Email;

  public HomeViewModel(IAccountStore accountStore) {
    _accountStore = accountStore;

    _accountStore.CurrentAccountChanged += OnCurrentAccountChanged;
  }

  private void OnCurrentAccountChanged() {
    OnPropertyChanged(nameof(Username));
    OnPropertyChanged(nameof(Email));
  }

  public override void Dispose() {
    base.Dispose();

    _accountStore.CurrentAccountChanged -= OnCurrentAccountChanged;

    GC.SuppressFinalize(this);
  }
}
