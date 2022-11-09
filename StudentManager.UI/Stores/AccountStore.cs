using System;

using StudentManager.UI.Models;

namespace StudentManager.UI.Stores {
  public class AccountStore : IAccountStore {
    public event Action? CurrentAccountChanged;

    private Account? _currentAccount;
    public Account? CurrentAccount {
      get => _currentAccount;
      set {
        _currentAccount = value;
        CurrentAccountChanged?.Invoke();
      }
    }

    public bool IsLoggedIn => CurrentAccount is not null;

    public void Logout() => CurrentAccount = null;
  }
}
