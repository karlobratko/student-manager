using System;

using StudentManager.UI.Models;

namespace StudentManager.UI.Stores {
  public interface IAccountStore {
    Account? CurrentAccount { get; set; }
    bool IsLoggedIn { get; }

    event Action? CurrentAccountChanged;

    void Logout();
  }
}