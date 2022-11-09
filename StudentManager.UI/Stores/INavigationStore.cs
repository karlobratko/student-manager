using System;

using StudentManager.UI.ViewModels;

namespace StudentManager.UI.Stores {
  public interface INavigationStore {
    ViewModelBase? CurrentViewModel { get; set; }

    event Action? CurrentViewModelChanged;
  }
}