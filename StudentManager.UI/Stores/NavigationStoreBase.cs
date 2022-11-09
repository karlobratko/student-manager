using System;

using StudentManager.UI.ViewModels;

namespace StudentManager.UI.Stores {
  public abstract class NavigationStoreBase : INavigationStore {
    public event Action? CurrentViewModelChanged;

    private ViewModelBase? _currentViewModel;
    public ViewModelBase? CurrentViewModel {
      get => _currentViewModel;
      set {
        _currentViewModel?.Dispose();
        _currentViewModel = value;
        OnCurrentViewModelChanged();
      }
    }

    private void OnCurrentViewModelChanged() => CurrentViewModelChanged?.Invoke();
  }
}
